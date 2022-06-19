using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

namespace Test.API.Controller
{
    [TestFixture]
    public class FileControllerTest : BaseController
    {
        public IFormFile Should_Upload_Single_File()
        {
            //Arrange

            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile with desired data
            return new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
        }

        private async Task<HttpResponseMessage> testFile()
        {
            using (var file2 = File.OpenRead(@"../../../test.pdf"))
            using (var content2 = new StreamContent(file2))
            using (var formData = new MultipartFormDataContent())
            {
                // Add file (file, field name, file name)
                formData.Add(content2, "FormFiles.File", "test.pdf");

                return await _httpClient.PostAsync("/api/file", formData);
            }
        }

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        //https://codingcanvas.com/different-ways-of-uploading-files-using-http-based-apis-part-1/
        [Test]
        public async Task ShouldStore()
        {
            // // byte[] data;

            // var content = new MultipartFormDataContent();
            // // var file = File.OpenRead(@"../../../test.pdf");
            // // var fileName = Path.GetFileName("../../../test.pdf");

            // // MemoryStream memoryStream = new MemoryStream();
            // // file.InputStream.CopyTo(memoryStream);
            // // data = memoryStream.ToArray();

            // ByteArrayContent bytecontent = new ByteArrayContent(File.ReadAllBytes(@"../../../test.pdf"));
            // bytecontent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            // bytecontent.Headers.ContentDisposition.Name = "FormFiles.File";
            // bytecontent.Headers.ContentDisposition.FileName = "test.pdf";
            // bytecontent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            // content.Add(bytecontent);



            using var content = new MultipartFormDataContent();
            // foreach (var val in values)
            // {
            //     content.Add(new StringContent(val.Value), val.Key);
            // }
            var fileContent = new StreamContent(new FileStream(@"../../../test.pdf", FileMode.Open));
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
            {
                Name = "File",
                FileName = "test.pdf"
            };
            content.Add(fileContent);

            var response = await _httpClient.PostAsync("/api/file", content);
            // //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        // static void PostMultipartFormData()
        // {
        //     using var client = new HttpClient();
        //     var values = new[]
        //     {
        //     new KeyValuePair<string,string>("CustomerId","123123"),
        //     new KeyValuePair<string,string>("CustomerName","ABC Ltd."),
        //     };
        //     using var content = new MultipartFormDataContent();
        //     foreach (var val in values)
        //     {
        //         content.Add(new StringContent(val.Value), val.Key);
        //     }
        //     var fileContent = new StreamContent(new FileStream(uploadFilePath, FileMode.Open));
        //     fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(fileContentType);
        //     fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
        //     {
        //         Name = "File",
        //         FileName = "Text.txt"
        //     };
        //     content.Add(fileContent);
        //     var requestUri = "/api/Upload/FormDataUpload";
        //     client.BaseAddress = new Uri(apiBaseAddress);
        //     var result = client.PostAsync(requestUri, content).Result;
        //     Console.WriteLine($"Response : {result.StatusCode}");
        // }


        // public class FileFormData
        // {
        //     public string CustomerId { get; set; }

        //     public string CustomerName { get; set; }

        //     public IFormFile File { get; set; }
        // }
    }
}