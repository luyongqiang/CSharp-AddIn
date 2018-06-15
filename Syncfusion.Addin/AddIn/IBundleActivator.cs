namespace Syncfusion.Addin
{
    /// <summary>
    /// Customizes the starting and stopping of a bundle.
    /// </summary>
    public interface IBundleActivator
    {
        /// <summary>
        /// 组件启动
        /// </summary>
        /// <param name="context">组件环境</param>
        void Start(IBundleContext context);

        /// <summary>
        /// 组件停止
        /// </summary>
        /// <param name="context">组件环境</param>
        void Stop(IBundleContext context);
    }
}