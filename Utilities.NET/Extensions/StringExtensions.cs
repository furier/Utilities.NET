using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

// ReSharper disable MemberCanBePrivate.Global

namespace Utilities.NET.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Trims the <paramref name="value" /> off the end of the <paramref name="source" /> string.
        /// </summary>
        /// <remarks>
        ///     Trims only the <paramref name="value" />, nothing more noting less trailing or leading whitespaces are left
        ///     alone.
        /// </remarks>
        /// <param name="source"> The string to trim. </param>
        /// <param name="value"> The value to trim off the end of the string. </param>
        /// <param name="stringComparison"> (Optional) String comparison option. </param>
        /// <returns> The trimmed string. </returns>
        public static string TrimEnd(this string source, string value, StringComparison stringComparison = StringComparison.Ordinal) => 
            !source.EndsWith(value) ? source : source.Remove(source.LastIndexOf(value, stringComparison));

        /// <summary>
        ///     Determines if a string is null or empty.
        /// </summary>
        /// <param name="source"> The string to perform the test on. </param>
        /// <example>
        ///     Native:
        ///     <code>
        ///     if (string.IsNullOrEmpty(s)) { ... }
        ///     </code>
        ///     Extension:
        ///     <code>
        ///     if (s.IsNullOrEmpty()) { ... }
        ///     </code>
        /// </example>
        /// <returns> A boolean value expressing if the string was null or empty. </returns>
        public static bool IsNullOrEmpty(this string source) => string.IsNullOrEmpty(source);

        /// <summary>
        ///     Determines if a string is not null or empty.
        /// </summary>
        /// <param name="source"> The string to perform the test on. </param>
        /// <returns> A boolean value expressing if the string was not null or empty. </returns>
        public static bool IsNotNullOrEmpty(this string source) => !string.IsNullOrEmpty(source);

        /// <summary>
        ///     Determines if a string is null, empty or white space.
        /// </summary>
        /// <param name="source"> The string to perform the test on. </param>
        /// <returns> A boolean value expressing if the string was null, empty or whitespace. </returns>
        public static bool IsNullOrWhiteSpace(this string source) => string.IsNullOrWhiteSpace(source);

        /// <summary>
        ///     Determines if a string is not null, empty or white space.
        /// </summary>
        /// <param name="source"> The string to perform the test on. </param>
        /// <returns> A boolean value expressing if the string was not null, empty or whitespace. </returns>
        public static bool IsNotNullOrWhiteSpace(this string source) => !string.IsNullOrWhiteSpace(source);

        /// <summary>
        ///     Turns the first character in any string to upper case, and the rest to lowercase.
        /// </summary>
        /// <param name="source"> The string to convert the casing on. </param>
        /// <returns> The new string with the changed casing. </returns>
        public static string FirstLetterToUpper(this string source)
        {
            if (source == null)
                return null;

            if (source.Length > 1)
                return char.ToUpper(source[0]) + source.Substring(1).ToLower();

            return source.ToUpper();
        }

        /// <summary>
        ///     Converts a string to title case, meaning every word delimited by space have its first character in uppercase and
        ///     the rest lowercase.
        /// </summary>
        /// <example>
        ///     This is an example => This Is An Example.
        /// </example>
        /// <remarks>
        ///     CultureInfo.CurrentCulture is used for converting the string to title case.
        /// </remarks>
        /// <param name="source"> The string to convert to title case. </param>
        /// <returns> The new title case string. </returns>
        public static string ToTitleCase(this string source) => ToTitleCase(source.ToLower(), CultureInfo.CurrentCulture);

        /// <summary>
        ///     Converts a string to title case, meaning every word delimited by space have its first character in uppercase and
        ///     the rest lowercase.
        /// </summary>
        /// <example> This is an example => This Is An Example. </example>
        /// <param name="source"> The string to convert to title case. </param>
        /// <param name="cultureInfo"> The culture info used to convert the string to title case. </param>
        /// <returns> The new title case string. </returns>
        public static string ToTitleCase(this string source, CultureInfo cultureInfo) => cultureInfo.TextInfo.ToTitleCase(source.ToLower());

        /// <summary>
        ///     Converts a string to an enum value of passed in enum type: <typeparamref name="T" />.
        /// </summary>
        /// <param name="source"> The string to convert to an enum of passed in enum type: <typeparamref name="T" /> </param>
        /// <typeparam name="T"> The enum type to match the </typeparam>
        /// <exception cref="ArgumentException">
        ///     When <typeparamref name="T" /> is not an enum type.
        /// </exception>
        /// <returns>
        ///     The passed in <paramref name="source" /> converted to an enum value of enum type: <typeparamref name="T" />
        /// </returns>
        public static T ToEnum<T>(this string source) => source.ToEnum(default(T), false);

        /// <summary>
        ///     Converts a string to an enum value of passed in enum type: <typeparamref name="T" />.
        /// </summary>
        /// <param name="source"> The string to convert to an enum of passed in enum type: <typeparamref name="T" /> </param>
        /// <param name="silent">
        ///     When set to true, default value for <typeparamref name="T" /> will be returned if we are unable to
        ///     convert the <paramref name="source" /> to an enum value of enum type: <typeparamref name="T" />.
        ///     Otherwise the exception is thrown.
        /// </param>
        /// <typeparam name="T"> The enum type to match the </typeparam>
        /// <exception cref="ArgumentException">
        ///     When <typeparamref name="T" /> is not an enum type, and is thrown regardless of
        ///     <paramref name="silent" /> value.
        /// </exception>
        /// <returns>
        ///     The passed in <paramref name="source" /> converted to an enum value of enum type: <typeparamref name="T" />
        /// </returns>
        public static T ToEnum<T>(this string source, bool silent) => source.ToEnum(default(T), silent);

        /// <summary>
        ///     Converts a string to an enum value of passed in enum type: <typeparamref name="T" />.
        /// </summary>
        /// <param name="source"> The string to convert to an enum of passed in enum type: <typeparamref name="T" /> </param>
        /// <param name="default">
        ///     The default value to return if we were unable to convert the
        ///     <paramref name="source" /> to an enum value of enum type: <typeparamref name="T" />
        /// </param>
        /// <typeparam name="T"> The enum type to match the </typeparam>
        /// <exception cref="ArgumentException">
        ///     When <typeparamref name="T" /> is not an enum type.
        /// </exception>
        /// <returns>
        ///     The passed in <paramref name="source" /> converted to an enum value of enum type: <typeparamref name="T" />
        /// </returns>
        public static T ToEnum<T>(this string source, T @default) => source.ToEnum(@default, false);

        /// <summary>
        ///     Converts a string to an enum value of passed in enum type: <typeparamref name="T" />.
        /// </summary>
        /// <param name="source"> The string to convert to an enum of passed in enum type: <typeparamref name="T" /> </param>
        /// <param name="default">
        ///     The default value to return if <paramref name="silent" /> is set to true and we were unable to convert the
        ///     <paramref name="source" /> to an enum value of enum type: <typeparamref name="T" />
        /// </param>
        /// <param name="silent">
        ///     When set to true, the <paramref name="default" /> value will be returned if we are unable to
        ///     convert the <paramref name="source" /> to an enum value of enum type: <typeparamref name="T" />.
        ///     Otherwise the exception is thrown.
        /// </param>
        /// <typeparam name="T"> The enum type to match the </typeparam>
        /// <exception cref="ArgumentException">
        ///     When <typeparamref name="T" /> is not an enum type, and is thrown regardless of
        ///     <paramref name="silent" /> value.
        /// </exception>
        /// <returns>
        ///     The passed in <paramref name="source" /> converted to an enum value of enum type: <typeparamref name="T" />
        /// </returns>
        public static T ToEnum<T>(this string source, T @default, bool silent) => (T) source.ToEnum(typeof(T), @default, silent);

        /// <summary>
        ///     Converts a string to an enum value of passed in enum <paramref name="type" />.
        /// </summary>
        /// <param name="source"> The string to convert to an enum of passed in enum <paramref name="type" />. </param>
        /// <param name="type"> The enum type to convert the <paramref name="source" /> to. </param>
        /// <param name="default">
        ///     The default value to return if <paramref name="silent" /> is set to true and we were unable to convert the
        ///     <paramref name="source" /> to an enum value of enum <paramref name="type" />
        /// </param>
        /// <param name="silent">
        ///     When set to true, the <paramref name="default" /> value will be returned if we are unable to
        ///     convert the <paramref name="source" /> to an enum value of enum <paramref name="type" />.
        ///     Otherwise the exception is thrown.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     When <paramref name="type" /> is not an enum type, and is thrown regardless of
        ///     <paramref name="silent" /> value.
        /// </exception>
        /// <returns>
        ///     The passed in <paramref name="source" /> converted to an enum value of enum <paramref name="type" />.
        /// </returns>
        public static object ToEnum(this string source, Type type, object @default, bool silent)
        {
            if (!type.IsEnum) throw new ArgumentException($"Generic parameter for method: {MethodBase.GetCurrentMethod().Name} only support Enum types.");

            try
            {
                if (Enum.IsDefined(type, source))
                    return Enum.Parse(type, source);
                if (source.IsNotNullOrWhiteSpace() && source.All(char.IsDigit))
                    return int.Parse(source).ToEnum(type, @default, silent);

                throw new ArgumentOutOfRangeException(nameof(source), source, $"The value is not a valid enum member for the enum type: {type.Name}.");
            }
            catch (Exception ex)
            {
                return silent ? @default : throw ex;
            }
        }
    }
}