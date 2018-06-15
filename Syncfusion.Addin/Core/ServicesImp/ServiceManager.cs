using Syncfusion.Addin.Core.Collection;
using Syncfusion.Addin.Core.Services;
using Syncfusion.Addin.Utility;
using System;
using System.Collections.Generic;

namespace Syncfusion.Addin.Core.ServicesImp
{
    /// <summary>
    /// 服务管理
    /// </summary>
    public class ServiceManager : IServiceManager
    {
        public class ObjectCreator<T>
        {
            private T _serviceInstance;

            public IBundle Owner
            {
                get;
                private set;
            }

            public string Class
            {
                get;
                private set;
            }

            public Func<string, T> Creator
            {
                get;
                private set;
            }

            public ObjectCreator(IBundle owner, string classType, Func<string, T> creator)
            {
                AssertUtility.ArgumentNotNull(classType, "classType");
                AssertUtility.ArgumentNotNull(creator, "creator");
                Class = classType;
                Creator = creator;
                Owner = owner;
            }

            public ObjectCreator(IBundle owner, T instance)
            {
                AssertUtility.ArgumentNotNull(instance, "defaultValue");
                _serviceInstance = instance;
                Owner = owner;
            }

            public T GetOrCreateInstance()
            {
                return GetOrCreateInstance(true);
            }

            public T GetOrCreateInstance(bool allowCreate)
            {
                if (allowCreate && _serviceInstance == null)
                {
                    _serviceInstance = Creator(Class);
                }
                return _serviceInstance;
            }

            public override string ToString()
            {
                if (_serviceInstance != null)
                {
                    return _serviceInstance.ToString();
                }
                return Class;
            }
        }

        public class InterfaceHolder
        {
            private ServiceManager.ObjectCreator<Type> _creator;

            public InterfaceHolder(Type type)
            {
                _creator = new ServiceManager.ObjectCreator<Type>(null, type);
            }

            public InterfaceHolder(string serviceType, Func<string, Type> creator)
            {
                _creator = new ServiceManager.ObjectCreator<Type>(null, serviceType, creator);
            }

            public bool Match(string serviceType)
            {
                if (_creator.Class == serviceType)
                {
                    return true;
                }
                Type orCreateInstance = _creator.GetOrCreateInstance();
                return orCreateInstance != null && orCreateInstance.FullName == serviceType;
            }

            public bool Match(Type serviceType)
            {
                return serviceType.FullName == _creator.Class || _creator.GetOrCreateInstance() == serviceType;
            }

            public Type GetServiceType()
            {
                return GetServiceType(true);
            }

            public Type GetServiceType(bool allowCreate)
            {
                if (_creator != null)
                {
                    return _creator.GetOrCreateInstance();
                }
                return null;
            }

            public override string ToString()
            {
                return _creator.ToString();
            }
        }

        public class ServiceInstancesHolder
        {
            private ThreadSafeList<ServiceManager.ObjectCreator<object>> ServiceCreators = new ThreadSafeList<ServiceManager.ObjectCreator<object>>();

            public int Remove(object owner, object serviceInstance)
            {
                return RemoveAll((ServiceManager.ObjectCreator<object> item) => item.Owner == owner && item.GetOrCreateInstance(false) == serviceInstance);
            }

            public int RemoveAll(Predicate<ServiceManager.ObjectCreator<object>> match)
            {
                return ServiceCreators.RemoveAll(match);
            }

            public List<object> RemoveAll(IBundle owner)
            {
                List<object> result = new List<object>();
                ServiceCreators.RemoveAll(delegate (ServiceManager.ObjectCreator<object> item)
              {
                  if (item.Owner == owner)
                  {
                      object orCreateInstance = item.GetOrCreateInstance(false);
                      if (orCreateInstance != null)
                      {
                          result.Add(orCreateInstance);
                      }
                      return true;
                  }
                  return false;
              });
                return result;
            }

            public void Add(IBundle owner, object serviceInstance)
            {
                ServiceCreators.Add(new ServiceManager.ObjectCreator<object>(owner, serviceInstance));
            }

            public void AddRange(IBundle owner, IEnumerable<object> serviceInstance)
            {
                foreach (object current in serviceInstance)
                {
                    Add(owner, current);
                }
            }

            public void AddClass(IBundle owner, string classType, Func<string, object> creator)
            {
                ServiceCreators.Add(new ServiceManager.ObjectCreator<object>(owner, classType, creator));
            }

            public List<object> GetServiceInstances(bool allowCreate)
            {
                List<object> result = new List<object>();
                ServiceCreators.ForEach(delegate (ServiceManager.ObjectCreator<object> item)
              {
                  if (item.GetOrCreateInstance(allowCreate) != null)
                  {
                      result.Add(item.GetOrCreateInstance(allowCreate));
                  }
              });
                return result;
            }

            public List<object> GetServiceInstances()
            {
                return GetServiceInstances(true);
            }

            public override string ToString()
            {
                return ServiceCreators.ToString();
            }
        }

        private class FindServiceResult
        {
            public Dictionary<ServiceManager.InterfaceHolder, ServiceManager.ServiceInstancesHolder> Container
            {
                get;
                private set;
            }

            public ServiceManager.InterfaceHolder Key
            {
                get;
                private set;
            }

            public ServiceManager.ServiceInstancesHolder Value
            {
                get;
                private set;
            }

            public FindServiceResult(Dictionary<ServiceManager.InterfaceHolder, ServiceManager.ServiceInstancesHolder> container, ServiceManager.InterfaceHolder key)
            {
                Container = container;
                Key = key;
                Value = container[key];
            }
        }

        private class ServiceCollection : ThreadSafeDictionary<InterfaceHolder, ServiceInstancesHolder>
        {
            public List<Type> RemoveServiceInstance(IBundle owner, object serviceInstance)
            {
                List<Type> serviceInterfaces = new List<Type>();
                base.ForEach(delegate (KeyValuePair<ServiceManager.InterfaceHolder, ServiceManager.ServiceInstancesHolder> item)
                {
                    if (item.Value.Remove(owner, serviceInstance) > 0)
                    {
                        serviceInterfaces.Add(item.Key.GetServiceType());
                    }
                });
                return serviceInterfaces;
            }

            public int RemoveServiceInstance(IBundle owner, Type serviceType, object serviceInstance)
            {
                return Find<int>(serviceType, false, (ServiceManager.FindServiceResult result) => result.Value.Remove(owner, serviceInstance));
            }

            public int RemoveServiceInstance(IBundle owner, Type serviceType)
            {
                return Find<int>(serviceType, false, result => result.Value.RemoveAll((ServiceManager.ObjectCreator<object> item) => item.Owner == owner));
            }

            public T Find<T>(string serviceType, Func<ServiceManager.InterfaceHolder> creator, Func<ServiceManager.FindServiceResult, T> func)
            {
                return Find(item => item.Match(serviceType), creator, func);
            }

            public T Find<T>(Type serviceType, bool createIfNotFound, Func<FindServiceResult, T> func)
            {
                Func<ServiceManager.InterfaceHolder> creator = null;
                if (createIfNotFound)
                {
                    creator = (() => new InterfaceHolder(serviceType));
                }
                return Find<T>((ServiceManager.InterfaceHolder item) => item.Match(serviceType), creator, func);
            }

            private T Find<T>(Func<ServiceManager.InterfaceHolder, bool> comparer, Func<ServiceManager.InterfaceHolder> creator, Func<ServiceManager.FindServiceResult, T> func)
            {
                T result;
                using (DictionaryLocker<ServiceManager.InterfaceHolder, ServiceManager.ServiceInstancesHolder> dictionaryLocker = CreateLocker())
                {
                    foreach (KeyValuePair<ServiceManager.InterfaceHolder, ServiceManager.ServiceInstancesHolder> current in dictionaryLocker)
                    {
                        if (comparer(current.Key) && func != null)
                        {
                            result = func(new ServiceManager.FindServiceResult(base.Container, current.Key));
                            return result;
                        }
                    }
                    if (creator != null)
                    {
                        AssertUtility.ArgumentNotNull(creator, "creator");
                        ServiceManager.InterfaceHolder key = creator();
                        dictionaryLocker[key] = new ServiceManager.ServiceInstancesHolder();
                        if (func != null)
                        {
                            result = func(new ServiceManager.FindServiceResult(base.Container, key));
                            return result;
                        }
                    }
                    result = default(T);
                }
                return result;
            }
        }

        private ServiceCollection _serviceCollection = new ServiceCollection();

        public ServiceManager(IFramework framework)
        {
            AssertUtility.ArgumentNotNull(framework, "framework");

            //  framework.EventManager.AddBundleEventListener(new EventHandler<BundleStateChangedEventArgs>(  BundleEventListener), true);
        }

        public void AddService(IBundle owner, Type serviceType, params object[] serviceInstance)
        {
            for (int i = 0; i < serviceInstance.Length; i++)
            {
                object obj = serviceInstance[i];
                if (!serviceType.IsAssignableFrom(obj.GetType()))
                {
                    throw new Exception(string.Format("The {0} must be of type {1}", obj, serviceType));
                }
            }
            GetOrCreateInstanceCollection(serviceType).AddRange(owner, serviceInstance);
        }

        public void RemoveService<T>(IBundle owner, object serviceInstance)
        {
            RemoveService(owner, typeof(T), serviceInstance);
        }

        public void RemoveService(IBundle owner, Type serviceType, object serviceInstance)
        {
            if (_serviceCollection.RemoveServiceInstance(owner, serviceType, serviceInstance) == 0)
            {
                return;
            }
        }

        public void RemoveServiceByOwner(IBundle owner)
        {
            _serviceCollection.ForEach(delegate (KeyValuePair<ServiceManager.InterfaceHolder, ServiceManager.ServiceInstancesHolder> item)
          {
              List<object> list = item.Value.RemoveAll(owner);
              if (list.Count > 0)
              {
                    //    _framework.EventManager.DispatchServiceEvent(this, new ServiceEventArgs(item.Key.GetServiceType().FullName, list.ToArray(), ServiceEventType.Remove));
                }
          });
        }

        public void RemoveService(IBundle owner, object serviceInstance)
        {
            List<Type> list = _serviceCollection.RemoveServiceInstance(owner, serviceInstance);
            if (list.Count == 0)
            {
                return;
            }
        }

        private ServiceInstancesHolder GetOrCreateInstanceCollection(Type serviceType)
        {
            return _serviceCollection.Find<ServiceManager.ServiceInstancesHolder>(serviceType, true, (ServiceManager.FindServiceResult result) => result.Value);
        }

        private ServiceInstancesHolder GetOrCreateInstanceCollection(string serviceType, Func<string, Type> classLoader)
        {
            return _serviceCollection.Find(serviceType, () => new InterfaceHolder(classLoader(serviceType)), result => result.Value);
        }

        public void AddService<T>(IBundle owner, params object[] serviceInstance)
        {
            AddService(owner, typeof(T), serviceInstance);
        }

        public List<T> GetService<T>()
        {
            List<object> service = GetService(typeof(T));
            if (service == null)
            {
                return new List<T>();
            }
            return service.ConvertAll<T>((object item) => (T)((object)item));
        }

        public List<object> GetService(Type serviceType)
        {
            ServiceManager.ServiceInstancesHolder orCreateInstanceCollection = GetOrCreateInstanceCollection(serviceType);
            if (orCreateInstanceCollection == null)
            {
                return new List<object>();
            }
            return new List<object>(orCreateInstanceCollection.GetServiceInstances());
        }

        public T GetFirstOrDefaultService<T>()
        {
            object firstOrDefaultService = GetFirstOrDefaultService(typeof(T));
            if (firstOrDefaultService == null)
            {
                return default(T);
            }
            return (T)((object)firstOrDefaultService);
        }

        public void AddService(IBundle owner, object serviceInstance, params Type[] serviceTypes)
        {
            for (int i = 0; i < serviceTypes.Length; i++)
            {
                Type serviceType = serviceTypes[i];
                GetOrCreateInstanceCollection(serviceType).Add(owner, serviceInstance);
            }
        }

        public object GetFirstOrDefaultService(Type serviceType)
        {
            ServiceInstancesHolder orCreateInstanceCollection = GetOrCreateInstanceCollection(serviceType);
            if (orCreateInstanceCollection == null)
            {
                return null;
            }
            List<object> serviceInstances = orCreateInstanceCollection.GetServiceInstances();
            if (serviceInstances.Count > 0)
            {
                return serviceInstances[0];
            }
            return null;
        }

        public List<object> GetService(string serviceType)
        {
            var serviceInstancesHolder = _serviceCollection.Find(serviceType, null, result => result.Value);
            if (serviceInstancesHolder != null)
            {
                return new List<object>(serviceInstancesHolder.GetServiceInstances());
            }
            return new List<object>();
        }

        public object GetFirstOrDefaultService(string serviceType)
        {
            List<object> service = GetService(serviceType);
            if (service.Count > 0)
            {
                return service[0];
            }
            return null;
        }

        public Dictionary<Type, List<object>> GetServices()
        {
            Dictionary<Type, List<object>> services = new Dictionary<Type, List<object>>();
            _serviceCollection.ForEach(delegate (KeyValuePair<ServiceManager.InterfaceHolder, ServiceManager.ServiceInstancesHolder> item)
          {
              Type serviceType = item.Key.GetServiceType();
              List<object> serviceInstances = item.Value.GetServiceInstances();
              if (serviceInstances.Count > 0)
              {
                  services.Add(serviceType, serviceInstances);
              }
          });
            return services;
        }
    }
}