using AddinManager.ViewModel;
using DevExpress.XtraEditors;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace AddinManager.Views
{
    public partial class View3 : XtraUserControl
    {
        public View3()
        {
            InitializeComponent();
            btnSure.Click += btnSure_Click;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog saveDialog = new FolderBrowserDialog();
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                txtOupPath.Text = saveDialog.SelectedPath;
            }
        }

        internal string BundleOutputDirectory { get; set; }
        internal string BundlePackageFile { get; set; }
        internal string BundleSymbolicName { get; set; }

        internal List<PluginFile> Assemblies = new List<PluginFile>();

        public bool GenerateZip()
        {
            if (!isVersible())
            {
                return false;
            }
            bool result;
            try
            {
                this.BundlePackageFile = Path.Combine(this.txtOupPath.Text.Trim(), this.BundleSymbolicName + ".zip");
                ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(this.BundlePackageFile));
                zipOutputStream.SetLevel(9);
                List<string> list = new List<string>();
                // list.Add("Manifest.xml");
                foreach (PluginFile current in this.Assemblies)
                {
                    list.Add(current.FileName);
                }
                foreach (string current2 in list)
                {
                    ZipEntry entry = new ZipEntry(current2);
                    zipOutputStream.PutNextEntry(entry);
                    FileStream fileStream = File.OpenRead(Path.Combine(this.BundleOutputDirectory, current2));
                    byte[] array = new byte[fileStream.Length];
                    fileStream.Read(array, 0, array.Length);
                    zipOutputStream.Write(array, 0, array.Length);
                }
                zipOutputStream.Finish();
                zipOutputStream.Close();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        private bool isVersible()
        {
            if (this.txtOupPath.Text.Trim().Length <= 0)
            {
                return false;
            }

            return true;
        }
    }
}