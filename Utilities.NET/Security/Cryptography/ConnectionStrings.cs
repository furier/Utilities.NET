using System.Configuration;

// ReSharper disable InheritdocConsiderUsage

namespace Utilities.NET.Security.Cryptography
{
    /// <summary> Connection strings. </summary>
    public class ConnectionStrings : AppSettingsBase
    {
        /// <summary> Constructor. </summary>
        /// <param name="section"> The section. </param>
        /// <param name="salt"> The salt. </param>
        public ConnectionStrings(string section, string salt) : base(section, salt) { }

        /// <summary> Indexer to get or set items within this collection using array index syntax. </summary>
        /// <param name="key"> The key. </param>
        /// <returns> The indexed item. </returns>
        public override string this[string key]
        {
            get
            {
                var encryptedString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                return string.IsNullOrEmpty(encryptedString) ? string.Empty : Decrypt(encryptedString);
            }
            set => UpdateSetting(key, string.IsNullOrEmpty(value) ? value : Encrypt(value));
        }

        /// <summary> Updates the setting. </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        protected override void UpdateSetting(string key, string value)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.ConnectionStrings.ConnectionStrings[key].ConnectionString = value;
            configuration.Save();
            ConfigurationManager.RefreshSection(Section);
        }
    }
}