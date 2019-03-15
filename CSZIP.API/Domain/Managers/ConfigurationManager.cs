using CSZIP.API.Domain.Interfaces.Managers;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CSZIP.API.Domain.Managers
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
        public string GetValue(string name)
        {
            return Configuration.GetValue<string>(name);
        }
        //public string GetValueFromDb(string name)
        //{
        //    using(var context = ...)
        //    {
        //        // get value from DB
        //    }
        //}
    }
}
