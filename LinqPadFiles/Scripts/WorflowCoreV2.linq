<Query Kind="Statements">
  <NuGetReference>WorkflowCore</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>WorkflowCore.Interface</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>WorkflowCore.Models</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>WorkflowCore.Services</Namespace>
</Query>


var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddWorkflow()
    .AddSingleton<IWorkflow, MyWorkflow>()
    .AddTransient<IStepBody, Step1>()
    .AddTransient<IStepBody, Step2>()
    .AddTransient<IStepBody, Step3>()
    .BuildServiceProvider();

var host = serviceProvider.GetService<IWorkflowHost>();
host.RegisterWorkflow<MyWorkflow>();
host.Start();

host.StartWorkflow("MyWorkflow");

public class MyWorkflow : IWorkflow
{
    public string Id => "MyWorkflow";
    public int Version => 1;

    public void Build(IWorkflowBuilder<object> builder)
    {
        builder
            .StartWith<Step1>()
            .Then<Step2>()
            .Then<Step3>();
    }
}

public class Step1 : IStepBody
{
    public async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        Console.WriteLine("Step 1 executed");
        return ExecutionResult.Next();
    }
}

public class Step2 : IStepBody
{
    public async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        Console.WriteLine("Step 2 executed");
        return ExecutionResult.Next();
    }
}

public class Step3 : IStepBody
{
    public async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        Console.WriteLine("Step 3 executed");
        return ExecutionResult.Next();
    }
}


public class PluginResult<T>
{
    public T? Data { get; set; }

    public bool Success { get; set; }
}

public interface IPlugin
{
    Task<PluginResult<List<string>>> GetNames();

    Task<PluginResult<bool>> AddNames(List<string> names);
}



public class SystemAPlugin : IPlugin
{
    public async Task<PluginResult<List<string>>> GetNames()
    {
        var names = new List<string> { "John", "Jane", "Bob" };
        return new PluginResult<List<string>> { Data = names, Success = true };
    }

    public async Task<PluginResult<bool>> AddNames(List<string> names)
    {
        // Add names to System A
        return new PluginResult<bool> { Success = true };
    }
}

public class SystemBPlugin : IPlugin
{
    public async Task<PluginResult<List<string>>> GetNames()
    {
        var names = new List<string> { "Alice", "Charlie", "David" };
        return new PluginResult<List<string>> { Data = names, Success = true };
    }

    public async Task<PluginResult<bool>> AddNames(List<string> names)
    {
        // Add names to System B
        return new PluginResult<bool> { Success = true };
    }
}