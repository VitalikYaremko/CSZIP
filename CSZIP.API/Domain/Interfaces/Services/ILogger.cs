using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSZIP.API.Domain.Interfaces.Services
{
    public interface ILogger
    {
        void LogError(string error);
    }
}
