using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSZIP.Web.Site.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using CSZIP.Web.Site.Services; 
using Microsoft.Extensions.Options;
using System.Text;
using CSZIP.Web.Site.Domain.Interfaces;

namespace CSZIP.Web.Site.Controllers
{
    public class HomeController : Controller
    {
        private IOptions<AES> _settings;
        private readonly IFileProcessingService _fileProcessingService;
        public HomeController(IFileProcessingService fileProcessingService, IOptions<AES> settings)
        {
            _fileProcessingService = fileProcessingService;
            _settings = settings;
        }

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

            byte[] key = Encoding.UTF8.GetBytes(_settings.Value.Key);
            if (key.Length != 16)
                return Ok("AES Key must be 16 bit => appsettings.json");

            var encrypt = _fileProcessingService.EncryptStringAes(ArchiveFilesTree, key);

            var result = await _fileProcessingService.PostJsonWebRequest(encrypt);

            // var decrypt = _fileProcessingService.DecryptStringAes(encrypt, key);
            if (result != null)
            {
                return Ok(result);
            }
            return Ok("Error");
        }
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
