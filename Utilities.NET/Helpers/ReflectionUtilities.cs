using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Utilities.NET.Enums;

// ReSharper disable MemberCanBePrivate.Global

namespace Utilities.NET.Helpers
{
    /// <summary> Reflection utilities. </summary>
    public static class ReflectionUtilities
    {
        /// <summary>
        ///     Returns a collection of objects of type <typeparamref name="T"/> of none public fields a target object may
        ///     have.
        /// </summary>
        public static IList<T> GetAllPrivateFields<T>(object target)
        {
            return target
                .GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(x => typeof(T) == x.FieldType)
                .Select(x => x.GetValue(target))
                .Cast<T>()
                .ToList();
        }

        /// <summary> Gets property information. </summary>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="propertyName"> Name of the property. </param>
        /// <returns> The property. </returns>
        public static PropertyInfo GetPropertyInfo<T>(string propertyName)
        {
            var type = typeof(T);
            var propertyInfo = type.GetRuntimeProperty(propertyName);
            return propertyInfo;
        }

        /// <summary> Gets property information. </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or
        ///     illegal values.
        /// </exception>
        /// <typeparam name="TSource"> Type of the source. </typeparam>
        /// <typeparam name="TProperty"> Type of the property. </typeparam>
        /// <param name="propertyLambda"> The property lambda. </param>
        /// <returns> The property information. </returns>
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            if (!(propertyLambda.Body is MemberExpression member))
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");

            return propInfo;
        }

        /// <summary> An object extension method that query if 'obj' is of simple type. </summary>
        /// <param name="obj"> The obj to act on. </param>
        /// <returns> true if simple type, false if not. </returns>
        public static bool IsSimpleType(this object obj)
        {
            var type = obj.GetType();
            return type.IsPrimitive || type == typeof(string);
        }

        /// <summary> An object extension method that query if 'obj' is of a collection type. </summary>
        /// <param name="obj"> The obj to act on. </param>
        /// <returns> true if collection type, false if not. </returns>
        public static bool IsCollectionType(this object obj)
        {
            return obj is IEnumerable collection && !(collection is string);
        }

        /// <summary> An object extension method that returns the classification of its type. </summary>
        /// <param name="obj"> The obj to act on. </param>
        /// <returns> A Type. </returns>
        public static ObjectType TypeOfObject(this object obj)
        {
            if (obj.IsSimpleType())
                return ObjectType.Simple;
            if (obj.IsCollectionType())
                return ObjectType.Collection;

            return ObjectType.Complex;
        }
    }
}