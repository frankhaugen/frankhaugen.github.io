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
  <Namespace>Microsoft.AspNetCore.Hosting.Server</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets</Namespace>
  <Namespace>Microsoft.Extensions.Options</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

ILoggerFactory _loggerFactory = new LinqPadLoggerFactory();

async Task Main()
{
    
}


IConnectionListenerFactory GetListenerFactory()
{
    var socketOptions = new SocketTransportOptions();
    var options = Options.Create(socketOptions);
    return new SocketTransportFactory(options,_loggerFactory);
}

IConnectionListener GetIrcListener()
{
    return new IrcListener();
}

public class IrcListener : IConnectionListener
{
    public EndPoint EndPoint => throw new NotImplementedException();

    public ValueTask<ConnectionContext?> AcceptAsync(CancellationToken cancellationToken = default)
    {
        
        var context = new ConnectionContext();
        
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask UnbindAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

public class IrcConnectionListenerFactory : IConnectionListenerFactory
{
    public async ValueTask<IConnectionListener> BindAsync(EndPoint endpoint, CancellationToken cancellationToken = default)
    {
        
    }
}

public class IrcService : IServer
{
    private readonly TcpListener _listener;

    public IrcService()
    {
        _listener = new TcpListener(IPAddress.Any, 6667);
    }

    public IFeatureCollection Features => new FeatureCollection();

    public void Dispose()
    {
        _listener.Stop();
    }

    public async Task StartAsync<TContext>(IHttpApplication<TContext> application, CancellationToken cancellationToken) where TContext : notnull
    {
        _listener.Start();
        Console.WriteLine("Listening for IRC data on port 6667...");

        while (!cancellationToken.IsCancellationRequested)
        {
            var client = await _listener.AcceptTcpClientAsync();
            Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");

            // Handle the client connection here
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _listener.Stop();
        await Task.CompletedTask;
    }
}