using Services.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace Services.DIClasses;

public class PdfPigTextExtractor : IPdfPigTextExtractor
{
    public Task<string> ExtractTextAsync(Stream pdfStream)
    {
        ArgumentNullException.ThrowIfNull(pdfStream);

        using var document = PdfDocument.Open(pdfStream);
        var text = new StringBuilder();

        foreach (var page in document.GetPages())
        {
            var pageText = ContentOrderTextExtractor.GetText(page);

            if (!string.IsNullOrWhiteSpace(pageText))
            {
                text.AppendLine(pageText);
                text.AppendLine();
            }
        }

        return Task.FromResult(text.ToString());
    }
}