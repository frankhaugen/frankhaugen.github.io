<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>Microsoft.Extensions.Options</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
</Query>

async Task Main()
{
    var builder = AppUtil.GetBuiler();

    builder.ConfigureServices((context, services) =>
    {
        services.AddSocketClient(new SocketClientOptions(ProtocolType.Tcp, SocketType.Stream, IPAddress.Loopback, 12345));
        services.AddHostedService<MessageGeneratorService>();
    });

    var app = builder.Build();

    await app.RunAsync();
}

public class MessageGeneratorService : BackgroundService
{
    private readonly ISocketClient _client;
    
    public MessageGeneratorService(ISocketClient client)
    {
        _client = client;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _client.SendMessage("My message", new SocketClientOptions(ProtocolType.Tcp, SocketType.Stream, IPAddress.Loopback, 12345));
    }
}

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSocketClient(this IServiceCollection services, SocketClientOptions options)
    {
        services.AddTransient<ISocketClient, SocketClient>();
        //services.AddSingleton<IOptions<SocketClientOptions>>(Options.Create(options));
        return services;
    }
}

public interface ISocketClient
{
    void SendMessage(string message, SocketClientOptions options);
    void Send(byte[] data, SocketClientOptions options);
}

public class SocketClientOptions
{
    public ProtocolType ProtocolType { get; set; }
    public SocketType SocketType { get; set; }
    public IPAddress IPAddress { get; set; }
    public int Port { get; set; }

    public SocketClientOptions(ProtocolType protocolType, SocketType socketType, IPAddress ipAddress, int port)
    {
        ProtocolType = protocolType;
        SocketType = socketType;
        IPAddress = ipAddress;
        Port = port;
    }

    public IPEndPoint ToIPEndpoint() => new IPEndPoint(IPAddress, Port);
}

public class SocketClient : ISocketClient
{
    private readonly ILogger<SocketClient> _logger;

    public SocketClient(ILogger<SocketClient> logger)
    {
        _logger = logger;
    }

    public void Send(byte[] data, SocketClientOptions options)
    {
        try
        {
            using var socket = new Socket(options.IPAddress.AddressFamily, options.SocketType, options.ProtocolType);
            socket.Connect(options.ToIPEndpoint());
            socket.Send(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message: {ExceptionMessage}", ex.Message);
        }
    }

    public void SendMessage(string message, SocketClientOptions options)
    {
        var data = Encoding.UTF8.GetBytes(message);
        Send(data, options);
    }
}
