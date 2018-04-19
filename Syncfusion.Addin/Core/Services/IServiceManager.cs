using System;
using System.Collections.Generic;

namespace Syncfusion.Addin.Core.Services
{
    /// <summary>
    /// 组件服务管理器
    /// </summary>
    public interface IServiceManager
    {
        /// <summary>
        /// 服务添加管理器
        /// </summary>
        /// <param name="owner">组件</param>
        /// <param name="serviceType">服务类型</param>
        /// <param name="serviceInstances">服务实例对像</param>
        void AddService(IBundle owner, Type serviceType, params object[] serviceInstances);

        void AddService<T>(IBundle owner, params object[] serviceInstances);

        void AddService(IBundle owner, object serviceInstance, params Type[] serviceTypes);

        void RemoveService(IBundle owner, Type serviceType, object serviceInstance);

        void RemoveService<T>(IBundle owner, object serviceInstance);

        void RemoveService(IBundle owner, object serviceInstance);

        T GetFirstOrDefaultService<T>();

        object GetFirstOrDefaultService(Type serviceType);

        object GetFirstOrDefaultService(string serviceTypeName);

        List<object> GetService(string serviceTypeName);

        List<T> GetService<T>();

        List<object> GetService(Type serviceType);

        Dictionary<Type, List<object>> GetServices();
    }
}