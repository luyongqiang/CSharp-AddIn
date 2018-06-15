using System;

namespace Syncfusion.Addin.Configuration.Loggin
{
    public delegate string ExceptionFormatter(Exception ex, LogLevel level);

    public delegate string MessageGenerator();

    public delegate string MessageFormatter(string userMessage, LogLevel level);

    /// <summary>
    /// Interface ILogginContext
    /// </summary>
    internal interface ILogginContext
    {
        void Error(string message);

        void Error(Exception ex);

        void Error(MessageGenerator message);

        void Fatal(Exception ex);

        void Fatal(MessageGenerator message);

        void Fatal(string message);

        void Inform(string message);

        void Inform(MessageGenerator message);

        void Inform(Exception ex);
    }

    public enum LogLevel
    {
        Fatal,
        Error,
        Warn,
        Inform,
        Debug
    }
}