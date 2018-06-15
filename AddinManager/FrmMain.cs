using AddinManager.Views;
using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace AddinManager
{
    public partial class FrmMain : XtraForm
    {
        private View1 view1;
        private View2 view2;
        private View3 view3;

        private int _stateLevel = 1;

        public FrmMain()
        {
            InitializeComponent();
            btnPre.Click += btnPre_Click;
            btnNext.Click += btnNext_Click;
            btnCancel.Click += btnCancel_Click;

            view1 = new View1();
            view1.Dock = DockStyle.Fill;
            PanelMain.Controls.Add(view1);

            view2 = new View2();
            view2.Dock = DockStyle.Fill;
            PanelMain.Controls.Add(view2);

            view3 = new View3();
            view3.Assemblies = view1.FileInfos;
            view3.Dock = DockStyle.Fill;
            PanelMain.Controls.Add(view3);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            switch (_stateLevel)
            {
                case 1:
                    if (view1 != null)
                    {
                        if (view1.FileInfos.Count > 0)
                        {
                            view1.Visible = false;
                            btnPre.Enabled = true;
                            view2.Visible = true;
                            _stateLevel = 2;
                        }
                        else
                        {
                            XtraMessageBox.Show("请选择组件！谢谢");
                        }
                    }
                    break;

                case 2:

                    if (view2 != null)
                    {
                        if (view2.isVerify())
                        {
                            view2.Visible = false;
                            btnPre.Enabled = true;
                            view3.Visible = true;
                            _stateLevel = 3;
                            view2.GenMenu();
                        }
                        else
                        {
                            XtraMessageBox.Show("请选择组件！谢谢");
                        }
                    }
                    break;

                case 3:
                    if (view3 != null)
                    {
                        if (!view3.GenerateZip())
                        {
                            XtraMessageBox.Show("请查看路经是否正确！谢谢");
                        }
                    }
                    break;
            }
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            switch (_stateLevel)
            {
                case 1:
                    view1.Visible = true;
                    break;

                case 2:
                    view1.Visible = false;
                    view2.Visible = true;
                    _stateLevel = 1;
                    break;

                case 3:
                    view2.Visible = false;
                    view3.Visible = true;
                    _stateLevel = 2;
                    break;
            }
        }
    }
}