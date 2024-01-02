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
	var basexContainer = new ContainerBuilder()
		.WithImage(new BaseXImage())
		.WithExposedPort(1984) // Expose the BaseX API port
		.WithPortBinding(1984, 1984) // Map the BaseX API port
		.WithExposedPort(8984) // Expose the BaseX Web/HTTP port
		.WithPortBinding(8984, 8984) // Map the BaseX Web/HTTP port
		.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1984)) // Wait until BaseX is ready
		.Build();

	await basexContainer.StartAsync();
	var session = new BaseXClient.Session("localhost", basexContainer.GetMappedPublicPort(1984), "admin", "admin");

	try
	{
		// Create Database
		session.Execute("""
						CHECK MyBooks
						""");
		
		// Add data
		session.Execute("""
						ADD MyBooks
						<books>
						    <book>
						        <title>Sample Book Title</title>
						        <author>Author Name</author>
						        <year>2023</year>
						    </book>
						</books>
						""").Dump();
						
		// Get data
		//session.Execute("""
		//				XQUERY for $book in doc("MyBooks")/books/book/title
		//				""").Dump();
	}
	catch (Exception ex)
	{
		ex.Dump();
		session.Execute("help").Dump();
	}
	finally
	{
		await basexContainer.StopAsync();
		await basexContainer.DisposeAsync();
	}	
}

public class BaseXTestcontainer : DockerContainer
{
	private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
	
	public BaseXTestcontainer(ContainerConfiguration configuration) : base(configuration, new LinqPadLogger(nameof(BaseXTestcontainer)))
	{
	}
}

public class BaseXImage : IImage
{
	public string Repository => "basex";

	public string Name => "basexhttp";

	public string Tag => "latest";

	public string FullName => $"{Repository}/{Name}:{Tag}";

	public string GetHostname()
	{
		return "localhost";
	}
}