<Query Kind="Program">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <NuGetReference>NetIRC</NuGetReference>
  <NuGetReference>SimpleIRCLib</NuGetReference>
  <NuGetReference>Testcontainers</NuGetReference>
  <Namespace>DotNet.Testcontainers</Namespace>
  <Namespace>DotNet.Testcontainers.Builders</Namespace>
  <Namespace>DotNet.Testcontainers.Configurations</Namespace>
  <Namespace>DotNet.Testcontainers.Containers</Namespace>
  <Namespace>DotNet.Testcontainers.Images</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>NetIRC</Namespace>
  <Namespace>NetIRC.Connection</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>NetIRC.Messages</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

async Task Main()
{
	var container = new ContainerBuilder()
		.WithImage("carver/ngircd")
		.WithPortBinding(6667, 6667)
		.Build();

	await container.StartAsync();

	try
	{
		var user = new User("Bob");
		var connection = new TcpClientConnection(container.Hostname, container.GetMappedPublicPort(6667));
		var client = new Client(user, connection);

		await client.ConnectAsync();

		var channels = client.Channels;
		channels.Dump();
		
		
		var message = new MyMessage();

		message.Tokens.Append("Hello, world!");
		
		await client.SendAsync(message);

		await Task.Delay(TimeSpan.FromDays(1), QueryCancelToken);
	}
	catch (Exception ex)
	{
		ex.Dump();
	}
	finally
	{
		await container.StopAsync();
		await container.DisposeAsync();
	}	
}

public class MyMessage : IClientMessage
{
	public IEnumerable<string> Tokens => new List<string>();
}

public class IrcdImage : IImage
{
	public string Repository => "ircd";

	public string Name => "unrealircd";

	public string Tag => "latest";

	public string FullName => $"{Repository}/{Name}:{Tag}";

	public string GetHostname()
	{
		return "localhost";
	}
}

// docker pull ircd/unrealircd