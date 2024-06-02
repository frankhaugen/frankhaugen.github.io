<Query Kind="Program">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Threading.Channels</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

void Main()
{
	var channel = Channel.CreateBounded<MyDataType>(new BoundedChannelOptions(20) // Limit to 10 items
	{
		FullMode = BoundedChannelFullMode.Wait, // Wait for space to become available
		SingleReader = true,
		SingleWriter = true
	});

	var producerTask = ProduceAsync(channel.Writer);
	var consumerTask = ConsumeAsync(channel.Reader);

	Task.WaitAll(producerTask, consumerTask)
	;
}

// Define your data type
public class MyDataType

{
	public int Value { get; set; }
}

// Producer method
public async Task ProduceAsync(ChannelWriter<MyDataType> writer)
{
	for (int i = 0; i < 100; i++) // Producing 100 items
	{
		var data = new MyDataType { Value = i };
		await writer.WriteAsync(data);
		//Console.WriteLine($"Produced: {data.Value}");
		await Task.Delay(5); // Simulate work
	}
	writer.Complete();
}

// Consumer method
public async Task ConsumeAsync(ChannelReader<MyDataType> reader)
{
	await foreach (var item in reader.ReadAllAsync())
	{
		Console.WriteLine($"Consumed: {item.Value}");
		await Task.Delay(10); // Simulate longer work than production to cause backpressure
	}
}
