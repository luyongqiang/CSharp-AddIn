namespace Syncfusion.Addin.Core
{
    /// <summary>
    /// Interface IServiceRegistry
    /// </summary>
    public interface IServiceRegistry
    {
        /**
	     * Publishes a service to this ServiceRegistry.
	     * @param context the BundleContext that registered the service.
	     * @param serviceReg the ServiceRegistration to register.
	     */

        /// <summary>
        /// Publishes the service.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="serviceRegistration">The service registration.</param>
        void PublishService(IBundleContext context, IServiceRegistration serviceRegistration);

        /**
         * Unpublishes a service from this ServiceRegistry
         * @param context the BundleContext that registered the service.
         * @param serviceReg the ServiceRegistration to unpublish.
         */

        /// <summary>
        /// Unpublishes the service.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="serviceRegistration">The service registration.</param>
        void UnpublishService(IBundleContext context, IServiceRegistration serviceRegistration);

        /**
         * Unpublishes all services from this ServiceRegistry that the
         * specified BundleContext registered.
         * @param context the BundleContext to unpublish all services for.
         */

        /// <summary>
        /// Unpublishes the services.
        /// </summary>
        /// <param name="context">The context.</param>
        void UnpublishServices(IBundleContext context);

        /**
         * Performs a lookup for ServiceReferences that are bound to this
         * ServiceRegistry. If both clazz and filter are null then all bound
         * ServiceReferences are returned.
         * @param clazz A fully qualified class name.  All ServiceReferences that
         * reference an object that implement this class are returned.  May be
         * null.
         * @param filter Used to match against published Services.  All
         * ServiceReferences that match the filter are returned.  If a clazz is
         * specified then all ServiceReferences that match the clazz and the
         * filter parameter are returned. May be null.
         * @return An array of all matching ServiceReferences or null
         * if none exist.
         */

        /// <summary>
        /// Lookups the service references.
        /// </summary>
        /// <param name="clazz">The clazz.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>IServiceReference[].</returns>
        IServiceReference[] LookupServiceReferences(string clazz, IFilter filter);

        /**
         * Performs a lookup for ServiceReferences that are bound to this
         * ServiceRegistry using the specified BundleContext.
         * @param context The BundleContext to lookup the ServiceReferences on.
         * @return An array of all matching ServiceReferences or null if none
         * exist.
         */

        /// <summary>
        /// Lookups the service references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>IServiceReference[].</returns>
        IServiceReference[] LookupServiceReferences(IBundleContext context);
    }
}