using Microsoft.Extensions.Logging;
using Services.DTOs;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Services.DIClasses;

public class Orchestrator : IOrchestrator
{
    private readonly IConvertPDFToText _convert;
    private readonly IPostToAI _postToAI;
    private readonly ILogger<Orchestrator> _logger;

    public Orchestrator(IConvertPDFToText convert, IPostToAI postToAI, ILogger<Orchestrator> logger)
    {
        this._convert = convert;
        this._postToAI = postToAI;
        this._logger = logger;
    }

    public async Task<string> PostDocToAI(string documentFileName)
    {
        string textContent = await _convert.ConvertToText(documentFileName);

        ResumeAnalysis result = await _postToAI.Post("v1/responses", textContent);
        _logger.LogInformation(result.FullName);
        _logger.LogInformation(result.CurrentTitle);
        _logger.LogInformation(string.Join(", ", result.Skills));
        _logger.LogInformation(result.Summary);

        return "OK";
    }

}
