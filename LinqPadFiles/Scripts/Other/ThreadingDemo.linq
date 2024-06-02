<Query Kind="Statements">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

var tasks = new List<Task>();
int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };

// Using Parallel.ForEachAsync
var parallelCounter = 0;
await Parallel.ForEachAsync(items, async (item, cancellationToken) =>
{
	await Task.Delay(1000); // Simulate async work
	Console.WriteLine($"Parallel.ForEachAsync completed {item}");
	parallelCounter++;
});

Console.WriteLine("Parallel.ForEachAsync section done");

// Using Task.WhenAll
foreach (var item in items)
{
	tasks.Add(Task.Run(async () =>
	{
		await Task.Delay(1000); // Simulate async work
		Console.WriteLine($"Task {item} completed");
	}));
}

await Task.WhenAll(tasks);
Console.WriteLine("Task.WhenAll section done");