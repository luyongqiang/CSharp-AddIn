namespace Syncfusion.Addin.Configuration.Plugin
{
    /// <summary>
    /// Enum ActivatorPolicy
    /// </summary>
    public enum ActivatorPolicy
    {
        /// <summary>
        /// 及时加载
        /// </summary>
        Immediate,//及时加载

        /// <summary>
        /// 延迟加载
        /// </summary>
        Lazy    //延迟加载
    }

    /// <summary>
    /// 组件加载
    /// </summary>
    public class ActivatorData
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// 加载方式
        /// </summary>
        public ActivatorPolicy Policy
        {
            get;
            set;
        }

        public ActivatorData()
        {
            //默认为及时加载
            this.Policy = ActivatorPolicy.Immediate;
        }
    }
}