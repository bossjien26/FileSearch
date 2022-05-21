using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Request;
using MongoDB.Driver;
using Services;
using Services.Interface;

namespace api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;

        private IFileService _fileService;

        private MongoClient _mongoClient;

        public FileController(AppSettings appSettings, ILogger<FileController> logger)
        {
            _logger = logger;
            _fileService = new FileService(appSettings);
        }

        [HttpPost]
        [Route("")]
        public IActionResult UploadFile(UploadFileRequest uploadFile)
        {
            _fileService.UploadFile(uploadFile.FormFiles);
            return Ok();
        }
    }
}