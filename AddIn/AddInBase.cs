using System;

namespace Syncfusion.Addin
{
    /// <summary>
    /// Class AddInBase.
    /// </summary>
    [Serializable]
    public abstract class AddInBase : IBundleActivator
    {
        #region IBundleActivator Members

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="context"></param>
        public abstract void Start(IBundleContext context);

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="context"></param>
        public abstract void Stop(IBundleContext context);

        #endregion IBundleActivator Members
    }
}