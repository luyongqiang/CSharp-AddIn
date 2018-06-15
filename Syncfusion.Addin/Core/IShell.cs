namespace Syncfusion.Addin.Core
{
    /// <summary>
    /// Interface IShell
    /// </summary>
    public interface IShell
    {
        /// <summary>
        /// Closes this instance.
        /// </summary>
        void Close();

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        IBundleContext Context { get; }

        /// <summary>
        /// Gets the framework.
        /// </summary>
        /// <value>The framework.</value>
        IFramework Framework { get; }

        /// <summary>
        /// Launches this instance.
        /// </summary>
        void Launch();

        /// <summary>
        /// Gets the setup.
        /// </summary>
        /// <value>The setup.</value>
        ShellSetup Setup { get; }

        /// <summary>
        /// Shutdowns this instance.
        /// </summary>
        void Shutdown();
    }
}