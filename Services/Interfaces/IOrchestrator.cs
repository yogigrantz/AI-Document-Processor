using System.Threading.Tasks;

namespace Services.Interfaces;

public interface IOrchestrator
{
    Task<string> PostDocToAI(string documentFileName);
}