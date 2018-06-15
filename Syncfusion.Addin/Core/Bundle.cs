using Syncfusion.Addin.Configuration.Plugin;
using Syncfusion.Addin.Core.Reflection;
using Syncfusion.Addin.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Syncfusion.Core;

namespace Syncfusion.Addin.Core
{
    /// <summary>
    /// Summary description for Bundle.
    /// </summary>
    public class Bundle : IBundle
    {
        #region --- Fields ---

        private readonly Framework _framework;
        private readonly BundleData _bundleData;
        protected BundleState _state;
        private Int32 id;
        private Assembly _assembly;
        private AppDomain _domain;

        //private IBundleActivator activator;
        private IBundleActivator[] _activators;

        private readonly string _location;
        private readonly string _symbolicName;
        private IBundleContext _context;
        private DirectoryInfo _storage;
        private string _dynamicDirectory;
        private string _dynamicFile;

        #endregion --- Fields ---

        #region --- Properties ---

        public string BundleDynamicDirectory { get { return _dynamicDirectory; } }
        public string BundleDynamicFile { get { return _dynamicFile; } }

        public BundleState State
        {
            get
            {
                return _state;
            }
        }

        public Int32 Id
        {
            get
            {
                return id;
            }
        }

        public string Location
        {
            get
            {
                return _location;
            }
        }

        public Framework Framework
        {
            get
            {
                if (_framework == null)
                {
                    throw new NullReferenceException("Framework is null.");
                }
                return _framework;
            }
        }

        public IBundleContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new BundleContext(this);
                }

                return _context;
            }
        }

        public BundleData DataInfo { get { return _bundleData; } }

        private IBundleActivator[] Acitvators
        {
            get
            {
                if (_activators == null)
                {
                    // Look for the activator attribute
                    AttributeInfo[] attributes = ReflectionUtil.GetCustomAttributes(_assembly, typeof(AddInAttribute));
                    if (attributes == null || attributes.Length <= 0)
                    {
                        //TracesProvider.TracesOutput.OutputTrace("No activator found");
                        return null;
                    }
                    else
                    {
                        HashSet<IBundleActivator> activatorSet = new HashSet<IBundleActivator>();
                        foreach (AttributeInfo attribute in attributes)
                        {
                            string typeName = attribute.Owner.FullName;
                            //  object obj1  = domain.CreateInstanceAndUnwrap(assembly.FullName, typeName); //原先代码

                            //
                            System.Runtime.Remoting.ObjectHandle objHandle = _domain.CreateInstanceFrom(_assembly.Location, typeName);
                            object objBuild = objHandle.Unwrap();

                            if (objBuild == null)
                                continue;
                            //todoo 会锁定文件
                            //object obj = domain.CreateInstanceFromAndUnwrap(location, typeName);
                            IBundleActivator proxy = objBuild as IBundleActivator;
                            //动态代理
                            // IBundleActivator proxy = (IBundleActivator)DynamicProxyFactory.Instance.CreateProxy(obj, new InvocationDelegate(InvocationHandler));
                            if (null != proxy)
                            {
                                activatorSet.Add(proxy);
                            }
                        }
                        _activators = new IBundleActivator[activatorSet.Count];
                        activatorSet.CopyTo(this._activators);
                    }
                }

                return _activators;
            }
        }

        internal Assembly Assembly
        {
            get
            {
                return _assembly;
            }
            set
            {
                _assembly = value;
            }
        }

        public string Version
        {
            get
            {
                return _bundleData.Version;
            }
        }

        #endregion --- Properties ---

        internal Bundle(BundleData bundleData, Framework framework)
        {
            id = bundleData.Id;
            _storage = new DirectoryInfo(bundleData.Location);
            this._framework = framework;
            this._bundleData = bundleData;
            _location = bundleData.Location;
            _symbolicName = Path.GetFileNameWithoutExtension(bundleData.Location);
            _state = BundleState.Installed;
            SetDynamicInfo();
        }

        /// <summary>
        /// Sets the dynamic information.
        /// </summary>
        private void SetDynamicInfo()
        {
            if (!String.IsNullOrEmpty(AppDomain.CurrentDomain.DynamicDirectory))
            {
                string ddir = Path.Combine(AppDomain.CurrentDomain.DynamicDirectory, Guid.NewGuid().ToString());
                if (!Directory.Exists(ddir))
                {
                    Directory.CreateDirectory(ddir);
                }
                else
                {
                    ddir = Path.Combine(ddir, Guid.NewGuid().ToString());
                    Directory.CreateDirectory(ddir);
                }
                _dynamicDirectory = ddir;

                FileInfo fi = new FileInfo(_location);
                _dynamicFile = _dynamicDirectory + "\\" + fi.Name;
            }
        }

        private static object InvocationHandler(object target, MethodBase method, object[] parameters)
        {
            Debug.WriteLine("Before: " + method.Name);

            object result = method.Invoke(target, parameters);

            Debug.WriteLine("After: " + method.Name);

            return result;
        }

        /// <summary>
        /// Start Bundle
        /// </summary>
        /// <exception cref="Syncfusion.Addin.BundleException">No activator for:  +Location</exception>
        public virtual void Start()
        {
            try
            {
                _state = BundleState.Starting;

                EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Starting, this));

                _domain = _framework.CreateDomain(this.Context);
                Debug.Assert(_domain != null, "Bundle AppDomain can't be set to null.");

                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolve);

                _assembly = Assembly.LoadFrom(FileHelper.FileCopyToDynamicDirectory(_location));

                Debug.Assert(_assembly != null, "Bundle Assembly can's be set to null.");

                foreach (IBundleActivator activator in Acitvators)
                {
                    if (activator == null)
                    {
                        throw new BundleException("No activator for: " + Location);
                    }

                    activator.Start(this.Context);
                }
                if (_activators == null)
                {
                    FileLogUtility.Error(string.Format("{0}:{1}:{2}", DateTime.Now, this.GetType().FullName, "Start().Method()   Not find Acitvators! 未找到插件启动入口！"));
                }
                _state = BundleState.Active;

                EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Started, this));
            }
            catch (Exception ex)
            {
                _state = BundleState.Installed;
                FileLogUtility.Error(string.Format("{0}:{1}", ex.Message, "Bundle.Start()"));
            }
        }

        private string GetAssemblyPatch(string filepatch)
        {
            string result = string.Empty;
            int index = filepatch.LastIndexOf("\\") + 1;
            if (index < 1)
            {
                index = filepatch.LastIndexOf("/") + 1;
            }
            result = filepatch.Substring(0, index);

            return result;
        }

        /// <summary>
        /// Search Assembly By Directory
        /// </summary>
        /// <param name="assemblyName">assemblyName</param>
        /// <param name="directorys">plugin directorys</param>
        /// <returns></returns>
        private static string SearchAssemblyByDirectory(String assemblyName, List<String> directorys)
        {
            List<String> filePaths = new List<string>();
            for (int i = 0; i < directorys.Count; i++)
            {
                String[] paths = Directory.GetFiles(directorys[i], assemblyName + ".dll", SearchOption.TopDirectoryOnly);
                if (null != paths)
                {
                    filePaths.AddRange(paths);
                }
                String path = directorys[i] + "\\bin";
                if (Directory.Exists(path))
                {
                    paths = Directory.GetFiles(path, assemblyName + ".dll", SearchOption.TopDirectoryOnly);

                    if (null != paths)
                    {
                        filePaths.AddRange(paths);
                    }
                }
                if (filePaths != null && filePaths.Count > 0)
                {
                    return filePaths[0];
                }
            }
            return null;
        }

        //查询程序
        private static string SearchAssembly(string assemblyName, List<String> dirs)
        {
            string assemblyPath = SearchAssemblyByDirectory(assemblyName, dirs);
            return assemblyPath;
        }

        /// <summary>
        /// Assemblies the resolve.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="ResolveEventArgs"/> instance containing the event data.</param>
        /// <returns>Assembly.</returns>
        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            List<String> dirs = BundleRuntime.Instance.Framework.Bundles.BundlePaths;
            AssemblyName name = new AssemblyName(args.Name);
            string assemblyFile = SearchAssembly(name.Name, dirs);
            if (!String.IsNullOrEmpty(assemblyFile) && File.Exists(assemblyFile))
            {
                return Assembly.LoadFrom(FileHelper.FileCopyToDynamicDirectory(assemblyFile));
            }
            return null;
        }

        public virtual void Stop()
        {
            try
            {
                _state = BundleState.Stopping;
                EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Stopping, this));

                foreach (IBundleActivator activator in Acitvators)
                {
                    if (activator == null)
                    {
                        throw new Exception("No activator for: " + Location);
                    }

                    activator.Stop(this.Context);
                }
                _activators = null;
                _framework.UnloadDomain(_domain);
            }
            catch (Exception ex)
            {
                FileLogUtility.Error(string.Format("{0}:{1}", ex.Message, "Bundle. Stop()"));
                throw new BundleException(ex.Message, ex);
            }

            _state = BundleState.Installed;
            EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Stopped, this));
        }

        /// <summary>
        /// Bundle Uninstall
        /// </summary>
        public void Uninstall()
        {
            _framework.UninstallBundle(id);
            _state = BundleState.Uninstalled;
            EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Uninstalled, this));
        }

        #region IBundle Members

        /// <summary>
        /// Bundle Update
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Update()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Bundle Update with Stream
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Update(Stream inputStream)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Bundle GetProperties
        /// </summary>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Dictionary<string, object> GetProperties()
        {
            try
            {
                return _framework.GetPropertyValue<Dictionary<string, object>>(Location);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get Registered Services
        /// </summary>
        /// <returns>IServiceReference[].</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IServiceReference[] GetRegisteredServices()
        {
            return _framework.ServiceRegistry.LookupServiceReferences(this.Context);
        }

        /// <summary>
        /// Get Services In Use
        /// </summary>
        /// <returns>IServiceReference[].</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IServiceReference[] GetServicesInUse()
        {
            return _framework.ServiceRegistry.LookupServiceReferences(Context);
        }

        /// <summary>
        /// GetResource
        /// </summary>
        /// <param name="name">Resource Name</param>
        /// <returns>Object</returns>
        public Uri GetResource(string name)
        {
            Assembly assembly = _assembly;
            if (assembly != null)
            {
                var manifestResourceInfo = assembly.GetManifestResourceInfo(name);
                if (manifestResourceInfo != null)
                    return new Uri(manifestResourceInfo.ResourceLocation.ToString());
            }
            return null;
        }

        /// <summary>
        /// Get Properties
        /// </summary>
        /// <param name="locale">文件</param>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Dictionary<string, object> GetProperties(string locale)
        {
            try
            {
                return Assembly.GetPropertyValue<Dictionary<string, object>>(locale);
            }
            catch
            {
                return null;
            }
        }

        public object LoadResource(string resourceName)
        {
            Assembly assembly = _assembly;
            if (assembly != null)
            {
                return assembly.GetManifestResourceStream(resourceName);
            }
            return null;
        }

        /// <summary>
        /// Symbolic Name
        /// </summary>
        /// <value>The name of the symbolic.</value>
        public string SymbolicName
        {
            get { return _symbolicName; }
        }

        /// <summary>
        /// Load Class by Name
        /// </summary>
        /// <param name="name">Type name</param>
        /// <returns>Type.</returns>
        public Type LoadClass(string name)
        {
            Assembly assembly = _assembly;
            if (assembly != null)
            {
                return assembly.GetType(name);
            }
            return null;
        }

        /// <summary>
        /// Get Resources
        /// </summary>
        /// <param name="name">Resources name</param>
        /// <returns>System.Object.</returns>
        public object GetResources(object name)
        {
            Assembly assembly = _assembly;
            if (assembly != null)
            {
                return assembly.GetManifestResourceStream(name.ToString());
            }
            return null;
        }

        /// <summary>
        /// Get Entry Paths
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>IEnumerator.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerator GetEntryPaths(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Entry
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>IEnumerator.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerator GetEntry(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Last Modified
        /// </summary>
        /// <returns>System.Int64.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public long GetLastModified()
        {
            return typeof(Bundle).Assembly.ManifestModule.MDStreamVersion;
        }

        /// <summary>
        /// Find Entries
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="filePattern">file Pattern</param>
        /// <param name="recurse">recurse</param>
        /// <returns>IEnumerator.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerator FindEntries(string path, string filePattern, bool recurse)
        {
            throw new NotImplementedException();
        }

        #endregion IBundle Members
    }
}