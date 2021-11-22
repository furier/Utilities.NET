using System;
using System.Reflection;

// ReSharper disable MemberCanBePrivate.Global

namespace Utilities.NET.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        ///     Converts an int to an enum value of passed in enum type: <typeparamref name="T"/>.
        /// </summary>
        /// <param name="source"> The int to convert to an enum of passed in enum type: <typeparamref name="T"/> </param>
        /// <typeparam name="T"> The enum type to match the </typeparam>
        /// <exception cref="ArgumentException">
        ///     When <typeparamref name="T"/> is not an enum type.
        /// </exception>
        /// <returns>
        ///     The passed in <paramref name="source"/> converted to an enum value of enum type: <typeparamref name="T"/>
        /// </returns>
        public static T ToEnum<T>(this int source) => source.ToEnum(default(T), false);

        /// <summary>
        ///     Converts an int to an enum value of passed in enum type: <typeparamref name="T"/>.
        /// </summary>
        /// <param name="source"> The int to convert to an enum of passed in enum type: <typeparamref name="T"/> </param>
        /// <param name="silent">
        ///     When set to true, the default value for <typeparamref name="T"/> will be returned if we are unable to
        ///     convert the <paramref name="source"/> to an enum value of enum type: <typeparamref name="T"/>.
        ///     Otherwise the exception is thrown.
        /// </param>
        /// <typeparam name="T"> The enum type to match the </typeparam>
        /// <exception cref="ArgumentException">
        ///     When <typeparamref name="T"/> is not an enum type, and is thrown regardless of
        ///     <paramref name="silent"/> value.
        /// </exception>
        /// <returns>
        ///     The passed in <paramref name="source"/> converted to an enum value of enum type: <typeparamref name="T"/>
        /// </returns>
        public static T ToEnum<T>(this int source, bool silent) => source.ToEnum(default(T), silent);

        /// <summary>
        ///     Converts an int to an enum value of passed in enum type: <typeparamref name="T"/>.
        /// </summary>
        /// <param name="source"> The int to convert to an enum of passed in enum type: <typeparamref name="T"/> </param>
        /// <param name="default">
        ///     The default value to return if we were unable to convert the
        ///     <paramref name="source"/> to an enum value of enum type: <typeparamref name="T"/>
        /// </param>
        /// <typeparam name="T"> The enum type to match the </typeparam>
        /// <exception cref="ArgumentException">
        ///     When <typeparamref name="T"/> is not an enum type.
        /// </exception>
        /// <returns>
        ///     The passed in <paramref name="source"/> converted to an enum value of enum type: <typeparamref name="T"/>
        /// </returns>
        public static T ToEnum<T>(this int source, T @default) => source.ToEnum(@default, false);

        /// <summary>
        ///     Converts an int to an enum value of passed in enum type: <typeparamref name="T"/>.
        /// </summary>
        /// <param name="source"> The int to convert to an enum of passed in enum type: <typeparamref name="T"/> </param>
        /// <param name="default">
        ///     The default value to return if <paramref name="silent"/> is set to true and we were unable to convert the
        ///     <paramref name="source"/> to an enum value of enum type: <typeparamref name="T"/>
        /// </param>
        /// <param name="silent">
        ///     When set to true, the <paramref name="default"/> value will be returned if we are unable to
        ///     convert the <paramref name="source"/> to an enum value of enum type: <typeparamref name="T"/>.
        ///     Otherwise the exception is thrown.
        /// </param>
        /// <typeparam name="T"> The enum type to match the </typeparam>
        /// <exception cref="ArgumentException">
        ///     When <typeparamref name="T"/> is not an enum type, and is thrown regardless of
        ///     <paramref name="silent"/> value.
        /// </exception>
        /// <returns>
        ///     The passed in <paramref name="source"/> converted to an enum value of enum type: <typeparamref name="T"/>
        /// </returns>
        public static T ToEnum<T>(this int source, T @default, bool silent) => (T) source.ToEnum(typeof(T), @default, silent);

        /// <summary>
        ///     Converts a string to an enum value of passed in enum <paramref name="type"/>.
        /// </summary>
        /// <param name="source"> The string to convert to an enum of passed in enum <paramref name="type"/>. </param>
        /// <param name="type"> The enum type to convert the <paramref name="source"/> to. </param>
        /// <param name="default">
        ///     The default value to return if <paramref name="silent"/> is set to true and we were unable to convert the
        ///     <paramref name="source"/> to an enum value of enum <paramref name="type"/>
        /// </param>
        /// <param name="silent">
        ///     When set to true, the <paramref name="default"/> value will be returned if we are unable to
        ///     convert the <paramref name="source"/> to an enum value of enum <paramref name="type"/>.
        ///     Otherwise the exception is thrown.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     When <paramref name="type"/> is not an enum type, and is thrown regardless of
        ///     <paramref name="silent"/> value.
        /// </exception>
        /// <returns>
        ///     The passed in <paramref name="source"/> converted to an enum value of enum <paramref name="type"/>.
        /// </returns>
        public static object ToEnum(this int source, Type type, object @default, bool silent)
        {
            if (!type.IsEnum) throw new ArgumentException($"Generic parameter for method: {MethodBase.GetCurrentMethod().Name} only support Enum types.");

            try
            {
                return Enum.IsDefined(type, source) ? Enum.ToObject(type, source) : throw new ArgumentOutOfRangeException(nameof(source), source, $"The value is not a valid enum member for the enum type: {type.Name}.");
            }
            catch (Exception ex)
            {
                return silent ? @default : throw ex;
            }
        }
    }
}