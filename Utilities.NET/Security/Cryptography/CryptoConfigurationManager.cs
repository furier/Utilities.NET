#region File Header

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
        
        /// <summary>   Static constructor. </summary>
        /// <remarks>   Furier, 25.09.2013. </remarks>
        static CryptoConfigurationManager()
        {
            AppSettings = new AppSettings();
        }
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
        public string this[string key]
        {
            get
            {
                var encryptedString = ConfigurationManager.AppSettings[key];
                var encryptedData = Encoding.UTF8.GetBytes(encryptedString);
                var decryptedData = ProtectedData.Unprotect(encryptedData, Entropy, DataProtectionScope.CurrentUser);
                var decryptedString = Convert.ToBase64String(decryptedData);
                return decryptedString;
            }
            set
            {
                var decryptedString = Convert.ToString(value);
                var decryptedData = Encoding.UTF8.GetBytes(decryptedString);
                var encryptedData = ProtectedData.Protect(decryptedData, Entropy, DataProtectionScope.CurrentUser);
                var encryptedString = Convert.ToBase64String(encryptedData);
                UpdateSetting(key, encryptedString);
            }
        }
        
        /// <summary>   Updates the setting. </summary>
        /// <remarks>   Furier, 25.09.2013. </remarks>
        /// <param name="key">      The key. </param>
        /// <param name="value">    The value. </param>
        private static void UpdateSetting(string key, string value)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
