using System.Text;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;

namespace Helpers.GetFileContent
{
    public class PdfContent
    {
        private static Regex _compiledUnicodeRegex = new Regex(@"[^\u0000-\u007F]", RegexOptions.Compiled);

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
            return StripUnicodeCharactersFromString(content.ToString());
        }


        private static string StripUnicodeCharactersFromString(string inputValue)
        {
            return _compiledUnicodeRegex.Replace(inputValue, string.Empty);
        }
    }
}