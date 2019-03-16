using CSZIP.Web.Site.Domain.Interfaces;
using CSZIP.Web.Site.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CSZIP.Web.Site.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly string _serviceUrl;
        public FileProcessingService(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            _serviceUrl = _configurationManager.GetConnectionString("CSZIP");
        }
        public async Task<object> SendJsonToSaveInServer(string json,string fileName)
        {
           return await HttpUtils.PostJson($"{_serviceUrl}/api/decrypt-and-save-json/{fileName}", json);
        }
        public string ParseZipDirToJSON(string filePath)
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
        public string EncryptStringAes(string plainText, byte[] Key)
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

                    // Encryptor to convert the stream
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.Key);

                    // Threads that are required for encryption
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
                //here you can connect your log service and write error logs // _logger.LogError(e);
                throw;
            }
        }
    }
}
