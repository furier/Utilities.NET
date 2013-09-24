﻿#region File Header

// //////////////////////////////////////////////////////
// /// File: AppConfigCryptoUtil.cs
// /// Author: Sander Struijk
// /// Date: 2013-09-25 00:07
// //////////////////////////////////////////////////////

#endregion

#region Using Directives

using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace Utilities.NET.Security.Cryptography
{
    /// <summary>   Application configuration crypto utility. </summary>
    /// <remarks>   Furier, 25.09.2013. </remarks>
    public static class CryptoConfigurationManager
    {
        /// <summary>   Gets or sets the application settings. </summary>
        /// <value> The application settings. </value>
        public static AppSettings AppSettings { get; set; }
    }

    /// <summary>   Application settings. </summary>
    /// <remarks>   Furier, 25.09.2013. </remarks>
    public sealed class AppSettings
    {
        /// <summary>   The Salt. Salt is not a password! </summary>
        private const string Salt = "!%¤%/¤#SF@//%SDV##¤%/)ASD!";

        /// <summary>   The entropy. </summary>
        private static readonly byte[] Entropy = Encoding.Unicode.GetBytes(Salt);

        /// <summary>   Indexer to get or set items within this collection using array index syntax. </summary>
        /// <param name="key">  The key. </param>
        /// <returns>   The indexed item. </returns>
        public object this[string key]
        {
            get
            {
                var encryptedString = ConfigurationManager.AppSettings[key];
                var encryptedData = Convert.FromBase64String(encryptedString);
                return ProtectedData.Unprotect(encryptedData, Entropy, DataProtectionScope.CurrentUser);
            }
            set
            {
                var decryptedString = Convert.ToString(value);
                var decryptedData = Convert.FromBase64String(decryptedString);
                var encryptedData = ProtectedData.Protect(decryptedData, Entropy, DataProtectionScope.CurrentUser);
                var encryptedString = Convert.ToBase64String(encryptedData);
                ConfigurationManager.AppSettings[key] = encryptedString;
            }
        }
    }
}