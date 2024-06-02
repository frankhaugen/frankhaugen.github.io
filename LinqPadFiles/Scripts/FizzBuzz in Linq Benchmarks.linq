<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <NuGetReference>BenchmarkDotNet.Diagnostics.dotTrace</NuGetReference>
  <NuGetReference>BenchmarkDotNet.Diagnostics.Windows</NuGetReference>
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
  <Namespace>BenchmarkDotNet.Diagnostics.Windows.Configs</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

#LINQPad optimize+
#LINQPad admin

void Main()
{
	Util.AutoScrollResults = true;
	BenchmarkRunner.Run<Benchmarks>();
}

[MemoryDiagnoser]
[ThreadingDiagnoser]
[TailCallDiagnoser]
[InliningDiagnoser(true, true)]
[DisassemblyDiagnoser]
[InProcess]
public class Benchmarks
{
	string FizzBuzz(bool parallel)
	{
		return string.Join(
			"\n",
			(parallel ? Range.AsParallel() : Range)
				.Select(key => (Key: key, Value: string.Empty))
				.Select(data => data.Key % 3 == 0 ? (data.Key, Value: "Fizz") : data)
				.Select(data => data.Key % 5 == 0 ? (data.Key, Value: data.Value + "Buzz") : data)
				.Select(data => string.IsNullOrEmpty(data.Value) ? (data.Key, Value: data.Key.ToString()) : data)
				.OrderBy(p => p.Key)
				.Select(p => p.Value));
	}

	IEnumerable<int> Range;

	[Benchmark]
	public void BenchmarkParallel()   // Benchmark methods must be public.
	{
		FizzBuzz(true);
	}

	[Benchmark]
	public void BenchmarkNonParallel()   // Benchmark methods must be public.
	{
		FizzBuzz(false);
	}

	[GlobalSetup]
	public void BenchmarkSetup()
	{
		Range = Enumerable.Range(1, 1_000);
	}
}