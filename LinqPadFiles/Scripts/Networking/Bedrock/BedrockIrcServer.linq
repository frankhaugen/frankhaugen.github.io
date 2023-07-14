<Query Kind="Program">
  <NuGetReference Prerelease="true">Bedrock.Framework</NuGetReference>
  <Namespace>Bedrock.Framework</Namespace>
  <Namespace>Microsoft.AspNetCore.Connections</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Bedrock.Framework.Protocols</Namespace>
  <Namespace>System.Buffers</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

#load ".\BedrockIrcCommon"

async Task Main()
{
    var builder = AppUtil.GetBuiler();
    builder.ConfigureServices((context, services) =>
    {
        
    });
    builder.ConfigureServer(config => 
    {
        config.UseSockets(sockets =>
        {
            sockets.ListenAnyIP(6667, x => x.UseConnectionLogging().UseConnectionHandler<IrcConnectionHandler>());
        });
    });
    
    var app = builder.Build();
    
    await app.RunAsync(GetCancellationToken());
}

static CancellationToken GetCancellationToken() => new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

public class IrcConnectionHandler : Microsoft.AspNetCore.Connections.ConnectionHandler
{
    private readonly ILogger<IrcConnectionHandler> _logger;
    
    public IrcConnectionHandler(ILogger<IrcConnectionHandler> logger)
    {
        _logger = logger;
    }
    
    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        var protocol = new IrcProtocol();
        var reader = connection.CreateReader();
        var writer = connection.CreateWriter();

        while (true)
        {
            try
            {
                var result = await reader.ReadAsync(protocol);
                var message = result.Message;
                
                if (result.IsCompleted)
                {
                    break;
                }
                
                if (message.Command.Equals("ping", StringComparison.OrdinalIgnoreCase))
                {
                    await writer.WriteAsync(protocol, new IrcMessage("PONG"));
                }
                

                if (result.IsCompleted)
                {
                    break;
                }
            }
            finally
            {
                reader.Advance();
            }
        }
    }
}