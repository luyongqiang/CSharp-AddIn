using Syncfusion.Addin.Configuration.Plugin;
using Syncfusion.Addin.Core.Services;

namespace Syncfusion.Addin.Core.ServicesImp
{
    /// <summary>
    /// 组件
    /// </summary>
    internal class BundleFactory : IBundleFactory
    {
        /// <summary>
        /// 框架
        /// </summary>
        public Framework Framework
        {
            get;
            private set;
        }

        /// <summary>
        /// 初始化ID
        /// </summary>
        public int InitialBundleId
        {
            get;
            private set;
        }

        /// <summary>
        /// 最大ID
        /// </summary>
        public int MaxBundleId
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建组件
        /// </summary>
        /// <param name="bundleData">组件数据</param>
        /// <returns>IBundle</returns>
        public IBundle CreateBundle(BundleData bundleData)
        {
            IBundle bundle = new Bundle(bundleData, Framework);
            return bundle;
        }

        public BundleFactory(Framework framework)
        {
            this.InitialBundleId = 2;
            this.MaxBundleId = this.InitialBundleId;
            this.Framework = framework;
        }
    }
}