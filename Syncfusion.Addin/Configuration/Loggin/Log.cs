namespace Syncfusion.Addin.Configuration.Loggin
{
    /// <summary>
    /// Class Log.
    /// </summary>
    public abstract class Log
    {
        private volatile bool _isLogging = true;

        public virtual bool IsLogging
        {
            get
            {
                return this._isLogging;
            }
            set
            {
                this._isLogging = value;
            }
        }

        public abstract void WriteLine(string message, LogLevel level);
    }
}