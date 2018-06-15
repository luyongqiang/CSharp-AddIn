using System;

namespace Syncfusion.Addin.Configuration.Loggin
{
    /// <summary>
    /// 日志等级
    /// </summary>
    internal class LogLevelUtils
    {
        internal static bool IsValid(LogLevel @this)
        {
            return @this >= LogLevel.Fatal;
        }

        internal static LogLevel Valid(LogLevel @this, string name)
        {
            if (!LogLevelUtils.IsValid(@this))
            {
                throw new ArgumentOutOfRangeException(name, @this, Properties.Resources.ResourceManager.GetString("LogFileOverflow"));
            }
            return @this;
        }
    }
}