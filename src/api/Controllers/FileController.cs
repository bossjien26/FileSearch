using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Enums;
using Helpers;
using Microsoft.AspNetCore.Http;
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

        private IGridFSService _gridFSService;

        private string _path;

        private readonly static Dictionary<string, string> _contentTypes = new Dictionary<string, string>
        {
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".gif", "image/gif"}
        };

        public FileController(AppSettings appSettings, ILogger<FileController> logger)
        {
            _path = appSettings.FilePath;
            _logger = logger;
            _fileService = new FileService(appSettings);
            _fileToMongoService = new FileToMongoService(appSettings);
            _gridFSService = new GridFSService(appSettings);
        }

        // [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin)]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest uploadFile)
        {
            if (uploadFile.File == null) { return NotFound(); }
            // if (uploadFile.FormFiles == null) { return NotFound(); }
            // if (uploadFile.FormFiles.Count == 0) { return NotFound(); }
            var medias = _fileService.UploadFile(uploadFile.File);
            return medias.Count == 0 ?
                NotFound() :
                Ok(await _fileToMongoService.InsertMediaListToMongo(medias));
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("upload")]
        public async Task<IActionResult> UploadFileToMongo(IFormFile file)
        {
            var id = new MongoDB.Bson.ObjectId();
            var ms = new MemoryStream();
            file.CopyTo(ms);
            var source = ms.ToArray();
            id = await _gridFSService.UploadFile(file.FileName, source);
            // act on the Base64 data
            return Ok(id);
        }
        // 62a5e6b397103f9a62ce8072
        [HttpGet]
        [Route("DownloadToByte")]
        public async Task<IActionResult> DownloadToByte(string id)
        {
            var fileInfo = await _gridFSService.FindFile(id);
            var bytes = await _gridFSService.Download(fileInfo.Id);
            return File(bytes, "application/pdf", "test.pdf");
        }

        // [HttpPost]
        // [Route("DownloadToStream")]
        // public async Task<IActionResult> DownloadToStream(MongoDB.Bson.ObjectId id)
        // {
        //     var list = await _gridFSService.FindFile("test.pdf");

        //     var memoryStream = await _gridFSService.DownloadToStream(list.First().Id);
        //     // memoryStream.Seek(0, SeekOrigin.Begin);
        //     // var b = await _gridFSService.Download(list.First().Id);
        //     // var path = $@"{}/test.pdf";
        //     return Ok();
        //     // 回傳檔案到 Client 需要附上 Content Type，否則瀏覽器會解析失敗。
        //     // return new FileStreamResult(memoryStream, _contentTypes[Path.GetExtension("application/pdf").ToLowerInvariant()]);
        // }

        [HttpPost]
        [Route("find")]
        public async Task<IActionResult> FindFile(string id)
        {
            var fileInfo = await _gridFSService.FindFile(id);
            return Ok(fileInfo);
        }

        [HttpPost]
        [Route("find/list")]
        public async Task<IActionResult> FindFileList(string fileName)
        {
            var fileInfos = await _gridFSService.FindFileList(fileName);
            return Ok(fileInfos);
        }

        // [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin)]
        [HttpPost]
        [Route("aa")]
        public IActionResult test([FromForm] string a)
        {
            return NoContent();
        }
    }
}