using System;
using System.Linq.Expressions;

namespace Utilities.NET.Extensions
{
    public static class ExpressionHelper
    {
        /// <summary>
        ///     Gets the <see cref="MemberExpression"/> for a property.
        /// </summary>
        /// <typeparam name="T">
        ///     The generic type of the object with the <paramref name="property"/> to get the
        ///     <see cref="MemberExpression"/> of.
        /// </typeparam>
        /// <param name="property"> The property to get the <see cref="MemberExpression"/> of. </param>
        /// <returns> The <see cref="MemberExpression"/> of the <paramref name="property"/>. </returns>
        public static MemberExpression GetMemberExpression<T>(Expression<Func<T, object>> property) => 
            property.Body as MemberExpression ?? ((UnaryExpression) property.Body).Operand as MemberExpression;
    }
}