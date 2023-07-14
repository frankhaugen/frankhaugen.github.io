<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Hosting</NuGetReference>
  <NuGetReference>Silk.NET.Input</NuGetReference>
  <NuGetReference>Silk.NET.OpenGL</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Silk.NET.Windowing</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
    var builder = Host.CreateApplicationBuilder();


    builder.Services.AddSingleton<IWindow>(Window.Create(new WindowOptions() {
        
    }));    
    builder.Services.AddHostedService<HostService>();    
    
    var app = builder.Build();
    
    app.Run();
}



public class HostService : BackgroundService
{
    private readonly IWindow _window;
    
    public HostService(IWindow window)
    {
        _window = window;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _window.Run();
    }
}