using Syncfusion.Addin.Properties;
using Syncfusion.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Syncfusion.Addin.Core
{
    /// <summary>
    /// IBundleContext implementation.
    /// </summary>
    public class BundleContext : IBundleContext
    {
        #region --- Fields ---

        private Bundle bundle;

        //private DirectoryInfo storage;
        private Framework framework;

        private Dictionary<ServiceReference, ServiceRegistration> servicesInUse;

        #endregion --- Fields ---

        #region --- Events ---

        public event BundleEventHandler BundleEvent
        {
            add
            {
                EventManager.BundleEvent += value;
            }
            remove
            {
                EventManager.BundleEvent -= value;
            }
        }

        public event ServiceEventHandler ServiceEvent
        {
            add
            {
                EventManager.ServiceEvent += value;
            }
            remove
            {
                EventManager.ServiceEvent -= value;
            }
        }

        public event FrameworkEventHandler FrameworkEvent
        {
            add
            {
                EventManager.FrameworkEvent += value;
            }
            remove
            {
                EventManager.FrameworkEvent -= value;
            }
        }

        #endregion --- Events ---



        #region --- Properties ---

        public IBundle Bundle
        {
            get
            {
                return bundle;
            }
        }

        public IBundle[] Bundles
        {
            get
            {
                Bundle[] bundles = new Bundle[framework.Bundles.Count];
                framework.Bundles.GetBundles().CopyTo(bundles, 0);
                return bundles;
            }
        }

        public Framework Framework
        {
            get
            {
                return framework;
            }
        }

        public Dictionary<ServiceReference, ServiceRegistration> ServicesInUse
        {
            get
            {
                return servicesInUse;
            }
        }

        #endregion --- Properties ---

        /// <summary>
        /// Initializes a new instance of the <see cref="BundleContext"/> class.
        /// </summary>
        /// <param name="bundle">The bundle.</param>
        internal BundleContext(Bundle bundle)
        {
            this.bundle = bundle;
            //this.storage = storage;
            this.framework = bundle.Framework;
            this.servicesInUse = new Dictionary<ServiceReference, ServiceRegistration>();
        }

        /// <summary>
        /// 获取组件ID
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IBundle.</returns>
        public IBundle GetBundle(int id)
        {
            return framework.Bundles.GetBundle(id);
        }

        /// <summary>
        /// Gets the data file.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>FileSystemInfo.</returns>
        /// <exception cref="System.InvalidOperationException">The bundle has stopped</exception>
        public FileSystemInfo GetDataFile(string name)
        {
            if (bundle.State == BundleState.Installed)
            {
                throw new InvalidOperationException("The bundle has stopped");
            }
            else
            {
                string path = bundle.Location + bundle.Assembly.Location;

                //if (!name.StartsWith(Path.DirectorySeparatorChar.ToString()))
                //{
                //    path += Path.DirectorySeparatorChar;
                //}

                //path += name;

                return new FileInfo(path);
            }
        }

        #region IBundleContext Members

        public string GetProperty(string key)
        {
            try
            {
                return bundle.Assembly.GetPropertyValue<string>(key);
            }
            catch
            {
                return "";
            }
        }

        public IBundle Install(string location)
        {
            return framework.InstallBundle(location);
        }

        public IBundle Install(string location, Stream inputStream)
        {
            return framework.InstallBundle(location);
        }

        public IServiceRegistration RegisterService(string[] clazzes, object service, Dictionary<string, object> properties)
        {
            if (service == null)
            {
#if DEBUG
                Debug.Write("Service object is null");
#endif
                throw new ArgumentNullException("service", Resources.BundleContext_RegisterService_SERVICE_ARGUMENT_NULL_EXCEPTION);
            }

            int size = clazzes.Length;

            if (size == 0)
            {
                //if (Debug
                throw new ArgumentException("Classes array is empty");
            }

            /* copy the array so that changes to the original will not affect us. */
            string[] copy = new string[clazzes.Length];
            // doing this the hard way so we can intern the strings
            for (int i = clazzes.Length - 1; i >= 0; i--)
            {
                copy[i] = (string)clazzes[i].Clone();
            }
            clazzes = copy;

            if (!(service is IServiceFactory))
            {
                string invalidService = CheckServiceClass(clazzes, service);
                if (!string.IsNullOrEmpty(invalidService))
                {
                    //if (Debug.DEBUG && Debug.DEBUG_SERVICES) {
                    //    Debug.println("Service object is not an instanceof " + invalidService); //$NON-NLS-1$
                    //}
                    throw new ArgumentException(Resources.BundleContext_RegisterService_Service_is_not_instance_of_class, "clazzes");
                }
            }

            return (CreateServiceRegistration(clazzes, service, properties));
        }

        public IServiceRegistration RegisterService(string clazz, object service, Dictionary<string, object> properties)
        {
            String[] clazzes = new String[] { clazz };

            return (RegisterService(clazzes, service, properties));
        }

        public IServiceRegistration RegisterService(Type type, object service, Dictionary<string, object> properties)
        {
            return RegisterService(type.FullName, service, properties);
        }

        public IServiceRegistration RegisterService<T>(object service, Dictionary<string, object> properties)
        {
            return RegisterService(typeof(T), service, properties);
        }

        public IServiceReference[] GetServiceReferences(string clazz, string filter)
        {
            return framework.GetServiceReferences(clazz, null, this, false);
        }

        public IServiceReference[] GetAllServiceReferences(string clazz, string filter)
        {
            return framework.GetServiceReferences(clazz, filter, this, true);
        }

        public IServiceReference GetServiceReference(string clazz)
        {
            try
            {
                IServiceReference[] references = GetServiceReferences(clazz, null);
                /* if more than one service, select highest ranking */
                if (references != null)
                {
                    int index = 0;
                    int length = references.Length;

                    //if (length > 1)
                    //  {
                    //index =
                    // }

                    return references[index];
                }
            }
            catch (InvalidSyntaxException)
            {
                throw;
            }

            return null;
        }

        public object GetRegisterService(IServiceReference reference)
        {
            ServiceRegistration registration = ((ServiceReference)reference).Registration;
            return registration.GetService(this);
        }

        public object GetRegisterService(string clazz)
        {
            IServiceReference reference = GetServiceReference(clazz);
            if (reference != null)
            {
                return GetRegisterService(reference);
            }

            return null;
        }

        public object GetRegisterService(Type type)
        {
            return GetRegisterService(type.FullName);
        }

        public T GetRegisterService<T>()
        {
            return (T)GetRegisterService(typeof(T));
        }

        public bool UngetService(IServiceReference reference)
        {
            ServiceRegistration registration = ((ServiceReference)reference).Registration;

            return registration.UngetService(this);
        }

        #region 服务实现

        //public void AddService(Type serviceType, params object[] serviceInstances)
        //{
        //    this.Framework.AddSystemService(serviceType, new object[]
        //    {
        //        serviceInstances
        //    });
        //}

        //public void AddService<T>( params object[] serviceInstances)
        //{
        //    this.Framework.AddSystemService(typeof(T), new object[]
        //    {
        //        serviceInstances
        //    });
        //}

        //public void AddService(object serviceInstance, params Type[] serviceTypes)
        //{
        //    this.Framework.AddSystemService(serviceInstance,serviceTypes);
        //}

        //public void AddService(Type serviceInterface, object serviceInstance)
        //{
        //    this.framework.AddSystemService(serviceInterface,new object[]
        //    {
        //        serviceInstance
        //    });
        //}

        //public void AddService<TServiceInterface>(object serviceInstance)
        //{
        //    this.Framework.AddSystemService(typeof(TServiceInterface), new object[]
        //    {
        //        serviceInstance
        //    });
        //}

        //public void RemoveService(Type serviceType, object serviceInstance)
        //{
        //    this.Framework.RemoveSystemService(serviceType, serviceInstance);
        //}

        //public void RemoveService<T>(object serviceInstance)
        //{
        //   this.Framework.RemoveSystemService(typeof(T), serviceInstance);

        //}

        //public void RemoveService(object serviceInstance)
        //{
        //    if (this.Framework.ServiceContainer != null)
        //    {
        //      Framework.ServiceContainer.RemoveService(this.bundle,serviceInstance);
        //    }
        //}

        //public T GetFirstOrDefaultService<T>()
        //{
        //    if (this.Framework.ServiceContainer != null)
        //    {
        //        return this.Framework.ServiceContainer .GetFirstOrDefaultService<T>();
        //    }
        //    return default(T);
        //}

        //public object GetFirstOrDefaultService(Type serviceType)
        //{
        //    if (this.Framework.ServiceContainer != null)
        //    {
        //        return this.Framework.ServiceContainer.GetFirstOrDefaultService(serviceType);
        //    }
        //    return null;
        //}

        //public object GetFirstOrDefaultService(string serviceTypeName)
        //{
        //    if (this.framework.ServiceContainer != null)
        //    {
        //        return this.framework.ServiceContainer.GetFirstOrDefaultService(serviceTypeName);
        //    }
        //    return null;
        //}

        #endregion 服务实现

        public IFilter CreateFilter(string filter)
        {
            return new Filter(filter);
        }

        #endregion IBundleContext Members

        /// <summary>
        /// Creates the service registration.
        /// </summary>
        /// <param name="clazzes">The clazzes.</param>
        /// <param name="service">The service.</param>
        /// <param name="properties">The properties.</param>
        /// <returns>IServiceRegistration.</returns>
        private IServiceRegistration CreateServiceRegistration(string[] clazzes, object service, Dictionary<string, object> properties)
        {
            return (new ServiceRegistration(this, clazzes, service, properties));
        }

        /// <summary>
        /// Checks the service class.
        /// </summary>
        /// <param name="clazzes">The clazzes.</param>
        /// <param name="serviceObject">The service object.</param>
        /// <returns>System.String.</returns>
        private static string CheckServiceClass(string[] clazzes, object serviceObject)
        {
            Type serviceClass = serviceObject.GetType();

            for (int i = 0; i < clazzes.Length; i++)
            {
                try
                {
                    Type serviceClazz = Type.GetType(clazzes[i]);
                    if (!serviceClass.IsInstanceOfType(serviceObject))
                    {
                        return clazzes[i];
                    }
                }
                catch (TypeLoadException ex)
                {
                    return ex.TypeName;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 匹配
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <returns>bool</returns>
        public bool IsAssignableTo(IServiceReference reference)
        {
            //if (!scopeEvents)
            //    return true;
            string[] clazzes = reference.GetClasses();
            for (int i = 0; i < clazzes.Length; i++)
                if (!reference.IsAssignableTo(bundle, clazzes[i]))
                    return false;
            return true;
        }
    }
}