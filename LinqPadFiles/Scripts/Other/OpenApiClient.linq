<Query Kind="Statements">
  <NuGetReference>Betalgo.OpenAI.GPT3</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Hosting</NuGetReference>
  <NuGetReference>OpenAI.Net.Client</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>OpenAI.GPT3.Extensions</Namespace>
  <Namespace>OpenAI.Net</Namespace>
  <Namespace>OpenAI.GPT3.ObjectModels.RequestModels</Namespace>
</Query>

using var host = Host.CreateDefaultBuilder()
            .ConfigureServices((builder, services) =>
            {
                services.AddOpenAIService(settings => { settings.ApiKey = "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"; });
                //services.AddOpenAIServices(options => { options.ApiKey = "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"; });
            })
            .Build();

var openAi = host.Services.GetRequiredService<OpenAI.GPT3.Interfaces.IOpenAIService>();

//await foreach (var response in openAi.TextCompletion.GetStream(ModelTypes.CodeDavinci002, "Can you show me what does the Q# syntax look like?"))
//{
var response = await openAi.Completions.Create(new CompletionCreateRequest() { Prompt = "Can you show me what does the Q# syntax look like?", BestOf = 5 }, OpenAI.GPT3.ObjectModels.Models.Model.Davinci);

    if (response.Successful)
    {
        foreach (var result in response.Choices)
        {
            Console.WriteLine(result.Text);
        }
    }
    else
    {
    }
    //Console.WriteLine(response?.Result?.Choices[0].Text);
//}

//var response = await openAi.TextCompletion.TextCompletion.Get("Can you show me what does the Q# syntax look like?");

//if (response.IsSuccess)
//{
//    Console.WriteLine(response.Result.Model);
//
//    foreach (var result in response.Result.Choices)
//    {
//        Console.WriteLine(result.Text);
//    }
//}
//else
//{
//    Console.WriteLine($"{response.ErrorMessage}");
//}