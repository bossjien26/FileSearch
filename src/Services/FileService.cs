using System;
using System.Collections.Generic;
using System.IO;
using Helpers;
using Helpers.GetFileContent;
using Microsoft.AspNetCore.Http;
using MongoEntities;
using Services.Interface;

namespace Services
{
    public class FileService : IFileService
    {
        private readonly string _path;

        private PdfContent _pdfContent;

        public FileService(AppSettings appSettings)
        {
            _path = appSettings.FilePath;
            _pdfContent = new PdfContent();
        }

        public List<Media> UploadFile(IFormFile file)
        {
            var medias = new List<Media>();
            string newFileName = GenericToFileName(System.IO.Path.GetExtension(file.FileName));
            var fileBytes = new byte[] { };
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
                // act on the Base64 data
            }
            SaveFile(file, newFileName);
            medias.Add(new Media()
            {
                OriginName = file.FileName,
                NewFileName = newFileName,
                Content = _pdfContent.PrintPDF(fileBytes)
            });
            return medias;
        }

        private string GenericToFileName(string extensions)
            => DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_security(C)" + extensions;

        private bool SaveFile(IFormFile file, string newFileName)
        {
            try
            {
                string fileNameWithPath = Path.Combine(_path, newFileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}