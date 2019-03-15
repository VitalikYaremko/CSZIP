using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSZIP.API.Domain.Interfaces.Services
{
    public interface IProcessingService
    {
        string DecryptStringAes(string cipherTextString, byte[] Key);
        Task<string> StoreJsonInFile(string json, string fileName);
    }
}
