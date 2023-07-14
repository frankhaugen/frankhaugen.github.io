<Query Kind="Program">
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>Microsoft.Extensions.Options</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "xunit"

void Main()
{
    //RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.

    var appBuilder = AppUtil.GetBuiler();
    
    appBuilder.ConfigureAppConfiguration (appBuilder) =>
    {
        appBuilder.Add
    };
}



public class IrcClientConfiguration
{
    public string ServerName { get; set; }
    public string ServerIpAddress { get; set; }
    public int ServerPort { get; set; } = 6667;
    public string ChannelName { get; set; }
    public string UserName { get; set; }
    public string RealName { get; set; }
    public string NickName { get; set; }
}

public class IrcClient : IDisposable
{
    private readonly IOptions<IrcClientConfiguration> _options;
    private readonly ILogger<IrcClient> _logger;
    private TcpClient _client;
    private StreamReader _reader;
    private StreamWriter _writer;

    public IrcClient(
        IOptions<IrcClientConfiguration> options,
        ILogger<IrcClient> logger)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        _client = new TcpClient();
        await _client.ConnectAsync(_options.Value.ServerIpAddress, _options.Value.ServerPort);
        _reader = new StreamReader(_client.GetStream());
        _writer = new StreamWriter(_client.GetStream()) { AutoFlush = true };

        await _writer.WriteLineAsync($"NICK {_options.Value.NickName}");
        await _writer.WriteLineAsync($"USER {_options.Value.UserName} 0 * :{_options.Value.RealName}");

        while (!cancellationToken.IsCancellationRequested)
        {
            var message = await _reader.ReadLineAsync();
            if (message == null)
            {
                break;
            }

            _logger.LogInformation($"Received message: {message}");

            if (message.StartsWith("PING"))
            {
                var pongMessage = message.Replace("PING", "PONG");
                await _writer.WriteLineAsync(pongMessage);
            }
        }
    }

    public async Task SendMessageAsync(string message)
    {
        await _writer.WriteLineAsync($"PRIVMSG {_options.Value.ChannelName} :{message}");
    }

    public void Dispose()
    {
        _client?.Dispose();
        _reader?.Dispose();
        _writer?.Dispose();
    }
}


public class IrcClientTests : IDisposable
{
    private readonly IrcServer _server;
    private readonly IrcClient _client;

    public IrcClientTests()
    {
        var serverConfig = new IrcServerConfiguration
        {
            ServerName = "TestServer",
            IpAddress = "127.0.0.1",
            Port = 6667,
            Channels = new[] { "#test" }
        };

        var clientConfig = new IrcClientConfiguration
        {
            ServerName = serverConfig.ServerName,
            ServerIpAddress = serverConfig.IpAddress,
            ServerPort = serverConfig.Port,
            ChannelName = serverConfig.Channels[0],
            NickName = "TestClient",
            UserName = "testuser",
            RealName = "Test User"
        };

        var services = new ServiceCollection();
        services.AddSingleton(Options.Create(serverConfig));
        services.AddSingleton(Options.Create(clientConfig));
        services.AddSingleton<ILoggerFactory, LinqPadLoggerFactory>();
        services.AddSingleton(typeof(ILogger<>), typeof(LinqPadLogger<>));
        services.AddSingleton<IrcServer>();
        services.AddSingleton<IrcClient>();

        var serviceProvider = services.BuildServiceProvider();

        _server = serviceProvider.GetRequiredService<IrcServer>();
        _client = serviceProvider.GetRequiredService<IrcClient>();

        _server.StartAsync(default).Wait();
        _client.ConnectAsync(default).Wait();
    }

    [Fact]
    public async Task SendMessageAsync_SendsMessageToServer()
    {
        var message = "Hello, world!";
        await _client.SendMessageAsync(message);

        var receivedMessage = await WaitForMessageAsync($"PRIVMSG {_client.Options.ChannelName} :{message}");
        Assert.Equal($"TestClient!testuser@127.0.0.1 PRIVMSG {_client.Options.ChannelName} :{message}", receivedMessage);
    }

    private async Task<string> WaitForMessageAsync(string expectedMessage)
    {
        var timeout = TimeSpan.FromSeconds(5);
        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed < timeout)
        {
            var message = await _server.ReceiveMessageAsync();
            if (message == expectedMessage)
            {
                return message;
            }
        }

        throw new TimeoutException($"Timed out waiting for message: {expectedMessage}");
    }

    public void Dispose()
    {
        _client.Dispose();
        _server.Dispose();
    }
}