using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utilities.NET.Security.Cryptography
{
    /// <summary> Cryptography utilities. </summary>
    public static class CryptoUtilities
    {
        /// <summary> Encrypts a string using a password and salt. </summary>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="text"> The string to encrypt. </param>
        /// <param name="password"> Password or Key to be used with the encryption. </param>
        /// <param name="salt"> Salt to be used with the encryption. </param>
        /// <returns> The encrypted value. </returns>
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
                {
                    writer.Write(text);
                }
                return Convert.ToBase64String(buffer.ToArray());
            }
        }

        /// <summary> Encrypt in memory data. </summary>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        /// <param name="buffer"> The buffer. </param>
        /// <param name="scope"> The scope. </param>
        public static void EncryptInMemoryData(byte[] buffer, MemoryProtectionScope scope)
        {
            if (buffer.Length <= 0) throw new ArgumentException("Buffer");
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            // Encrypt the data in memory. The result is stored in the same same array as the original data.
            ProtectedMemory.Protect(buffer, scope);
        }

        /// <summary> Encrypt data to stream. </summary>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        /// <param name="buffer"> The buffer. </param>
        /// <param name="entropy"> The entropy. </param>
        /// <param name="scope"> The scope. </param>
        /// <param name="s"> The Stream to process. </param>
        /// <returns> The length of the encrypted data. </returns>
        public static int EncryptDataToStream(byte[] buffer, byte[] entropy, DataProtectionScope scope, Stream s)
        {
            if (buffer.Length <= 0) throw new ArgumentException("Buffer");
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (entropy.Length <= 0) throw new ArgumentException("Entropy");
            if (entropy == null) throw new ArgumentNullException(nameof(entropy));
            if (s == null) throw new ArgumentNullException(nameof(s));
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

        /// <summary> Decrypts a string using a password and salt. </summary>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="text"> The string to decrypt. </param>
        /// <param name="password"> Password or Key to be used with the decryption. </param>
        /// <param name="salt"> Salt to be used with the decryption. </param>
        /// <returns> The decrypted value. </returns>
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
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary> Decrypt in memory data. </summary>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        /// <param name="buffer"> The buffer. </param>
        /// <param name="scope"> The scope. </param>
        public static void DecryptInMemoryData(byte[] buffer, MemoryProtectionScope scope)
        {
            if (buffer.Length <= 0) throw new ArgumentException("Buffer");
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            // Decrypt the data in memory. The result is stored in the same same array as the original data.
            ProtectedMemory.Unprotect(buffer, scope);
        }

        /// <summary> Decrypt data from stream. </summary>
        /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <exception cref="IOException"> Thrown when an IO failure occurred. </exception>
        /// <param name="entropy"> The entropy. </param>
        /// <param name="scope"> The scope. </param>
        /// <param name="s"> The Stream to process. </param>
        /// <param name="length"> The length. </param>
        /// <returns> The decrypted bytes. </returns>
        public static byte[] DecryptDataFromStream(byte[] entropy, DataProtectionScope scope, Stream s, int length)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            if (length <= 0) throw new ArgumentException("Length");
            if (entropy == null) throw new ArgumentNullException(nameof(entropy));
            if (entropy.Length <= 0) throw new ArgumentException("Entropy");
            var inBuffer = new byte[length];
            byte[] outBuffer;
            // Read the encrypted data from a stream. 
            if (s.CanRead)
            {
                s.Read(inBuffer, 0, length);
                outBuffer = ProtectedData.Unprotect(inBuffer, entropy, scope);
            }
            else
            {
                throw new IOException("Could not read the stream.");
            }
            return outBuffer;
        }

        /// <summary> Creates random entropy. </summary>
        /// <returns> A new array of byte. </returns>
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