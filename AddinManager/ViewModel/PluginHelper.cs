namespace AddinManager.ViewModel
{
    public class PluginHelper
    {
        private static PluginHelper _ugin;

        internal static PluginHelper Instance
        {
            get
            {
                if (_ugin == null)
                {
                    _ugin = new PluginHelper();
                }
                return _ugin;
            }
        }

        public string PlguinName { get; set; }
        public string CopyRight { get; set; }
        public string Company { get; set; }
        public string AssemblyVersion { get; set; }
        public string Description { get; set; }
        public bool isMain { get; set; }
        public bool isGenMenu { get; set; }
    }
}