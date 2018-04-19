using System;
using System.Collections.Generic;

namespace Syncfusion.Addin
{
    /// <summary>
    /// 过滤
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// 匹配
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <returns>Bool</returns>
        bool Match(IServiceReference reference);

        /// <summary>
        /// 匹配
        /// </summary>
        /// <param name="dictionary">字典信息</param>
        /// <returns>Bool</returns>
        bool Match(Dictionary<string, object> dictionary);

        /// <summary>
        /// 匹配  区分大小写
        /// </summary>
        /// <param name="dictionary">字典信息</param>
        /// <returns>Bool</returns>
        bool MatchCase(Dictionary<string, object> dictionary);

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>string</returns>
        string ToString();

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj">基类</param>
        /// <returns>bool</returns>
        bool Equals(Object obj);

        /// <summary>
        /// 获取Hash编号
        /// </summary>
        /// <returns>int</returns>
        int GetHashCode();
    }
}