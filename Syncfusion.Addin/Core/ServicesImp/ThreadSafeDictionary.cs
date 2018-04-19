using Syncfusion.Addin.Core.Collection;
using System;
using System.Collections.Generic;

namespace Syncfusion.Addin.Core.ServicesImp
{
    /// <summary>
    /// 线程安全
    /// </summary>
    /// <typeparam name="TKey">Key</typeparam>
    /// <typeparam name="TValue">Value</typeparam>
    public class ThreadSafeDictionary<TKey, TValue> : ThreadSafeCollection<KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>, DictionaryLocker<TKey, TValue>>
    {
        public IEqualityComparer<TKey> Comparer
        {
            get
            {
                IEqualityComparer<TKey> comparer;
                using (new DisposableLocker(this.SyncRoot, base.MillisecondsTimeoutOnLock))
                {
                    comparer = base.Container.Comparer;
                }
                return comparer;
            }
        }

        public ThreadSafeDictionary()
            : this(10000)
        {
        }

        public ThreadSafeDictionary(int millisecondsTimeoutOnLock)
            : base(millisecondsTimeoutOnLock)
        {
            base.Container = new Dictionary<TKey, TValue>();
        }

        public ThreadSafeDictionary(IDictionary<TKey, TValue> dictionary, int millisecondsTimeoutOnLock)
            : base(millisecondsTimeoutOnLock)
        {
            base.Container = new Dictionary<TKey, TValue>(dictionary);
        }

        public ThreadSafeDictionary(IEqualityComparer<TKey> comparer, int millisecondsTimeoutOnLock)
            : base(millisecondsTimeoutOnLock)
        {
            base.Container = new Dictionary<TKey, TValue>(comparer);
        }

        public ThreadSafeDictionary(int capacity, int millisecondsTimeoutOnLock)
            : base(millisecondsTimeoutOnLock)
        {
            base.Container = new Dictionary<TKey, TValue>(capacity);
        }

        public ThreadSafeDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer, int minsecondsTimeoutOnLock)
            : base(minsecondsTimeoutOnLock)
        {
            base.Container = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        public ThreadSafeDictionary(int capacity, IEqualityComparer<TKey> comparer, int minsecondsTimeoutOnLock)
            : base(minsecondsTimeoutOnLock)
        {
            base.Container = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        public void Clear()
        {
            using (new DisposableLocker(this.SyncRoot, base.MillisecondsTimeoutOnLock))
            {
                base.Container.Clear();
            }
        }

        public bool Remove(TKey key)
        {
            bool result;
            using (new DisposableLocker(this.SyncRoot, base.MillisecondsTimeoutOnLock))
            {
                result = base.Container.Remove(key);
            }
            return result;
        }

        public void ForEach(Action<KeyValuePair<TKey, TValue>> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            using (new DisposableLocker(this.SyncRoot, base.MillisecondsTimeoutOnLock))
            {
                foreach (KeyValuePair<TKey, TValue> current in base.Container)
                {
                    action(current);
                }
            }
        }

        protected override DictionaryLocker<TKey, TValue> CreateLocker()
        {
            return new DictionaryLocker<TKey, TValue>(this.SyncRoot, base.Container, base.MillisecondsTimeoutOnLock);
        }
    }
}