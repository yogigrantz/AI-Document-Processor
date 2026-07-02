using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AIReadDocs;

public class Worker1 : BackgroundService
{
    private readonly IOrchestrator _orchestrator;
    private readonly ILogger<Worker1> _logger;
    private string _folderName = @"C:\Temp\ProcessResume";

    public Worker1(IOrchestrator orchestrator, ILogger<Worker1> logger)
    {
        this._orchestrator = orchestrator;
        this._logger = logger;
        DirectoryInfo d = new DirectoryInfo(Path.Combine(_folderName, "processed"));

        if (!Directory.Exists(d.FullName))
            Directory.CreateDirectory(d.FullName);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        DirectoryInfo d = new DirectoryInfo(_folderName);

        while (!stoppingToken.IsCancellationRequested)
        {

            FileInfo[] files = d.GetFiles();

            foreach (FileInfo file in files)
            {
                try
                {
                    string result = await _orchestrator.PostDocToAI(file.FullName);
                    File.Move(file.FullName, Path.Combine(_folderName, "processed", file.Name));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing {file.FullName}");
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }

    }
}
