<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;



public class Connection : IDisposable
{
    private readonly Socket _socket;
    private readonly ConnectionSettings _settings;
    private readonly ILogger<Connection> _logger;

    public Connection(IOptions<ConnectionSettings> settings, ILogger<Connection> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        var endpoint = new IPEndPoint(_settings.Host, _settings.Port);

        _logger.LogInformation("Connecting to {Host}:{Port}...", _settings.Host, _settings.Port);

        await _socket.ConnectAsync(endpoint, cancellationToken);

        _logger.LogInformation("Connected to {Host}:{Port}.", _settings.Host, _settings.Port);
    }

    public async Task<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Sending {Length} bytes...", buffer.Length);

        var bytesSent = await _socket.SendAsync(buffer, SocketFlags.None, cancellationToken);

        _logger.LogDebug("Sent {BytesSent} bytes.", bytesSent);

        return bytesSent;
    }

    public async Task<int> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Receiving data...");

        int totalBytesRead = 0;
        int bytesRead;

        do
        {
            bytesRead = await _socket.ReceiveAsync(buffer.Slice(totalBytesRead), SocketFlags.None, cancellationToken);
            totalBytesRead += bytesRead;

            _logger.LogDebug("Received {BytesRead} bytes.", bytesRead);
        }
        while (bytesRead > 0 && totalBytesRead < buffer.Length);

        return totalBytesRead;
    }
    public void Dispose()
    {
        _socket.Dispose();
    }
}

public class ConnectionSettings
{
    public IPAddress Host { get; set; }
    public int Port { get; set; }
}
