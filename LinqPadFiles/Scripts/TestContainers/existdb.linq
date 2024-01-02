<Query Kind="Program">
  <NuGetReference>BaseXClient</NuGetReference>
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <NuGetReference>Testcontainers</NuGetReference>
  <NuGetReference>Testcontainers.Azurite</NuGetReference>
  <Namespace>DotNet.Testcontainers</Namespace>
  <Namespace>DotNet.Testcontainers.Builders</Namespace>
  <Namespace>DotNet.Testcontainers.Configurations</Namespace>
  <Namespace>DotNet.Testcontainers.Containers</Namespace>
  <Namespace>DotNet.Testcontainers.Images</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

async Task Main()
{
	var container = new ContainerBuilder()
		.WithImage(new ExistDbImage())
		// todo
		.Build();

	await container.StartAsync();

	try
	{
		// Create Database
		
		// Add data
		
		// Get data
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

public class ExistDbImage : IImage
{
	public string Repository => "existdb";

	public string Name => "existdb";

	public string Tag => "latest";

	public string FullName => $"{Repository}/{Name}:{Tag}";

	public string GetHostname()
	{
		return "localhost";
	}
}