using Syncfusion.Addin.Core.Collection;
using System.Collections.Generic;

namespace Syncfusion.Addin.Core.ServicesImp
{
    /// <summary>
    /// 字典
    /// </summary>
    /// <typeparam name="TKey">KEY 键</typeparam>
    /// <typeparam name="TValue">Value 值</typeparam>
    public class DictionaryLocker<TKey, TValue> : EnumerableLocker<KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>>
    {
        public int Count
        {
            get
            {
                return base.Container.Count;
            }
        }

        public Dictionary<TKey, TValue>.KeyCollection Keys
        {
            get
            {
                return base.Container.Keys;
            }
        }

        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                return base.Container.Values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return base.Container[key];
            }
            set
            {
                base.Container[key] = value;
            }
        }

        public DictionaryLocker(object mutex, Dictionary<TKey, TValue> list, int millisecondsTimeout)
            : base(mutex, list, millisecondsTimeout)
        {
        }

        public void Add(TKey key, TValue value)
        {
            base.Container.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return base.Container.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return base.Container.ContainsValue(value);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return base.Container.TryGetValue(key, out value);
        }
    }
}