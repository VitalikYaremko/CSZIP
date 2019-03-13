using CSZIP.Web.Site.Helpers;
using CSZIP.Web.Site.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSZIP.Web.Site.Services
{
    public class FileProcessingService
    {
        public static string ParseZipDirToJSON(string filePath)
        {
            try
            {
                List<string> paths = new List<string>();
                string jsonString = null;
                using (ZipArchive archive = ZipFile.OpenRead(filePath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        #region
                        //string file = null;
                        //var dirs = Regex.Split(entry.FullName, "/");
                        //if (dirs.Last() != "")
                        //{
                        //    file = dirs.Last();
                        //}
                        //foreach (string dir in dirs)
                        //{
                        //    TreeModel tree = new TreeModel()
                        //    {
                        //        Id = Guid.NewGuid(),
                        //        Level = dirs.Length - 1,
                        //        DirName = dir
                        //    };
                        //    tree.FileNames.Add(file);
                        //}
                        //stringBuilder.Append(entry); 
                        #endregion
                        paths.Add(entry.ToString());
                    }
                    jsonString = JSONHelper.ToJSON(paths);
                }
                return jsonString; 
            }
            catch (Exception e)
            { 
                throw;
            }
        }
        public static string EncryptStringAes(string plainText, byte[] Key)
        {
            try
            {
                if (plainText == null || plainText.Length <= 0)
                    throw new ArgumentNullException("plainText");
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("Key");

                byte[] encrypted;

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;

                    // Шифратор для перетворення потоку
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.Key);

                    // Потоки які необіхдні для шифрування
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }

                return Convert.ToBase64String(encrypted);
            }
            catch (Exception e)
            {
                //тут можна підключити свій лог сервіс і писати логи про помилки // _logger.LogError(e);
                throw;
            }
        }
        public static string DecryptStringAes(string cipherTextString, byte[] Key)
        {
            try
            {
                byte[] cipherText = Convert.FromBase64String(cipherTextString);

                if (cipherText == null || cipherText.Length <= 0)
                    throw new ArgumentNullException("cipherText");
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("Key");

                string plaintext = null;

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.Key);

                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                }

                return plaintext;
            }
            catch (Exception e)
            {
                //тут можна підключити свій лог сервіс і писати логи про помилки // _logger.LogError(e);
                throw;
            }
        }
    }
}
