<Query Kind="Statements">
  <NuGetReference>PowerPipe</NuGetReference>
  <NuGetReference>PowerPipe.Extensions.MicrosoftDependencyInjection</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

using PowerPipe;
using PowerPipe.Builder;
using PowerPipe.Extensions.MicrosoftDependencyInjection;
using PowerPipe.Interfaces;

var services = new ServiceCollection();

services.AddPowerPipe<MyDataContext, Person>(builder =>
	builder
		.Add<FetchStep>()
		.Add<PushStep>()
	);

var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions() { ValidateOnBuild = true, ValidateScopes = true});

var pipeline = serviceProvider.GetRequiredService<IPipeline<Person>>();

await pipeline.RunAsync();

public class FetchStep : IPipelineStep<MyDataContext>
{
	public IPipelineStep<MyDataContext> NextStep { get; set; }

	public async Task ExecuteAsync(MyDataContext context, CancellationToken cancellationToken)
	{
		context.People.Add("Bob", 24);
		context.People.Add("Lisa", 21);
		Console.WriteLine("Fetched");
		await NextStep.ExecuteAsync(context, cancellationToken);
	}
}

public class PushStep : IPipelineStep<MyDataContext>
{
	public IPipelineStep<MyDataContext> NextStep { get; set; }

	public async Task ExecuteAsync(MyDataContext context, CancellationToken cancellationToken)
	{
		Console.WriteLine("Pushed");
		await NextStep.ExecuteAsync(context, cancellationToken);
	}
}

public class MyDataContext : PipelineContext<Person>
{
	public Dictionary<string, int> People { get; } = new Dictionary<string, int>();
	
	public override Person GetPipelineResult()
	{
		var person = People.First();
		return new Person()
		{
			Name = person.Key,
			Age = person.Value
		};
	}
}

public class Person
{
	public string Name { get; set; }
	public int Age { get; set; }
}

public interface IPipelineDefintitionBuilder<TContext> where TContext : class
{
	IPipelineDefintitionBuilder<TContext> Add<T>() where T : class, IPipelineStep<TContext>;
}

public class PipelineDefintitionBuilder<TContext> : IPipelineDefintitionBuilder<TContext> where TContext : class
{
	private readonly IServiceCollection _services;
	
	public PipelineDefintitionBuilder(IServiceCollection services)
	{
		_services = services;
	}

	public IPipelineDefintitionBuilder<TContext> Add<T>() where T : class, IPipelineStep<TContext>
	{
		_services.AddPowerPipeStep<T, TContext>();
		return this;
	}
}

public static class PowerPipeExtensions
{
	public static void AddPowerPipe<TContext, TResult>(this IServiceCollection services, Action<IPipelineDefintitionBuilder<TContext>> configurePipeline)
		where TContext : PipelineContext<TResult> where TResult : class
	{
		services.AddPowerPipe();
		
		var definitionBuilder = new PipelineDefintitionBuilder<TContext>(services);
		configurePipeline(definitionBuilder);
		
		services.AddSingleton<TContext>();
		services.AddSingleton<IPipeline<TResult>>(provider =>
		{
			var pipelineStepFactory = provider.GetRequiredService<IPipelineStepFactory>();
			var context = provider.GetRequiredService<TContext>();
			var builder = new PipelineBuilder<TContext, TResult>(pipelineStepFactory, context);
			return builder.Build();
		});
	}
}