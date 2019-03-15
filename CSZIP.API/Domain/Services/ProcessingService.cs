using CSZIP.API.Domain.Interfaces;
using CSZIP.API.Domain.Interfaces.Managers;
using CSZIP.API.Domain.Interfaces.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CSZIP.API.Domain.Services
{
    public class ProcessingService : IProcessingService
    {
        private readonly IConfigurationManager _configurationManager;
        public ProcessingService(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }
        public async Task<string> StoreJsonInFile(string json, string fileName)
        {
            try
            {
                string Name = fileName;

                if (Name == null)
                    Name += Guid.NewGuid() + ".json";

                Name += ".json";

                //Check if there is such a file name
                string pathFoulder = "wwwroot\\Files";
                string[] files = Directory.GetFiles(pathFoulder);
                if (files.Contains(pathFoulder +"\\"+ Name))
                {
                    Name = fileName + "_" + Guid.NewGuid() + ".json";
                }

                string path = Path.Combine(Directory.GetCurrentDirectory(), pathFoulder, Name);

                await File.WriteAllTextAsync(path, json);

                return path;
            }
            catch (Exception e)
            { 
                throw;
            }
        }
        

        public string DecryptStringAes(string cipherTextString, byte[] Key)
        {
            try
            {
                if (Key == null)
                {
                    Key = Encoding.UTF8.GetBytes(_configurationManager.GetValue("AES:Key"));
                }

                if (Key.Length != 16)
                    return "AES Key must be 16 bit => appsettings.json";

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
                //here you can connect your log service and write error logs // _logger.LogError(e);
                throw;
            }
        }
    }
}
