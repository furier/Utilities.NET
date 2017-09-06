using System;
using System.ComponentModel;

namespace Utilities.NET.Extensions
{
    /// <summary> Invoke extensions. </summary>
    public static class InvokeExtensions
    {
        /// <summary> A T extension method that executes the ex on a different thread, and waits for the result. </summary>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="this"> The @this to act on. </param>
        /// <param name="action"> The action. </param>
        public static void InvokeEx<T>(this T @this, Action<T> action) where T : ISynchronizeInvoke
        {
            if (@this.InvokeRequired) @this.Invoke(action, new object[] { @this });
            else action(@this);
        }
    }
}