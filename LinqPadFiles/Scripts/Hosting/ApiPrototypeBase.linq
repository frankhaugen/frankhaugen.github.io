<Query Kind="Statements">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Hosting.Internal</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.AspNetCore.Routing</Namespace>
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>


var isExperimentalVisible = false;

var builder = WebApplication.CreateBuilder();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseRouting();
app.UseHealthChecks("/health");

app.MapGet("/", () => "Hello World!");

app.MapGet("/experimental", () => "This is secret!");

// Endpoint to list all registered endpoints
app.MapGet("/endpoints", async context =>
{
    var endpointDataSource = app.Services.GetRequiredService<EndpointDataSource>();

    var detailed = context.Request.Query.ContainsKey("details") && 
                   context.Request.Query["details"] == "true";

    var sb = new StringBuilder();
    foreach (var endpoint in endpointDataSource.Endpoints)
	{
		var routeEndpoint = endpoint as RouteEndpoint;
		if (routeEndpoint != null)
		{
			sb.AppendLine(routeEndpoint.RoutePattern.RawText);
			if (detailed)
			{
				sb.AppendLine("  Methods: " + string.Join(", ", routeEndpoint.Metadata.OfType<HttpMethodMetadata>().SelectMany(m => m.HttpMethods)));
				sb.AppendLine("  Metadata: " + string.Join(", ", routeEndpoint.Metadata.Select(m => m.GetType().Name)));
			}
		}
	}

	await context.Response.WriteAsync(sb.ToString());
});


var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
QueryCancelToken.Register(() => { lifetime.StopApplication(); });

await app.RunAsync();
