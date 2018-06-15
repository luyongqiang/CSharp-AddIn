using System;

namespace Syncfusion.Addin
{
    /// <summary>
    /// Enum BundleTransition
    /// </summary>
    [Flags]
    public enum BundleTransition
    {
        /**
	     * The bundle has been installed.
	     * <p>
	     * The owner of <code>INSTALLED</code> is 0x00000001.
	     *
	     * @see BundleContext#installBundle(String)
	     */
        Installed = 0x00000001,

        /**
         * The bundle has been started.
         * <p>
         * The owner of <code>STARTED</code> is 0x00000002.
         *
         * @see Bundle#start
         */
        Started = 0x00000002,

        /**
         * The bundle has been stopped.
         * <p>
         * The owner of <code>STOPPED</code> is 0x00000004.
         *
         * @see Bundle#stop
         */
        Stopped = 0x00000004,

        /**
         * The bundle has been updated.
         * <p>
         * The owner of <code>UPDATED</code> is 0x00000008.
         *
         * @see Bundle#update()
         */
        Updated = 0x00000008,

        /**
         * The bundle has been uninstalled.
         * <p>
         * The owner of <code>UNINSTALLED</code> is 0x00000010.
         *
         * @see Bundle#uninstall
         */
        Uninstalled = 0x00000010,

        /**
         * The bundle has been resolved.
         * <p>
         * The owner of <code>RESOLVED</code> is 0x00000020.
         *
         * @see Bundle#RESOLVED
         * @since 1.3
         */
        Resolved = 0x00000020,

        /**
         * The bundle has been unresolved.
         * <p>
         * The owner of <code>UNRESOLVED</code> is 0x00000040.
         *
         * @see Bundle#INSTALLED
         * @since 1.3
         */
        Unresolved = 0x00000040,

        /**
         * The bundle is about to start.
         * <p>
         * The owner of <code>STARTING</code> is 0x00000080.
         *
         * @see Bundle#start()
         * @since 1.3
         */
        Starting = 0x00000080,

        /**
         * The bundle is about to stop.
         * <p>
         * The owner of <code>STOPPING</code> is 0x00000100.
         *
         * @see Bundle#stop()
         * @since 1.3
         */
        Stopping = 0x00000100
    }
}