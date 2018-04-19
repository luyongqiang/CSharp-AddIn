using System;

namespace Syncfusion.Addin.Configuration.Loggin
{
    /// <summary>
    /// Class LoggerMsg.
    /// </summary>
    internal class LoggerMsg : ILogginContext
    {
        private Log _log;

        public MessageFormatter MessageFormatter { get; set; }

        public ExceptionFormatter ExceptionFormatter { get; set; }

        public Log LogMessage
        {
            get { return _log; }
            set { _log = value; }
        }

        private LogLevel _level;

        public LoggerMsg(string name, Log log)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            _log = log;
        }

        public LogLevel Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public void Debug(string message)
        {
            Log(message, LogLevel.Debug);
        }

        public void Debug(Exception ex)
        {
            Log(ex, LogLevel.Debug);
        }

        public void Debug(MessageGenerator message)
        {
            Log(message, LogLevel.Debug);
        }

        public void Error(string message)
        {
            Log(message, LogLevel.Debug);
        }

        public void Error(Exception ex)
        {
            Log(ex, LogLevel.Debug);
        }

        public void Error(MessageGenerator message)
        {
            Log(message, LogLevel.Debug);
        }

        public void Fatal(Exception ex)
        {
            Log(ex, LogLevel.Debug);
        }

        public void Fatal(MessageGenerator message)
        {
            Log(message, LogLevel.Debug);
        }

        public void Fatal(string message)
        {
            Log(message, LogLevel.Debug);
        }

        public void Inform(string message)
        {
            Log(message, LogLevel.Debug);
        }

        public void Inform(MessageGenerator message)
        {
            Log(message, LogLevel.Debug);
        }

        public void Inform(Exception ex)
        {
            Log(ex, LogLevel.Debug);
        }

        public bool WillLogAt(LogLevel queryLevel)
        {
            return _log.IsLogging && LogLevelUtils.IsValid(queryLevel) && _level >= queryLevel;
        }

        public void Log(string message, LogLevel messageLevel)
        {
            if (WillLogAt(messageLevel))
            {
                _log.WriteLine(Format(message, messageLevel), messageLevel);
            }
        }

        public void Log(MessageGenerator message, LogLevel messageLevel)
        {
            if (WillLogAt(messageLevel))
            {
                string message2 = string.Empty;
                if (message != null)
                {
                    message2 = message();
                }
                _log.WriteLine(Format(message2, messageLevel), messageLevel);
            }
        }

        public void Log(Exception ex, LogLevel exceptionLevel)
        {
            if (WillLogAt(exceptionLevel))
            {
                _log.WriteLine(Format(ex, exceptionLevel), exceptionLevel);
            }
        }

        private string Format(string message, LogLevel messageLevel)
        {
            return string.Format(message, messageLevel);
        }

        private string Format(Exception ex, LogLevel exceptionLevel)
        {
            return string.Format(ex.Message, exceptionLevel);
        }
    }
}