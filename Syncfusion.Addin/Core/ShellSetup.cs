namespace Syncfusion.Addin.Core
{
    /// <summary>
    /// Class ShellSetup.
    /// </summary>
    public class ShellSetup
    {
        private string[] _consoleArgs;

        /// <summary>
        /// Gets or sets the console arguments.
        /// </summary>
        /// <value>The console arguments.</value>
        public string[] ConsoleArgs
        {
            get
            {
                return _consoleArgs;
            }
            set
            {
                _consoleArgs = value;
            }
        }

        public ShellSetup()
        {
        }
    }
}