<Query Kind="Statements">
  <NuGetReference>LLamaSharp</NuGetReference>
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>


var url = "http://localhost:11434/api/generate";


var json = JsonSerializer.Serialize(new { Model = "llama2-uncensored", Prompt = "This is placeholder" });
var request = new HttpRequestMessage(HttpMethod.Post, url);
request.Content = new StringContent(json, Encoding.UTF8, "application/json");
var client = new HttpClient();
var response = await client.SendAsync(request, QueryCancelToken);

if (response.IsSuccessStatusCode)
{
	var responseContentString = await response.Content.ReadAsStringAsync(QueryCancelToken);

	var lines = responseContentString.Split("\n");
	var responseItems = new List<ResponseItem>();

	foreach (var line in lines)
	{
		
		try
		{
			var responseItem = JsonSerializer.Deserialize<ResponseItem?>(line);
			responseItems.Add(responseItem);
		}
		catch (Exception ex)
		{
			//$"Error deserializing response: {ex.Message}\n{line}".Dump();
		}

	}
	
	foreach (var responseItem in responseItems ?? Enumerable.Empty<ResponseItem>())
	{
		Console.Write(responseItem?.Response);

		if (responseItem?.Done.GetValueOrDefault(false) ?? false)
		{
			break;
		}
	}
}
else
{
	Console.WriteLine($"Error: {response.StatusCode}");
}


public class ResponseItem
{
	[JsonPropertyName("model")]
	public string? Model { get; set; }

	[JsonPropertyName("created_at")]
	public DateTime? CreatedAt { get; set; }

	[JsonPropertyName("response")]
	public string? Response { get; set; }

	[JsonPropertyName("done")]
	public bool? Done { get; set; }
	
	[JsonPropertyName("context")]
	public IEnumerable<int>? Context { get; set; }

	[JsonPropertyName("total_duration")]
	public ulong? TotalDuration { get; set; }

	[JsonPropertyName("load_duration")]
	public ulong? LoadDuration { get; set; }

	[JsonPropertyName("prompt_eval_count")]
	public int? PromptEvalCount { get; set; }

	[JsonPropertyName("prompt_eval_duration")]
	public ulong? PromptEvalDuration { get; set; }

	[JsonPropertyName("eval_count")]
	public int? EvalCount { get; set; }

	[JsonPropertyName("eval_duration")]
	public ulong? EvalDuration { get; set; }
}