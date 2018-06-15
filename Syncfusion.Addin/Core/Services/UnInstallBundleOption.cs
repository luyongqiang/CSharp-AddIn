namespace Syncfusion.Addin.Core.Services
{
    public class UnInstallBundleOption
    {
        public string Location
        {
            get;
            set;
        }

        public bool NeedRemove
        {
            get;
            set;
        }

        public UnInstallBundleOption()
        {
            this.NeedRemove = false;
        }
    }
}