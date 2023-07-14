using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Irc;

namespace Tests;

public class IrcServerTests
{
    private readonly ITestOutputHelper _outputHelper;   

    public IrcServerTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public async Task Test1()
    {
        var serverConfiguration = new IrcServerConfiguration
        {
            Backlog = 10,
            EndPoint = new IPEndPoint(IPAddress.Loopback, 6667)
        };
        var options = Options.Create(serverConfiguration);

        var ircServer = new IrcServer(options);

        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        var task = ircServer.AcceptAsync(cancellationToken);

        await Task.Delay(1000, cancellationToken);

        cancellationTokenSource.Cancel();

        await task;
    }
}