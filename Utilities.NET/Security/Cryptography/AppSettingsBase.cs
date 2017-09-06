using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Utilities.NET.Security.Cryptography
{
    /// <summary> Application settings base. </summary>
    public abstract class AppSettingsBase
    {
        /// <summary> Filename of the isolated file. </summary>
        private const string IsolatedFileName = ".key";

        /// <summary> The Salt. Salt is not a password! </summary>
        private readonly string _salt;

        /// <summary> The section. </summary>
        protected readonly string Section;

        /// <summary> Specialised constructor for use only by derived classes. </summary>
        /// <param name="section"> The section. </param>
        /// <param name="salt"> The Salt. Salt is not a password! </param>
        protected AppSettingsBase(string section, string salt)
        {
            Section = section;
            _salt = salt;
            InitializeIsolatedStorageKey();
        }

        /// <summary> Indexer to get or set items within this collection using array index syntax. </summary>
        /// <param name="key"> The key. </param>
        /// <returns> The indexed item. </returns>
        public abstract string this[string key] { get; set; }

        /// <summary> Initializes the isolated storage key. </summary>
        private static void InitializeIsolatedStorageKey()
        {
            using (var isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                var fileNames = isoStore.GetFileNames(IsolatedFileName);
                if (fileNames.Any(file => file == IsolatedFileName)) return;
                using (var isoStream = new IsolatedStorageFileStream(IsolatedFileName, FileMode.Create, isoStore))
                {
                    using (var isoWriter = new StreamWriter(isoStream))
                    {
                        var generatedKey = GenerateKey(128);
                        isoWriter.WriteLine(generatedKey);
                    }
                }
            }
        }

        /// <summary> Generates a key. </summary>
        /// <param name="keySize"> Size of the key. </param>
        /// <returns> The key. </returns>
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

        /// <summary> Gets isolated storage key. </summary>
        /// <returns> The isolated storage key. </returns>
        private static string GetIsolatedStorageKey()
        {
            using (var isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                using (var isoStream = new IsolatedStorageFileStream(IsolatedFileName, FileMode.Open, isoStore))
                {
                    using (var isoReader = new StreamReader(isoStream))
                    {
                        var isolatedStorageKey = isoReader.ReadLine();
                        return isolatedStorageKey;
                    }
                }
            }
        }

        /// <summary> Deletes the isolated storage key. </summary>
        public void DeleteIsolatedStorageKey()
        {
            using (var isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                var fileNames = isoStore.GetFileNames(IsolatedFileName);
                if (fileNames.Any(file => file == IsolatedFileName))
                    isoStore.DeleteFile(IsolatedFileName);
            }
        }

        /// <summary> Decrypts the passed in value. </summary>
        /// <param name="encryptedString"> The encrypted string. </param>
        /// <returns> The decrypted value. </returns>
        protected string Decrypt(string encryptedString)
        {
            return CryptoUtilities.Decrypt<AesManaged>(encryptedString, GetIsolatedStorageKey(), _salt);
        }

        /// <summary> Encrypts the passed in value. </summary>
        /// <param name="decryptedString"> The decrypted string. </param>
        /// <returns> The encrypted value. </returns>
        protected string Encrypt(string decryptedString)
        {
            return CryptoUtilities.Encrypt<AesManaged>(decryptedString, GetIsolatedStorageKey(), _salt);
        }

        /// <summary> Updates the setting. </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        protected abstract void UpdateSetting(string key, string value);
    }
}