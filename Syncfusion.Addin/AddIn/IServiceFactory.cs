namespace Syncfusion.Addin
{
    /// <summary>
    /// 服务工厂
    /// </summary>
    public interface IServiceFactory
    {
        /// <summary>
        /// 获取注册的服务
        /// </summary>
        /// <param name="bundle">组件</param>
        /// <param name="registration">服务</param>
        /// <returns></returns>
        object GetService(IBundle bundle, IServiceRegistration registration);

        /// <summary>
        ///  联合获取服务
        /// </summary>
        /// <param name="bundle">组件</param>
        /// <param name="registration">注册服务</param>
        /// <param name="service">服务</param>
        void UngetService(IBundle bundle, IServiceRegistration registration, object service);
    }
}