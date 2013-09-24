#region File Header

// //////////////////////////////////////////////////////
// /// File: ListExt.cs
// /// Author: Sander Struijk
// /// Date: 2013-09-24 20:21
// //////////////////////////////////////////////////////

#endregion

#region Using Directives

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Utilities.NET.Collections.Extensions
{
    /// <summary>   List utilitie. </summary>
    /// <remarks>   Furier, 24.09.2013. </remarks>
    public static class ListExt
    {
        /// <summary>   A List extension method that splits a list into smaller lists. </summary>
        /// <remarks>   Furier, 24.09.2013. </remarks>
        /// <param name="source">       The source to act on. </param>
        /// <param name="splitSize">    Size of the split. </param>
        /// <returns>   A List with Lists! </returns>
        public static List<List<object>> Split(this List<object> source, int splitSize)
        {
            return source.Select((x, i) => new {Index = i, Value = x}).GroupBy(x => x.Index / splitSize).Select(x => x.Select(v => v.Value).ToList()).ToList();
        }
    }
}