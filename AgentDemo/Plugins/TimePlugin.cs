using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace AgentDemo.Plugins;

public class TimePlugin
{
    [KernelFunction]
    [Description("Gets current local date and time")]
    public string GetCurrentTime()
    {
        Console.WriteLine("[PLUGIN] GetCurrentTime was called.");

        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
