using System.IO;
using System.Threading.Tasks;

namespace Services.Interfaces;

public interface IPdfPigTextExtractor
{
    Task<string> ExtractTextAsync(Stream pdfStream);
}