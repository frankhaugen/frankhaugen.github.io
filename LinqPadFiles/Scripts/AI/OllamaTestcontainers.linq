<Query Kind="Statements">
  <NuGetReference>OllamaSharp</NuGetReference>
  <NuGetReference>Testcontainers</NuGetReference>
  <Namespace>Docker.DotNet</Namespace>
  <Namespace>DotNet.Testcontainers.Builders</Namespace>
  <Namespace>DotNet.Testcontainers.Configurations</Namespace>
  <Namespace>DotNet.Testcontainers.Containers</Namespace>
  <Namespace>OllamaSharp</Namespace>
  <Namespace>OllamaSharp.Models.Chat</Namespace>
  <Namespace>OllamaSharp.Streamer</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>


var container = new ContainerBuilder()
	.WithName("ollama-container")
	.WithImage("ollama/ollama")
	.WithPortBinding(11434, 11434)
	.WithVolumeMount("ollama", "/root/.ollama")
	.WithExposedPort(11434)
	.Build();
	
var keepAlive = true;

container.Started += async (sender, args) => 
{
	Console.WriteLine("Container started");
	try
	{
		Console.WriteLine("Start ollama service");
		await container.ExecAsync(new List<string>() {
			"ollama run llama2"
		});
		var result = await RunAsync();
		result.Dump();
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex);
	}
	
	keepAlive = false;
};

await container.StartAsync();

while (!QueryCancelToken.IsCancellationRequested && keepAlive)
{
	await Task.Delay(1000);
}

var logs = await container.GetLogsAsync();

await container.StopAsync();
await container.DisposeAsync();

async Task<string> RunAsync()
{
	try
	{
		var client = new OllamaApiClient("http://localhost:11434/api/generate", "llama2");
		ChatRequest request = new ChatRequest();
		request.Messages ??= new List<Message>();
		request.Messages.Add(new Message(ChatRole.User, "Write me a dad-joke about cats please?", Enumerable.Empty<string>().ToArray()));
		request.Model = "llama2";
		request.Stream = false;
		var messages = new List<ChatResponseStream>();
		var responses = await client.SendChat(request, x => messages.Add(x));
		return string.Join("\n\n", responses.Select(r => r.Content));
	}
	catch (Exception ex)
	{
		return ex.ToString();
	}
}