using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSZIP.Web.Site.Domain.Interfaces
{
    public interface IConfigurationManager
    {
        string GetConnectionString(string name);
    }
}
