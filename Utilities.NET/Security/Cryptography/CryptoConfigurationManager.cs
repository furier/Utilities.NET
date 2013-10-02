#region File Header

// //////////////////////////////////////////////////////
// /// File: CryptoConfigurationManager.cs
// /// Author: Sander Struijk
// /// Date: 2013-09-28 15:07
// //////////////////////////////////////////////////////

#endregion

#region Using Directives

using System;
using System.Configuration;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace Utilities.NET.Security.Cryptography
{
    /// <summary>   Manager for crypto configurations. </summary>
    /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
    public static class CryptoConfigurationManager
    {
        /// <summary>  The Salt. Salt is not a password! </summary>
        private const string Salt = "GpQg4gc5R2TyBBR2Dxrxnb/3/XrmTZc/rHBLY9OjyPc=";
    
        /// <summary>   Static constructor. </summary>
        /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
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
    /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
    public class AppSettings : AppSettingsBase
    {
        /// <summary>   Constructor. </summary>
        /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
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
        /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
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
    /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
    public class ConnectionStrings : AppSettingsBase
    {
        /// <summary>   Constructor. </summary>
        /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
        /// <param name="section">  The section. </param>
        /// <param name="salt">     The salt. </param>
        public ConnectionStrings(string section, string salt) : base(section, salt) {}
    
        /// <summary>   Indexer to get or set items within this collection using array index syntax. </summary>
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
        /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
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
    /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
    public abstract class AppSettingsBase
    {
        /// <summary>   The Salt. Salt is not a password! </summary>
        private readonly string _salt;
    
        /// <summary>   The section. </summary>
        protected readonly string Section;
    
        /// <summary>   Filename of the isolated file. </summary>
        private const string ISOLATED_FILE_NAME = ".key";
    
        /// <summary>   Specialised constructor for use only by derived classes. </summary>
        /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
        /// <param name="section">  The section. </param>
        /// <param name="salt">     The Salt. Salt is not a password! </param>
        protected AppSettingsBase(string section, string salt)
        {
            Section = section;
            _salt = salt;
            InitializeIsolatedStorageKey();
        }
    
        /// <summary>   Initializes the isolated storage key. </summary>
        /// <remarks>   Sander Struijk, 30.09.2013. </remarks>
        private void InitializeIsolatedStorageKey()
        {
            using (var isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                var fileNames = isoStore.GetFileNames(ISOLATED_FILE_NAME);
                if (fileNames.Any(file => file == ISOLATED_FILE_NAME)) return;
                using (var isoStream = new IsolatedStorageFileStream(ISOLATED_FILE_NAME, FileMode.Create, isoStore))
                {
                    using (var isoWriter = new StreamWriter(isoStream))
                    {
                        var generatedKey = GenerateKey(128);
                        isoWriter.WriteLine(generatedKey);
                    }
                }
            }
        }
    
        /// <summary>   Generates a key. </summary>
        /// <remarks>   Sander Struijk, 30.09.2013. </remarks>
        /// <param name="keySize">  Size of the key. </param>
        /// <returns>   The key. </returns>
        private static string GenerateKey(int keySize)
        {
            var aesEncryption = new RijndaelManaged
                                    {
                                            KeySize = keySize,
                                            BlockSize = 128,
                                            Mode = CipherMode.CBC,
                                            Padding = PaddingMode.PKCS7
                                    };
            aesEncryption.GenerateIV();
            var ivStr = Convert.ToBase64String(aesEncryption.IV);
            aesEncryption.GenerateKey();
            var keyStr = Convert.ToBase64String(aesEncryption.Key);
            var completeKey = ivStr + "," + keyStr;
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(completeKey));
        } 
    
        /// <summary>   Gets isolated storage key. </summary>
        /// <remarks>   Sander Struijk, 30.09.2013. </remarks>
        /// <returns>   The isolated storage key. </returns>
        private string GetIsolatedStorageKey()
        {
            using (var isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                using (var isoStream = new IsolatedStorageFileStream(ISOLATED_FILE_NAME, FileMode.Open, isoStore))
                {
                    using (var isoReader = new StreamReader(isoStream))
                    {
                        var isolatedStorageKey = isoReader.ReadLine();
                        return isolatedStorageKey;
                    }
                }
            }
        }
    
        /// <summary>   Indexer to get or set items within this collection using array index syntax. </summary>
        /// <param name="key">  The key. </param>
        /// <returns>   The indexed item. </returns>
        public abstract string this[string key] { get; set; }
    
        /// <summary>   Decrypts. </summary>
        /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
        /// <param name="encryptedString">  The encrypted string. </param>
        /// <returns>   . </returns>
        protected string Decrypt(string encryptedString)
        {
            return CryptoUtil.Decrypt<AesManaged>(encryptedString, GetIsolatedStorageKey(), _salt);
        }
    
        /// <summary>   Encrypts. </summary>
        /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
        /// <param name="decryptedString">  The decrypted string. </param>
        /// <returns>   . </returns>
        protected string Encrypt(string decryptedString)
        {
            return CryptoUtil.Encrypt<AesManaged>(decryptedString, GetIsolatedStorageKey(), _salt);
        }
    
        /// <summary>   Updates the setting. </summary>
        /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
        /// <param name="key">      The key. </param>
        /// <param name="value">    The value. </param>
        protected abstract void UpdateSetting(string key, string value);
    }
}
