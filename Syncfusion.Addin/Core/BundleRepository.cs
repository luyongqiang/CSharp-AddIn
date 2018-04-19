using System;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Addin.Core
{
    /// <summary>
    /// Class BundleRepository.
    /// </summary>
    public class BundleRepository : IBundleRepository
    {
        /// <summary>
        /// The _bundles by install order
        /// </summary>
        private readonly List<IBundle> _bundlesByInstallOrder;

        /// <summary>
        /// The _bundles by identifier
        /// </summary>
        private readonly Dictionary<int, IBundle> _bundlesById;

        /// <summary>
        /// The _bundles by symbolic name
        /// </summary>
        private readonly Dictionary<string, IBundle> _bundlesBySymbolicName;

        /// <summary>
        /// The synchronize object
        /// </summary>
        private object syncObj;

        /// <summary>
        /// 组件数量
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get
            {
                lock (this)
                {
                    return _bundlesByInstallOrder.Count;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BundleRepository"/> class.
        /// </summary>
        public BundleRepository()
        {
            _bundlesByInstallOrder = new List<IBundle>();
            _bundlesById = new Dictionary<int, IBundle>();
            _bundlesBySymbolicName = new Dictionary<string, IBundle>();

            this.syncObj = new object();
        }

        /// <summary>
        /// Gets the <see cref="IBundle"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>IBundle.</returns>
        public IBundle this[int index]
        {
            get
            {
                lock (syncObj)
                {
                    return _bundlesByInstallOrder[index];
                }
            }
        }

        /// <summary>
        /// 获取所有组件
        /// </summary>
        /// <returns>IBundle</returns>
        public IBundle[] GetBundles()
        {
            lock (syncObj)
            {
                return _bundlesByInstallOrder.ToArray<IBundle>();
            }
        }

        /// <summary>
        /// 根据组件编号获取组件
        /// </summary>
        /// <param name="bundleId">组件编号</param>
        /// <returns>IBundle</returns>
        public IBundle GetBundle(int bundleId)
        {
            lock (syncObj)
            {
                return _bundlesById[bundleId];
            }
        }

        /// <summary>
        /// 根据位置获取组件
        /// </summary>
        /// <param name="location">位置</param>
        /// <returns>IBundle</returns>
        public IBundle GetBundle(string location)
        {
            return _bundlesByInstallOrder.Find(bundle => System.String.Compare(bundle.Location, location, System.StringComparison.Ordinal) == 0);
        }

        /// <summary>
        /// 获取组件列表
        /// </summary>
        /// <param name="symbolicName">名称</param>
        /// <returns>IBundle[] 数组</returns>
        public IBundle[] GetBundles(string symbolicName)
        {
            lock (syncObj)
            {
                List<IBundle> bundles = new List<IBundle>();
                Dictionary<string, IBundle>.Enumerator enumerator = _bundlesBySymbolicName.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (System.String.Compare(enumerator.Current.Key, symbolicName, System.StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        bundles.Add(enumerator.Current.Value);
                    }
                }

                return bundles.ToArray<IBundle>();
            }
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <param name="symbolicName">名称</param>
        /// <param name="version">版本</param>
        /// <returns>IBundle</returns>
        public IBundle GetBundle(string symbolicName, Version version)
        {
            IBundle[] bundles = GetBundles(symbolicName);
            if (bundles != null)
            {
                if (bundles.Length > 0)
                {
                    // for (int i = 0; i < bundles.Length; i++)
                    //{
                    //if (bundles[i].Version.Equals(version))
                    //   {
                    return bundles[0];
                    //  }
                    //}
                }
            }
            return null;
        }

        /// <summary>
        /// 注册组件
        /// </summary>
        /// <param name="bundle">组件</param>
        public void Register(IBundle bundle)
        {
            lock (syncObj)
            {
                _bundlesByInstallOrder.Add(bundle);
                _bundlesById.Add(bundle.Id, bundle);
                _bundlesBySymbolicName.Add(bundle.SymbolicName, bundle);
            }
        }

        /// <summary>
        /// 卸载组件
        /// </summary>
        /// <param name="bundle">组件</param>
        /// <returns>bool</returns>
        public bool Unregister(IBundle bundle)
        {
            lock (syncObj)
            {
                // remove by install order
                bool found = _bundlesByInstallOrder.Remove(bundle);
                if (!found)
                {
                    return false;
                }

                // remove by bundle ID
                _bundlesById.Remove(bundle.Id);

                _bundlesBySymbolicName.Remove(bundle.SymbolicName);

                return true;
            }
        }

        /// <summary>
        /// 组件路经
        /// </summary>
        /// <value>The bundle paths.</value>
        public List<String> BundlePaths
        {
            get
            {
                lock (syncObj)
                {
                    List<String> dirs = new List<String>();
                    Dictionary<string, IBundle>.Enumerator enumerator = _bundlesBySymbolicName.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        String path = AppDomain.CurrentDomain.BaseDirectory + enumerator.Current.Value.DataInfo.Path;
                        if (!dirs.Contains(path))
                        {
                            dirs.Add(path);
                        }
                    }
                    return dirs;
                }
            }
        }
    }
}