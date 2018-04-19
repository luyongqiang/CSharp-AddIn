using Syncfusion.Addin.Configuration.Plugin;
using Syncfusion.Addin.Core.Services;
using Syncfusion.Addin.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Threading;

namespace Syncfusion.Addin.Core
{
    /// <summary>
    /// Main class of the Innosys framework. This class is internal, not accessible from outside the
    /// Innosys assembly.
    /// </summary>
    public class Framework : IFramework
    {
        /// <summary>
        /// Class ServiceItem.
        /// </summary>
        private class ServiceItem
        {
            public Type ServiceType
            {
                get;
                set;
            }

            public object ServiceInstance
            {
                get;
                set;
            }

            public ServiceItem(Type serviceType, object serviceInstance)
            {
                this.ServiceType = serviceType;
                this.ServiceInstance = serviceInstance;
            }
        }

        #region --- Constant ---

        private static readonly string[] BundleExtention = { ".dll", ".exe" };

        #endregion --- Constant ---

        #region --- Fields ---

        private readonly List<Framework.ServiceItem> _pendingSystemServices = new List<Framework.ServiceItem>();
        private BundleRepository _bundleRepository;
        private ServiceRegistry _serviceRegistry;
        private DirectoryInfo _cache;
        private SystemBundle _systemBundle;
        private BundleContext _systemBundleContext;
        private int _bundleAppDomains;
        private int _serviceId;
        //public IServiceManager ServiceContainer
        //{
        //    get;
        //    set;
        //}

        #endregion --- Fields ---

        #region --- Properties ---

        public IBundleRepository Bundles => _bundleRepository;
        public ServiceRegistry ServiceRegistry => _serviceRegistry;

        public BundleContext SystemBundleContext => _systemBundleContext;

        internal DirectoryInfo Cache
        {
            get
            {
                return _cache;
            }
            set
            {
                _cache = value;
            }
        }

        #endregion --- Properties ---

        /// <summary>
        /// Occurs when [framework event].
        /// </summary>
        public event FrameworkEventHandler FrameworkEvent
        {
            add
            {
                EventManager.FrameworkEvent += value;
            }
            remove
            {
                EventManager.FrameworkEvent -= value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Framework"/> class.
        /// </summary>
        public Framework()
        {
            InitializeFramework();
        }

        /// <summary>
        /// Initializes the framework.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// The framework is already started
        /// or
        /// The framework is already started
        /// </exception>
        private void InitializeFramework()
        {
            _serviceId = 1;
            if (_bundleRepository == null)
            {
                _bundleRepository = new BundleRepository();
            }
            else
            {
                throw new InvalidOperationException("The framework is already started");
            }

            if (_serviceRegistry == null)
            {
                _serviceRegistry = new ServiceRegistry();
            }
            else
            {
                throw new InvalidOperationException("The framework is already started");
            }

            _systemBundle = new SystemBundle(this);
            _systemBundleContext = (BundleContext)_systemBundle.Context;
            // ServiceContainer=new ServiceManager(this);
        }

        private void SetupConfiguration(string fileName)
        {
            string configDirectory = Path.GetDirectoryName(fileName);
            if (configDirectory != null && !Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }
            if (!File.Exists(fileName))
            {
                Preferences.Instance.Save(fileName);
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        public void Launch()
        {
            InstallBundleInternal(_systemBundle);
            StartBundle(_systemBundle);
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            Debug.Assert(_bundleRepository != null);

            for (Int32 i = _bundleRepository.Count - 1; i > -1; i--)
            {
                _bundleRepository[i].Stop();
            }
        }

        /// <summary>
        /// 关闭/卸载
        /// </summary>
        public void Shutdown()
        {
            Debug.Assert(_bundleRepository != null);

            for (int i = _bundleRepository.Count - 1; i >= 0; i--)
            {
                IBundle bundle = _bundleRepository[i];
                int bundleId = bundle.Id;
                StopBundle(bundleId);
                UninstallBundle(bundleId);
                bundle = null;
            }
        }

        /// <summary>
        /// ValidExtention
        /// </summary>
        /// <param name="location">File full path</param>
        /// <returns>Boolean.</returns>
        private Boolean ValidExtention(String location)
        {
            for (Int32 i = 0; i < BundleExtention.Length; i++)
            {
                if (location.IndexOf(BundleExtention[i],
                 StringComparison.OrdinalIgnoreCase) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 注册组件
        /// </summary>
        /// <param name="location">位置</param>
        /// <returns>IBundle</returns>
        /// <exception cref="Syncfusion.Addin.BundleException"></exception>
        /// <exception cref="System.IO.FileNotFoundException"></exception>
        public IBundle InstallBundle(String location)
        {
            if (!ValidExtention(location))
            {
                location = location + BundleExtention[0];
            }

            string fullLocation = string.Empty;

            bool isPathRooted = Path.IsPathRooted(location);

            if (isPathRooted)
            {
                fullLocation = location;
            }

            if (!File.Exists(fullLocation))
            {
                throw new BundleException(String.Format("Bundle {0} not found.", location),
                    new FileNotFoundException(String.Format("file:{0} not found.", fullLocation)));
            }

            // Create the bundle object
            BundleData bundleData = new BundleData();
            bundleData.Id = _bundleRepository.Count;
            bundleData.Location = fullLocation;
            Bundle bundle = new Bundle(bundleData, this);

            if (CheckInstallBundle(bundle))
            {
                InstallBundleInternal(bundle);
            }
            return bundle;
        }

        /// <summary>
        /// 注册组件
        /// </summary>
        /// <param name="location">位置</param>
        /// <param name="bd">组件数据</param>
        /// <returns>IBundle</returns>
        public IBundle InstallBundle(string location, BundleData bd)
        {
            if (!ValidExtention(location))
            {
                location = location + BundleExtention;
            }

            string fullLocation = string.Empty;

            bool isPathRooted = Path.IsPathRooted(location);

            if (isPathRooted)
            {
                fullLocation = location;
            }

            if (!File.Exists(fullLocation))
            {
                FileLogUtility.Debug(string.Format("{0}: Bundle {0} not found. {1}", location, DateTime.Now));
                FileLogUtility.Debug(string.Format("{0}: file:{0} not found. {1}", fullLocation, DateTime.Now));
            }

            // Create the bundle object
            BundleData bundleData = bd;
            bundleData.Id = _bundleRepository.Count;
            bundleData.Location = fullLocation;

            Bundle bundle = new Bundle(bundleData, this);

            if (CheckInstallBundle(bundle))
            {
                InstallBundleInternal(bundle);
            }
            return bundle;
        }

        /// <summary>
        /// Checks the install bundle.
        /// </summary>
        /// <param name="bundle">The bundle.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool CheckInstallBundle(Bundle bundle)
        {
            IBundle existsBundle = _bundleRepository.GetBundle(bundle.SymbolicName, null);
            if (existsBundle != null)
            {
                FileLogUtility.Debug($"{bundle.SymbolicName} Bundle is already installed {DateTime.Now} FilePath{bundle.Location}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Installs the bundle internal.
        /// </summary>
        /// <param name="bundle">The bundle.</param>
        /// <returns>IBundle.</returns>
        private IBundle InstallBundleInternal(Bundle bundle)
        {
            _bundleRepository.Register(bundle);

            if (bundle.State == BundleState.Installed)
            {
                EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Installed, bundle));
            }

            return bundle;
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <param name="location">位置</param>
        /// <returns>IBundle.</returns>
        public IBundle GetBundle(string location)
        {
            return this._bundleRepository.GetBundle(location);
        }

        /// <summary>
        /// 卸载组件
        /// </summary>
        /// <param name="id">组件编号</param>
        public void UninstallBundle(int id)
        {
            try
            {
                IBundle bundle = _bundleRepository.GetBundle(id);
                _bundleRepository.Unregister(bundle);
            }
            catch (Exception ex)
            {
                FileLogUtility.Debug(string.Format("{0}: Uninstall bundle threw a exception {1}", ex.Message, DateTime.Now));
            }
        }

        ///// <summary>
        ///// Removes the system service.
        ///// </summary>
        ///// <param name="serviceType">Type of the service.</param>
        ///// <param name="serviceInstance">The service instance.</param>
        //public void RemoveSystemService(Type serviceType, object serviceInstance)
        //{
        //    if (this._systemBundle == null)
        //    {
        //        this._pendingSystemServices.RemoveAll((Framework.ServiceItem item) => item.ServiceInstance == serviceInstance && item.ServiceType == serviceType);
        //        return;
        //    }
        //    this.ServiceContainer.RemoveService(this._systemBundle, serviceType, serviceInstance);
        //}

        ///// <summary>
        ///// 添加服务
        ///// </summary>
        ///// <param name="serviceInstance">服务实例</param>
        ///// <param name="serviceTypes">服务类型</param>
        ///// <exception cref="System.ArgumentNullException"></exception>
        //public void AddSystemService(object serviceInstance, params Type[] serviceTypes)
        //{
        //    if (serviceTypes != null && serviceInstance != null)
        //    {
        //        if (this._systemBundle == null)
        //        {
        //            foreach (Type serviceType in serviceTypes)
        //            {
        //                this._pendingSystemServices.Add(new Framework.ServiceItem(serviceType, serviceInstance));
        //            }
        //            return;
        //        }
        //        else
        //        {
        //            this.ServiceContainer.AddService(this._systemBundle, serviceInstance, serviceTypes);
        //        }
        //        return;
        //    }
        //    throw new ArgumentNullException();
        //}

        ///// <summary>
        ///// Adds the system service.
        ///// </summary>
        ///// <param name="serviceType">Type of the service.</param>
        ///// <param name="serviceInstances">The service instances.</param>
        ///// <exception cref="System.ArgumentNullException"></exception>
        //public void AddSystemService(Type serviceType, params object[] serviceInstances)
        //{
        //    if (serviceType != null && serviceInstances != null)
        //    {
        //        if (this._systemBundle == null)
        //        {
        //            foreach (object serviceInstance in serviceInstances)
        //            {
        //                this._pendingSystemServices.Add(new Framework.ServiceItem(serviceType, serviceInstance));
        //            }
        //            return;
        //        }
        //        else
        //        {
        //            this.ServiceContainer.AddService(this._systemBundle, serviceType, serviceInstances);
        //        }
        //        return;
        //    }
        //    throw new ArgumentNullException();
        //}

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="id">组件编号</param>
        /// <returns>IBundle</returns>
        public IBundle StartBundle(int id)
        {
            IBundle bundle = _bundleRepository.GetBundle(id);
            if (bundle == null)
            {
                FileLogUtility.Error(String.Format("Bundle not found.BundleId:{0}", id));
            }
            if (bundle != null && bundle.State != BundleState.Installed)
            {
                FileLogUtility.Error(String.Format("Bundle is aready started:{0}", id));
            }
            if (bundle != null)
            {
                bundle.Start();
            }
            return bundle;
        }

        /// <summary>
        /// 停止组件根据编号
        /// </summary>
        /// <param name="id">组件编号</param>
        public void StopBundle(int id)
        {
            IBundle bundle = _bundleRepository.GetBundle(id);
            if (bundle == null)
            {
                FileLogUtility.Error($"Bundle not found.BundleId:{id}");
            }
            if (bundle != null && bundle.State != BundleState.Active)
            {
                FileLogUtility.Error($"{bundle.SymbolicName}Bundle is not active.");
            }
            //判断是否为空
            bundle?.Stop();
        }/// <summary>

         /// 启动组件
         /// </summary>
         /// <param name="bundle">组件</param>
        public void StartBundle(IBundle bundle)
        {
            if (bundle == null)
            {
                FileLogUtility.Error("Bundle is Null.");
                return;
            }
            bundle.Start();
        }

        /// <summary>
        /// 获取服务引用
        /// </summary>
        /// <param name="clazz">类</param>
        /// <param name="filterString">过滤字符串</param>
        /// <param name="context">组件</param>
        /// <param name="allservices">是否所有服务</param>
        /// <returns>IServiceReference[]</returns>
        public IServiceReference[] GetServiceReferences(string clazz, string filterString, IBundleContext context, bool allservices)
        {
            Filter filter = string.IsNullOrEmpty(filterString) ? null : new Filter(filterString);
            IServiceReference[] services = null;

            lock (_serviceRegistry)
            {
                services = _serviceRegistry.LookupServiceReferences(clazz, filter);
                if (services == null)
                {
                    return null;
                }
                int removed = 0;
                for (int i = services.Length - 1; i >= 0; i--)
                {
                    ServiceReference reference = (ServiceReference)services[i];
                    string[] classes = reference.GetClasses();
                    if (allservices || context.IsAssignableTo((ServiceReference)services[i]))
                    {
                        if (clazz == null)
                            try
                            { /* test for permission to the classes */
                                //checkGetServicePermission(classes);
                            }
                            catch (SecurityException)
                            {
                                services[i] = null;
                                removed++;
                            }
                    }
                    else
                    {
                        services[i] = null;
                        removed++;
                    }
                }
                if (removed > 0)
                {
                    IServiceReference[] temp = services;
                    services = new ServiceReference[temp.Length - removed];
                    for (int i = temp.Length - 1; i >= 0; i--)
                    {
                        if (temp[i] == null)
                            removed--;
                        else
                            services[i - removed] = temp[i];
                    }
                }
            }
            return services.Length == 0 ? null : services;
        }

        /// <summary>
        /// 返回服务编号
        /// </summary>
        /// <returns>Int</returns>
        public int GetNextServiceId()
        {
            int id = this._serviceId;
            id++;
            return id;
        }

        /// <summary>
        /// Creates the domain.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>AppDomain.</returns>
        public AppDomain CreateDomain(IBundleContext context)
        {
            string binpatch = Path.GetDirectoryName(context.Bundle.Location);
            AppDomainSetup info = new AppDomainSetup();
            if (binpatch != null)
            {
                var dirinfo = new DirectoryInfo(binpatch);
                if (String.Equals(dirinfo.Name.ToLower(), "bin"))
                {
                    if (dirinfo.Parent != null)
                        info.ApplicationBase = dirinfo.Parent.FullName;
                }
                else
                {
                    info.ApplicationBase = binpatch;
                }
            }

            if (!String.IsNullOrEmpty(AppDomain.CurrentDomain.DynamicDirectory))
            {
                info.ShadowCopyDirectories = Path.Combine(info.ApplicationBase, @"cache");
                info.ShadowCopyFiles = "true";
                info.PrivateBinPath = binpatch;
            }

            string domainName = "Bundle-" + context.Bundle.Id.ToString().PadLeft(3, '0');
            AppDomain domain = AppDomain.CreateDomain(domainName, AppDomain.CurrentDomain.Evidence, info);
            Interlocked.Increment(ref this._bundleAppDomains);
            //Assembly[] asses = domain.GetAssemblies();
            return domain;
        }

        /// <summary>
        /// Unloads the domain.
        /// </summary>
        /// <param name="domain">The domain.</param>
        public void UnloadDomain(AppDomain domain)
        {
            if (domain != null)
            {
                AppDomain.Unload(domain);
                Interlocked.Decrement(ref this._bundleAppDomains);
            }
        }
    }
}