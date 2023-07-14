using System.Net.Sockets;
using Microsoft.AspNetCore.Connections.Features;

namespace Irc;

public class ConnectionSocketFeature : IConnectionSocketFeature
{
    public Socket Socket { get; set; } = default!;
}