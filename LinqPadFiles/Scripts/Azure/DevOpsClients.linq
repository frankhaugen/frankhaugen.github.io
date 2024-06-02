<Query Kind="Program">
  <NuGetReference Prerelease="true">Frank.AzureDevOps</NuGetReference>
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>Frank.AzureDevOps</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

async Task Main()
{
	var services = new ServiceCollection();


	var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions() {});

	var devOpsGitClient = serviceProvider.GetRequiredService<IDevOpsGitClient>();
	var devOpsProjectClient = serviceProvider.GetRequiredService<IDevOpsProjectClient>();
	var projects = await devOpsProjectClient.GetProjectsAsync();
	var repositories = await devOpsGitClient.GetRepositoriesAsync(projects);

	repositories.Dump();
}
