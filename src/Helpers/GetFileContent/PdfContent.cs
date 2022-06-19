using System.Text;
using UglyToad.PdfPig;

namespace Helpers.GetFileContent
{
    public class PdfContent
    {
        public string PrintPDF(byte[] bytes)
        {
            // var content = new StringBuilder();
            var content = "";
            using (PdfDocument document = PdfDocument.Open(bytes))
            {
                for (var i = 0; i < document.NumberOfPages; i++)
                {
                    // This starts at 1 rather than 0.
                    var page = document.GetPage(i + 1);

                    foreach (var word in page.GetWords())
                    {
                        // content.AppendFormat(word.Text);
                        content += word.Text;
                    }
                }
            }
            return content.ToString().Replace("\u0000", string.Empty);
        }
    }
}