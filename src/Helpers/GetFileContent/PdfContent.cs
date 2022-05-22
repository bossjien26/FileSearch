using System.Text;
using UglyToad.PdfPig;

namespace Helpers.GetFileContent
{
    public class PdfContent
    {
        public string PrintPDF(string filePath)
        {
            var content = new StringBuilder();

            using (PdfDocument document = PdfDocument.Open(filePath))
            {
                for (var i = 0; i < document.NumberOfPages; i++)
                {
                    // This starts at 1 rather than 0.
                    var page = document.GetPage(i + 1);

                    foreach (var word in page.GetWords())
                    {
                        content.AppendFormat(word.Text);
                    }
                }
            }
            return content.ToString().Replace("\u0000", string.Empty);
        }
   }
}