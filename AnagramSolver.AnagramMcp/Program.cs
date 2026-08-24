using AnagramSolver.BusinessLogic;
using Microsoft.Extensions.Configuration;
using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AnagramSolver.AnagramMcp.Services;


var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});

builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

var anagramSettings = new AnagramSettings();
builder.Configuration.GetSection("AnagramSettings").Bind(anagramSettings);
anagramSettings.TextFileName = Path.Combine(AppContext.BaseDirectory, anagramSettings.TextFileName);

builder.Services.AddAnagramSolverServices(anagramSettings);
builder.Services.AddSingleton<ISearchLogRepository, NoOpSearchLogRepository>();

builder.Services.AddMcpServer().WithStdioServerTransport().WithToolsFromAssembly();

await builder.Build().RunAsync();




