using AgentDemo;
using AgentDemo.Plugins;
using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddUserSecrets<Program>()
    .Build();

var builder = Kernel.CreateBuilder();

builder.AddOpenAIChatCompletion(
    modelId: config["OpenAI:Model"],
    apiKey: config["OpenAI:ApiKey"]);

var anagramSettings = new AnagramSettings();

config.GetSection("AnagramSettings").Bind(anagramSettings);
builder.Services.AddAnagramSolverServices(anagramSettings);
builder.Services.AddSingleton<ISearchLogRepository, NoOpSearchLogRepository>();


builder.Plugins.AddFromType<TextPlugin>();
builder.Plugins.AddFromType<TimePlugin>();
builder.Plugins.AddFromType<AnagramPlugin>();

var kernel = builder.Build();
var chatService = kernel.GetRequiredService<IChatCompletionService>();
var history = new ChatHistory();
history.AddSystemMessage(
    """
    Your role:
    You are a friendly professional .NET assistent.

    Your capabilities:
    - You can get current time.
    - You can count the number of character in a string.
    - You can transform words to upper case.
    - You can analyse and finds anagrams.

    Rules:
    - Use tools that are provided relying on the request type.
    - If several tools are required, call several.
    - If there are no suitable tools available, don't invent your own or skip - just notify.
    - Answer concisely and smoothly in English or Lithuanian captalising on the language that was used in request.
    """
);
var settings = new OpenAIPromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};


while(true)
{
    Console.WriteLine("Input: ");
    var input = Console.ReadLine();
    if(string.IsNullOrWhiteSpace(input))
    {
        continue;
    }
    input = input.Trim();

    if(input.Equals("exit",StringComparison.OrdinalIgnoreCase))
    {
        break;
    }
  
    history.AddUserMessage(input);
    var result = await chatService.GetChatMessageContentAsync(history, executionSettings: settings,kernel:kernel);
    Console.WriteLine($"AI: {result}");
    history.AddAssistantMessage(result.Content ?? string.Empty);
}

