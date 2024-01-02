<Query Kind="Statements">
  <NuGetReference>NuGetUtils</NuGetReference>
  <Namespace>NuGetUtils.Extensions</Namespace>
  <Namespace>NuGetUtils.Model</Namespace>
  <Namespace>NuGetUtils.Services</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
</Query>

// nuclear delete Frank.Libraries.AzureStorage 2023.* --api-key oy2jen27ljvbq7tjdqsa7ngfzspawxherotmh35bzdvnr4

var apiKey = "oy2jen27ljvbq7tjdqsa7ngfzspawxherotmh35bzdvnr4";

var helper = new NuGetHelper(apiKey);


public class NuGetHelper
{
    private readonly NuGetClient _nugetClient;

    public NuGetHelper(string apiKey)
	{
		_nugetClient = new NuGetClient(new LinqPadLoggerFactory().CreateLogger<NuGetClient>(), new NuGetClientConfiguration() {
			ApiKey = apiKey
		});
    }

    public async Task<	IEnumerable> GetPackages()
    {
        try
        {
            var result = await _nugetClient.SearchAsync("", null, false, false);
			
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to get installed packages: {ex.Message}");
            return Enumerable.Empty<PackageInfo>();
        }
    }
}