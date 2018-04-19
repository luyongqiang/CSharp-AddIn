using Syncfusion.Addin.Configuration.Plugin;
using Syncfusion.Addin.Core;
using Syncfusion.Addin.Core.Metadata;
using Syncfusion.Addin.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Syncfusion.Addin.Configuration
{
    /// <summary>
    /// 插件文件读取信息
    /// </summary>
    internal class PluginXmlProcess
    {
        //插件配置文件信息
        private const string AddinFile = "Plugin.addin";

        /// <summary>
        /// Search *.addin for dirName
        /// 搜索文件下面为*.addin 文件
        /// </summary>
        /// <returns>错误返回NULL</returns>
        private static AddinMetadata SearchXml(string dirName)
        {
            string xmlpatch = dirName + "\\" + AddinFile;
            if (!File.Exists(xmlpatch))
            {
                xmlpatch = dirName + "\\bin\\" + AddinFile;

                if (!File.Exists(xmlpatch))
                {
                    xmlpatch = FileName(new DirectoryInfo(dirName));
                }
            }
            if (System.IO.File.Exists(xmlpatch))
            {
                string xml = string.Empty;
                using (StreamReader reader = File.OpenText(xmlpatch))
                {
                    xml = reader.ReadToEnd();
                }
                return (AddinMetadata)XmlConvertor.XmlToObject(typeof(AddinMetadata), xml);
            }
            else
            {
                FileLogUtility.Error($"{dirName}:没有找到合适的插件配置文件");
                return null;
            }
        }

        /// <summary>
        /// Files the name. </summary>
        /// <param name="dir">The dir.</param>
        /// <returns>System.String.</returns>
        private static string FileName(DirectoryInfo dir)
        {
            try
            {
                if (dir == null)
                    return null;
                FileInfo[] files = dir.GetFiles("*.addin");
                if (files.Length > 0)
                {
                    return files[0].FullName;
                }
                if (dir.GetDirectories().Any())
                {
                    return FileName(dir.GetDirectories()[0]);
                }
            }
            catch (IOException ex)
            {
                FileLogUtility.Error($"{ex.Source}:无效的插件！请联系软件开发商！");
            }

            return null;
        }

        /// <summary>
        /// Get Xml For BundleData
        /// </summary>
        /// <param name="dirName">Name of the dir.</param>
        /// <returns>BundleData.</returns>
        internal static BundleData GetXmlForBundleData(string dirName)
        {
            BundleData result = new BundleData();
            //获取配置信息
            AddinMetadata metadata = SearchXml(dirName);
            //将配置信息加入组件信息中
            if (null != metadata)
            {
                try
                {
                    result.Name = metadata.Name;
                    result.SymbolicName = metadata.Name;
                    result.Path = metadata.Path;
                    result.Runtime = GetRuntimeDataForRuntimeData(metadata);
                    result.Extensions = GetXmlForExtensionData(metadata);
                    result.PageSerivce = GetXmlForPageServiceData(metadata);
                    result.Enable = metadata.Enabled;
                    result.Immediate = metadata.Immediate;
                    result.Description = metadata.Description;
                    result.Activator = GetActivatorData(metadata);
                    result.BundleInfo = GetBundleInfoData(metadata);
                    result.StartLevel = metadata.StartLevel;
                    result.Company = metadata.Company;
                    result.Copyright = metadata.Copyright;
                    result.Product = metadata.Product;
                    result.AppSettings = metadata.AppSettings;
                    if (metadata.AssemblyVersion == null)
                    {
                        metadata.AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    }
                    result.Version = metadata.AssemblyVersion;
                    if (metadata.ApplicationMenu != null && !string.IsNullOrEmpty(metadata.ApplicationMenu.ApplicationIco))
                    {
                        result.ApplicationIco = metadata.ApplicationMenu.ApplicationIco;
                    }
                }
                catch (ArgumentNullException ex)
                {
                    FileLogUtility.Error(dirName + ex.Message);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取组件信息
        /// </summary>
        /// <param name="metadata">数据元素</param>
        /// <returns>BundleInfoData</returns>
        private static BundleInfoData GetBundleInfoData(AddinMetadata metadata)
        {
            BundleInfoData bundle = new BundleInfoData();
            bundle.AssemblyVersion = metadata.AssemblyVersion;
            bundle.Company = metadata.Company;
            bundle.Copyright = metadata.Copyright;
            bundle.ContactAddress = metadata.Path;
            bundle.Description = metadata.Description;
            bundle.Title = metadata.Title;
            return bundle;
        }

        /// <summary>
        /// 获取插件加载信息
        /// </summary>
        /// <param name="metadata">数据元素</param>
        /// <returns>ActivatorData</returns>
        private static ActivatorData GetActivatorData(AddinMetadata metadata)
        {
            ActivatorData activator = new ActivatorData();
            activator.Policy = metadata.Immediate ? ActivatorPolicy.Immediate : ActivatorPolicy.Lazy;
            return activator;
        }

        /// <summary>
        /// 获取XML扩张数据信息
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>ExtensionData</returns>
        private static ExtensionData GetXmlForExtensionData(AddinMetadata metadata)
        {
            ExtensionData dd = new ExtensionData();
            //获取配置信息
            //将配置信息加入组件信息中
            if (null != metadata.ApplicationMenu)
            {
                var menuItem = metadata.ApplicationMenu.Items.Where(o => o.Language == BundleRuntime.CultureInfoLanguage);
                var enumerable = menuItem as ExtendsMenu[] ?? menuItem.ToArray();
                if (enumerable.Any())
                {
                    dd.ApplicationExtends.ApplicationIco = metadata.ApplicationMenu.ApplicationIco;
                    foreach (var extendsMenu in enumerable)
                    {
                        dd.ApplicationExtends.ApplicationName = extendsMenu.ApplicationName;
                        dd.ApplicationExtends.Language = extendsMenu.Language;
                        if (extendsMenu.Items.Any())
                        {
                            foreach (var de in extendsMenu.Items)
                            {
                                List<LoadOtherContainerPanles> loadOther = null;
                                if (de.Items != null)
                                {
                                    loadOther = de.Items.ToList();
                                }
                                dd.ApplicationExtends.MenuList.Add(new ExtendionMenu()
                                {
                                    Caption = de.Caption,
                                    ClassForm = de.ClassForm,
                                    Group = de.Group,
                                    MenuIco = de.MenuIco,
                                    ToolTip = de.ToolTip,
                                    PluginDockStyle = de.DockType,
                                    Width = de.Width,
                                    High = de.High,
                                    Sort = de.Sort,
                                    LoadOtherContainer = loadOther
                                });
                            }
                        }
                    }
                }
            }

            return dd;
        }

        /// <summary>
        /// Gets the XML for page service data.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>PageServiceData.</returns>
        private static PageServiceData GetXmlForPageServiceData(AddinMetadata metadata)
        {
            PageServiceData page = new PageServiceData();
            if (null != metadata.PageService)
            {
                page.PageServicePoint = metadata.PageService.ServicePoint;
                foreach (var pageServiceData in metadata.PageService.Items)
                {
                    page.PageNodeList.Add(new PageNode()
                    {
                        PageName = pageServiceData.PageName,
                        PagePlguinClassValue = pageServiceData.PagePlguinClassValue,
                        PageType = pageServiceData.PageType
                    });
                }
            }
            return page;
        }

        /// <summary>
        /// Get Runtime Data For RuntimeData
        /// </summary>
        /// <returns></returns>
        private static RuntimeData GetRuntimeDataForRuntimeData(AddinMetadata metadata)
        {
            RuntimeData result = new RuntimeData();
            GetDependencyForRuntimeData(result, metadata);
            GetAssemblyDataForRuntimeData(result, metadata);
            return result;
        }

        /// <summary>
        /// Get Dependency For RuntimeData
        /// </summary>
        /// <returns></returns>
        private static void GetDependencyForRuntimeData(RuntimeData runtimeData, AddinMetadata metadata)
        {
            if (null != metadata.Runtime.Items1 && metadata.Runtime.Items1.Length > 0)
            {
                foreach (Dependency de in metadata.Runtime.Items1)
                {
                    DependencyData dd = new DependencyData();
                    dd.AssemblyName = de.AssemblyName;
                    dd.BundleSymbolicName = de.BundleSymbolicName;
                    runtimeData.AddDependency(dd);
                }
            }
        }

        /// <summary>
        /// Get Assembly Data For RuntimeData
        /// </summary>
        /// <returns></returns>
        private static void GetAssemblyDataForRuntimeData(RuntimeData runtimeData, AddinMetadata metadata)
        {
            if (null != metadata.Runtime.Items && metadata.Runtime.Items.Length > 0)
            {
                foreach (ImportInfo de in metadata.Runtime.Items)
                {
                    AssemblyData dd = new AssemblyData();
                    dd.AssemblyPatch = de.assembly;
                    dd.IsWeb = de.isweb;
                    runtimeData.SetAssembly(dd);
                }
            }
        }

        /// <summary>
        /// Get Bundle Datas
        /// </summary>
        /// <returns></returns>
        internal static List<BundleData> GetBundleDatas()
        {
            List<BundleData> bundledatas = new List<BundleData>();
            List<string> dirs = FileHelper.SearchDir();

            for (int index = 0; index < dirs.Count; index++)
            {
                BundleData bd = GetXmlForBundleData(dirs[index]);
                if (null != bd)
                {
                    bundledatas.Add(bd);
                }
            }

            return bundledatas;
        }
    }
}