using System.Text.RegularExpressions;

namespace Utilities.NET.Helpers
{
    public static class RegexUtilities
    {
        /// <summary> Query if 'pattern' is a valid regular expression. </summary>
        /// <param name="pattern"> Specifies the pattern. </param>
        /// <returns> true if regular expression pattern valid, false if not. </returns>
        public static bool IsRegexPatternValid(string pattern)
        {
            try
            {
                new Regex(pattern);
                return true;
            }
            catch { }
            return false;
        }
    }
}