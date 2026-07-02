using Services.Interfaces;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace Services.DIClasses;

public class PdfPigTextExtractor : IPdfPigTextExtractor
{
    public Task<string> ExtractTextAsync(Stream pdfStream)
    {
        using var document = PdfDocument.Open(pdfStream);

        StringBuilder sb = new StringBuilder();

        foreach (var page in document.GetPages())
        {
            sb.AppendLine(page.Text);
        }

        return Task.FromResult(sb.ToString());
    }
}