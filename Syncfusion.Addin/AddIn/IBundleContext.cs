using System;
using System.Collections.Generic;
using System.IO;

namespace Syncfusion.Addin
{
    /// <summary>
    /// 组件环境
    /// </summary>
    public interface IBundleContext
    {
        /// <summary>
        /// Event raised when a bundle transition occurs.
        /// </summary>
        event BundleEventHandler BundleEvent;

        /// <summary>
        /// 服务事件
        /// </summary>
        event ServiceEventHandler ServiceEvent;

        /// <summary>
        /// 框架事件
        /// </summary>
        event FrameworkEventHandler FrameworkEvent;

        /// <summary>
        /// 根据key获取属性
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        string GetProperty(string key);

        /// <summary>
        /// 组件对象
        /// </summary>
        IBundle Bundle { get; }

        /// <summary>
        /// 根据组件文件路径安装组件
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        IBundle Install(string location);

        /// <summary>
        /// 根据组件路径及内存信息安装组件
        /// </summary>
        /// <param name="location">文件路径</param>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        IBundle Install(string location, Stream inputStream);

        /// <summary>
        /// 获取组件ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IBundle GetBundle(int id);

        /// <summary>
        /// 组件
        /// </summary>
        IBundle[] Bundles { get; }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="clazzes">类 对象 名称集合</param>
        /// <param name="service">对象服务</param>
        /// <param name="properties">对象属性</param>
        /// <returns>IServiceRegistration</returns>

        IServiceRegistration RegisterService(string[] clazzes,
            object service, Dictionary<string, object> properties);

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="clazz">类 对象名称</param>
        /// <param name="service">服务</param>
        /// <param name="properties">属性</param>
        /// <returns></returns>
        IServiceRegistration RegisterService(string clazz,
            object service, Dictionary<string, object> properties);

        /// <summary>
        ///  注册服务
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="service">服务</param>
        /// <param name="properties">属性</param>
        /// <returns></returns>
        /// <example>
        /// IUserName userInfo=new UserName();
        /// Context.RegisterService(TypeOf(IUserName),userInfo,null);
        /// </example>
        IServiceRegistration RegisterService(Type type,
            object service, Dictionary<string, object> properties);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="service">服务</param>
        /// <param name="properties">属性</param>
        /// <returns></returns>
        IServiceRegistration RegisterService<T>(object service,
            Dictionary<string, object> properties);

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="clazz">类</param>
        /// <param name="filter">过滤</param>
        /// <returns></returns>
        IServiceReference[] GetServiceReferences(string clazz, string filter);

        /// <summary>
        /// 获取所有服务
        /// </summary>
        /// <param name="clazz">类</param>
        /// <param name="filter">过滤</param>
        /// <returns>IServiceReference[]</returns>
        IServiceReference[] GetAllServiceReferences(string clazz, string filter);

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="clazz">类</param>
        /// <returns></returns>
        IServiceReference GetServiceReference(string clazz);

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="reference">服务</param>
        /// <returns></returns>
        object GetRegisterService(IServiceReference reference);

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="clazz">类</param>
        /// <returns>object</returns>
        object GetRegisterService(string clazz);

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>object</returns>
        object GetRegisterService(Type type);

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <example>
        /// IMyService service=(IMyService)Activator.Context.GetService<IMyService>(typeof(IMyService));
        /// </example>
        /// <typeparam name="T">服务对像</typeparam>
        /// <returns>object</returns>
        T GetRegisterService<T>();

        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <returns>Bool</returns>
        bool UngetService(IServiceReference reference);

        #region 流程

        ///// <summary>
        ///// 添加服务
        ///// </summary>
        ///// <param name="serviceType">服务类型</param>
        ///// <param name="serviceInstances">服务实例</param>
        //void AddService(Type serviceType, params object[] serviceInstances);

        ///// <summary>
        ///// 添加服务
        ///// </summary>
        ///// <param name="serviceInstances">服务实例</param>
        //void AddService<T>(params object[] serviceInstances);
        ///// <summary>
        ///// 添加服务
        ///// </summary>
        ///// <param name="serviceTypes">服务类型</param>
        ///// <param name="serviceInstance">服务实例</param>
        //void AddService(object serviceInstance, params Type[] serviceTypes);

        ///// <summary>
        ///// 添加服务
        ///// </summary>
        ///// <param name="serviceInterface">服务类型</param>
        ///// <param name="serviceInstance">服务实例</param>
        //void AddService(Type serviceInterface, object serviceInstance);

        ///// <summary>
        ///// 添加服务
        ///// </summary>
        ///// <typeparam name="TServiceInterface">服务类型(泛型)</typeparam>
        ///// <param name="serviceInstance">服务实例</param>
        //void  AddService<TServiceInterface>(object serviceInstance);

        ///// <summary>
        ///// 删除服务
        ///// </summary>
        ///// <param name="serviceType">服务类型</param>
        ///// <param name="serviceInstance">服务实例</param>
        //void RemoveService(Type serviceType, object serviceInstance);

        ///// <summary>
        ///// 删除服务
        ///// </summary>
        ///// <param name="serviceInstance">服务实例</param>
        //void RemoveService<T>(object serviceInstance);

        ///// <summary>
        ///// 删除服务
        ///// </summary>
        ///// <param name="serviceInstance">服务实例</param>
        //void RemoveService(object serviceInstance);

        ///// <summary>
        ///// 获取服务
        ///// </summary>
        ///// <typeparam name="T">对像</typeparam>
        ///// <returns>T</returns>
        //T GetFirstOrDefaultService<T>();

        ///// <summary>
        ///// 获取服务
        ///// </summary>
        ///// <param name="serviceType">对像</param>
        ///// <returns>object</returns>
        //object GetFirstOrDefaultService(Type serviceType);

        ///// <summary>
        ///// 获取服务
        ///// </summary>
        ///// <param name="serviceTypeName">服务类型名称</param>
        ///// <returns>object</returns>
        //object GetFirstOrDefaultService(string serviceTypeName);

        #endregion 流程

        /// <summary>
        /// 获取文件的信息
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <returns>FileSystemInfo</returns>
        FileSystemInfo GetDataFile(string filename);

        /// <summary>
        /// 创建过滤
        /// </summary>
        /// <param name="filter">过滤字符串</param>
        /// <returns>IFilter</returns>
        IFilter CreateFilter(string filter);

        /// <summary>
        /// 匹配
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <returns>bool</returns>
        bool IsAssignableTo(IServiceReference reference);
    }
}