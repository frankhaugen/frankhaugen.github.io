<Query Kind="Statements">
  <Reference Relative="..\..\..\..\..\Frank.Networking\Frank.Networking\bin\Debug\net7.0\Frank.Networking.dll">C:\repos\frankhaugen\Frank.Networking\Frank.Networking\bin\Debug\net7.0\Frank.Networking.dll</Reference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Frank.Networking.Common</Namespace>
  <Namespace>Frank.Networking.Server</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Frank.Networking.Client</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Net</Namespace>
</Query>



var builder = Host.CreateApplicationBuilder();

builder.Logging.ClearProviders().AddProvider(new LinqPadLoggerProvider());
builder.Services.AddNetworkClient(x => { x.IPAddress = IPAddress.Loopback; });
builder.Services.AddHostedService<MessageSender>();

var app = builder.Build();

await app.RunAsync(CancellationTokenUtil.Get(TimeSpan.FromSeconds(15)));

public class MessageSender : BackgroundService
{
    private readonly INetworkClient _client;
    
    public MessageSender(INetworkClient client)
    {
        _client = client;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var text = "Hello";
        var data = Encoding.UTF8.GetBytes(text);
        await _client.SendAsync(data, stoppingToken);
    }
}