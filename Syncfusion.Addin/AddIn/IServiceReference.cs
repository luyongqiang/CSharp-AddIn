using System;
using System.Collections.Generic;

namespace Syncfusion.Addin
{
    /// <summary>
    /// Interface IServiceReference
    /// </summary>
    public interface IServiceReference
    {
        /// <summary>
        /// 过时
        /// </summary>
        [Obsolete]
        object GetProperty(string key);

        /// <summary>
        /// 过时
        /// </summary>
        [Obsolete]
        string[] GetPropertyKeys();

        Dictionary<string, object> Properties { get; }

        /// <summary>
        /// 获取组件信息
        /// </summary>
        /// <returns>IBundle</returns>
        IBundle GetBundle();

        /// <summary>
        /// 获取使用的组件信息
        /// </summary>
        /// <returns>IBundle[]</returns>
        IBundle[] GetUsingBundles();

        /// <summary>
        /// 分配
        /// </summary>
        /// <param name="bundle">组件</param>
        /// <param name="className">类的名称</param>
        /// <returns></returns>
        bool IsAssignableTo(IBundle bundle, string className);

        /// <summary>
        /// 获取类的信息
        /// </summary>
        /// <returns>string[]</returns>
        string[] GetClasses();
    }
}