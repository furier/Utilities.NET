using System.Linq;

namespace Utilities.NET.Extensions
{
    /// <summary> Version comparer extensions. </summary>
    public static class VersionComparerExtensions
    {
        /// <summary> Compares two version string objects to determine their relative ordering. </summary>
        /// <param name="x"> String to be compared. e.g. "2.1.1.3" </param>
        /// <param name="y"> String to be compared. e.g. "2.2.6.0" </param>
        /// <returns> Negative if 'x' is less than 'y', 0 if they are equal, or positive if it is greater. </returns>
        public static int Compare(this string x, string y)
        {
            if (x.Equals(y)) return 0;
            var xparts = x.Split('.');
            var yparts = y.Split('.');
            var length = new[] { xparts.Length, yparts.Length }.Max();
            for (var i = 0; i < length; i++)
            {
                if (!int.TryParse(xparts.ElementAtOrDefault(i), out var xint)) xint = 0;
                if (!int.TryParse(yparts.ElementAtOrDefault(i), out var yint)) yint = 0;
                if (xint > yint) return 1;
                if (yint > xint) return -1;
            }
            //they're equal value but not equal strings, eg 1 and 1.0
            return 0;
        }
    }
}