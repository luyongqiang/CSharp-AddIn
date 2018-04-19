using System;

namespace Syncfusion.Addin
{
    /// <summary>
    /// Defines the Bundle's states.
    /// </summary>
    [Flags]
    public enum BundleState
    {
        /// <summary>
        /// Uninstalled
        /// </summary>
        Uninstalled = 0x00000001,

        /// <summary>
        /// Installed
        /// </summary>
        Installed = 0x00000002,

        /// <summary>
        /// Resolved
        /// </summary>
        Resolved = 0x00000004,

        /// <summary>
        /// Starting
        /// </summary>
        Starting = 0x00000008,

        /// <summary>
        /// Stopping
        /// </summary>
        Stopping = 0x00000010,

        /// <summary>
        /// Active
        /// </summary>
        Active = 0x00000020
    }
}