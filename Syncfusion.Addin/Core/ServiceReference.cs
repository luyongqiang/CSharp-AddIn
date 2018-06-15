using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Addin.Core
{
    /// <summary>
    /// 服务引用
    /// </summary>
    public class ServiceReference : IServiceReference
    {
        /// <summary>
        /// The registration
        /// </summary>
        private ServiceRegistration registration = null;

        /// <summary>
        /// The bundle
        /// </summary>
        private IBundle bundle = null;

        /// <summary>
        /// Gets the registration.
        /// </summary>
        /// <value>The registration.</value>
        public ServiceRegistration Registration
        {
            get
            {
                return registration;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceReference"/> class.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <param name="bundle">The bundle.</param>
        public ServiceReference(ServiceRegistration registration, IBundle bundle)
        {
            this.registration = registration;
            this.bundle = bundle;
        }

        #region IServiceReference Members

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>Object</returns>
        public object GetProperty(string key)
        {
            if (registration.Properties != null)
            {
                return registration.Properties[key];
            }
            return null;
        }

        /// <summary>
        /// 获取所有属性
        /// </summary>
        /// <returns>数组</returns>
        public string[] GetPropertyKeys()
        {
            return registration.Properties.Keys.ToArray<string>();
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public Dictionary<string, object> Properties
        {
            get { return registration.Properties; }
        }

        /// <summary>
        /// 获取组件信息
        /// </summary>
        /// <returns>IBundle</returns>
        public IBundle GetBundle()
        {
            return bundle;
        }

        /// <summary>
        /// 获取使用的组件信息
        /// </summary>
        /// <returns>IBundle[]</returns>
        public IBundle[] GetUsingBundles()
        {
            return registration.GetUsingBundles();
        }

        /// <summary>
        /// 获取类的信息
        /// </summary>
        /// <returns>string[]</returns>
        public string[] GetClasses()
        {
            return registration.Classes;
        }

        public bool IsAssignableTo(IBundle requester, string className)
        {
            // Always return true if the requester is the same as the provider.
            // if (requester == bundle)
            // {
            return true;
            // }

            // Boolean flag.
            //  bool allow = true;

            // return allow;
        }

        #endregion IServiceReference Members
    }
}