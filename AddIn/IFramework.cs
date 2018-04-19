using Syncfusion.Addin.Configuration.Plugin;
using Syncfusion.Addin.Core;
using System;

namespace Syncfusion.Addin
{
    /// <summary>
    /// Interface IFramework
    /// </summary>
    public interface IFramework
    {
        ServiceRegistry ServiceRegistry { get; }

        /// <summary>
        /// 组件信息
        /// </summary>
        IBundleRepository Bundles { get; }

        BundleContext SystemBundleContext { get; }

        //关闭
        void Close();

        //应用程序域
        AppDomain CreateDomain(IBundleContext context);

        ////服务管理
        //IServiceManager ServiceContainer
        //{
        //    get;
        //    set;
        //}

        event FrameworkEventHandler FrameworkEvent;

        /// <summary>
        /// 返回服务编号
        /// </summary>
        /// <returns>Int</returns>
        int GetNextServiceId();

        /// <summary>
        /// 获取服务引用
        /// </summary>
        /// <param name="clazz">类</param>
        /// <param name="filterString">过滤字符串</param>
        /// <param name="context">组件</param>
        /// <param name="allservices">是否所有服务</param>
        /// <returns>IServiceReference[]</returns>
        IServiceReference[] GetServiceReferences(string clazz, string filterString, IBundleContext context, bool allservices);

        /// <summary>
        /// 注册组件
        /// </summary>
        /// <param name="location">位置</param>
        /// <returns>IBundle</returns>
        IBundle InstallBundle(string location);

        /// <summary>
        /// 注册组件
        /// </summary>
        /// <param name="location">位置</param>
        /// <param name="bd">组件数据</param>
        /// <returns>IBundle</returns>
        IBundle InstallBundle(string location, BundleData bd);

        /// <summary>
        /// 加载
        /// </summary>
        void Launch();

        /// <summary>
        /// 关闭/卸载
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="id">组件编号</param>
        /// <returns>IBundle</returns>
        IBundle StartBundle(int id);

        /// <summary>
        /// 启动组件
        /// </summary>
        /// <param name="bundle">组件</param>
        void StartBundle(IBundle bundle);

        /// <summary>
        /// 停止组件根据编号
        /// </summary>
        /// <param name="id">组件编号</param>
        void StopBundle(int id);

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <param name="location">位置</param>
        /// <returns></returns>
        IBundle GetBundle(string location);

        /// <summary>
        /// 卸载组件
        /// </summary>
        /// <param name="id">组件编号</param>
        void UninstallBundle(int id);

        ///// <summary>
        ///// 删除服务
        ///// </summary>
        ///// <param name="serviceType">服务类型<param>
        ///// <param name="serviceInstance">服务实例</param>
        //void RemoveSystemService(Type serviceType, object serviceInstance);
        ///// <summary>
        ///// 添加服务
        ///// </summary>
        ///// <param name="serviceInstance">服务实例</param>
        ///// <param name="serviceTypes">服务类型</param>
        //void AddSystemService(object serviceInstance, params Type[] serviceTypes);

        ///// <summary>
        ///// 添加服务
        ///// </summary>
        ///// <param name="serviceType">服务类型<param>
        ///// <param name="serviceInstance">多个服务实例</param>
        //void AddSystemService(Type serviceType, params object[] serviceInstances);
    }
}