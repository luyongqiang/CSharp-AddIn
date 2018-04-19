using Syncfusion.Addin.Utility;
using System;
using System.Reflection;

namespace Syncfusion.Addin
{
    /// <summary>
    /// 框架管理
    /// </summary>
    public class FrameworkUtil
    {
        /// <summary>
        /// Framework Util Class Path
        /// </summary>
        private const string FrameworkUtilClassPath = "Syncfusion.Addin.FrameworkUtil,Syncfusion.Addin.Core.Framework";

        /// <summary>
        /// create Filter
        /// </summary>
        private static MethodInfo createFilter;

        static FrameworkUtil()
        {
            Type type = Type.GetType(FrameworkUtilClassPath);
            createFilter = type.GetMethod("CreateFilter", new Type[] { typeof(string) });
        }

        /// <summary>
        /// Create Filter
        /// </summary>
        /// <param name="filter">filter String</param>
        /// <returns></returns>
        public static IFilter CreateFilter(string filter)
        {
            try
            {
                try
                {
                    return (IFilter)createFilter
                        .Invoke(null, new Object[] { filter });
                }
                catch (TargetException e)
                {
                    throw e.InnerException;
                }
            }
            catch (InvalidSyntaxException e)
            {
                FileLogUtility.Error(string.Format("{0}:{1}", e.Message, "CreateFilter(string filter)"));
            }
            catch (Exception e)
            {
                FileLogUtility.Error(string.Format("{0}:{1}", e.Message, "CreateFilter(string filter)"));
            }
            return null;
        }
    }
}