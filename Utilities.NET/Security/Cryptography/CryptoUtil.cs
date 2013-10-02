#region File Header

// //////////////////////////////////////////////////////
// /// File: CryptoUtil.cs
// /// Author: Sander Struijk
// /// Date: 2013-09-28 15:07
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
    /// <remarks>   Sander Struijk, 24.09.2013. </remarks>
    public class CryptoUtil
    {
        /// <summary>   Encrypts a string using a password and salt. </summary>
        /// <remarks>   Sander Struijk, 24.09.2013. </remarks>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="text">     The string to encrypt. </param>
        /// <param name="password"> Password or Key to be used with the encryption. </param>
        /// <param name="salt">     Salt to be used with the encryption. </param>
        /// <returns>   . </returns>
        public static string Encrypt<T>(string text, string password, string salt) where T : SymmetricAlgorithm, new()
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt));
            SymmetricAlgorithm algorithm = new T();
            var rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            var rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);
            var transform = algorithm.CreateEncryptor(rgbKey, rgbIV);
            using (var buffer = new MemoryStream())
            {
                using (var stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(stream, Encoding.UTF8))
                        writer.Write(text);
                return Convert.ToBase64String(buffer.ToArray());
            }
        }

        /// <summary>   Encrypt in memory data. </summary>
        /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
        /// <exception cref="ArgumentException">        Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="buffer">   The buffer. </param>
        /// <param name="scope">    The scope. </param>
        public static void EncryptInMemoryData(byte[] buffer, MemoryProtectionScope scope)
        {
            if (buffer.Length <= 0) throw new ArgumentException("Buffer");
            if (buffer == null) throw new ArgumentNullException("buffer");
            // Encrypt the data in memory. The result is stored in the same same array as the original data.
            ProtectedMemory.Protect(buffer, scope);
        }

        /// <summary>   Encrypt data to stream. </summary>
        /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
        /// <exception cref="ArgumentException">        Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="buffer">   The buffer. </param>
        /// <param name="entropy">  The entropy. </param>
        /// <param name="scope">    The scope. </param>
        /// <param name="s">        The Stream to process. </param>
        /// <returns>   . </returns>
        public static int EncryptDataToStream(byte[] buffer, byte[] entropy, DataProtectionScope scope, Stream s)
        {
            if (buffer.Length <= 0) throw new ArgumentException("Buffer");
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (entropy.Length <= 0) throw new ArgumentException("Entropy");
            if (entropy == null) throw new ArgumentNullException("entropy");
            if (s == null) throw new ArgumentNullException("s");
            var length = 0;
            // Encrypt the data in memory. The result is stored in the same same array as the original data. 
            var encrptedData = ProtectedData.Protect(buffer, entropy, scope);
            // Write the encrypted data to a stream. 
            if (s.CanWrite)
            {
                s.Write(encrptedData, 0, encrptedData.Length);
                length = encrptedData.Length;
            }
            // Return the length that was written to the stream.  
            return length;
        }

        /// <summary>   Decrypts a string using a password and salt. </summary>
        /// <remarks>   Sander Struijk, 24.09.2013. </remarks>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="text">     The string to decrypt. </param>
        /// <param name="password"> Password or Key to be used with the decryption. </param>
        /// <param name="salt">     Salt to be used with the decryption. </param>
        /// <returns>   . </returns>
        public static string Decrypt<T>(string text, string password, string salt) where T : SymmetricAlgorithm, new()
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt));
            SymmetricAlgorithm algorithm = new T();
            var rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            var rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);
            var transform = algorithm.CreateDecryptor(rgbKey, rgbIV);
            using (var buffer = new MemoryStream(Convert.FromBase64String(text)))
                using (var stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                        return reader.ReadToEnd();
        }

        /// <summary>   Decrypt in memory data. </summary>
        /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
        /// <exception cref="ArgumentException">        Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="buffer">   The buffer. </param>
        /// <param name="scope">    The scope. </param>
        public static void DecryptInMemoryData(byte[] buffer, MemoryProtectionScope scope)
        {
            if (buffer.Length <= 0) throw new ArgumentException("Buffer");
            if (buffer == null) throw new ArgumentNullException("buffer");
            // Decrypt the data in memory. The result is stored in the same same array as the original data.
            ProtectedMemory.Unprotect(buffer, scope);
        }

        /// <summary>   Decrypt data from stream. </summary>
        /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <exception cref="ArgumentException">        Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <exception cref="IOException">              Thrown when an IO failure occurred. </exception>
        /// <param name="entropy">  The entropy. </param>
        /// <param name="scope">    The scope. </param>
        /// <param name="s">        The Stream to process. </param>
        /// <param name="length">   The length. </param>
        /// <returns>   . </returns>
        public static byte[] DecryptDataFromStream(byte[] entropy, DataProtectionScope scope, Stream s, int length)
        {
            if (s == null) throw new ArgumentNullException("s");
            if (length <= 0) throw new ArgumentException("Length");
            if (entropy == null) throw new ArgumentNullException("entropy");
            if (entropy.Length <= 0) throw new ArgumentException("Entropy");
            var inBuffer = new byte[length];
            byte[] outBuffer;
            // Read the encrypted data from a stream. 
            if (s.CanRead)
            {
                s.Read(inBuffer, 0, length);
                outBuffer = ProtectedData.Unprotect(inBuffer, entropy, scope);
            }
            else throw new IOException("Could not read the stream.");
            // Return the length that was written to the stream.  
            return outBuffer;
        }

        /// <summary>   Creates random entropy. </summary>
        /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
        /// <returns>   A new array of byte. </returns>
        public static byte[] CreateRandomEntropy()
        {
            // Create a byte array to hold the random value. 
            var entropy = new byte[16];
            // Create a new instance of the RNGCryptoServiceProvider. 
            // Fill the array with a random value. 
            new RNGCryptoServiceProvider().GetBytes(entropy);
            // Return the array. 
            return entropy;
        }
    }
}
