using System.Threading.Tasks;

namespace Services.Interfaces;

public interface IConvertPDFToText
{
    Task<string> ConvertToText(string fileName);
}