using Syncfusion.Addin.Configuration.Plugin;

namespace Syncfusion.Addin.Core.Services
{
    public interface IBundleFactory
    {
        int InitialBundleId
        {
            get;
        }

        int MaxBundleId
        {
            get;
        }

        IBundle CreateBundle(BundleData bundleData);
    }
}