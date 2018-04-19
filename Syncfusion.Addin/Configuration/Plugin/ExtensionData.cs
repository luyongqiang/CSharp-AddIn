namespace Syncfusion.Addin.Configuration.Plugin
{
    /// <summary>
    /// 扩展数据
    /// </summary>
    public class ExtensionData
    {
        /// <summary>
        /// 应用程序扩展信息
        /// </summary>
        public ApplicationExtensionData ApplicationExtends
        {
            get;
            set;
        }

        /// <summary>
        /// 图标信息
        /// </summary>
        public string ApplicationIco
        {
            get; set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionData"/> class.
        /// </summary>
        public ExtensionData()
        {
            ApplicationExtends = new ApplicationExtensionData();
        }
    }
}