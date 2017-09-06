using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace Utilities.NET.Extensions
{
    /// <summary> Secure string utilities. </summary>
    public static class SecureStringExtensions
    {
        /// <summary> A SecureString extension method that converts a secureString to an unsecure string. </summary>
        /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        /// <param name="secureString"> The secureString to act on. </param>
        /// <returns> The given data converted to an unsecure string. </returns>
        public static string ConvertToUnsecureString(this SecureString secureString)
        {
            if (secureString == null) throw new ArgumentNullException(nameof(secureString));
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

        /// <summary> A string extension method that converts an unsecureString to a secure string. </summary>
        /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        /// <param name="unsecureString"> The unsecureString to act on. </param>
        /// <returns> The given data converted to a secure string. </returns>
        public static SecureString ConvertToSecureString(this string unsecureString)
        {
            if (unsecureString == null) throw new ArgumentNullException(nameof(unsecureString));
            var secureString = new SecureString();
            unsecureString.ToList().ForEach(secureString.AppendChar);
            secureString.MakeReadOnly();
            return secureString;
        }
    }
}