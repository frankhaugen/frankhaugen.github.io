<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Hosting.Abstractions</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Logging</NuGetReference>
  <NuGetReference>Serilog</NuGetReference>
  <NuGetReference>Serilog.AspNetCore</NuGetReference>
  <NuGetReference>Serilog.Enrichers.AspNetCore</NuGetReference>
  <NuGetReference>Serilog.Enrichers.AssemblyName</NuGetReference>
  <NuGetReference>Serilog.Enrichers.ClientInfo</NuGetReference>
  <NuGetReference>Serilog.Enrichers.Context</NuGetReference>
  <NuGetReference>Serilog.Enrichers.CorrelationId</NuGetReference>
  <NuGetReference>Serilog.Enrichers.Demystifier</NuGetReference>
  <NuGetReference>Serilog.Enrichers.ExceptionData</NuGetReference>
  <NuGetReference>Serilog.Enrichers.Memory</NuGetReference>
  <NuGetReference>Serilog.Enrichers.OpenTracing</NuGetReference>
  <NuGetReference>Serilog.Enrichers.Span</NuGetReference>
  <NuGetReference>Serilog.Exceptions</NuGetReference>
  <NuGetReference>Serilog.Exceptions.EntityFrameworkCore</NuGetReference>
  <NuGetReference>Serilog.Extensions.Hosting</NuGetReference>
  <NuGetReference>Serilog.Extensions.Logging</NuGetReference>
  <NuGetReference>Serilog.Extensions.Logging.File</NuGetReference>
  <NuGetReference>Serilog.Formatting.Compact</NuGetReference>
  <NuGetReference>Serilog.Settings.Configuration</NuGetReference>
  <NuGetReference>Serilog.Sinks.Debug</NuGetReference>
  <NuGetReference>Serilog.ThrowContext</NuGetReference>
  <NuGetReference>SerilogAnalyzer</NuGetReference>
  <NuGetReference>Swashbuckle.AspNetCore.Swagger</NuGetReference>
  <NuGetReference>Swashbuckle.AspNetCore.SwaggerUI</NuGetReference>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>Serilog</Namespace>
  <Namespace>Serilog.Enrichers.AspNetCore</Namespace>
  <Namespace>Serilog.Extensions.Hosting</Namespace>
  <Namespace>Serilog.Hosting</Namespace>
  <Namespace>Serilog.Settings.Configuration</Namespace>
  <Namespace>Swashbuckle.AspNetCore.Swagger</Namespace>
  <Namespace>Swashbuckle.AspNetCore.SwaggerUI</Namespace>
  <Namespace>System.Configuration</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

async Task Main()
{
    var builder = WebApplication.CreateBuilder();
    
    builder.Services.AddSwaggerGen();
    
    var app = builder.Build();

    app.MapGet("/", () => "Hello World!");
    

    app.Run();
}






















public interface IBuilder
{
    public IServiceProvider ServiceProvider {get;}
}



public interface IBuilderContext
{
    /// <summary>
    /// Provides information about the web hosting environment an application is running.
    /// </summary>
    public IHostEnvironment Environment { get; }

    /// <summary>
    /// A collection of services for the application to compose. This is useful for adding user provided or framework provided services.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// A collection of configuration providers for the application to compose. This is useful for adding new configuration sources and providers.
    /// </summary>
    public ConfigurationManager Configuration { get; }

    /// <summary>
    /// A collection of logging providers for the application to compose. This is useful for adding new logging providers.
    /// </summary>
    public ILoggingBuilder Logging { get; }
}

