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
    /// <summary>   Manager for crypto configurations. </summary>
    /// <remarks>   Furier, 25.09.2013. </remarks>
    public static class CryptoConfigurationManager
    {
        /// <summary>  The Salt. Salt is not a password! </summary>
        private const string Salt = "!%¤%/¤#SF@//%SDV##¤%/)ASD!";

        /// <summary>   Static constructor. </summary>
        /// <remarks>   Furier, 25.09.2013. </remarks>
        static CryptoConfigurationManager()
        {
            AppSettings = new AppSettings("appSettings", Salt);
            ConnectionStrings = new ConnectionStrings("connectionStrings", Salt);
        }

        /// <summary>   Gets or sets the application settings. </summary>
        /// <value> The application settings. </value>
        public static AppSettings AppSettings { get; set; }

        /// <summary>   Gets or sets the connection strings. </summary>
        /// <value> The connection strings. </value>
        public static ConnectionStrings ConnectionStrings { get; set; }
    }

    /// <summary>   Application settings. </summary>
    /// <remarks>   Furier, 25.09.2013. </remarks>
    public class AppSettings : AppSettingsBase
    {
        /// <summary>   Constructor. </summary>
        /// <remarks>   Furier, 25.09.2013. </remarks>
        /// <param name="section">  The section. </param>
        /// <param name="salt">     The salt. </param>
        public AppSettings(string section, string salt) : base(section, salt) {}

        /// <summary>   Indexer to get or set items within this collection using array index syntax. </summary>
        /// <param name="key">  The key. </param>
        /// <returns>   The indexed item. </returns>
        public override string this[string key]
        {
            get
            {
                var encryptedString = ConfigurationManager.AppSettings[key];
                return string.IsNullOrEmpty(encryptedString) ? string.Empty : Decrypt(encryptedString);
            }
            set { UpdateSetting(key, string.IsNullOrEmpty(value) ? value : Encrypt(value)); }
        }

        /// <summary>   Updates the setting. </summary>
        /// <remarks>   Furier, 25.09.2013. </remarks>
        /// <param name="key">      The key. </param>
        /// <param name="value">    The value. </param>
        protected override void UpdateSetting(string key, string value)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();
            ConfigurationManager.RefreshSection(Section);
        }
    }

    /// <summary>   Connection strings. </summary>
    /// <remarks>   Furier, 25.09.2013. </remarks>
    public class ConnectionStrings : AppSettingsBase
    {
        /// <summary>   Constructor. </summary>
        /// <remarks>   Furier, 25.09.2013. </remarks>
        /// <param name="section">  The section. </param>
        /// <param name="salt">     The salt. </param>
        public ConnectionStrings(string section, string salt) : base(section, salt) {}

        //// <summary>   Indexer to get or set items within this collection using array index syntax. </summary>
        /// <param name="key">  The key. </param>
        /// <returns>   The indexed item. </returns>
        public override string this[string key]
        {
            get
            {
                var encryptedString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                return string.IsNullOrEmpty(encryptedString) ? string.Empty : Decrypt(encryptedString);
            }
            set { UpdateSetting(key, string.IsNullOrEmpty(value) ? value : Encrypt(value)); }
        }

        /// <summary>   Updates the setting. </summary>
        /// <remarks>   Furier, 25.09.2013. </remarks>
        /// <param name="key">      The key. </param>
        /// <param name="value">    The value. </param>
        protected override void UpdateSetting(string key, string value)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.ConnectionStrings.ConnectionStrings[key].ConnectionString = value;
            configuration.Save();
            ConfigurationManager.RefreshSection(Section);
        }
    }

    /// <summary>   Application settings base. </summary>
    /// <remarks>   Furier, 25.09.2013. </remarks>
    public abstract class AppSettingsBase
    {
        /// <summary>   The entropy. </summary>
        private readonly byte[] _entropy;

        /// <summary>   The Salt. Salt is not a password! </summary>
        private readonly string _salt;

        /// <summary>   The section. </summary>
        protected readonly string Section;

        /// <summary>   Specialised constructor for use only by derived classes. </summary>
        /// <remarks>   Furier, 25.09.2013. </remarks>
        /// <param name="section">  The section. </param>
        /// <param name="salt">     The Salt. Salt is not a password! </param>
        protected AppSettingsBase(string section, string salt)
        {
            Section = section;
            _salt = salt;
            _entropy = Encoding.Unicode.GetBytes(_salt);
        }

        /// <summary>   Indexer to get or set items within this collection using array index syntax. </summary>
        /// <param name="key">  The key. </param>
        /// <returns>   The indexed item. </returns>
        public abstract string this[string key] { get; set; }

        /// <summary>   Decrypts. </summary>
        /// <remarks>   Furier, 25.09.2013. </remarks>
        /// <param name="encryptedString">  The encrypted string. </param>
        /// <returns>   . </returns>
        protected string Decrypt(string encryptedString)
        {
            var encryptedData = Encoding.UTF8.GetBytes(encryptedString);
            var decryptedData = ProtectedData.Unprotect(encryptedData, _entropy, DataProtectionScope.CurrentUser);
            var decryptedString = Convert.ToBase64String(decryptedData);
            return decryptedString;
        }

        /// <summary>   Encrypts. </summary>
        /// <remarks>   Furier, 25.09.2013. </remarks>
        /// <param name="decryptedString">  The decrypted string. </param>
        /// <returns>   . </returns>
        protected string Encrypt(string decryptedString)
        {
            var decryptedData = Encoding.UTF8.GetBytes(decryptedString);
            var encryptedData = ProtectedData.Protect(decryptedData, _entropy, DataProtectionScope.CurrentUser);
            var encryptedString = Convert.ToBase64String(encryptedData);
            return encryptedString;
        }

        /// <summary>   Updates the setting. </summary>
        /// <remarks>   Furier, 25.09.2013. </remarks>
        /// <param name="key">      The key. </param>
        /// <param name="value">    The value. </param>
        protected abstract void UpdateSetting(string key, string value);
    }
}
