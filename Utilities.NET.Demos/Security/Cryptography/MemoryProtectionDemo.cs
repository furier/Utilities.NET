#region File Header

// //////////////////////////////////////////////////////
// /// File: MemoryProtectionDemo.cs
// /// Author: Sander Struijk
// /// Date: 2013-09-28 15:07
// //////////////////////////////////////////////////////

#endregion

#region Using Directives

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Utilities.NET.Security.Cryptography;

#endregion

namespace Utilities.NET.Demos.Security.Cryptography
{
    /// <summary>   Memory protection demo. </summary>
    /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
    public class MemoryProtectionDemo
    {
        /// <summary>   Runs this object. </summary>
        /// <remarks>   Sander Struijk, 25.09.2013. </remarks>
        public static void Run()
        {
            try
            {
                /////////////////////////////// 
                // 
                // Memory Encryption - ProtectedMemory 
                // 
                /////////////////////////////// 
                // Create the original data to be encrypted (The data length should be a multiple of 16). 
                var toEncrypt = Encoding.ASCII.GetBytes("ThisIsSomeData16");
                Console.WriteLine("Original data: " + Encoding.ASCII.GetString(toEncrypt));
                Console.WriteLine("Encrypting...");
                // Encrypt the data in memory.
                CryptoUtil.EncryptInMemoryData(toEncrypt, MemoryProtectionScope.SameLogon);
                Console.WriteLine("Encrypted data: " + Encoding.ASCII.GetString(toEncrypt));
                Console.WriteLine("Decrypting...");
                // Decrypt the data in memory.
                CryptoUtil.DecryptInMemoryData(toEncrypt, MemoryProtectionScope.SameLogon);
                Console.WriteLine("Decrypted data: " + Encoding.ASCII.GetString(toEncrypt));
                /////////////////////////////// 
                // 
                // Data Encryption - ProtectedData 
                // 
                /////////////////////////////// 
                // Create the original data to be encrypted
                toEncrypt = Encoding.ASCII.GetBytes("This is some data of any length.");
                // Create a file.
                var fStream = new FileStream("Data.dat", FileMode.OpenOrCreate);
                // Create some random entropy. 
                var entropy = CryptoUtil.CreateRandomEntropy();
                Console.WriteLine();
                Console.WriteLine("Original data: " + Encoding.ASCII.GetString(toEncrypt));
                Console.WriteLine("Encrypting and writing to disk...");
                // Encrypt a copy of the data to the stream. 
                var bytesWritten = CryptoUtil.EncryptDataToStream(toEncrypt, entropy, DataProtectionScope.CurrentUser, fStream);
                fStream.Close();
                Console.WriteLine("Reading data from disk and decrypting...");
                // Open the file.
                fStream = new FileStream("Data.dat", FileMode.Open);
                // Read from the stream and decrypt the data. 
                var decryptData = CryptoUtil.DecryptDataFromStream(entropy, DataProtectionScope.CurrentUser, fStream, bytesWritten);
                fStream.Close();
                Console.WriteLine("Decrypted data: " + Encoding.ASCII.GetString(decryptData));
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
        }
    }
}