<Query Kind="Statements">
  <NuGetReference Version="2.9.3">Elsa</NuGetReference>
  <NuGetReference Version="6.0.0">Microsoft.Extensions.Hosting.Abstractions</NuGetReference>
  <NuGetReference>Rebus.ServiceProvider</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Elsa.Services</Namespace>
  <Namespace>Elsa.Builders</Namespace>
  <Namespace>Elsa.Activities.Console</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Elsa.ActivityResults</Namespace>
  <Namespace>Elsa.Services.Models</Namespace>
</Query>

// Create a service container with Elsa services.
var services = new ServiceCollection()
    .AddElsa(options => options
        .AddConsoleActivities()
        .AddWorkflow<HelloWorld>()
       
        ) // Defined a little bit below.
        
    .BuildServiceProvider();



// Get a workflow runner.
var workflowRunner = services.GetRequiredService<IBuildsAndStartsWorkflow>();

// Run the workflow.
await workflowRunner.BuildAndStartWorkflowAsync<HelloWorld>();

/// <summary>
/// A basic workflow with just one WriteLine activity.
/// </summary>
public class HelloWorld : IWorkflow
{
    public void Build(IWorkflowBuilder builder) => builder.WriteLine("Hello World!");
}

public class MyActivity : Elsa.Services.Activity
{
    public override ValueTask<IActivityExecutionResult> ExecuteAsync(ActivityExecutionContext context)
    {
        context.GetService
        return base.ExecuteAsync(context);
    }
}