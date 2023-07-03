using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;

namespace Plugins;

public class SampleNativeSkill
{
    [SKFunction("引数に取った二つの値を足す")]
    [SKFunctionContextParameter(Name = "input", Description = "一つ目の値")]
    [SKFunctionContextParameter(Name = "number2", Description = "二つ目の値")]
    public string Add(SKContext context)
    {
        Console.WriteLine("Add : " + context["input"] + " + " + context["number2"]);

        return (
            Convert.ToDouble(context["input"]) + Convert.ToDouble(context["number2"])
        ).ToString();
    }
}