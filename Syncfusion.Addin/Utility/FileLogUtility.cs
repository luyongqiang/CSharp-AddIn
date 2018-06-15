using Syncfusion.Addin.Configuration.Loggin;
using System;
using System.IO;

namespace Syncfusion.Addin.Utility
{
    /// <summary>
    /// 日志信息查看
    /// </summary>
    /// <example>
    /// Using Syncfusion.Addin.Utility;
    /// FileLogUtility.Error(ex.Message);
    /// </example>
    public class FileLogUtility
    {
        //文件为10MB上限
        private const int MbToByte = 1048576;

        private static LoggerMsg _logger;
        internal static int DefaultMaxSizeMb;
        internal static int MaxFileSizeByte;
        internal static bool CreateNewOnFileMaxSize;

        public static LogLevel LogLevel
        {
            get
            {
                return _logger.Level;
            }
        }
        //System.Threading.Tasks
        /// <summary>
        /// Initializes static members of the <see cref="FileLogUtility"/> class.
        /// </summary>
        static FileLogUtility()
        {
            DefaultMaxSizeMb = 10;
            MaxFileSizeByte = DefaultMaxSizeMb * 1048576;
            CreateNewOnFileMaxSize = false;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            Log log;
            try
            {
                string arg = DateTime.Now.ToString("yyyy.MM.dd");
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{arg}.Log.log");
                log = new FileLog(filePath);
            }
            catch
            {
                log = NullLog.Instance;
            }
            _logger = new LoggerMsg("Syncfusion.Addin.Configuration.Logger", log);
            SetLogLevel(LogLevel.Debug);
        }

        /// <summary>
        /// Sets the log level.
        /// </summary>
        /// <param name="level">The level.</param>
        private static void SetLogLevel(LogLevel level)
        {
            _logger.Level = level;
        }

        /// <summary>
        /// Currents the domain unhandled exception.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                Fatal(ex.Message);
                Fatal(ex);
            }
        }

        /// <summary>
        /// 设置文件上限大小
        /// </summary>
        /// <param name="sizeMb"></param>
        public static void SetMaxFileSizeByMb(int sizeMb)
        {
            if (sizeMb <= 0)
            {
                sizeMb = DefaultMaxSizeMb;
            }
            MaxFileSizeByte = sizeMb * 1048576;
        }

        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            _logger.Debug("Debug:" + message);
            System.Diagnostics.Debug.WriteLine(message);
        }

        public static void Debug(Exception ex)
        {
            _logger.Debug("Debug:" + ex);
            System.Diagnostics.Debug.WriteLine(ex);
        }

        public static void Debug(MessageGenerator message)
        {
            _logger.Debug("Debug:" + message);
            System.Diagnostics.Debug.WriteLine(message);
        }

        public static void Error(string message)
        {
            _logger.Error("Error:" + message); System.Diagnostics.Debug.WriteLine(message);
        }

        public static void Error(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            _logger.Error("Error:" + ex);
        }

        public static void Error(MessageGenerator message)
        {
            _logger.Error("Error:" + message);
        }

        public static void SetCreateNewFileOnMaxSize(bool createNew)
        {
            CreateNewOnFileMaxSize = createNew;
        }

        private static void Fatal(Exception ex)
        {
            _logger.Fatal(ex);
        }

        private static void Fatal(string format)
        {
            _logger.Fatal(format);
        }

        public static void Inform(string message)
        {
            _logger.Inform(message);
        }

        public static void Inform(MessageGenerator message)
        {
            _logger.Inform(message);
        }

        public static void Inform(Exception ex)
        {
            _logger.Inform(ex);
        }
    }
}