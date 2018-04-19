using System;

namespace Syncfusion.Addin
{
    /// <summary>
    /// Class BundleEventArgs.
    /// </summary>
    public class BundleEventArgs : EventArgs
    {
        /// <summary>
        /// 组件对象
        /// </summary>
        private IBundle bundle;

        /// <summary>
        /// 当前状态
        /// </summary>
        private BundleTransition transition;

        /// <summary>
        /// 组件对象
        /// </summary>
        public IBundle Bundle
        {
            get { return bundle; }
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        public BundleTransition Transition
        {
            get { return transition; }
        }

        public BundleEventArgs(BundleTransition transition, IBundle bundle)
        {
            this.bundle = bundle;
            this.transition = transition;
        }
    }
}