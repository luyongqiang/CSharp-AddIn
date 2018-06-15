using AddinManager.ViewModel;
using DevExpress.XtraEditors;

namespace AddinManager.Views
{
    public partial class View2 : XtraUserControl
    {
        public View2()
        {
            InitializeComponent();
            layoutControl1.AllowCustomizationMenu = false;
        }

        internal bool isVerify()
        {
            return dxValidationProvider1.Validate();
        }

        internal void GenMenu()
        {
            if (!isVerify())
            {
                return;
            }
            PluginHelper.Instance.PlguinName = txtPlguinName.Text;
            PluginHelper.Instance.Company = txtCompany.Text;
            PluginHelper.Instance.AssemblyVersion = txtVersion.Text;
            PluginHelper.Instance.CopyRight = txtCopyRight.Text;
            PluginHelper.Instance.isGenMenu = txtGenMenu.Checked;
            PluginHelper.Instance.isMain = txtisMain.Checked;
            PluginHelper.Instance.Description = txtDescription.Text;
        }
    }
}