using Syncfusion.Addin.Core.Metadata;
using System;
using System.Collections.Generic;
using System.IO;

namespace Syncfusion.Addin.Utility
{
    /// <summary>
    /// 文件帮助
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// File Copy To Dynamic Directory
        /// </summary>
        /// <returns>File path</returns>
        public static string FileCopyToDynamicDirectory(string location)
        {
            string result = "";
            string dynamicDirectory = AppDomain.CurrentDomain.DynamicDirectory;
            FileInfo fi = new FileInfo(location);
            result = dynamicDirectory + "\\" + fi.Name;

            try
            {
                if (!String.IsNullOrEmpty(dynamicDirectory))
                {
                    File.Copy(location, result, true);
                }
                else
                {
                    result = location;
                }
            }
            catch (Exception e)
            {
                FileLogUtility.Error(string.Format("{0}:{1}", e.Message, "FileCopyToDynamicDirectory"));
            }
            return result;
        }

        /// <summary>
        /// Copy Files
        /// </summary>
        /// <param name="sourcedir">source dir</param>
        /// <param name="targetdir">target dir</param>
        public static void CopyFiles(string sourcedir, string targetdir)
        {
            string dir = Directory.GetParent(sourcedir).FullName;// fi.Directory.Name;
            string[] files = Directory.GetFiles(dir);
            if (null != files && files.Length > 0)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo fi = new FileInfo(files[i]);
                    File.Copy(files[i], targetdir + "\\" + fi.Name);
                }
            }
        }

        /// <summary>
        /// Search Dir for web application`s "Plugins"
        /// </summary>
        /// <returns></returns>
        public static List<string> SearchDir()
        {
            List<string> result = new List<string>();

            result.Add(AppDomain.CurrentDomain.BaseDirectory);

            string[] dirs = System.IO.Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "Plugins");
            result.AddRange(dirs);

            return result;
        }

        /// <summary>
        /// Get Component Directory
        /// </summary>
        /// <param name="filepatch">file patch</param>
        /// <returns></returns>
        private static string GetComponentDirectory(string filepatch)
        {
            string result = string.Empty;
            int index = filepatch.LastIndexOf("\\") + 1;
            if (!string.IsNullOrEmpty(filepatch))
            {
                result = filepatch.Substring(0, index);
            }
            return result;
        }

        /// <summary>
        /// Search Bundle`s *.addin file
        /// </summary>
        /// <returns>错误返回NULL</returns>
        public static AddinMetadata SearchXml(string dirName, string addinFile)
        {
            string xmlpatch = dirName + "\\" + addinFile;
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
                return null;
            }
        }
    }
}