using CSZIP.API.Domain.Interfaces;
using CSZIP.API.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSZIP.API.Controllers.API
{
    [ApiController]
    public class JsonController : Controller
    {
        private readonly IProcessingService _processingService;
        public JsonController(IProcessingService processingService)
        {
            _processingService = processingService;
        }

        [HttpPost,Route("~/api/decrypt-and-save-json/{fileName}")]
        public async Task<IActionResult> StoreJson([FromBody]string json,string fileName)
        {
            var res = _processingService.DecryptStringAes(json, null);
            var path = await _processingService.StoreJsonInFile(res, fileName);
            return Ok(path);
        }
    }
}
