using System;
using System.Threading;

namespace Syncfusion.Addin.Core.Collection
{
    public abstract class ThreadSafeCollection<T, TContainterType, TLockerType> where TLockerType : ContainerLocker<TContainterType>
    {
        private object _syncRoot;
        private int _millisecondsTimeoutOnLock;

        protected virtual object SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
                }
                return this._syncRoot;
            }
        }

        public int MillisecondsTimeoutOnLock
        {
            get
            {
                return this._millisecondsTimeoutOnLock;
            }
            set
            {
                if (value < 0)
                {
                    this._millisecondsTimeoutOnLock = 0;
                }
                this._millisecondsTimeoutOnLock = value;
            }
        }

        protected TContainterType Container
        {
            get;
            set;
        }

        protected ThreadSafeCollection(int minsecondsTimeoutOnLock)
        {
            this.MillisecondsTimeoutOnLock = minsecondsTimeoutOnLock;
        }

        protected abstract TLockerType CreateLocker();

        public TLockerType Lock()
        {
            return this.CreateLocker();
        }

        public void Lock(Action<TLockerType> callback)
        {
            if (callback != null)
            {
                using (TLockerType lockerType = this.CreateLocker())
                {
                    callback(lockerType);
                }
            }
        }
    }
}