using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSZIP.Web.Site.Domain.Interfaces
{
    public interface IFileProcessingService
    {
        Task<object> PostJsonWebRequest(string json);
        string ParseZipDirToJSON(string filePath);
        string EncryptStringAes(string plainText, byte[] Key);
    }
}
