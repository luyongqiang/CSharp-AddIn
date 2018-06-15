namespace Syncfusion.Addin.Configuration.Plugin
{
    /// <summary>
    /// 组件信息
    /// </summary>
    public class BundleInfoData
    {
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 作者信息
        /// </summary>
        public string Author
        {
            get;
            set;
        }

        /// <summary>
        /// 地址
        /// </summary>
        public string ContactAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 版权
        /// </summary>
        public string Copyright
        {
            get;
            set;
        }

        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// 程序集版本
        /// </summary>
        public string AssemblyVersion { get; set; }

        /// <summary>
        /// 文件版本
        /// </summary>
        public string FileVersion { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
    }
}