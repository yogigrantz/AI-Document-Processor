using AIReadDocs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.DIClasses;
using Services.Interfaces;
using System;
using System.Net.Http.Headers;
using YG.LogProvider;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
var config = builder.Configuration;
builder.Logging.ClearProviders();
builder.Logging.AddYGLogProvider(option =>
{
    option.BaseDirectory = config["Log:FolderName"]?? "C:\\Temp\\MyLog";
    option.FolderName = "Log";
    option.MinimumLevel = LogLevel.Information;
    option.MaxSize = 50000;
    option.ExpiryDays = 5;
});
builder.Services.AddSingleton<IPdfPigTextExtractor, PdfPigTextExtractor>();
builder.Services.AddTransient<IConvertPDFToText, ConvertPDFToText>();
builder.Services.AddHttpClient<IPostToAI, PostToAI>(client =>
{
    client.BaseAddress = new Uri(config["OpenAI:BaseUrl"]?? "https://api.openai.com/");
    client.Timeout = TimeSpan.FromSeconds(60);

    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer",
            builder.Configuration["OpenAI:ApiKey"]);
})
.AddStandardResilienceHandler();

builder.Services.AddTransient<IOrchestrator, Orchestrator>();
builder.Services.AddHostedService<Worker1>();

var host = builder.Build();


try
{
    await host.RunAsync();
}
catch (Exception ex)
{
    var logger = host.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Unhandled error in PostDocToAI.");
    throw;
}

