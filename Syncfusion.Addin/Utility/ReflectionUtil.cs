using Syncfusion.Addin.Core.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Syncfusion.Addin.Utility
{
    /// <summary>
    /// Class ReflectionUtil.
    /// </summary>
    public static class ReflectionUtil
    {
        // Methods

        /// <summary>
        /// 获取程序目录
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns>文件路经</returns>
        public static string GetAssemblyFilename(Assembly assembly)
        {
            return assembly.CodeBase.Replace("file:///", "").Replace('/', '\\');
        }

        /// <summary>
        /// 获取程序的版本信息
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns>返回版本号   如：1.0.0.0</returns>
        public static string GetAssemblyVersion(Assembly assembly)
        {
            foreach (string part in assembly.FullName.Split(','))
            {
                string trimmed = part.Trim();

                if (trimmed.StartsWith("Version="))
                    return trimmed.Substring(8);
            }

            return "0.0.0.0";
        }

        /// <summary>
        /// 获取自定义属性信息
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="attribute">类型</param>
        /// <returns>AttributeInfo[] 数组</returns>
        public static AttributeInfo[] GetCustomAttributes(Assembly assembly, Type attribute)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            HashSet<AttributeInfo> attributes = new HashSet<AttributeInfo>();
            foreach (Type type in assembly.GetExportedTypes())
            {
                foreach (Attribute attr in type.GetCustomAttributes(attribute, true))
                {
                    AttributeInfo attributeInfo = new AttributeInfo(type, attr);
                    attributes.Add(attributeInfo);
                }
            }
            AttributeInfo[] result = new AttributeInfo[attributes.Count];
            attributes.CopyTo(result);
            return result;
        }
    }
}