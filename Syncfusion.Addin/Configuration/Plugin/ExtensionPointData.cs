using System.Collections.Generic;
using System.Xml;

namespace Syncfusion.Addin.Configuration.Plugin
{
    /// <summary>
    /// 扩展点数据
    /// </summary>
    public class ExtensionPointData
    {
        public string Point
        {
            get;
            set;
        }

        /// <summary>
        /// XML子节点信息
        /// </summary>
        public List<XmlNode> ChildNodes
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the schema.
        /// </summary>
        /// <value>The schema.</value>
        public string Schema
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionPointData"/> class.
        /// </summary>
        public ExtensionPointData()
        {
            this.ChildNodes = new List<XmlNode>();
        }
    }
}