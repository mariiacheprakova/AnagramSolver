using AgentDemo.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var config = new ConfigurationBuilder()
.AddUserSecrets<Program>()
.Build();
var builder = Kernel.CreateBuilder();
builder.AddOpenAIChatCompletion(
modelId: config["OpenAI:Model"]!,
apiKey: config["OpenAI:ApiKey"]!);
var kernel = builder.Build();
kernel.Plugins.AddFromType<TimePlugin>();
kernel.Plugins.AddFromType<TextPlugin>();

//// Paprastas LLM kvietimas
//var result = await kernel.InvokePromptAsync(
//"Kas yra Semantic Kernel? Atsakyk 2 sakiniais.");
//Console.WriteLine(result);

var chatService = kernel.GetRequiredService<IChatCompletionService>();
var history = new ChatHistory();
history.AddSystemMessage("Tu esi draugiškas .NET asistentas.");

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

