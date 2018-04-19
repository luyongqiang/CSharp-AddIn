using Syncfusion.Addin.Configuration;
using Syncfusion.Addin.Configuration.Plugin;
using Syncfusion.Addin.Core.Reflection;
using Syncfusion.Addin.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.Compilation;

namespace Syncfusion.Addin.Core
{
    /// <summary>
    /// Plugin Start
    /// </summary>
    /// <example>
    /// using Syncfusion.Addin.Core;
    /// using(BundleRuntime runtime=new BundleRuntime())
    /// {
    /// runtime.Start();//插件启动
    /// }
    /// </example>
    public class BundleRuntime : IDisposable
    {
        private bool _isDisposed;
        private bool _started;
        private BundleRuntimeState _state;
        public BundleRuntimeType BundleRuntimeType { get; set; }

        #region private member

        /// <summary>
        /// Bundle datas
        /// </summary>
        private readonly Dictionary<BundleData, List<Assembly>> _registeredBunldeCache = new Dictionary<BundleData, List<Assembly>>();

        private static IList<Assembly> _mTopLevelReferencedAssemblies;

        private static readonly List<String> MPluginDirs = new List<string>();

        public static string CultureInfoLanguage = "zh-CN";

        #endregion private member

        /// <summary>
        /// 单例模式
        /// </summary>
        public static BundleRuntime Instance
        {
            get;
            internal set;
        }

        public string[] StartArgs
        {
            get;
            private set;
        }

        public IFramework Framework
        {
            get;
            private set;
        }

        public BundleRuntimeState State
        {
            get
            {
                return this._state;
            }
            private set
            {
                this._state = value;
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        void IDisposable.Dispose()
        {
            if (this._isDisposed)
            {
                return;
            }
            GC.SuppressFinalize(this);
            this.State = BundleRuntimeState.Disposed;
            this._isDisposed = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BundleRuntime"/> class.
        /// </summary>
        public BundleRuntime() : this(new string[] { "Plugins" })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BundleRuntime"/> class.
        /// </summary>
        /// <param name="pluginsPath">The plugins path.</param>
        public BundleRuntime(string pluginsPath) : this(new string[] { pluginsPath }) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BundleRuntime"/> class.
        /// </summary>
        /// <param name="pluginsPathList">The plugins path list.</param>
        /// <exception cref="System.Exception">
        /// </exception>
        public BundleRuntime(string[] pluginsPathList)
        {
            if (Instance != null)
            {
                throw new Exception(Properties.Resources.ResourceManager.GetString("SingletonNO"));
            }
            if (pluginsPathList == null || pluginsPathList.Length == 0)
            {
                throw new Exception(Properties.Resources.ResourceManager.GetString("AtLeastOnePluginsPathMustBeSpecified"));
            }
            GetPluginDirs();
            Instance = this;
            this.Framework = new Framework();
            BundleRuntimeType = BundleRuntimeType.CsNet;
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = "FrameworkThread";
            }
            this.Start(null);
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="args">参数</param>
        public void Start(string[] args)
        {
            if (this._started)
            {
                return;
            }
            this.StartArgs = args;
            this.State = BundleRuntimeState.Starting;
            try
            {
                this.Framework.Launch();

                //加载配置文件和组件
                GetBundleDatas(_registeredBunldeCache);
                //根据组件信息 安装 组件
                InstallAllBundlesByBundleData(_registeredBunldeCache);
                //启动所有已经安装的组件
                StartAllBundles();

                switch (BundleRuntimeType)
                {
                    case BundleRuntimeType.AspNet:
                        GetHostAssembly();
                        break;
                }

                this.State = BundleRuntimeState.Started;
                this._started = true;
            }
            catch (Exception ex)
            {
                this.State = BundleRuntimeState.Stopped;
                this._started = false;
                FileLogUtility.Error("start  is Error!" + ex.Message);
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (!this._started)
            {
                return;
            }
            this.State = BundleRuntimeState.Stopping;
            try
            {
                this.Framework.Shutdown();
            }
            catch (Exception ex)
            {
                FileLogUtility.Error("StopTheFrameworkFailed" + ex.Message);
            }
            this.State = BundleRuntimeState.Stopped;
            this._started = false;
        }

        /// <summary>
        /// get Verison
        /// </summary>
        public static string Verison
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Get Bundle Datas
        /// </summary>
        /// <param name="bunldeCache">Bundle Cache</param>
        internal void GetBundleDatas(Dictionary<BundleData, List<Assembly>> bunldeCache)
        {
            List<BundleData> dirs = PluginXmlProcess.GetBundleDatas();
            //安装所有组件
            if (dirs.Count > 0)
            {
                foreach (BundleData t in dirs.OrderBy(o => o.StartLevel))
                {
                    List<Assembly> lass = new List<Assembly>();

                    bunldeCache.Add(t, lass);
                }
            }
        }

        internal List<String> PluginDirs
        {
            get { return MPluginDirs; }
        }

        internal String BaseDir
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        private static void GetPluginDirs()
        {
            String path = AppDomain.CurrentDomain.BaseDirectory + "Plugins";//获取当前程序的插件目录
            if (System.IO.Directory.Exists(path))
            {
                MPluginDirs.AddRange(System.IO.Directory.GetDirectories(path));
            }
        }

        #region 安装组件

        /// <summary>
        /// Install All Bundles By BundleData
        /// </summary>
        /// <param name="bunldeCache">bunldeCache</param>
        internal void InstallAllBundlesByBundleData(Dictionary<BundleData, List<Assembly>> bunldeCache)
        {
            foreach (KeyValuePair<BundleData, List<Assembly>> value in bunldeCache)
            {
                try
                {
                    LoadBundleForBundleData(value.Key);
                }
                catch (Exception ex)
                {
                    FileLogUtility.Debug(ex.Message);
                    continue;
                }
            }
        }

        /// <summary>
        /// load Bundle by BundleData info
        /// </summary>
        /// <param name="bd">BundleData info </param>
        private void LoadBundleForBundleData(BundleData bd)
        {
            if (bd.Runtime == null)
                return;
            if (null != bd.Runtime.Assemblie)
            {
                string location = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, bd.Runtime.Assemblie.AssemblyPatch);
                //Install Bundle
                this.Framework.InstallBundle(location, bd);

                Assembly assembly = GetAssemblyForLocation(location);
                if (assembly != null)
                {
                    AttributeInfo[] attributes = ReflectionUtil.GetCustomAttributes(assembly, typeof(AddInAttribute));
                    if (!attributes.Any())
                    {
                        AppDomain.CurrentDomain.CreateInstanceFrom(location, assembly.FullName);
                    }
                }
            }
        }

        #endregion 安装组件

        #region 启动组件

        //加载程序集
        /// <summary>
        /// load Assembly For Location
        /// </summary>
        /// <param name="location">Assembly file full path</param>
        /// <returns></returns>
        private Assembly GetAssemblyForLocation(string location)
        {
            Assembly ass = Assembly.LoadFrom(FileHelper.FileCopyToDynamicDirectory(location));
            return ass;
        }

        /// <summary>
        /// Start All Bundles
        /// </summary>
        internal void StartAllBundles()
        {
            IBundle[] bundles = this.Framework.Bundles.GetBundles();
            foreach (IBundle bundle in bundles)
            {
                if (bundle.State == BundleState.Starting || bundle.State == BundleState.Active)
                {
                    continue;
                }
                //check bundle`s Dependencies and state
                if (null == bundle.DataInfo.Runtime || null == bundle.DataInfo.Runtime.Dependencies || bundle.DataInfo.Runtime.Dependencies.Count < 1)
                {
                    StartBundle(bundle);
                }
                else
                {
                    for (int i = 0; i < bundle.DataInfo.Runtime.Dependencies.Count; i++)
                    {
                        StartBundleByDependencie(bundle.DataInfo.Runtime.Dependencies[i]);
                    }
                    StartBundle(bundle);
                }
            }
        }

        /// <summary>
        /// Start Dependencie Bundles(note:Recursive)
        /// </summary>
        /// <param name="depenData">DependencyData</param>
        private void StartBundleByDependencie(DependencyData depenData)
        {
            IBundle[] bundles = this.Framework.Bundles.GetBundles(depenData.BundleSymbolicName);
            if (null != bundles && bundles.Length > 0)
            {
                for (int i = 0; i < bundles.Length; i++)
                {
                    if (!(bundles[i].State == BundleState.Starting || bundles[i].State == BundleState.Active))
                    {
                        if (null != bundles[i].DataInfo.Runtime.Dependencies && bundles[i].DataInfo.Runtime.Dependencies.Count > 0)
                        {
                            for (int index = 0; index < bundles[i].DataInfo.Runtime.Dependencies.Count; index++)
                            {
                                StartBundleByDependencie(bundles[i].DataInfo.Runtime.Dependencies[index]);
                            }
                        }
                        else
                        {
                            StartBundle(bundles[i]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Start Bundle and add Assembly to TopLevelReferencedAssemblies
        /// (note: web application need Assembly to TopLevelReferencedAssemblies)
        /// </summary>
        /// <param name="bundle">bundle</param>
        private void StartBundle(IBundle bundle)
        {
            Assembly ass = null;
            //加载组件程序集
            if (!File.Exists(bundle.Location))
                return;
            ass = GetAssemblyForLocation(bundle.Location);
            //启动组件
            this.Framework.StartBundle(bundle);
            //将web项目的dll加入到动态编译区
            if (null != _mTopLevelReferencedAssemblies &&
                bundle.DataInfo.Runtime.Assemblie.IsWeb)
            {
                _mTopLevelReferencedAssemblies.Add(ass);
            }
        }

        #endregion 启动组件

        #region Web

        /// <summary>
        /// Get Current web application`s TopLevelReferencedAssemblies
        /// </summary>
        internal void GetHostAssembly()
        {
            PropertyInfo buildManagerProp = typeof(BuildManager).GetProperty("TheBuildManager",
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty);
            if (buildManagerProp != null)
            {
                BuildManager buildManager = buildManagerProp.GetValue(null, null) as BuildManager;
                if (buildManager != null)
                {
                    PropertyInfo toplevelAssembliesProp = typeof(BuildManager).GetProperty("TopLevelReferencedAssemblies", BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.GetProperty);
                    if (toplevelAssembliesProp != null)
                    {
                        _mTopLevelReferencedAssemblies = toplevelAssembliesProp.GetValue(buildManager, null) as IList<Assembly>;
                    }
                    else
                    {
                        throw new Exception(Properties.Resources.ResourceManager.GetString("TopAssemlyError"));
                    }
                }
            }
        }

        #endregion Web
    }
}