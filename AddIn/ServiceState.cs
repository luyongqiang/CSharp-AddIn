using System;

namespace Syncfusion.Addin
{
    /// <summary>
    /// Enum ServiceState
    /// </summary>
    [Flags]
    public enum ServiceState
    {
        /**
	     * This service has been registered.
	     * <p>
	     * This event is synchronously delivered <strong>after</strong> the service
	     * has been registered with the Framework.
	     *
	     * <p>
	     * The owner of <code>REGISTERED</code> is 0x00000001.
	     *
	     * @see BundleContext#registerService(String[],Object,java.util.Dictionary)
	     */
        Registered = 0x00000001,

        /**
         * The properties of a registered service have been modified.
         * <p>
         * This event is synchronously delivered <strong>after</strong> the service
         * properties have been modified.
         *
         * <p>
         * The owner of <code>MODIFIED</code> is 0x00000002.
         *
         * @see ServiceRegistration#setProperties
         */
        Modified = 0x00000002,

        /**
         * This service is in the process of being unregistered.
         * <p>
         * This event is synchronously delivered <strong>before</strong> the
         * service has completed unregistering.
         *
         * <p>
         * If a bundle is using a service that is <code>UNREGISTERING</code>, the
         * bundle should release its use of the service when it receives this event.
         * If the bundle does not release its use of the service when it receives
         * this event, the Framework will automatically release the bundle's use of
         * the service while completing the service unregistration operation.
         *
         * <p>
         * The owner of UNREGISTERING is 0x00000004.
         *
         * @see ServiceRegistration#unregister
         * @see BundleContext#ungetService
         */
        Unregistering = 0x00000004
    }
}