using Syncfusion.Addin.Core.Metadata;
using System.Collections.Generic;

namespace Syncfusion.Addin.Configuration.Plugin
{
    /// <summary>
    /// 程序菜单信息
    /// </summary>
    public class ExtendionMenu
    {
        public ExtendionMenu()
        {
            _loadOtherContainer = new List<LoadOtherContainerPanles>();
        }

        private string _menuIco;
        private string _caption;
        private string _group;
        private string _toolTip;
        private string _classForm;
        private PluginDockStyle _pluginDockStyle;
        private int _sort;
        private int _high;
        private int _width;
        private List<LoadOtherContainerPanles> _loadOtherContainer;

        /// <summary>
        //宽度
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// 设置高度
        /// </summary>
        /// <value>The high.</value>
        public int High
        {
            get { return _high; }
            set { _high = value; }
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <value>The sort.</value>
        public int Sort
        {
            get { return _sort; }
            set { _sort = value; }
        }

        /// <summary>
        /// 窗口停靠样式
        /// </summary>
        /// <value>The plugin dock style.</value>
        public PluginDockStyle PluginDockStyle
        {
            get { return _pluginDockStyle; }
            set { _pluginDockStyle = value; }
        }

        /// <summary>
        /// 窗口名称
        /// </summary>
        public string ClassForm
        {
            get
            {
                return this._classForm;
            }
            set
            {
                this._classForm = value;
            }
        }

        /// <summary>
        /// 图标
        /// </summary>
        public string MenuIco
        {
            get
            {
                return this._menuIco;
            }
            set
            {
                this._menuIco = value;
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Caption
        {
            get
            {
                return this._caption;
            }
            set
            {
                this._caption = value;
            }
        }

        /// <summary>
        /// 组
        /// </summary>
        public string Group
        {
            get
            {
                return this._group;
            }
            set
            {
                this._group = value;
            }
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string ToolTip
        {
            get
            {
                return this._toolTip;
            }
            set
            {
                this._toolTip = value;
            }
        }

        public List<LoadOtherContainerPanles> LoadOtherContainer
        {
            get
            {
                return _loadOtherContainer;
            }

            set
            {
                _loadOtherContainer = value;
            }
        }
    }
}