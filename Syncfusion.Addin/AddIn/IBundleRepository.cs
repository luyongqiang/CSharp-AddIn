using System;
using System.Collections.Generic;

namespace Syncfusion.Addin
{
    /// <summary>
    /// 组件库
    /// </summary>
    public interface IBundleRepository
    {
        /// <summary>
        /// 注册组件
        /// </summary>
        /// <param name="bundle">组件</param>
        void Register(IBundle bundle);

        /// <summary>
        /// 卸载组件
        /// </summary>
        /// <param name="bundle">组件</param>
        /// <returns>bool</returns>
        bool Unregister(IBundle bundle);

        /// <summary>
        /// 组件数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 根据组件编号获取组件
        /// </summary>
        /// <param name="bundleId">组件编号</param>
        /// <returns>IBundle</returns>
        IBundle GetBundle(int bundleId);

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <param name="symbolicName">名称</param>
        /// <param name="version">版本</param>
        /// <returns>IBundle</returns>
        IBundle GetBundle(string symbolicName, Version version);

        /// <summary>
        /// 获取组件列表
        /// </summary>
        /// <param name="symbolicName">名称</param>
        /// <returns>IBundle[] 数组</returns>
        IBundle[] GetBundles(string symbolicName);

        /// <summary>
        /// 获取所有组件
        /// </summary>
        /// <returns>IBundle</returns>
        IBundle[] GetBundles();

        /// <summary>
        /// 根据位置获取组件
        /// </summary>
        /// <param name="location">位置</param>
        /// <returns>IBundle</returns>
        IBundle GetBundle(string location);

        /// <summary>
        /// 当前组件编号
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        IBundle this[int index] { get; }

        /// <summary>
        /// 组件路经
        /// </summary>
        List<String> BundlePaths { get; }
    }
}