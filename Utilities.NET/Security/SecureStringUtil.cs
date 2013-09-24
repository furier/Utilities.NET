#region File Header

// //////////////////////////////////////////////////////
// /// File: SecureStringUtil.cs
// /// Author: Sander Struijk
// /// Date: 2013-09-24 23:21
// //////////////////////////////////////////////////////

#endregion

#region Using Directives

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

#endregion

namespace Utilities.NET.Security
{
    /// <summary>   Secure string utility. </summary>
    /// <remarks>   Furier, 24.09.2013. </remarks>
    public static class SecureStringUtil
    {
        /// <summary>   A SecureString extension method that converts a secureString to an unsecure string. </summary>
        /// <remarks>   Furier, 24.09.2013. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="secureString"> The secureString to act on. </param>
        /// <returns>   The given data converted to an unsecure string. </returns>
        public static string ConvertToUnsecureString(this SecureString secureString)
        {
            if (secureString == null) throw new ArgumentNullException("secureString");
            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>   A string extension method that converts an unsecureString to a secure string. </summary>
        /// <remarks>   Furier, 24.09.2013. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="unsecureString">   The unsecureString to act on. </param>
        /// <returns>   The given data converted to a secure string. </returns>
        public static SecureString ConvertToSecureString(this string unsecureString)
        {
            if (unsecureString == null) throw new ArgumentNullException("unsecureString");
            var secureString = new SecureString();
            unsecureString.ToList().ForEach(secureString.AppendChar);
            secureString.MakeReadOnly();
            return secureString;
        }
    }
}