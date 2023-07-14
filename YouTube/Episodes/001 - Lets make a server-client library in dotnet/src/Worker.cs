namespace Irc;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IrcServer _ircServer;

    public Worker(ILogger<Worker> logger, IrcServer ircServer)
    {
        _logger = logger;
        _ircServer = ircServer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        var result = await _ircServer.AcceptAsync(stoppingToken);

        
    }
}
