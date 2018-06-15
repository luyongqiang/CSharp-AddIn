using System.Collections.Generic;

namespace Syncfusion.Addin.Configuration.Plugin
{
    /// <summary>
    /// 运行数据信息
    /// </summary>
    public class RuntimeData
    {
        /// <summary>
        /// 依赖对像信息
        /// </summary>
        public List<DependencyData> Dependencies
        {
            get;
            private set;
        }

        /// <summary>
        /// 模块信息
        /// </summary>
        public AssemblyData Assemblie
        {
            get;
            private set;
        }

        /// <summary>
        /// 设置程序集信息
        /// </summary>
        /// <param name="assembly">程序集数据元素</param>
        public void SetAssembly(AssemblyData assembly)
        {
            Assemblie = assembly;
        }

        /// <summary>
        /// 依赖对像信息
        /// </summary>
        /// <param name="newItem">DependencyData </param>
        public void AddDependency(DependencyData newItem)
        {
            if (null == Dependencies)
            {
                Dependencies = new List<DependencyData>();
            }
            Dependencies.Add(newItem);
        }
    }
}