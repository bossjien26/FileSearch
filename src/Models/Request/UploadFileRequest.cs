using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Models.Request
{
    public class UploadFileRequest
    {
        [Required]
        public List<IFormFile> FormFiles { get; set; }
    }
}