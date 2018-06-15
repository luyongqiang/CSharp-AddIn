using System;
using System.Collections.Generic;
using System.Linq;

namespace     Syncfusion.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static T ObjectWithMin<T, TResult>(this IEnumerable<T> sequence, Func<T, TResult> predicate)
            where T : class
            where TResult : IComparable
        {
            if (!sequence.Any()) return null;

            //get the first object with its predicate value
            var seed = sequence.Select(x => new { Object = x, Value = predicate(x) }).FirstOrDefault();
            //compare against all others, replacing the accumulator with the lesser value
            //tie goes to first object found
            return
                sequence.Select(x => new { Object = x, Value = predicate(x) })
                    .Aggregate(seed, (acc, x) => acc.Value.CompareTo(x.Value) <= 0 ? acc : x).Object;
        }

        public static T ObjectWithMax<T, TResult>(this IEnumerable<T> sequence, Func<T, TResult> predicate)
            where T : class
            where TResult : IComparable
        {
            if (!sequence.Any()) return null;

            //get the first object with its predicate value
            var seed = sequence.Select(x => new { Object = x, Value = predicate(x) }).FirstOrDefault();
            //compare against all others, replacing the accumulator with the greater value
            //tie goes to last object found
            return
                sequence.Select(x => new { Object = x, Value = predicate(x) })
                    .Aggregate(seed, (acc, x) => acc.Value.CompareTo(x.Value) > 0 ? acc : x).Object;
        }

        /// <summary>
        /// Generic iterator function that is useful to replace a foreach loop with at your discretion.  A provided action is performed on each element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action">Function that takes in the current value in the sequence. 
        /// <returns></returns>
        public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            return source.Each((value, index) =>
            {
                action(value);
                return true;
            });
        }

        /// <summary>
        /// Generic iterator function that is useful to replace a foreach loop with at your discretion.  A provided action is performed on each element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action">Function that takes in the current value and its index in the sequence.  
        /// <returns></returns>
        public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            return source.Each((value, index) =>
            {
                action(value, index);
                return true;
            });
        }

        /// <summary>
        /// Generic iterator function that is useful to replace a foreach loop with at your discretion.  A provided action is performed on each element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action">Function that takes in the current value in the sequence.  Returns a value indicating whether the iteration should continue.  So return false if you don't want to iterate anymore.</param>
        /// <returns></returns>
        public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Func<T, bool> action)
        {
            return source.Each((value, index) =>
            {
                return action(value);
            });
        }

        /// <summary>
        /// Generic iterator function that is useful to replace a foreach loop with at your discretion.  A provided action is performed on each element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action">Function that takes in the current value and its index in the sequence.  Returns a value indicating whether the iteration should continue.  So return false if you don't want to iterate anymore.</param>
        /// <returns></returns>
        public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Func<T, int, bool> action)
        {
            if (source == null)
                return source;

            int index = 0;
            foreach (var sourceItem in source)
            {
                if (!action(sourceItem, index))
                    break;
                index++;
            }
            return source;
        }
    }
}
