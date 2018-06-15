using System.Collections.Generic;
using Syncfusion.Addin.Core.Metadata;

namespace Syncfusion.Addin.Configuration.Plugin
{
    /// <summary>
    /// 组件数据
    /// </summary>
    public class BundleData
    {
        /// <summary>
        /// 扩展点信息
        /// </summary>
        public List<ExtensionPointData> ExtensionPoints
        {
            get;
            internal set;
        }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public ExtensionData Extensions
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 符号名称
        /// </summary>
        public string SymbolicName
        {
            get;
            set;
        }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version
        {
            get;
            set;
        }

        /// <summary>
        /// 组件数据信息
        /// </summary>
        public BundleInfoData BundleInfo
        {
            get;
            set;
        }

        /// <summary>
        /// 模块信息
        /// </summary>
        public ActivatorData Activator
        {
            get;
            set;
        }

        /// <summary>
        /// 运行组件信息
        /// </summary>
        public RuntimeData Runtime
        {
            get;
            set;
        }

        /// <summary>
        /// 页面服务数据
        /// </summary>
        public PageServiceData PageSerivce
        {
            get;
            set;
        }

        /// <summary>
        /// 服务接口
        /// </summary>
        public List<ServiceData> Services
        {
            get;
            set;
        }

        public AppSettings AppSettings { get; set; }

        /// <summary>
        /// 路经
        /// </summary>
        public string Path
        {
            get;
            set;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable
        {
            get;
            set;
        }

        /// <summary>
        /// 是否即时加载
        /// </summary>
        public bool Immediate
        {
            get; set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get; set;
        }

        /// <summary>
        /// 编号
        /// </summary>
        public int Id
        {
            get; set;
        }

        //启动级别
        public int StartLevel { get; set; }

        /// <summary>
        /// 文件本地位置
        /// </summary>
        public string Location { get; set; }

        public BundleData(int id, string location)
        {
            Id = id;
            Location = location;
        }

        public BundleData()
        {
        }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>The product.</value>
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        public string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the application icon.
        /// </summary>
        /// <value>The application icon.</value>
        public string ApplicationIco { get; set; }
    }
}