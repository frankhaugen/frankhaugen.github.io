<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Options</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>System.Net</Namespace>
</Query>


EndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
SocketAddress ad = new SocketAddress(AddressFamily.AppleTalk);

public class SocketConnection<T> where T : SocketConfiguration
{
    private readonly IOptions<T> _options;
    private readonly ILogger<SocketConnection<T>> _logger;
    private readonly Socket _socket;

    public SocketConnection(IOptions<T> options, ILogger<SocketConnection<T>> logger)
    {
        _options = options;
        _logger = logger;
        _socket = new Socket(_options.Value.AddressFamily, _options.Value.SocketType, _options.Value.ProtocolType);
    }

    public bool IsConnected => _socket?.Connected ?? false;

    public async Task ConnectAsync()
    {
        if (_socket == null)
        {
            _logger.LogError("Socket is null");
            throw new InvalidOperationException("Socket is null");
        }

        try
        {
            await _socket.ConnectAsync(_options.Value.Port.EndPoint);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to socket");
            throw;
        }
    }

    public void Disconnect()
    {
        if (_socket == null)
        {
            _logger.LogError("Socket is null");
            throw new InvalidOperationException("Socket is null");
        }

        if (!IsConnected)
        {
            _logger.LogError("Socket is not connected");
            throw new InvalidOperationException("Socket is not connected");
        }

        try
        {
            _socket.Disconnect(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to disconnect from socket");
            throw;
        }
    }

    public async Task SendAsync(byte[] data)
    {
        if (_socket == null)
        {
            _logger.LogError("Socket is null");
            throw new InvalidOperationException("Socket is null");
        }

        if (!IsConnected)
        {
            _logger.LogError("Socket is not connected");
            throw new InvalidOperationException("Socket is not connected");
        }

        try
        {
            await _socket.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send data to socket");
            throw;
        }
    }

    private async Task ReceiveAsync(CancellationToken cancellationToken)
    {
        var buffer = new byte[1024];
        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None, cancellationToken);
            if (result.BytesTransferred > 0)
            {
                try
                {
                    await _handler.OnDataReceivedAsync(new ReadOnlyMemory<byte>(buffer, 0, result.BytesTransferred), cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to handle received data");
                    throw;
                }
            }
        }
    }

    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        if (_socket == null)
        {
            _logger.LogError("Socket is null");
            throw new InvalidOperationException("Socket is null");
        }

        if (!IsConnected)
        {
            _logger.LogError("Socket is not connected");
            throw new InvalidOperationException("Socket is not connected");
        }

        try
        {
            await ReceiveAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start listening for data");
            throw;
        }
    }
}

public abstract class SocketConfiguration<T> where T : Port
{
    public abstract AddressFamily AddressFamily { get; }
    public abstract SocketType SocketType { get; }
    public abstract ProtocolType ProtocolType { get; }
    public abstract T Port { get; }
}

public interface ISocketDataReceivedHandler<TConfig, TPort> 
    where TConfig : SocketConfiguration<TPort>
{
    Task OnDataReceivedAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken);
}

public class MySocketDataReceivedHandler<T> : ISocketDataReceivedHandler<T> where T : SocketConfiguration
{
    public async Task OnDataReceivedAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken)
    {
        // Handle the received data here
    }
}

/// <summary> Represents a port. Intended to be used with <see cref="SocketConfiguration{T}"/>.
/// <para> This class is abstract. </para>
/// This is meant to be implemented by the user to provide a port number and name. Note that the name is optional.
/// </summary>
public abstract class Port
{
    /// <summary> Gets or sets the port number. </summary>
    public int Number { get; set; }

    /// <summary> Gets or sets the port name. </summary>
    public string Name { get; set; } = string.Empty;
}
