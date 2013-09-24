#region File Header

// //////////////////////////////////////////////////////
// /// File: InvokeExt.cs
// /// Author: Sander Struijk
// /// Date: 2013-09-24 21:15
// //////////////////////////////////////////////////////

#endregion

#region Using Directives

using System;
using System.ComponentModel;

#endregion

namespace Utilities.NET.Invocation
{
    /// <summary>   Invoke extensions. </summary>
    /// <remarks>   Furier, 24.09.2013. </remarks>
    public static class InvokeExt
    {
        /// <summary>   A T extension method that executes the ex on a different thread, and waits for the result. </summary>
        /// <remarks>   Furier, 24.09.2013. </remarks>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="this">     The @this to act on. </param>
        /// <param name="action">   The action. </param>
        public static void InvokeEx<T>(this T @this, Action<T> action) where T : ISynchronizeInvoke
        {
            if (@this.InvokeRequired) @this.Invoke(action, new object[] {@this});
            else action(@this);
        }
    }
}