using AnagramSolver.Contracts.Models;
using Microsoft.Extensions.Configuration;

namespace AnagramSolver.Cli;
public static class ConfigurationLoader
{
    public static AnagramSettings LoadAnagramSettings()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        return configuration.GetSection("AnagramSettings").Get<AnagramSettings>()
            ?? throw new InvalidOperationException("AnagramSettings section is missing.");
    }
}
