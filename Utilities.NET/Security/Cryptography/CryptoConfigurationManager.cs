// ReSharper disable MemberCanBePrivate.Global

namespace Utilities.NET.Security.Cryptography
{
    /// <summary> Manager for crypto configurations. </summary>
    public static class CryptoConfigurationManager
    {
        /// <summary> The Salt. Salt is not a password! </summary>
        private const string Salt = "GpQg4gc5R2TyBBR2Dxrxnb/3/XrmTZc/rHBLY9OjyPc=";

        /// <summary> Static constructor. </summary>
        static CryptoConfigurationManager()
        {
            AppSettings = new AppSettings("appSettings", Salt);
            ConnectionStrings = new ConnectionStrings("connectionStrings", Salt);
        }

        /// <summary> Gets or sets the application settings. </summary>
        /// <value> The application settings. </value>
        public static AppSettings AppSettings { get; }

        /// <summary> Gets or sets the connection strings. </summary>
        /// <value> The connection strings. </value>
        public static ConnectionStrings ConnectionStrings { get; }
    }
}