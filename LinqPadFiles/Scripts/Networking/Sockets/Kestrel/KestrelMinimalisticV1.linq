<Query Kind="Program">
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.AspNetCore.Connections</Namespace>
  <Namespace>Microsoft.AspNetCore.Hosting</Namespace>
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <Namespace>Microsoft.AspNetCore.Http.Features</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.IO.Pipelines</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

async Task Main()
{
    var host = new WebHostBuilder()
    .ConfigureLogging(logging => {
        logging.ClearProviders();
        logging.AddProvider(new LinqPadLoggerProvider());
    })
    .UseKestrel
    ((context, kestrel) => {
        kestrel.ListenAnyIP(6667);
        kestrel.
        kestrel.ConfigureEndpointDefaults(config =>
            {
                config.UseConnectionHandler<IrcConnectionHandler>();
                config.UseConnectionLogging();
            });
    })
    .Configure(app =>
    {
        app.Run(async context =>
        {
            
            await context.Response.WriteAsync("Hello, World!");
        });
    })
    .Build();

    await host.RunAsync(GetCancellationToken());
}

static CancellationToken GetCancellationToken() => new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token;

public class IrcConnectionContext : ConnectionContext
{
    public IrcConnectionContext(IDuplexPipe transport)
    {
        Transport = transport;
        ConnectionId = Guid.NewGuid().ToString();
    }

    public override string ConnectionId { get; set; }

    public override IFeatureCollection Features { get; } = new FeatureCollection();

    public override IDictionary<object, object> Items { get; set; }

    public override IDuplexPipe Transport { get; set; }
}

public class IrcConnectionContextFactory : IConnectionListenerFactory
{
    public ValueTask<IConnectionListener> BindAsync(EndPoint endpoint, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

public class IrcConnectionHandler : ConnectionHandler
{
    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        var ircConnectionContext = new IrcConnectionContext(connection.Transport);
        var result = await ircConnectionContext.Transport.Input.ReadAsync();

        result.Dump();

        await Task.CompletedTask;
    }
}
