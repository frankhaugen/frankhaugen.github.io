<Query Kind="Statements">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <NuGetReference>OllamaSharp</NuGetReference>
  <Namespace>OllamaSharp</Namespace>
  <Namespace>OllamaSharp.Models</Namespace>
  <Namespace>OllamaSharp.Streamer</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

var client = new OllamaApiClient("http://localhost:11434/api", "llama2");

var initialContext = new ConversationContext(new long[]
{ 
	123456789,
	9788707070070987,
	123456789,
	8707070070987,
	1234567890
});

client.ShowModelInformation("llama2").GetAwaiter().GetResult().Dump();

var words = new List<string>();
var hasStarted = false;
var firstPrompt = "Please write an a blurb for the book 'Eragon";
var continuePrompt = "please continue";
ConversationContext context = initialContext;

while (!QueryCancelToken.IsCancellationRequested && words.Count < 500)
{
	var prompt = hasStarted ? continuePrompt : firstPrompt;
	var response = await PromptAsync(prompt, context, words);
	context = response;
	hasStarted = true;
}

//Util.ClearResults();
string.Join("", words).Dump();

async Task<ConversationContext> PromptAsync(string prompt, ConversationContext context, List<string> words)
{
	var result = await client.StreamCompletion(prompt, context, async (responseStream) =>
	{
		words.Add(responseStream.Response);
		await Task.CompletedTask;
	});
	
	//Util.ClearResults();
	words.Count.Dump();
	
	return result;
}