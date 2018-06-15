using System;

namespace Syncfusion.Addin
{
    /// <summary>
    /// Enum FrameworkState
    /// </summary>
    [Flags]
    public enum FrameworkState
    {
        /**
	     * The Framework has started.
	     *
	     * <p>
	     * This event is fired when the Framework has started after all
	     * installed bundles that are marked to be started have been started and the
	     * Framework has reached the intitial start level.
	     *
	     * <p>
	     * The owner of <code>STARTED</code> is 0x00000001.
	     *
	     * @see "<code>StartLevel</code>"
	     */
        Started = 0x00000001,

        /**
         * An error has occurred.
         *
         * <p>
         * There was an error associated with a bundle.
         *
         * <p>
         * The owner of <code>ERROR</code> is 0x00000002.
         */
        Error = 0x00000002,

        /**
         * A PackageAdmin.refreshPackage operation has completed.
         *
         * <p>
         * This event is fired when the Framework has completed the refresh
         * packages operation initiated by a call to the
         * PackageAdmin.refreshPackages method.
         *
         * <p>
         * The owner of <code>PACKAGES_REFRESHED</code> is 0x00000004.
         *
         * @since 1.2
         * @see "<code>PackageAdmin.refreshPackages</code>"
         */
        PackagesRefreshed = 0x00000004,

        /**
         * A StartLevel.setStartLevel operation has completed.
         *
         * <p>
         * This event is fired when the Framework has completed changing the
         * active start level initiated by a call to the StartLevel.setStartLevel
         * method.
         *
         * <p>
         * The owner of <code>STARTLEVEL_CHANGED</code> is 0x00000008.
         *
         * @since 1.2
         * @see "<code>StartLevel</code>"
         */
        StartLevelChanged = 0x00000008,

        /**
         * A warning has occurred.
         *
         * <p>
         * There was a warning associated with a bundle.
         *
         * <p>
         * The owner of <code>WARNING</code> is 0x00000010.
         *
         * @since 1.3
         */
        Warning = 0x00000010,

        /**
         * An informational event has occurred.
         *
         * <p>
         * There was an informational event associated with a bundle.
         *
         * <p>
         * The owner of <code>INFO</code> is 0x00000020.
         *
         * @since 1.3
         */
        Info = 0x00000020
    }
}