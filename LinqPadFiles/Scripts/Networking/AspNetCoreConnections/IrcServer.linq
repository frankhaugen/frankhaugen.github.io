<Query Kind="Statements">
  <Namespace>Microsoft.AspNetCore.Connections</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>



public class Server : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    
    public Server(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var endpoint = new IPEndPoint(IPAddress.Any, 6667);
        var builder = new ConnectionBuilder(_serviceProvider)
            .UseSockets()
            .UseIrc();

        var server = builder.Build();
        await server.BindAsync(endpoint, stoppingToken);
    }
}



public class ConnectionHandler
{
    private readonly ConnectionContext _connection;
    private readonly ConnectionDelegate _next;

    public ConnectionHandler(ConnectionContext connection, ConnectionDelegate next)
    {
        _connection = connection;
        _next = next;
    }

    public async Task HandleConnectionAsync()
    {
        await _next(_connection);
    }
}