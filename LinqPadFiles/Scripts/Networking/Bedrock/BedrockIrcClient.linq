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
  <Namespace>System.IO.Pipelines</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

#load ".\BedrockIrcCommon"

async Task Main()
{
    var services = new ServiceCollection();
    services.AddLogging(logging => logging.ClearProviders().AddProvider(new LinqPadLoggerProvider()));
    var serviceProvider = services.BuildServiceProvider();

    var client = new ClientBuilder(serviceProvider)
                            .UseSockets()
                            .UseConnectionLogging(loggerFactory: serviceProvider.GetRequiredService<ILoggerFactory>())
                            .Build();
                            
    var connection = await client.ConnectAsync(new IPEndPoint(IPAddress.Loopback, 6667));
    await SendMessageAsync(connection);
}

private static async Task SendMessageAsync(ConnectionContext? connection)
{
    var protocol = new IrcProtocol();   
    var writer = connection.CreateWriter();
    var reader = connection.CreateReader();
    await writer.WriteAsync(protocol, new IrcMessage("PING :123456"));
    
    var response = await reader.ReadAsync(protocol);
    
    response.Message.Dump();
}
