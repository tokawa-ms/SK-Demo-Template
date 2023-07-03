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

using Plugins;

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
        //VSCode の Secret Store から AOAI にアクセスするための情報を取得する
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

        // ここで Semantic Kernel の初期化が終わるので、そのあと色々やる。
        // とりあえずログに色々出してみたり…
        kernel.Log.LogInformation("Deployment : " + model);
        kernel.Log.LogInformation("Endpoint : " + endpoint);
        kernel.Log.LogInformation("key : " + key);
        kernel.Log.LogInformation("Kernel initialized");

        // Semantic Skill を読み込んで使う場合のサンプル
        var myPlugin = kernel.ImportSemanticSkillFromDirectory("MyPluginsDirectory", "RouteSkill");
        var myContext = new ContextVariables();
        myContext.Set("input", "東京駅から大阪駅に行きたいです。");

        var myResult = await kernel.RunAsync(myContext, myPlugin["ExtractRoute"]);

        Console.WriteLine(myResult);

        // out-of-the-box プラグインを使う場合のサンプル
        // https://learn.microsoft.com/en-us/semantic-kernel/ai-orchestration/out-of-the-box-plugins?tabs=Csharp
        kernel.ImportSkill(new TimeSkill(), "time");
        const string ThePromptTemplate = @"
            今日は : {{time.Date}}
            いまの時間は : {{time.Time}}
            今日の曜日は : {{time.dayofWeek}}

            与えられたデータを含めて、以下の質問に JSON syntax で回答してください。
            深夜(0:00-4:59)ですか？朝(5:00-11:59)ですか？昼(12:00-17:59)ですか？夜(18:00-23:59)ですか？（朝/昼/夜/深夜)
            週末(土曜か日曜）ですか？平日（それ以外の曜日）ですか？（週末/平日）
            ";
        var myKindOfDay = kernel.CreateSemanticFunction(ThePromptTemplate, maxTokens: 150);

        var myOutput = await myKindOfDay.InvokeAsync();
        Console.WriteLine(myOutput);

        // Native Skill を呼び出すテスト
        var myNativeSkill = kernel.ImportSkill(new SampleNativeSkill(), "SampleNativeSkill");
        var myNativeContext = kernel.CreateNewContext();
        myNativeContext["input"] = "3";
        myNativeContext["number2"] = "7";
        var myResult2 = await myNativeSkill["Add"].InvokeAsync(myNativeContext);

        Console.WriteLine(myResult2);

        Console.WriteLine("実行終了するには何かキーを押してください。");
        Console.ReadLine();
    }
}
