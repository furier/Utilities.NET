#region File Header

// // ***********************************************************************
// // Author           : Sander Struijk
// // ***********************************************************************

#endregion

#region Using statements

using System.Collections;
using System.Linq;
using Utilities.NET.Reflection;

#endregion

namespace Utilities.NET.SimpleTypes.Strings
{
    /// <summary>   Object to String helper. </summary>
    /// <remarks>   Sander.struijk, 14.03.2014. </remarks>
    public static class OtSHelper
    {
        /// <summary>
        ///     If object is an IEnumerable, the items will be joined into one string by calling ToString on every item in the
        ///     collection and concatinating them.
        ///     Otherwise the objects ToString will be returned.
        /// </summary>
        /// <remarks>   Sander.struijk, 13.03.2014. </remarks>
        /// <param name="obj">  The object. </param>
        /// <returns>   A string. </returns>
        public static string AggregateToString(object obj)
        {
            switch(obj.TypeOfObject())
            {
                case ObjectType.Simple:
                case ObjectType.Complex:
                    return obj.ToString();
                case ObjectType.Collection:
                    return ((IEnumerable)obj).Cast<object>().Where(x => x != null).Aggregate((current, next) => string.Format("{0} {1}", AggregateToString(current), AggregateToString(next))).ToString();
                default:
                    return string.Empty;
            }
        }
    }
}