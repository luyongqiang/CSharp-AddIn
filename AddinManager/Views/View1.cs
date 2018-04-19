using AddinManager.ViewModel;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AddinManager.Views
{
    public partial class View1 : XtraUserControl
    {
        internal List<PluginFile> FileInfos = new List<PluginFile>();

        public View1()
        {
            InitializeComponent();

            btnAdd.ItemClick += btnAdd_ItemClick;
            btnDelete.ItemClick += btnDelete_ItemClick;
            btnDeleteList.ItemClick += btnDeleteList_ItemClick;
        }

        private void btnDeleteList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            treeList1.BeginUnboundLoad();
            treeList1.DataSource = null;
            treeList1.EndUnboundLoad();

            FileInfos.Clear();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode node = treeList1.FocusedNode;
            if (node != null)
            {
                string fileName = node.GetValue("FileName").ToString();
                if (!string.IsNullOrEmpty(fileName))
                {
                    PluginFile file = FileInfos.FirstOrDefault(o => o.FileName == fileName);
                    if (file != null)
                    {
                        FileInfos.Remove(file);
                    }
                    Bind();
                }
            }
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "插件文件|*.dll|插件配置文件|*.addin";
            if (open.ShowDialog() == DialogResult.OK)
            {
                PluginFile file = new PluginFile();
                file.FileName = open.SafeFileName;
                file.FilePath = open.FileName;
                if (open.FilterIndex == 2)
                {
                    file.IsAddInFile = true;
                }
                FileInfos.Add(file);
            }
            Bind();
        }

        private void Bind()
        {
            treeList1.BeginUnboundLoad();
            treeList1.DataSource = FileInfos;
            treeList1.EndUnboundLoad();
        }
    }
}