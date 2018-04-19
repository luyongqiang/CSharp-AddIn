using Syncfusion.Addin.Configuration.Plugin;
using Syncfusion.Addin.Utility;
using System;
using System.Collections.Generic;
using System.IO;

namespace Syncfusion.Addin.Core
{
    /// <summary>
    /// Class SystemBundle. This class cannot be inherited.
    /// </summary>
    [Serializable]
    internal sealed class SystemBundle : Bundle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemBundle"/> class.
        /// </summary>
        /// <param name="framework">The framework.</param>
        public SystemBundle(Framework framework)
            : base(new BundleData(0, typeof(SystemBundle).Assembly.Location), framework)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            EventManager.BundleEvent += new BundleEventHandler(OnBundleEvent);
        }

        /// <summary>
        /// Handles the <see cref="E:BundleEvent" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="BundleEventArgs"/> instance containing the event data.</param>
        private void OnBundleEvent(object sender, BundleEventArgs e)
        {
            if (e.Transition == BundleTransition.Started)
            {
                Context.RegisterService(typeof(IShell).FullName, base.Framework, null);
            }
        }

        #region IBundle Members

        /// <summary>
        /// Start Bundle
        /// </summary>
        public override void Start()
        {
            try
            {
                this._state = BundleState.Starting;

                EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Starting, this));

                this._state = BundleState.Active;

                EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Started, this));
            }
            catch (Exception ex)
            {
                this._state = BundleState.Installed;
                FileLogUtility.Error(string.Format("{0}:{1}", ex.Message, "SystemBundle. Start()"));
            }
        }

        public override void Stop()
        {
            try
            {
                this._state = BundleState.Stopping;
                EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Stopping, this));

                this._state = BundleState.Installed;
                EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Stopped, this));
            }
            catch (Exception ex)
            {
                //TracesProvider.TracesOutput.OutputTrace(ex.Message);
                FileLogUtility.Error(string.Format("{0}:{1}", ex.Message, "SystemBundle. Stop()"));
            }
        }

        /// <summary>
        /// Bundle Update
        /// </summary>
        public new void Update()
        {
        }

        /// <summary>
        /// Bundle Update with Stream
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        public new void Update(Stream inputStream)
        {
        }

        /// <summary>
        /// Bundle Uninstall
        /// </summary>
        public new void Uninstall()
        {
        }

        /// <summary>
        /// Bundle GetProperties
        /// </summary>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        public new Dictionary<string, object> GetProperties()
        {
            return null;
        }

        #endregion IBundle Members
    }
}