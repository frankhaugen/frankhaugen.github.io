<Query Kind="Statements">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Logging</NuGetReference>
  <NuGetReference>WorkflowCore</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>WorkflowCore.Interface</Namespace>
  <Namespace>WorkflowCore.Models</Namespace>
</Query>







var services = new ServiceCollection();

services.AddLogging(x => x.ClearProviders().AddProvider(new LinqPadLoggerProvider()));

services.AddWorkflow();
services.AddTransient<TestStep>();
services.AddTransient<TestStep2>();

var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions() {ValidateOnBuild = true, ValidateScopes = true});


var host = serviceProvider.GetRequiredService<IWorkflowHost>();
host.RegisterWorkflow<TestWorkflow, Data>();
await host.StartAsync(CancellationToken.None);

var id = await host.StartWorkflow("MyVeryOwnWorkflow");

host.Registry.GetDefinition("MyVeryOwnWorkflow").Dump();

public class TestWorkflow : IWorkflow<Data>
{
    public void Build(IWorkflowBuilder<Data> builder)
        => builder.StartWith<TestStep>()
                    .Output(data => data.Message, data => data.Output)
                  .Then<TestStep2>()
                    .Input(step => step.Input, data => data.Message);


    public string Id { get; } = "MyVeryOwnWorkflow";
    public int Version { get; }
}

public class Data
{
    public string Message { get; set; }
}

public class TestStep : StepBody
{
    public string Output { get; set; }

    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Output = "Hello World!";
        return ExecutionResult.Next();
    }
}

public class TestStep2 : StepBody
{
    private readonly ILogger<TestStep2> _logger;
    private readonly IEnumerable<ISeminePlugin> _plugins;

    public TestStep2(ILogger<TestStep2> logger) => _logger = logger;

    public string Input { get; set; }
    
    public ISeminePlugin GetPlugin(PluginRunContext context) => ;

    public override ExecutionResult Run(IStepExecutionContext context)
    {
        var suppliers = GetPlugin().GetSuppliersAsync();
        _logger.LogInformation("Input: {Input}", Input);
        return ExecutionResult.Next();
    }
}