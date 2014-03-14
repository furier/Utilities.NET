#region File Header

// //////////////////////////////////////////////////////
// /// File: CollectionExt.cs
// /// Author: Sander Struijk
// /// Date: 2013-09-28 15:07
// //////////////////////////////////////////////////////

#endregion

#region Using Directives

using System.Collections.Generic;

#endregion

namespace Utilities.NET.netcore45.Collections.Extensions
{
    /// <summary>   Collection extensions. </summary>
    /// <remarks>   Sander Struijk, 24.09.2013. </remarks>
    public static class CollectionExt
    {
        /// <summary>   An ICollection extension method that determine if two Collections are equal. </summary>
        /// <remarks>   Sander Struijk, 24.09.2013. </remarks>
        /// <param name="collectionA">  The collection a to act on. </param>
        /// <param name="collectionB">  The collection b to compare with. </param>
        /// <returns>   true if equal, false if not. </returns>
        public static bool AreEqual(this ICollection<object> collectionA, ICollection<object> collectionB)
        {
            var aEnumerator = collectionA.GetEnumerator();
            var bEnumerator = collectionB.GetEnumerator();
            while ((aEnumerator.MoveNext()) && (bEnumerator.MoveNext()))
            {
                if (aEnumerator.Current != null && bEnumerator.Current != null && aEnumerator.Current.Equals(bEnumerator.Current)) continue;
                return false;
            }
            return true;
        }
    }
}