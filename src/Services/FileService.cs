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

        public List<Media> UploadFile(List<IFormFile> formFiles)
        {
            var medias = new List<Media>();
            if (formFiles.Count > 0)
            {
                foreach (var file in formFiles)
                {
                    string newFileName = GenericToFileName(System.IO.Path.GetExtension(file.FileName));
                    SaveFile(file, newFileName);
                    medias.Add(new Media()
                    {
                        OriginName = file.FileName,
                        NewFileName = newFileName,
                        Content = _pdfContent.PrintPDF(_path + newFileName)
                    });
                }
            }
            return medias;
        }

        private string GenericToFileName(string extensions)
            => DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_security(C)." + extensions;

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