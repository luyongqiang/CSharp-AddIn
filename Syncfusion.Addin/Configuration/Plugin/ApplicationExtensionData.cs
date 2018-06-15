using System.Collections.Generic;

namespace Syncfusion.Addin.Configuration.Plugin
{
    /// <summary>
    /// 应用程序扩展信息
    /// </summary>
    public class ApplicationExtensionData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationExtensionData"/> class.
        /// </summary>
        public ApplicationExtensionData()
        {
            MenuList = new List<ExtendionMenu>();
        }

        /// <summary>
        /// 应用程序图标
        /// </summary>
        public string ApplicationIco { get; set; }

        /// <summary>
        /// 应用程序名称
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public List<ExtendionMenu> MenuList
        {
            get;
            set;
        }
    }
}