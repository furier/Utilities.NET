using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.NET.Extensions
{
    /// <summary> Enumerable extensions. </summary>
    public static class EnumerableExtensions
    {
        /// <summary> An Enumerable extension method that determine if two Enumerables are equal. </summary>
        /// <remarks> The order of the items must also match. </remarks>
        /// <param name="collectionA"> The Enumerable a to act on. </param>
        /// <param name="collectionB"> The Enumerable b to compare with. </param>
        /// <returns> true if equal, false if not. </returns>
        public static bool AreEqual(this IEnumerable<object> collectionA, IEnumerable<object> collectionB)
        {
            IEnumerator<object> aEnumerator = null;
            IEnumerator<object> bEnumerator = null;

            try
            {
                aEnumerator = collectionA.GetEnumerator();
                bEnumerator = collectionB.GetEnumerator();

                while (aEnumerator.MoveNext() && bEnumerator.MoveNext())
                {
                    if (aEnumerator.Current != null && bEnumerator.Current != null && aEnumerator.Current.Equals(bEnumerator.Current)) continue;
                    return false;
                }

                return true;
            }
            finally
            {
                aEnumerator?.Dispose();
                bEnumerator?.Dispose();
            }
        }

        /// <summary> An Enumerable extension method that splits a list into smaller Enumerables. </summary>
        /// <param name="source"> The source to act on. </param>
        /// <param name="splitSize"> Size of the split. </param>
        /// <returns> An Enumerable with Enumerables </returns>
        public static IEnumerable<IEnumerable<object>> Split(this IEnumerable<object> source, int splitSize) => 
            source.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / splitSize).Select(x => x.Select(v => v.Value));

        /// <summary>
        ///     Check if an enumerable is empty.
        /// </summary>
        /// <remarks> Inverse method of System.Linq.Any. </remarks>
        /// <param name="source"> The source to act on. </param>
        /// <returns> True if the enumerable is empty, otherwise false. </returns>
        public static bool Empty<T>(this IEnumerable<T> source) => !source.Any();

        /// <summary>
        ///     Check if none of the elements in <paramref name="source" /> match the <paramref name="criteria" />.
        /// </summary>
        /// <remarks> Inverse method of `System.Linq.Any` with a <paramref name="criteria" /> as input. </remarks>
        /// <example>
        ///     `System.Linq.Any`
        ///     <code>
        ///     if (!list.Any(criteria)) { ... }
        ///     </code>
        ///     Exstension method
        ///     <code>
        ///     if (list.None(criteria)) { ... }
        ///     </code>
        ///     One can argue that the extension method is much more readable, it is more clear what the intent is, and you have
        ///     less chance of missing a `!` which you need to add to `System.Linq.Any` for the inverse result.
        /// </example>
        /// <param name="source"> The source to act on. </param>
        /// <param name="criteria"> The criteria to match on. </param>
        /// <returns>
        ///     True if none of the elements in <paramref name="source" /> match the <paramref name="criteria" />, otherwise
        ///     false.
        /// </returns>
        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> criteria) => !source.Any(criteria);
    }
}