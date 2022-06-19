using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Models.Request
{
    public class UploadFileRequest
    {
        [Required]
        public IFormFile File { get; set; }

        // [Required]
        public string FileId { get; set; }
    }

    public class FileInformation
    {
        [Required]
        public IFormFile File { get; set; }

        // [Required]
        public string FileId { get; set; }
    }
}