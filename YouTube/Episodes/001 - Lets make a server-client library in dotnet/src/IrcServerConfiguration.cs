using System.Net;

namespace Irc;

public class IrcServerConfiguration
{
    public EndPoint? EndPoint { get; set; }
    public int Backlog { get; set; } = 100;
}
