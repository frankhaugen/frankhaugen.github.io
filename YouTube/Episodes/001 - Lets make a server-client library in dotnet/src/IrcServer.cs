using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;

namespace Irc;

/// <summary>
/// Represents an IRC server.
/// </summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class IrcServer : IConnectionListener, IDisposable
{
    private readonly IOptions<IrcServerConfiguration> _options;
    private readonly Socket _listener;

    public IrcServer(IOptions<IrcServerConfiguration> options)
    {
        _options = options;
        _listener = InitializeListener();
    }

    private Socket InitializeListener()
    {
        var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(_options.Value.EndPoint ?? throw new InvalidOperationException("No endpoint was configured."));
        listener.Listen(_options.Value.Backlog);
        return listener;
    }

    /// <summary>
    /// Gets the local endpoint to which the listener is bound.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the listener is not bound to an endpoint.</exception>
    public EndPoint EndPoint => _listener.LocalEndPoint ?? throw new InvalidOperationException("Listener is not bound to an endpoint.")!;

    /// <summary>
    /// Asynchronously accepts an incoming connection attempt and returns a <see cref="ConnectionContext"/> when a connection is established.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the accept operation.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> that represents the asynchronous accept operation. The result of the task contains a <see cref="ConnectionContext"/> when a connection is established.</returns>
    public async ValueTask<ConnectionContext?> AcceptAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var socket = await _listener.AcceptAsync(cancellationToken).ConfigureAwait(false);
                var connection = new IrcConnectionContext(socket);
                return connection;
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.OperationAborted)
            {
                // The listener was stopped, so we exit the loop and return null.
                break;
            }
            catch (ObjectDisposedException)
            {
                // The listener was disposed, so we exit the loop and return null.
                break;
            }
        }

        return null;
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        GC.SuppressFinalize(this);
        return default;
    }

    public void Dispose()
    {
        _listener.Dispose();
        GC.SuppressFinalize(this);
    }

    public ValueTask UnbindAsync(CancellationToken cancellationToken = default)
    {
        _listener.Close();
        return default;
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }

    public override string ToString()
    {
        return $"{nameof(IrcServer)}: {EndPoint}";
    }
}