using System.Threading.Tasks;
using Enums;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Middlewares.Authentication;
using Models.Request;
using Services;
using Services.Interface;

namespace api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;

        private IFileService _fileService;

        private IFileToMongoService _fileToMongoService;

        public FileController(AppSettings appSettings, ILogger<FileController> logger)
        {
            _logger = logger;
            _fileService = new FileService(appSettings);
            _fileToMongoService = new FileToMongoService(appSettings);
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin)]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest uploadFile)
        {
            if (uploadFile.FormFiles.Count == 0) { return NotFound(); }
            var medias = _fileService.UploadFile(uploadFile.FormFiles);
            return medias.Count == 0 ?
                NotFound() :
                Ok(await _fileToMongoService.InsertMediaListToMongo(medias));
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin)]
        [HttpGet]
        [Route("")]
        public IActionResult test()
        {
            return NoContent();
        }
    }
}