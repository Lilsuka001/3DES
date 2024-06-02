using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string originalFile = "Оригинальный файл.txt";
        string encryptedFile = "Зашифрованный файл.txt";
        string decryptedFile = "Расшифрованный файл.txt";

        
        File.WriteAllText(originalFile, "Тестовый текст для шифрования 3DES");

        
        byte[] key = GenerateRandomBytes(24);
        byte[] iv = GenerateRandomBytes(8);

        EncryptFile(originalFile, encryptedFile, key, iv);
        DecryptFile(encryptedFile, decryptedFile, key, iv);

        Console.WriteLine("Шифрование и дешифрование завершены");
    }

    /// <summary>
    /// Метод GenerateRandomBytes генерирует криптографически стойкие случайные байты.
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    static byte[] GenerateRandomBytes(int length)
    {
        byte[] bytes = new byte[length];
        RandomNumberGenerator.Fill(bytes);
        return bytes;
    }

    /// <summary>
    /// Метод EncryptFile предназначен для шифрования содержимого входного файла с использованием алгоритма Triple DES (3DES) и сохранения зашифрованных данных в выходной файл.
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile"></param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    static void EncryptFile(string inputFile, string outputFile, byte[] key, byte[] iv)
    {
        using (TripleDES tdes = TripleDES.Create())
        {
            tdes.Key = key;
            tdes.IV = iv;
            tdes.Padding = PaddingMode.PKCS7;
            tdes.Mode = CipherMode.CBC;

            using (FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            using (FileStream fsEncrypted = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (ICryptoTransform encryptor = tdes.CreateEncryptor())
            using (CryptoStream cs = new CryptoStream(fsEncrypted, encryptor, CryptoStreamMode.Write))
            {
                fsInput.CopyTo(cs);
            }
        }
    }
    /// <summary>
    /// Этот метод аналогичен методу EncryptFile, но выполняет обратную операцию
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile"></param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    static void DecryptFile(string inputFile, string outputFile, byte[] key, byte[] iv)
    {
        using (TripleDES tdes = TripleDES.Create())
        {
            tdes.Key = key;
            tdes.IV = iv;
            tdes.Padding = PaddingMode.PKCS7;
            tdes.Mode = CipherMode.CBC;

            using (FileStream fsEncrypted = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            using (FileStream fsDecrypted = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (ICryptoTransform decryptor = tdes.CreateDecryptor())
            using (CryptoStream cs = new CryptoStream(fsEncrypted, decryptor, CryptoStreamMode.Read))
            {
                cs.CopyTo(fsDecrypted);
            }
        }
    }
}
