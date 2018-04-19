using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Addin.Core.Collection
{
    public class EnumerableLocker<T, TContainterType> : ContainerLocker<TContainterType>, IEnumerable<T> where TContainterType : IEnumerable<T>
    {
        public EnumerableLocker(object syncRoot, TContainterType container, int millisecondsTimeout)
            : base(syncRoot, container, millisecondsTimeout)
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            TContainterType container = Container;
            return container.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Container.GetEnumerator();
        }
    }

    public class ListLocker<T> : EnumerableLocker<T, List<T>>
    {
        public T this[int index]
        {
            get
            {
                return Container[index];
            }
            set
            {
                Container[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return Container.Count;
            }
        }

        public int Capacity
        {
            get
            {
                return Container.Capacity;
            }
            set
            {
                Container.Capacity = value;
            }
        }

        public ListLocker(object syncRoot, List<T> list, int millisecondsTimeout)
            : base(syncRoot, list, millisecondsTimeout)
        {
        }

        public bool Exists(Predicate<T> match)
        {
            return Container.Exists(match);
        }

        public bool Contains(T item)
        {
            return Container.Contains(item);
        }

        public int FindIndex(Predicate<T> match)
        {
            return Container.FindIndex(match);
        }

        public int FindIndex(int startIndex, Predicate<T> match)
        {
            return Container.FindIndex(startIndex, match);
        }

        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            return Container.FindIndex(startIndex, match);
        }

        public T FindLast(Predicate<T> match)
        {
            return Container.FindLast(match);
        }

        public int FindLastIndex(Predicate<T> match)
        {
            return Container.FindIndex(match);
        }

        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            return Container.FindLastIndex(startIndex, match);
        }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            return Container.FindLastIndex(startIndex, count, match);
        }

        public List<T> GetRange(int index, int count)
        {
            return Container.GetRange(index, count);
        }

        public int IndexOf(T item)
        {
            return Container.IndexOf(item);
        }

        public int IndexOf(T item, int index)
        {
            return Container.IndexOf(item, index);
        }

        public int IndexOf(T item, int index, int count)
        {
            return Container.IndexOf(item, index, count);
        }

        public int LastIndexOf(T item)
        {
            return Container.LastIndexOf(item);
        }

        public int LastIndexOf(T item, int index)
        {
            return Container.LastIndexOf(item, index);
        }

        public int LastIndexOf(T item, int index, int count)
        {
            return Container.LastIndexOf(item, index, count);
        }

        public int BinarySearch(T item)
        {
            return Container.BinarySearch(item);
        }

        public void RemoveAt(int index)
        {
            Container.RemoveAt(index);
        }

        public void RemoveRange(int index, int count)
        {
            Container.RemoveRange(index, count);
        }

        public void Reverse()
        {
            Container.Reverse();
        }

        public void Reverse(int index, int count)
        {
            Container.Reverse(index, count);
        }

        public void Sort()
        {
            Container.Sort();
        }

        public void Sort(Comparison<T> comparison)
        {
            Container.Sort(comparison);
        }

        public void Sort(IComparer<T> comparer)
        {
            Container.Sort(comparer);
        }

        public void Sort(int index, int count, IComparer<T> comparer)
        {
            Container.Sort(index, count, comparer);
        }

        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return Container.BinarySearch(item, comparer);
        }

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            return Container.BinarySearch(index, count, item, comparer);
        }
    }

    public sealed class ThreadSafeList<T> : ThreadSafeCollection<T, List<T>, ListLocker<T>>
    {
        public ThreadSafeList()
            : this(10000)
        {
        }

        public ThreadSafeList(int millisecondsTimeoutOnLock)
            : base(millisecondsTimeoutOnLock)
        {
            Container = new List<T>();
        }

        public ThreadSafeList(IEnumerable<T> collection, int millisecondsTimeoutOnLock)
            : base(millisecondsTimeoutOnLock)
        {
            Container = new List<T>(collection);
        }

        public ThreadSafeList(int capacity, int millisecondsTimeoutOnLock)
            : base(millisecondsTimeoutOnLock)
        {
            Container = new List<T>(capacity);
        }

        public void Add(T value)
        {
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                Container.Add(value);
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                Container.AddRange(collection);
            }
        }

        public ReadOnlyCollection<T> AsReadOnly()
        {
            ReadOnlyCollection<T> result;
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                result = Container.AsReadOnly();
            }
            return result;
        }

        public void Clear()
        {
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                Container.Clear();
            }
        }

        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            List<TOutput> result;
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                result = Container.ConvertAll(converter);
            }
            return result;
        }

        public void CopyTo(T[] array)
        {
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                Container.CopyTo(array);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                Container.CopyTo(array, arrayIndex);
            }
        }

        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                Container.CopyTo(index, array, arrayIndex, count);
            }
        }

        public T Find(Predicate<T> match)
        {
            T result;
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                result = Container.Find(match);
            }
            return result;
        }

        public List<T> FindAll(Predicate<T> match)
        {
            List<T> result;
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                result = Container.FindAll(match);
            }
            return result;
        }

        public bool Remove(T value)
        {
            bool result;
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                result = Container.Remove(value);
            }
            return result;
        }

        public void ForEach(Action<T> action)
        {
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                Container.ForEach(action);
            }
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                Container.InsertRange(index, collection);
            }
        }

        public void Insert(int index, T item)
        {
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                Container.Insert(index, item);
            }
        }

        public int RemoveAll(Predicate<T> match)
        {
            int result;
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                result = Container.RemoveAll(match);
            }
            return result;
        }

        public T[] ToArray()
        {
            T[] result;
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                result = Container.ToArray();
            }
            return result;
        }

        public void TrimExcess()
        {
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                Container.TrimExcess();
            }
        }

        public bool TrueForAll(Predicate<T> match)
        {
            bool result;
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                result = Container.TrueForAll(match);
            }
            return result;
        }

        public bool TryGet(int index, out T value)
        {
            bool result;
            using (new DisposableLocker(SyncRoot, MillisecondsTimeoutOnLock))
            {
                if (index < Container.Count)
                {
                    value = Container[index];
                    result = true;
                }
                else
                {
                    value = default(T);
                    result = false;
                }
            }
            return result;
        }

        protected override ListLocker<T> CreateLocker()
        {
            return new ListLocker<T>(SyncRoot, Container, MillisecondsTimeoutOnLock);
        }
    }
}