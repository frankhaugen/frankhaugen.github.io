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


void Main()
{
    
}

public class IrcServer : BackgroundService
{
    private readonly IOptions<IrcServerConfiguration> _options;
    private readonly ILogger<IrcServer> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private TcpListener _listener;

    public IrcServer(
        IOptions<IrcServerConfiguration> options,
        ILogger<IrcServer> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _listener = new TcpListener(IPAddress.Parse(_options.Value.IpAddress), _options.Value.Port);
        _listener.Start();
        _logger.LogInformation($"IRC server listening on {_options.Value.IpAddress}:{_options.Value.Port}");

        while (!stoppingToken.IsCancellationRequested)
        {
            var client = await _listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client, stoppingToken);
        }
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken stoppingToken)
    {
        using var stream = client.GetStream();
        using var reader = new StreamReader(stream);
        using var writer = new StreamWriter(stream) { AutoFlush = true };

        var nick = "";
        var user = "";
        var realName = "";

        await writer.WriteLineAsync($"NOTICE AUTH :*** Looking up your hostname...");
        await writer.WriteLineAsync($"NOTICE AUTH :*** Found your hostname ({client.Client.RemoteEndPoint})");
        await writer.WriteLineAsync($"NOTICE AUTH :*** Checking ident");

        while (!stoppingToken.IsCancellationRequested && !client.Client.Poll(0, SelectMode.SelectError))
        {
            if (client.Client.Poll(0, SelectMode.SelectRead))
            {
                var message = await reader.ReadLineAsync();
                if (message == null)
                {
                    break;
                }

                _logger.LogInformation($"Received message: {message}");

                var parts = message.Split(' ');
                var command = parts[0].ToUpperInvariant();

                switch (command)
                {
                    case "NICK":
                        nick = parts[1];
                        break;
                    case "USER":
                        user = parts[1];
                        realName = string.Join(' ', parts.Skip(4));
                        var response = $"001 {nick} :Welcome to the IRC server, {nick}!{user}@{client.Client.RemoteEndPoint} {realName}";
                        _logger.LogInformation(response);
                        await writer.WriteLineAsync(response);
                        break;
                    case "QUIT":
                        var response2 = $"ERROR :Closing Link: {client.Client.RemoteEndPoint} (Quit: {string.Join(' ', parts.Skip(1))})";
                        _logger.LogInformation(response2);
                        await writer.WriteLineAsync(response2);
                        client.Close();
                        return;
                    default:
                        await writer.WriteLineAsync($"421 {command} :Unknown command");
                        break;
                }
            }
            else
            {
                await Task.Delay(100, stoppingToken);
            }
        }

        client.Close();
    }
}

public class IrcServerConfiguration
{
    public string ServerName { get; set; }
    public string IpAddress { get; set; }
    public int Port { get; set; } = 6667;
    public string[] Channels { get; set; }
}