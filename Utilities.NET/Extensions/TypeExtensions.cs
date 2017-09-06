using System;
using System.Linq.Expressions;

namespace Utilities.NET.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        ///     Get the default value for <see cref="Type"/>.
        /// </summary>
        /// <remarks>
        ///     Closest thing to `default(T)` without generics.
        /// </remarks>
        /// <param name="type"> The type to get the default value of. </param>
        /// <returns> The default value of <see cref="Type"/>. </returns>
        public static object GetDefaultValue(this Type type)
        {
            // Validate parameters.
            if (type == null) throw new ArgumentNullException(nameof(type));

            // We want an Func<object> which returns the default.
            // Create that expression here.
            var e = Expression.Lambda<Func<object>>(
                // Have to convert to object.
                Expression.Convert(
                    // The default value, always get what the *code* tells us.
                    Expression.Default(type),
                    typeof(object)
                )
            );

            // Compile and return the value.
            return e.Compile()();
        }
    }
}