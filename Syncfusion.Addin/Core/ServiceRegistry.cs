using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Addin.Core
{
    /// <summary>
    /// Class ServiceRegistry.
    /// </summary>
    public class ServiceRegistry : IServiceRegistry
    {
        /** Published services by class name. Key is a String class name; Value is a ArrayList of ServiceRegistrations */
        protected Dictionary<string, List<IServiceRegistration>> PublishedServicesByClass;
        /** All published services. Value is ServiceRegistrations */
        protected List<IServiceRegistration> AllPublishedServices;
        /** Published services by BundleContext.  Key is a BundleContext; Value is a ArrayList of ServiceRegistrations*/
        protected Dictionary<IBundleContext, List<IServiceRegistration>> PublishedServicesByContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRegistry"/> class.
        /// </summary>
        public ServiceRegistry()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            PublishedServicesByClass = new Dictionary<string, List<IServiceRegistration>>();
            AllPublishedServices = new List<IServiceRegistration>();
            PublishedServicesByContext = new Dictionary<IBundleContext, List<IServiceRegistration>>();
        }

        #region IServiceRegistry Members

        /// <summary>
        /// Publishes the service.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="serviceRegistration">The service registration.</param>
        /// Publishes a service to this ServiceRegistry.
        /// @param context the BundleContext that registered the service.
        /// @param serviceReg the ServiceRegistration to register.
        public void PublishService(IBundleContext context, IServiceRegistration serviceRegistration)
        {
            // Add the ServiceRegistration to the list of Services published by BundleContext.
            List<IServiceRegistration> contextServices = null;
            if (PublishedServicesByContext.ContainsKey(context))
            {
                contextServices = (List<IServiceRegistration>)PublishedServicesByContext[context];
            }
            if (contextServices == null)
            {
                contextServices = new List<IServiceRegistration>();
                PublishedServicesByContext.Add(context, contextServices);
            }
            contextServices.Add(serviceRegistration);

            // Add the ServiceRegistration to the list of Services published by Class Name.
            string[] clazzes = ((ServiceRegistration)serviceRegistration).Classes;
            int size = clazzes.Length;

            for (int i = 0; i < size; i++)
            {
                string clazz = clazzes[i];

                List<IServiceRegistration> services = null;
                if (PublishedServicesByClass.ContainsKey(clazz))
                {
                    services = (List<IServiceRegistration>)PublishedServicesByClass[clazz];
                }

                if (services == null)
                {
                    services = new List<IServiceRegistration>();
                    PublishedServicesByClass.Add(clazz, services);
                }

                services.Add(serviceRegistration);
            }

            // Add the ServiceRegistration to the list of all published Services.
            AllPublishedServices.Add(serviceRegistration);
        }

        /// <summary>
        /// Unpublishes the service.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="serviceRegistration">The service registration.</param>
        /// Unpublishes a service from this ServiceRegistry
        /// @param context the BundleContext that registered the service.
        /// @param serviceReg the ServiceRegistration to unpublish.
        public void UnpublishService(IBundleContext context, IServiceRegistration serviceRegistration)
        {
            // Remove the ServiceRegistration from the list of Services published by BundleContext.
            List<IServiceRegistration> contextServices = (List<IServiceRegistration>)PublishedServicesByContext[context];
            if (contextServices != null)
            {
                contextServices.Remove(serviceRegistration);
            }

            // Remove the ServiceRegistration from the list of Services published by Class Name.
            string[] clazzes = ((ServiceRegistration)serviceRegistration).Classes;
            int size = clazzes.Length;

            for (int i = 0; i < size; i++)
            {
                string clazz = clazzes[i];
                List<IServiceRegistration> services = (List<IServiceRegistration>)PublishedServicesByClass[clazz];
                services.Remove(serviceRegistration);
            }

            // Remove the ServiceRegistration from the list of all published Services.
            AllPublishedServices.Remove(serviceRegistration);
        }

        /// <summary>
        /// Unpublishes the services.
        /// </summary>
        /// <param name="context">The context.</param>
        /// Unpublishes all services from this ServiceRegistry that the
        /// specified BundleContext registered.
        /// @param context the BundleContext to unpublish all services for.
        public void UnpublishServices(IBundleContext context)
        {
            // Get all the Services published by the BundleContext.
            List<IServiceRegistration> serviceRegs = (List<IServiceRegistration>)PublishedServicesByContext[context];
            if (serviceRegs != null)
            {
                // Remove this list for the BundleContext
                PublishedServicesByContext.Remove(context);
                int size = serviceRegs.Count();
                for (int i = 0; i < size; i++)
                {
                    IServiceRegistration serviceReg = (IServiceRegistration)serviceRegs[i];
                    // Remove each service from the list of all published Services
                    AllPublishedServices.Remove(serviceReg);

                    // Remove each service from the list of Services published by Class Name.
                    string[] clazzes = ((ServiceRegistration)serviceReg).Classes;
                    int numclazzes = clazzes.Length;

                    for (int j = 0; j < numclazzes; j++)
                    {
                        string clazz = clazzes[j];
                        if (PublishedServicesByClass.ContainsKey(clazz))
                        {
                            List<IServiceRegistration> services = (List<IServiceRegistration>)PublishedServicesByClass[clazz];
                            services.Remove(serviceReg);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Lookups the service references.
        /// </summary>
        /// <param name="clazz">The clazz.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>IServiceReference[].</returns>
        /// Performs a lookup for ServiceReferences that are bound to this
        /// ServiceRegistry. If both clazz and filter are null then all bound
        /// ServiceReferences are returned.
        /// @param clazz A fully qualified class name.  All ServiceReferences that
        /// reference an object that implement this class are returned.  May be
        /// null.
        /// @param filter Used to match against published Services.  All
        /// ServiceReferences that match the filter are returned.  If a clazz is
        /// specified then all ServiceReferences that match the clazz and the
        /// filter parameter are returned. May be null.
        /// @return An array of all matching ServiceReferences or null
        /// if none exist.
        public IServiceReference[] LookupServiceReferences(string clazz, IFilter filter)
        {
            int size;
            List<IServiceReference> references = null;
            List<IServiceRegistration> serviceRegs = null;
            if (clazz == null) /* all services */
            {
                serviceRegs = AllPublishedServices;
            }
            else
            {
                /* services registered under the class name */
                if (PublishedServicesByClass.ContainsKey(clazz))
                {
                    serviceRegs = (List<IServiceRegistration>)PublishedServicesByClass[clazz];
                }
            }

            if (serviceRegs == null)
                return (null);

            size = serviceRegs.Count;

            if (size == 0)
                return (null);

            references = new List<IServiceReference>();
            for (int i = 0; i < size; i++)
            {
                IServiceRegistration registration = (IServiceRegistration)serviceRegs[i];

                IServiceReference reference = registration.GetReference();
                if ((filter == null) || filter.Match(reference))
                {
                    references.Add(reference);
                }
            }

            if (references.Count == 0)
            {
                return null;
            }

            return (IServiceReference[])references.ToArray();
        }

        /// <summary>
        /// Lookups the service references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>IServiceReference[].</returns>
        /// Performs a lookup for ServiceReferences that are bound to this
        /// ServiceRegistry using the specified BundleContext.
        /// @param context The BundleContext to lookup the ServiceReferences on.
        /// @return An array of all matching ServiceReferences or null if none
        /// exist.
        public IServiceReference[] LookupServiceReferences(IBundleContext context)
        {
            int size;
            List<IServiceReference> references;
            List<IServiceRegistration> serviceRegs = (List<IServiceRegistration>)PublishedServicesByContext[context];

            if (serviceRegs == null)
            {
                return (null);
            }

            size = serviceRegs.Count;

            if (size == 0)
            {
                return (null);
            }

            references = new List<IServiceReference>();
            for (int i = 0; i < size; i++)
            {
                IServiceRegistration registration = (IServiceRegistration)serviceRegs[i];

                IServiceReference reference = registration.GetReference();
                references.Add(reference);
            }

            if (references.Count == 0)
            {
                return null;
            }

            return (IServiceReference[])references.ToArray();
        }

        #endregion IServiceRegistry Members
    }
}