using System;
using System.Collections.Generic;

namespace     Syncfusion.Core.Extensions
{
    /// <summary>
    /// Class IListExtensions.
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// Binaries the search.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        /// <exception cref="System.InvalidOperationException">Item not found</exception>
        public static T BinarySearch<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, TKey key) where TKey : IComparable<TKey>
        {
            int min = 0;
            int max = list.Count;
            while (min < max)
            {
                int mid = (max + min) / 2;
                T midItem = list[mid];
                TKey midKey = keySelector(midItem);
                int comp = midKey.CompareTo(key);
                if (comp < 0)
                {
                    min = mid + 1;
                }
                else if (comp > 0)
                {
                    max = mid - 1;
                }
                else
                {
                    return midItem;
                }
            }
            if (min == max &&
                keySelector(list[min]).CompareTo(key) == 0)
            {
                return list[min];
            }
            throw new InvalidOperationException("Item not found");
        }

        /// <summary>
        /// Shuffle's list items.
        /// </summary>
        /// <typeparam name="T">List type.</typeparam>
        /// <param name="list">Generic list.</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            for (int i = list.Count - 1; i > 0; i--)
            {
                int swapIndex = rng.Next(i + 1);
                if (swapIndex != i)
                {
                    T tmp = list[swapIndex];
                    list[swapIndex] = list[i];
                    list[i] = tmp;
                }
            }
        }
    }
}
