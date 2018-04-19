using Syncfusion.Addin.Core.Metadata;
using System.Collections.Generic;

namespace Syncfusion.Addin.Configuration.Plugin
{
    /// <summary>
    /// 起始页面管理服务
    /// </summary>
    public class PageServiceData
    {
        /// <summary>
        /// 页面信息
        /// </summary>
        public string PageServicePoint { get; set; }

        /// <summary>
        /// 页面列表
        /// </summary>
        public List<PageNode> PageNodeList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageServiceData"/> class.
        /// </summary>
        public PageServiceData()
        {
            PageNodeList = new List<PageNode>();
        }
    }
}