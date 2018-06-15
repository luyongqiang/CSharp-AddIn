using System;
using System.Collections.Generic;

namespace Syncfusion.Addin.Core
{
    /// <summary>
    /// Class ServiceRegistration.
    /// </summary>
    public class ServiceRegistration : IServiceRegistration
    {
        // private void
        /// <summary>
        /// The referance
        /// </summary>
        private ServiceReference referance = null;

        /// <summary>
        /// The context
        /// </summary>
        private BundleContext context = null;

        private IBundle bundle = null;

        /// <summary>
        /// The framework
        /// </summary>
        private Framework framework = null;

        /// <summary>
        /// The classes
        /// </summary>
        private string[] classes = null;

        /// <summary>
        /// The service identifier
        /// </summary>
        private int serviceId = -1;

        /// <summary>
        /// The service object
        /// </summary>
        private object serviceObject = null;

        /// <summary>
        /// The factory
        /// </summary>
        private IServiceFactory factory = null;

        private Dictionary<string, object> properties = null;

        /// <summary>
        /// The available
        /// </summary>
        private bool available;

        /// <summary>
        /// The state
        /// </summary>
        protected RegistrationState state = RegistrationState.Registered;

        /// <summary>
        /// The contexts using
        /// </summary>
        protected List<IBundleContext> contextsUsing;

        /// <summary>
        /// The registration lock
        /// </summary>
        private object registrationLock = new object();

        /// <summary>
        /// Gets the classes.
        /// </summary>
        /// <value>The classes.</value>
        public string[] Classes
        {
            get
            {
                return classes;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRegistration"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="classes">The classes.</param>
        /// <param name="serviceObject">The service object.</param>
        /// <param name="properties">The properties.</param>
        public ServiceRegistration(BundleContext context, string[] classes,
            object serviceObject, Dictionary<string, object> properties)
        {
            this.context = context;
            this.bundle = context.Bundle;
            this.framework = context.Framework;
            this.classes = classes;
            this.serviceObject = serviceObject;
            this.serviceId = framework.GetNextServiceId();
            this.referance = new ServiceReference(this, bundle);
            this.contextsUsing = null;
            this.factory = serviceObject as IServiceFactory;
            available = true;
            InitializeProperties(properties);

            framework.ServiceRegistry.PublishService(context, this);

            EventManager.OnServiceChanged(new ServiceEventArgs(ServiceState.Registered, referance));
        }

        /// <summary>
        /// Initializes the properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void InitializeProperties(Dictionary<string, object> properties)
        {
            if (properties != null)
            {
                this.properties = properties;
            }
            else
            {
                this.properties = new Dictionary<string, object>();
            }
        }

        #region IServiceRegistration Members

        /// <summary>
        /// 得到引用
        /// </summary>
        /// <returns>IServiceReference</returns>
        /// <exception cref="System.NotSupportedException">Service is unregistered</exception>
        public IServiceReference GetReference()
        {
            if (referance != null)
            {
                return referance;
            }
            else
            {
                throw new NotSupportedException("Service is unregistered");
            }
        }

        /// <summary>
        /// 属性信息
        /// </summary>
        /// <value>The properties.</value>
        /// <exception cref="System.NotSupportedException">Service is unregistered.</exception>
        public Dictionary<string, object> Properties
        {
            get
            {
                lock (registrationLock)
                {
                    lock (properties)
                    {
                        return properties;
                    }
                }
            }
            set
            {
                lock (registrationLock)
                {
                    lock (properties)
                    {
                        if (available)
                        {
                            properties = value;
                        }
                        else
                        {
                            throw new NotSupportedException("Service is unregistered.");
                        }
                    }

                    EventManager.OnServiceChanged(new ServiceEventArgs(ServiceState.Modified, referance));
                }
            }
        }

        /// <summary>
        /// 卸载
        /// </summary>
        /// <exception cref="System.NotSupportedException">Service is unregistered</exception>
        public void Unregister()
        {
            lock (properties)
            {
                if (available)
                {
                    if (null != bundle)
                    {
                        //bundle.Framework.Bundles
                        framework.ServiceRegistry.UnpublishService(context, this);

                        EventManager.OnServiceChanged(new ServiceEventArgs(ServiceState.Unregistering, referance));
                    }
                    available = false;
                }
                else
                {
                    throw new NotSupportedException("Service is unregistered");
                }

                contextsUsing = null;

                referance = null;
                context = null;
            }
        }

        #endregion IServiceRegistration Members

        /// <summary>
        /// Gets the using bundles.
        /// </summary>
        /// <returns>IBundle[].</returns>
        public IBundle[] GetUsingBundles()
        {
            lock (registrationLock)
            {
                if (state == RegistrationState.Unregistered) /* service unregistered */
                {
                    return (null);
                }

                if (contextsUsing == null)
                {
                    return (null);
                }

                int size = contextsUsing.Count;
                if (size == 0)
                {
                    return (null);
                }

                /* Copy list of BundleContext into an array of Bundle. */
                IBundle[] bundles = new IBundle[size];
                for (int i = 0; i < size; i++)
                {
                    bundles[i] = ((BundleContext)contextsUsing[i]).Bundle;
                }

                return bundles;
            }
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>System.Object.</returns>
        public object GetService(BundleContext user)
        {
            lock (registrationLock)
            {
                if (state == RegistrationState.Unregistered)
                {
                    return null;
                }

                Dictionary<ServiceReference, ServiceRegistration> servicesInUse = user.ServicesInUse;

                ServiceRegistration serviceRegistration = null;
                if (servicesInUse.ContainsKey(referance))
                {
                    serviceRegistration = servicesInUse[referance];
                }

                if (serviceRegistration == null)
                {
                    serviceRegistration = this;

                    object service = serviceRegistration.serviceObject;

                    if (service != null)
                    {
                        servicesInUse.Add(referance, serviceRegistration);

                        if (contextsUsing == null)
                        {
                            contextsUsing = new List<IBundleContext>();
                        }

                        contextsUsing.Add(user);
                    }

                    return service;
                }
                else
                {
                    object service = null;
                    if (contextsUsing.Contains(user))
                    {
                        if (serviceRegistration.available)
                        {
                            service = serviceRegistration.serviceObject;
                        }
                    }
                    return service;
                }
            }
        }

        /// <summary>
        /// Ungets the service.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool UngetService(BundleContext user)
        {
            lock (registrationLock)
            {
                if (state == RegistrationState.Unregistered)
                {
                    return false;
                }

                Dictionary<ServiceReference, ServiceRegistration> servicesInUse = user.ServicesInUse;

                if (servicesInUse != null)
                {
                    ServiceRegistration serviceRegistration = servicesInUse[referance];

                    if (serviceRegistration != null)
                    {
                        serviceRegistration.Unregister();

                        #region 注释原无效注销代码

                        //  if (UngetServiceInternal())
                        //     {
                        /* use count is now zero */
                        //  servicesInUse.Remove(referance);

                        //contextsUsing.Remove(user);
                        //   }

                        #endregion 注释原无效注销代码

                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Releases the service.
        /// </summary>
        /// <param name="user">The user.</param>
        public void ReleaseService(BundleContext user)
        {
            lock (registrationLock)
            {
                if (referance == null)
                {
                    return;
                }

                Dictionary<ServiceReference, ServiceRegistration> servicesInUse = user.ServicesInUse;

                if (servicesInUse != null)
                {
                    servicesInUse.Remove(referance);
                    UngetServiceInternal();

                    if (contextsUsing != null)
                    {
                        contextsUsing.Remove(user);
                    }
                }
            }
        }

        /// <summary>
        /// Ungets the service internal.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool UngetServiceInternal()
        {
            factory.UngetService(bundle, this, serviceObject);
            return true;
        }
    }
}