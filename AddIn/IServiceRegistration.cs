using System.Collections.Generic;

namespace Syncfusion.Addin
{
    /// <summary>
    /// 服务注册信息
    /// </summary>
    public interface IServiceRegistration
    {
        /// <summary>
        /// 得到引用
        /// </summary>
        /// <returns>IServiceReference</returns>
        IServiceReference GetReference();

        /// <summary>
        /// 属性信息
        /// </summary>
        Dictionary<string, object> Properties { set; }

        /// <summary>
        /// 卸载
        /// </summary>
        void Unregister();
    }
}