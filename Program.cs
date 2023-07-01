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

        // テスト用に、UserSecrets から読み込んだ設定をコンソールに出力している。
        System.Console.WriteLine(key);
        System.Console.WriteLine(endpoint);
        System.Console.WriteLine(model);

        var kernel = Kernel.Builder
        .Configure(c =>
        {
            c.AddAzureChatCompletionService(model, endpoint, key);
        })
        .Build();

        //ここで Semantic Kernel の初期化が終わるので、そのあと色々やる。

        Console.WriteLine("実行終了するには何かキーを押してください。");
        Console.ReadLine();
    }
}
