using System;

namespace Syncfusion.Addin
{
    /// <summary>
    /// Class FrameworkEventArgs.
    /// </summary>
    public class FrameworkEventArgs : EventArgs
    {
        private IBundle bundle;
        private FrameworkState state;
        private Exception exception;

        /// <summary>
        /// 组件对象
        /// </summary>
        public IBundle Bundle
        {
            get { return bundle; }
        }

        /// <summary>
        /// 组件框架状态
        /// </summary>
        public FrameworkState State
        {
            get { return state; }
        }

        /// <summary>
        /// 异常
        /// </summary>
        public Exception Exception
        {
            get { return exception; }
        }

        /// <summary>
        /// 框架事件
        /// </summary>
        /// <param name="bundle">组件对象</param>
        /// <param name="state">状态</param>
        /// <param name="exception">异常信息</param>
        public FrameworkEventArgs(IBundle bundle, FrameworkState state,
            Exception exception)
        {
            this.bundle = bundle;
            this.state = state;
            this.exception = exception;
        }
    }
}