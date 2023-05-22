<Query Kind="Program">
  <Reference Relative="..\..\..\..\Frank.Networking\Frank.Networking\bin\Debug\net7.0\Frank.Networking.dll">C:\repos\frankhaugen\Frank.Networking\Frank.Networking\bin\Debug\net7.0\Frank.Networking.dll</Reference>
  <Namespace>Frank.Networking</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
</Query>

async Task Main()
{
    var builder = AppUtil.GetBuiler();

    builder.ConfigureServices((context, services) =>
    {
        services.AddSocketListener(new SocketListenerOptions() { IPAddress = IPAddress.Any, Port = 12345, ProtocolType = ProtocolType.Tcp, SocketType = SocketType.Stream, Backlog = 100 });

        services.AddSocketSender(new Frank.Networking.SocketConnectionOptions()
        {
            IPAddress = IPAddress.Loopback,
            Port = 12345,
            ProtocolType = ProtocolType.Tcp,
            SocketType = SocketType.Stream
        }
        );
        services.AddHostedService<MessageGeneratorService>();
        
    });

    var app = builder.Build();

    await app.RunAsync();
}



public class MessageGeneratorService : BackgroundService
{
    private readonly ILogger<MessageGeneratorService> _logger;
    private readonly ISocketSender _client;

    public MessageGeneratorService(ISocketSender client, ILogger<MessageGeneratorService> logger)
    {
        _client = client;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            var data = Encoding.UTF8.GetBytes("My message");
            await _client.SendAsync(data);
            _logger.LogInformation("Message sent successfully.");
        }
    }
}