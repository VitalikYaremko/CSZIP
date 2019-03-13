using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSZIP.Web.Site.Helpers
{
    public static class JSONHelper
    {
        public static string ToJSON(this object obj)
        { 
            return JsonConvert.SerializeObject(obj);
        } 
    }
}
