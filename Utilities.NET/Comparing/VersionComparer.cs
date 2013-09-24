#region File Header

// //////////////////////////////////////////////////////
// /// File: VersionComparer.cs
// /// Author: Sander Struijk
// /// Date: 2013-09-24 20:16
// //////////////////////////////////////////////////////

#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Utilities.NET.Comparing
{
    /// <summary>   Version comparer. </summary>
    /// <remarks>   Sastru, 13.09.2013. </remarks>
    public class VersionComparer : IComparer<string>
    {
        /// <summary>   Compares two version string objects to determine their relative ordering. </summary>
        /// <remarks>   Furier, 24.09.2013. </remarks>
        /// <param name="x">    String to be compared. e.g. "2.1.1.3" </param>
        /// <param name="y">    String to be compared. e.g. "2.2.6.0" </param>
        /// <returns>   Negative if 'x' is less than 'y', 0 if they are equal, or positive if it is greater. </returns>
        public int Compare(string x, string y)
        {
            if (x.Equals(y)) return 0;
            var xparts = x.Split('.');
            var yparts = y.Split('.');
            var length = new[] {xparts.Length, yparts.Length}.Max();
            for (var i = 0; i < length; i++)
            {
                int xint;
                int yint;
                if (!Int32.TryParse(xparts.ElementAtOrDefault(i), out xint)) xint = 0;
                if (!Int32.TryParse(yparts.ElementAtOrDefault(i), out yint)) yint = 0;
                if (xint > yint) return 1;
                if (yint > xint) return -1;
            }
            //they're equal value but not equal strings, eg 1 and 1.0
            return 0;
        }
    }
}