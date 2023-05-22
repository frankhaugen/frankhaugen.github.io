<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.Options</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
</Query>

async Task Main()
{
    var builder = AppUtil.GetBuiler();

    builder.ConfigureServices(services =>
    {
        services.AddTcpServer(new SocketServerOptions() { IPAddress = IPAddress.Any, Port = 12345, ProtocolType = ProtocolType.Tcp, SocketType = SocketType.Stream, Backlog = 100 });
    });
    
    var app = builder.Build();
    
    await app.RunAsync();
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTcpServer(this IServiceCollection services, SocketServerOptions options)
    {
        services.AddSingleton<IOptions<SocketServerOptions>>(Options.Create(options));
        services.AddSingleton<IDataReceivedHandler, DataReceivedHandler>();
        services.AddHostedService<SocketServer>();
        return services;
    }
    
}

public class SocketDataReceivedEventArgs : EventArgs
{
    public DateTime Timestamp { get; }
    public byte[] Data { get; }
    public EndPoint RemoteEndPoint { get; }

    public SocketDataReceivedEventArgs(EndPoint remoteEndPoint, byte[] data)
    {
        Timestamp = DateTime.UtcNow;
        Data = data;
        RemoteEndPoint = remoteEndPoint;
    }
}

public interface IDataReceivedHandler
{
    void OnDataReceived(object sender, SocketDataReceivedEventArgs e);
}

public class DataReceivedHandler : IDataReceivedHandler
{
    private readonly ILogger<DataReceivedHandler> _logger;
    
    public DataReceivedHandler(ILogger<DataReceivedHandler> logger)
    {
        _logger = logger;
    }
    
    public void OnDataReceived(object sender, SocketDataReceivedEventArgs e)
    {
        _logger.LogInformation("Data received from client {RemoteEndPoint}: {Data}", e.RemoteEndPoint, e.Data);
    }
}

public class SocketServerOptions
{
    public ProtocolType ProtocolType { get; set; }
    public SocketType SocketType { get; set; }
    public IPAddress IPAddress { get; set; }
    public int Port { get; set; }
    public int Backlog { get; set; } = 100;
    
    public IPEndPoint ToIPEndpoint() => new IPEndPoint(IPAddress, Port);
}

public class SocketServer : BackgroundService
{
    private readonly IOptions<SocketServerOptions> _options;
    private readonly ILogger<SocketServer> _logger;
    private readonly IDataReceivedHandler _dataReceivedHandler;
    
    private Socket _listener;
    private bool _isRunning;
    
    public SocketServer(IOptions<SocketServerOptions> options, ILogger<SocketServer> logger, IDataReceivedHandler dataReceivedHandler)
    {
        _options = options;
        _logger = logger;
        _dataReceivedHandler = dataReceivedHandler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _listener = new Socket(_options.Value.IPAddress.AddressFamily, _options.Value.SocketType, _options.Value.ProtocolType);
        _listener.Bind(_options.Value.ToIPEndpoint());
        _listener.Listen(_options.Value.Backlog);

        _isRunning = true;

        while (_isRunning)
        {
            var client = await _listener.AcceptAsync();
            _ = Task.Run(() => HandleClientAsync(client));
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _isRunning = false;
        _listener.Close();   
        await base.StopAsync(stoppingToken);
    }

    private async Task HandleClientAsync(Socket client)
    {
        using (client)
        {
            var buffer = new byte[1024];

            while (_isRunning && client.Connected)
            {
                var bytesRead = await client.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                if (bytesRead > 0)
                {
                    using var stream = new MemoryStream();
                    await stream.ReadAsync(buffer, 0, bytesRead);
                    _dataReceivedHandler.OnDataReceived(this, new SocketDataReceivedEventArgs(client.RemoteEndPoint, stream.ToArray()));
                }
            }
        }
    }
}
