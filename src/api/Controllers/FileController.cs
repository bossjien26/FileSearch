using System.Threading.Tasks;
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

        private IFileToMongoService _fileToMongoService;

        private MongoClient _mongoClient;

        public FileController(AppSettings appSettings, ILogger<FileController> logger)
        {
            _logger = logger;
            _fileService = new FileService(appSettings);
            _fileToMongoService = new FileToMongoService(appSettings);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> UploadFile(UploadFileRequest uploadFile)
        {
            var medias = _fileService.UploadFile(uploadFile.FormFiles);
            return medias.Count == 0 ?
                NoContent() :
                Ok(await _fileToMongoService.InsertMediaListToMongo(medias));
        }
    }
}