using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using CSZIP.Web.Site.Domain.Interfaces; 
using CSZIP.Web.Site.Domain.Helpers;
using CSZIP.Web.Site.Infrastructure.Filters; 

namespace CSZIP.Web.Site.Controllers
{
    [CustomExceptionFilter]
    public class HomeController : Controller
    {
        private readonly IFileProcessingService _fileProcessingService;
        private readonly IConfigurationManager _configurationManager;
        public HomeController(IFileProcessingService fileProcessingService, IConfigurationManager configurationManager)
        {
            _fileProcessingService = fileProcessingService;
            _configurationManager = configurationManager;
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewData["Result"] = "File not selected";
                return View();
            }
            if (file.ContentType != "application/x-zip-compressed")
            {
                ViewData["Result"] = "The wrong format of file, must be .zip ";
                return View();
            }

            var filePath = Path.GetTempFileName();

            string JsonFoulderAndFiles = null;

            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                JsonFoulderAndFiles = _fileProcessingService.ParseZipDirToJSON(filePath);
            }

            byte[] key = Encoding.UTF8.GetBytes(_configurationManager.GetValue("AES:Key"));// берем ключи із appsettings.json
            if (key.Length != 16)
                return Ok("AES Key must be 16 bit => appsettings.json");

            var encrypt = _fileProcessingService.EncryptStringAes(JsonFoulderAndFiles, key);// зашифровуємо 

            var result = await _fileProcessingService.SendJsonToSaveInServer(encrypt, TextHelper.RemoveExtension(file.FileName));// надсилаємо на сервер

            ViewData["Result"] = result;
            return View(); 
        }
        public IActionResult Index()
        {
            return View("UploadFile");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                if (statusCode.Value == 404 || statusCode.Value == 500)
                {
                    var viewName = "Error" + statusCode.ToString();
                    return View(viewName);
                }
            }
            return View();
        }
        
        public IActionResult TestInternalServerError()
        {
            _fileProcessingService.ParseZipDirToJSON("wrongPath");
            return View();
        }
    }
}
