#region File Header

// //////////////////////////////////////////////////////
// /// File: ReflectionUtil.cs
// /// Author: Sander Struijk
// /// Date: 2013-09-28 15:07
// //////////////////////////////////////////////////////

#endregion

#region Using Directives

using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Utilities.NET.netcore45.Reflection
{
    /// <summary>   Reflection utility. </summary>
    /// <remarks>   Sander Struijk, 24.09.2013. </remarks>
    public static class ReflectionUtil
    {
        /// <summary>   Gets property information. </summary>
        /// <remarks>   Sander.struijk, 03.03.2014. </remarks>
        /// <typeparam name="T">        Generic type parameter. </typeparam>
        /// <param name="propertyName"> Name of the property. </param>
        /// <returns>   The property. </returns>
        public static PropertyInfo GetPropertyInfo<T>(string propertyName)
        {
            var type = typeof(T);
            var propertyInfo = type.GetRuntimeProperty(propertyName);
            return propertyInfo;
        }

        /// <summary>   Gets property information. </summary>
        /// <remarks>   Sander.struijk, 04.03.2014. </remarks>
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///                                         illegal values. </exception>
        /// <typeparam name="TSource">      Type of the source. </typeparam>
        /// <typeparam name="TProperty">    Type of the property. </typeparam>
        /// <param name="propertyLambda">   The property lambda. </param>
        /// <returns>   The property information. </returns>
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            //var type = typeof(TSource);

            var member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", propertyLambda));

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", propertyLambda));

            //if (type != propInfo.ReflectedType &&
            //    !type.IsSubclassOf(propInfo.ReflectedType))
            //    throw new ArgumentException(string.Format(
            //        "Expresion '{0}' refers to a property that is not from type {1}.",
            //        propertyLambda,
            //        type));

            return propInfo;
        }

        /// <summary>   An object extension method that query if 'obj' is of simple type. </summary>
        /// <remarks>   Sander.struijk, 14.03.2014. </remarks>
        /// <param name="obj">  The obj to act on. </param>
        /// <returns>   true if simple type, false if not. </returns>
        public static bool IsSimpleType(this object obj)
        {
            var type = obj.GetType();
            return type.GetTypeInfo().IsPrimitive || type == typeof(string);
        }

        /// <summary>   An object extension method that query if 'obj' is of a collection type. </summary>
        /// <remarks>   Sander.struijk, 14.03.2014. </remarks>
        /// <param name="obj">  The obj to act on. </param>
        /// <returns>   true if collection type, false if not. </returns>
        public static bool IsCollectionType(this object obj)
        {
            var collection = obj as IEnumerable;
            return collection != null && !(collection is string);
        }

        /// <summary>   An object extension method that returns the classification of its type. </summary>
        /// <remarks>   Sander.struijk, 14.03.2014. </remarks>
        /// <param name="obj">  The obj to act on. </param>
        /// <returns>   A Type. </returns>
        public static ObjectType TypeOfObject(this object obj)
        {
            if(obj.IsSimpleType())
                return ObjectType.Simple;
            if(obj.IsCollectionType())
                return ObjectType.Collection;

            return ObjectType.Complex;
        }
    }
}