using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using CSZIP.Web.Site.Domain.Interfaces;
using CSZIP.Web.Site.Models;
using CSZIP.Web.Site.Domain.Helpers;
using CSZIP.Web.Site.Infrastructure.Filters;

namespace CSZIP.Web.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileProcessingService _fileProcessingService;
        private readonly IConfigurationManager _configurationManager;
        private readonly ILogger _logger;
        public HomeController(IFileProcessingService fileProcessingService, IConfigurationManager configurationManager)
        {
            _fileProcessingService = fileProcessingService;
            _configurationManager = configurationManager;
        }
        [CustomExceptionFilter]
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {

            if (file == null)
                return RedirectToAction("Index");

            var filePath = Path.GetTempFileName();

            string ArchiveFilesTree = null;

            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                ArchiveFilesTree = _fileProcessingService.ParseZipDirToJSON(filePath);
            }

            byte[] key = Encoding.UTF8.GetBytes(_configurationManager.GetValue("AES:Key"));
            if (key.Length != 16)
                return Ok("AES Key must be 16 bit => appsettings.json");

            var encrypt = _fileProcessingService.EncryptStringAes(ArchiveFilesTree, key);

            var result = await _fileProcessingService.SendJsonToSaveInServer(encrypt, TextHelper.RemoveExtension(file.FileName));

            if (result != null)
            {
                ViewData["Result"] = result;
                return View();
            }
            return Ok("Error");
        }
        public IActionResult Index()
        {
            return View();
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
    }
}
