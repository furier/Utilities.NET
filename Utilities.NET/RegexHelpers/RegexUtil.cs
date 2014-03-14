#region File Header

// // ***********************************************************************
// // Author           : Sander Struijk
// // ***********************************************************************

#endregion

#region Using statements

using System.Text.RegularExpressions;

#endregion

namespace Utilities.NET.RegexHelpers
{
    public static class RegexUtil
    {
        /// <summary>   Query if 'pattern' is a valid regular expression. </summary>
        /// <remarks>   Sander.struijk, 13.03.2014. </remarks>
        /// <param name="pattern">  Specifies the pattern. </param>
        /// <returns>   true if regular expression pattern valid, false if not. </returns>
        public static bool IsRegexPatternValid(string pattern)
        {
            try
            {
                new Regex(pattern);
                return true;
            }
            catch {}
            return false;
        }
    }
}