using CSZIP.Web.Site.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSZIP.Web.Site.Domain.Services
{
    public class Logger : ILogger
    {
        public async void LogError(string error)
        {
            string createdOn = DateTime.Now.ToString();
            string pathFoulder = "wwwroot\\Logs";
            string path = Path.Combine(Directory.GetCurrentDirectory(), pathFoulder, "Logs.txt");
            
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("--[");
            sb.AppendLine(error);
            sb.AppendLine("_____________________________");
            sb.AppendLine(createdOn);
            sb.AppendLine("]--");

            await File.AppendAllTextAsync(path, sb.ToString());
        }
    }
}
