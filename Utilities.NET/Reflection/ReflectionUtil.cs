#region File Header

// //////////////////////////////////////////////////////
// /// File: ReflectionUtil.cs
// /// Author: Sander Struijk
// /// Date: 2013-09-24 20:31
// //////////////////////////////////////////////////////

#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

#endregion

namespace Utilities.NET.Reflection
{
    /// <summary>   Reflection utility. </summary>
    /// <remarks>   Furier, 24.09.2013. </remarks>
    public static class ReflectionUtil
    {
        /// <summary>   Returns an object collection of type T of none public fields a target object may have. </summary>
        /// <remarks>   Furier, 24.09.2013. </remarks>
        /// <typeparam name="T">    . </typeparam>
        /// <param name="target">   . </param>
        /// <returns>   The found all objects&lt; t&gt; </returns>
        public static IList<T> GetAllPrivateFields<T>(object target)
        {
            var objects = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Where(x => typeof(T) == x.FieldType).Select(x => x.GetValue(target)).Cast<T>().ToList();
            return objects;
        }

        // Example of usage:
        //     
        // public enum PublishStatusses
        // {
        //     [Description("Not Completed")] 
        //     NotCompleted, 
        //     Completed, 
        //     Error
        // }
        //     
        // var myEnumTest = PublishStatusses.NotCompleted;
        // Console.WriteLine(myEnumTest.GetDescription());
        //     
        // Console output:
        //     
        // Not Completed.

        /// <summary>
        ///     
        /// </summary>
        /// <remarks>   Furier, 24.09.2013. </remarks>
        /// <param name="enumerationValue"> . </param>
        /// <returns>   Returns the value of a potensial Description Attribute on the enum value. </returns>
        public static string GetDescription(this Enum enumerationValue)
        {
            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            var memberInfo = enumerationValue.GetType().GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0) //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }
    }
}