namespace Syncfusion.Addin.Core
{
    /// <summary>
    /// 组件运行状态
    /// </summary>
    public enum BundleRuntimeState
    {
        /// <summary>
        /// 正在启动
        /// </summary>
        Starting,

        /// <summary>
        /// 已启动
        /// </summary>
        Started,

        /// <summary>
        /// 正在停止
        /// </summary>
        Stopping,

        /// <summary>
        /// 已停止
        /// </summary>
        Stopped,

        /// <summary>
        /// 已释放
        /// </summary>
        Disposed
    }
}