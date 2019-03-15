using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSZIP.API.Domain.Interfaces.Managers
{
    public interface IConfigurationManager
    {
        string GetConnectionString(string name);
        string GetValue(string name);
    }
}
