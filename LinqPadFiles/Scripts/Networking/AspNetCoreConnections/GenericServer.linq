<Query Kind="Program">
  <Namespace>Microsoft.AspNetCore.Connections</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

CancellationTokenSource _cancellationTokenSource = CancellationTokenUtil.GetSource(TimeSpan.FromSeconds(30));


async Task Main()
{
    var cancellationtoken = _cancellationTokenSource.Token;
    
    // Usage
    var endpoint = new IPEndPoint(IPAddress.Any, 5000);
    var connectionHandler = new MyConnectionHandler();
    var connectionListenerFactory = new MyConnectionListenerFactory(connectionHandler);
    var connectionListener = await connectionListenerFactory.BindAsync(endpoint, cancellationtoken);
    await connectionListener.AcceptAsync(cancellationtoken);
}

public class MyConnectionHandler : ConnectionHandler
{
    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        // Handle the connection here
        await connection.Transport.Output.WriteAsync(new ReadOnlyMemory<byte>(new byte[] { 0x01, 0x02, 0x03 }));
    }
}

public class MyConnectionListenerFactory : IConnectionListenerFactory
{
    private readonly MyConnectionHandler _connectionHandler;

    public MyConnectionListenerFactory(MyConnectionHandler connectionHandler)
    {
        _connectionHandler = connectionHandler;
    }

    public async ValueTask<IConnectionListener> BindAsync(EndPoint endpoint, CancellationToken cancellationToken)
    {
        return new MyConnectionlistener(endpoint);
    }
}

public class MyConnectionlistener : IConnectionListener
{
    
    public MyConnectionlistener(EndPoint endpoint)
    {
        EndPoint = endpoint;
    }
    
    public EndPoint EndPoint {get;}

    public async ValueTask<ConnectionContext?> AcceptAsync(CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        
        return null;
    }

    public async ValueTask DisposeAsync()
    {
        await Task.CompletedTask;
    }

    public async ValueTask UnbindAsync(CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
    }
}

