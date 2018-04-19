using System;

namespace Syncfusion.Addin
{
    /// <summary>
    /// Prototype of the method to be implemented to receive service events.
    /// </summary>
    /// <param name="sender">The event sender</param>
    /// <param name="e">The event argurment <see cref="ServiceEventArgs"/></param>
    public delegate void ServiceEventHandler(object sender, ServiceEventArgs e);

    /// <summary>
    /// 服务事件参数
    /// </summary>
    public class ServiceEventArgs : EventArgs
    {
        private IServiceReference reference;

        private ServiceState state;

        /// <summary>
        /// 服务引用
        /// </summary>
        public IServiceReference Reference
        {
            get { return reference; }
        }

        /// <summary>
        /// 服务状态
        /// </summary>
        public ServiceState State
        {
            get { return state; }
        }

        public ServiceEventArgs(ServiceState state, IServiceReference reference)
        {
            this.reference = reference;
            this.state = state;
        }
    }
}