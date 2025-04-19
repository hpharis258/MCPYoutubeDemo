using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System;
using System.Speech.Synthesis;
using System.Diagnostics;


var builder = Host.CreateEmptyApplicationBuilder(settings: null);

// config services
builder.Services.AddMcpServer().WithStdioServerTransport().WithToolsFromAssembly();

// run
await builder.Build().RunAsync();

// MCP Server tools
[McpServerToolType]
public static class MyTools
{
    [McpServerTool, Description("Reverses the input string.")]
    public static string Reverse(string message) => (string)message.Reverse();

    [McpServerTool, Description("Converts the input to synthesized speech, this only works on windows.")]
    public static void SpeakWindows(string message)
    {
        using (var synthesizer = new SpeechSynthesizer())
        {
            synthesizer.Speak(message);
        }
    }
    [McpServerTool, Description("Converts the input to synthesized speech, this only works on MacOs.")]
    public static void SpeakMacOS(string message)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "say",
            Arguments = message,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        });
    }
}