namespace Syncfusion.Addin.Configuration.Loggin
{
    /// <summary>
    /// Class NullLog.
    /// </summary>
    internal class NullLog : Log
    {
        public static readonly NullLog Instance = new NullLog();

        public override bool IsLogging
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        private NullLog()
        {
        }

        public override void WriteLine(string message, LogLevel level)
        {
        }
    }
}