<Query Kind="Program">
  <NuGetReference>BaseXClient</NuGetReference>
  <NuGetReference>DotNetCore.MongoDB</NuGetReference>
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <NuGetReference>Testcontainers</NuGetReference>
  <NuGetReference>Testcontainers.Azurite</NuGetReference>
  <NuGetReference>Testcontainers.MongoDb</NuGetReference>
  <NuGetReference>UblSharp</NuGetReference>
  <Namespace>DotNet.Testcontainers</Namespace>
  <Namespace>DotNet.Testcontainers.Builders</Namespace>
  <Namespace>DotNet.Testcontainers.Configurations</Namespace>
  <Namespace>DotNet.Testcontainers.Containers</Namespace>
  <Namespace>DotNet.Testcontainers.Images</Namespace>
  <Namespace>DotNetCore.MongoDB</Namespace>
  <Namespace>DotNetCore.Repositories</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>MongoDB.Driver</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Testcontainers.MongoDb</Namespace>
  <Namespace>UblSharp</Namespace>
  <Namespace>UblSharp.UnqualifiedDataTypes</Namespace>
  <Namespace>UblSharp.CommonAggregateComponents</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

async Task Main()
{
	var mongoContainer = new MongoDbBuilder()
		.Build();

	await mongoContainer.StartAsync();
	var connectionString = mongoContainer.GetConnectionString();
	var context = new MyMongoContext(connectionString);
	var repository = new MongoRepository<InvoiceType>(context);

	try
	{
		var invoice = new InvoiceType()
		{
			DueDate = DateTime.Now.AddDays(7),
			LegalMonetaryTotal = new MonetaryTotalType()
			{
				AllowanceTotalAmount = new AmountType() { Value = 1000 }
			}
		};
		
		repository.Add(invoice);
		
		// Get data
	}
	catch (Exception ex)
	{
		ex.Dump();
	}
	finally
	{
		await mongoContainer.StopAsync();
		await mongoContainer.DisposeAsync();
	}	
}

public class MyMongoContext : MongoContext
{
	public MyMongoContext(string connectionString) : base(connectionString)
	{
	}
}

public class MongoDbExpressImage : IImage
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