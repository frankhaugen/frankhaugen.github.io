<Query Kind="Program">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Order</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

#load "BenchmarkDotNet"

void Main()
{
	RunBenchmark();
	return;  // Uncomment this line to initiate benchmarking.
}

[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
[MemoryDiagnoser]
public class JsonSerializationBenchmarks
{
	private const string _json = "{\"Integer\":789,\"Name\":\"Test\",\"WeekDay\": 3 }";
	private static readonly TargetClass _target = new TargetClass
	{
		Integer = 123,
		Name = "Example",
		WeekDay = DayOfWeek.Monday
	};

	[Benchmark]
	public void SerializeNewtonsoftJson()
	{
		var json = Newtonsoft.Json.JsonConvert.SerializeObject(_target);
	}


	[Benchmark]
	public void SerializeSystemTextJson()
	{
		var json = System.Text.Json.JsonSerializer.Serialize(_target);
	}


	[Benchmark]
	public void DeserializeSystemTextJson()
	{
		var target = System.Text.Json.JsonSerializer.Deserialize<TargetClass>(_json);
	}


	[Benchmark]
	public void DeserializeNewtonsoftJson()
	{
		var target = Newtonsoft.Json.JsonConvert.DeserializeObject<TargetClass>(_json);
	}


	private record TargetClass
	{
		public int? Integer { get; set; }

		public string Name { get; set; }

		public DayOfWeek? WeekDay { get; set; }

		// continue
	}
}

