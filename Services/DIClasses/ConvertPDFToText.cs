using Services.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace Services.DIClasses;

public class ConvertPDFToText : IConvertPDFToText
{
    private readonly IPdfPigTextExtractor _pdfextractor;

    public ConvertPDFToText(IPdfPigTextExtractor pdfextractor)
    {
        _pdfextractor = pdfextractor;
    }

    public async Task<string> ConvertToText(string fileName)
    {
        if (!File.Exists(fileName))
            return "File does not exist";

        using var stream = File.OpenRead(fileName);
        return await _pdfextractor.ExtractTextAsync(stream);
    }
}
