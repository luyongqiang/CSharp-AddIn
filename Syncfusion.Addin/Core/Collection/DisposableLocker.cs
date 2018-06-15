using Syncfusion.Addin.Utility;
using System;
using System.Threading;

namespace Syncfusion.Addin.Core.Collection
{
    public class DisposableLocker : IDisposable
    {
        private object _syncRoot;
        private bool _lockAcquired;
        private int _millisecondsTimeout;

        public DisposableLocker(object syncRoot, int millisecondsTimeout)
        {
            AssertUtility.ArgumentNotNull(syncRoot, "syncRoot");
            this._syncRoot = syncRoot;
            this._millisecondsTimeout = millisecondsTimeout;
            this._lockAcquired = Monitor.TryEnter(this._syncRoot, this._millisecondsTimeout);
            this.LogWhenAcquireLockFailed();
        }

        private void LogWhenAcquireLockFailed()
        {
            if (!this._lockAcquired)
            {
                FileLogUtility.Error(string.Format("AcquireTheLockTimeout", this._millisecondsTimeout));
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && this._lockAcquired)
            {
                Monitor.Exit(this._syncRoot);
            }
        }

        ~DisposableLocker()
        {
            this.Dispose(false);
        }
    }
}