#region File Header

// //////////////////////////////////////////////////////
// /// File: CryptoUtil.cs
// /// Author: Sander Struijk
// /// Date: 2013-09-24 23:04
// //////////////////////////////////////////////////////

#endregion

#region Using Directives

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace Utilities.NET.Security.Cryptography
{
    /// <summary>   Cryptography utility. </summary>
    /// <remarks>   Furier, 24.09.2013. </remarks>
    public class CryptoUtil
    {
        /// <summary>   Encrypts a string using a password and salt. </summary>
        /// <remarks>   Furier, 24.09.2013. </remarks>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="text">     The string to encrypt. </param>
        /// <param name="password"> Password or Key to be used with the encryption. </param>
        /// <param name="salt">     Salt to be used with the encryption. </param>
        /// <returns>   . </returns>
        public static string Encrypt<T>(string text, string password, string salt) where T : SymmetricAlgorithm, new()
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));
            SymmetricAlgorithm algorithm = new T();
            var rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            var rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);
            var transform = algorithm.CreateEncryptor(rgbKey, rgbIV);
            using (var buffer = new MemoryStream())
            {
                using (var stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write)) using (var writer = new StreamWriter(stream, Encoding.Unicode)) writer.Write(text);
                return Convert.ToBase64String(buffer.ToArray());
            }
        }

        /// <summary>   Decrypts a string using a password and salt. </summary>
        /// <remarks>   Furier, 24.09.2013. </remarks>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="text">     The string to decrypt. </param>
        /// <param name="password"> Password or Key to be used with the decryption. </param>
        /// <param name="salt">     Salt to be used with the decryption. </param>
        /// <returns>   . </returns>
        public static string Decrypt<T>(string text, string password, string salt) where T : SymmetricAlgorithm, new()
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));
            SymmetricAlgorithm algorithm = new T();
            var rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            var rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);
            var transform = algorithm.CreateDecryptor(rgbKey, rgbIV);
            using (var buffer = new MemoryStream(Convert.FromBase64String(text))) 
                using (var stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read)) 
                    using (var reader = new StreamReader(stream, Encoding.Unicode)) 
                        return reader.ReadToEnd();
        }
    }
}