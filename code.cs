using System;
using System.IO;
using System.Security.Cryptography;

class FileEncryptionTool
{
    static void Main()
    {
        string inputFile = "input.txt";
        string encryptedFile = "encrypted.txt";
        string decryptedFile = "decrypted.txt";
        string key = "mysecretkey";

        EncryptFile(inputFile, encryptedFile, key);
        DecryptFile(encryptedFile, decryptedFile, key);
    }

    static void EncryptFile(string inputFile, string outputFile, string key)
    {
        try
        {
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

            using (var inputStream = new FileStream(inputFile, FileMode.Open))
            using (var outputStream = new FileStream(outputFile, FileMode.Create))
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.GenerateIV();

                outputStream.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                using (var cryptoStream = new CryptoStream(outputStream, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    inputStream.CopyTo(cryptoStream);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred during encryption: " + ex.Message);
        }
    }

    static void DecryptFile(string inputFile, string outputFile, string key)
    {
        try
        {
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

            using (var inputStream = new FileStream(inputFile, FileMode.Open))
            using (var outputStream = new FileStream(outputFile, FileMode.Create))
            {
                byte[] iv = new byte[16];
                inputStream.Read(iv, 0, 16);

                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = keyBytes;
                    aesAlg.IV = iv;

                    using (var cryptoStream = new CryptoStream(inputStream, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        cryptoStream.CopyTo(outputStream);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred during decryption: " + ex.Message);
        }
    }
}
