namespace SK_Demo_Template;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Skills.Web;
using Microsoft.SemanticKernel.Skills.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.Orchestration;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.SemanticKernel.Planning;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

class Program
{
    static void Main(string[] args)
    {
        //C# 7.0 までは Console App の Main で Async は使えなかったので、とりあえず　Async が必要なやつは全部 MainAsync に書いて、Main から呼び出すようにしている。
        Task task = MainAsync();
        task.Wait();
    }

    private static async Task MainAsync()
    {
        var builder = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        var key = builder["APIKey"];
        var endpoint = builder["Endpoint"];
        var model = builder["DeployName"];

        var kernel = Kernel.Builder
        .Configure(c =>
        {
            c.AddAzureChatCompletionService(model, endpoint, key);
        })
        .WithLogger(LoggerFactory.Create(b =>
        {
            b.AddFilter(_ => true);
            b.AddConsole();
        }).CreateLogger<Program>())
        .Build();

        //ここで Semantic Kernel の初期化が終わるので、そのあと色々やる。
        //とりあえずログに色々出してみたり…
        kernel.Log.LogInformation("Deployment : " + model);
        kernel.Log.LogInformation("Endpoint : " + endpoint);
        kernel.Log.LogInformation("key : " + key);
        kernel.Log.LogInformation("Kernel initialized");

        Console.WriteLine("実行終了するには何かキーを押してください。");
        Console.ReadLine();
    }
}
