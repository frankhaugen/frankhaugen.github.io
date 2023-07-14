using System.IO.Pipelines;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http.Features;

namespace Irc;

public class IrcConnectionContext : ConnectionContext
{
    private readonly Socket _socket;

    public IrcConnectionContext(Socket socket)
    {
        _socket = socket;
        ConnectionId = Guid.NewGuid().ToString("N");
        Features.Set<IConnectionSocketFeature>(new ConnectionSocketFeature { Socket = socket });
    }

    public override string ConnectionId { get; set; }

    public override IFeatureCollection Features { get; } = new FeatureCollection();

    public override IDictionary<object, object?> Items { get; set; } = new ConnectionItemsDictionary();

    public override IDuplexPipe Transport { get; set; } = default!;

    public override void Abort()
    {
        _socket.Dispose();
    }
}
