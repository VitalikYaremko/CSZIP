using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSZIP.Web.Site.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using CSZIP.Web.Site.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text;

namespace CSZIP.Web.Site.Controllers
{
    public class HomeController : Controller
    {
        private IOptions<AES> _settings;
        public HomeController(IOptions<AES> settings)
        {
            _settings = settings;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if(file == null)
            {
                return RedirectToAction("Index");
            }
            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream); 
                }
            }

            byte[] key = Encoding.UTF8.GetBytes(_settings.Value.Key);
            if(key.Length != 16)
            {
                return Ok("AES Key must be 16 bit => appsettings.json");
            } 
            var encrypt = FileProcessingService.EncryptStringAes("Some text", key); 
            var decrypt = FileProcessingService.DecryptStringFromBytes_Aes(encrypt, key);

            return Ok(new { file.Length, filePath });
        }
        public IActionResult Index()
        {
            byte[] key = Encoding.UTF8.GetBytes(_settings.Value.Key);
            if (key.Length != 16)
                return Ok("AES Key must be 16 bit => appsettings.json");
            
            string encrypt = FileProcessingService.EncryptStringAes("Some text", key);


            var decrypt = FileProcessingService.DecryptStringFromBytes_Aes(encrypt, key);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
