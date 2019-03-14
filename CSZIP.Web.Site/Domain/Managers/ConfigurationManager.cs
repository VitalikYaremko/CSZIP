using CSZIP.Web.Site.Domain.Interfaces;
using CSZIP.Web.Site.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CSZIP.Web.Site.Domain.Managers
{
    public class ConfigurationManager : IConfigurationManager
    {
        public static IConfiguration Configuration { get; set; }
        public ConfigurationManager()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }
        public string GetConnectionString(string name)
        {
            return Configuration.GetConnectionString(name);
        }
        //public string GetValue(string name)
        //{
        //    using(var context = ...)
        //    {
        //        // get value from DB
        //    }
        //}
    }
}
