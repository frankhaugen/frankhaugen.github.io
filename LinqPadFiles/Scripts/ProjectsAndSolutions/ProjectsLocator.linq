<Query Kind="Program">
  <NuGetReference>CliWrap</NuGetReference>
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.DirectoryServices</Namespace>
  <Namespace>CliWrap</Namespace>
  <Namespace>CliWrap.Buffered</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

async Task Main()
{
	Util.AutoScrollResults = true;
	var rootDirectories = new DirectoryInfo("D:/repos");

	var repositories = FindAllRepositoryDirectories(rootDirectories);
	await GitPullAllRepos(repositories);
	
	var projects = FindAllProjects(repositories).SelectMany(x => x.Value).ToList();

	var solutionFile = await CreateSolutionFileAsync(new DirectoryInfo("D:/repos/"), "MegaSolution");
	await AddProjectsToSolutionAsync(solutionFile, projects);
}

IEnumerable<DirectoryInfo> FindAllRepositoryDirectories(DirectoryInfo rootDirectory)
	=> rootDirectory
		.EnumerateDirectories("*", SearchOption.AllDirectories).Where(d => d.GetDirectories(".git", SearchOption.TopDirectoryOnly).Any() || d.GetDirectories(".git", SearchOption.AllDirectories).Any())
		.ToList();

IDictionary<DirectoryInfo, IEnumerable<FileInfo>> FindAllProjects(IEnumerable<DirectoryInfo> directories, string projectFileExtension = ".csproj")
	=> directories
		.Where(dir => !DirectoryPathContains(dir, "bin"))
		.ToDictionary(d => d, d => d.EnumerateFiles($"*{projectFileExtension}", SearchOption.AllDirectories));
	
bool DirectoryPathContains(DirectoryInfo directory, string directoryName)
	=> directory
		.FullName
		.Split(Path.DirectorySeparatorChar)
		.Select(x => x.ToLowerInvariant())
		.Contains(directoryName);

async Task GitPullAllRepos(IEnumerable<DirectoryInfo> repositories)
{
	foreach (var repository in repositories)
	{
		await GitPullAsync(repository);
	}
}

async Task GitPullAsync(DirectoryInfo repository)
{
	try
	{
		await Cli.Wrap("git")
		.WithArguments(new[] { "pull" })
		.WithWorkingDirectory(repository.FullName)
		.ExecuteAsync();

		Console.WriteLine($"Git pull completed for repository: {repository.Name}");
	}
	catch (Exception ex)
	{
		// Ignore
	}
}

async Task AddProjectsToSolutionAsync(FileInfo solutionFile, IEnumerable<FileInfo> projectfiles)
{
	foreach (var projectFile in projectfiles)
	{
		try
		{
			var addProjectResult = await Cli.Wrap("dotnet")
			.WithArguments(new[] { "sln", solutionFile.FullName, "add", projectFile.FullName })
			.ExecuteAsync();

			if (addProjectResult.IsSuccess)
			{
				Console.WriteLine($"Added project {projectFile.Name} to solution.");
			}
		}
		catch (Exception ex)
		{
			// Ignore
		}
	}
}


async Task<FileInfo> CreateSolutionFileAsync(DirectoryInfo outputDirectory, string? solutionName = null)
{
	var solutionFile = new FileInfo(Path.Combine(outputDirectory.FullName, $"{solutionName ?? outputDirectory.Name}.sln"));

	if (solutionFile.Exists)
	{
		solutionFile.Delete();
	}

	// Create new solution
	var createSlnResult = await Cli.Wrap("dotnet")
		.WithWorkingDirectory(solutionFile.Directory!.FullName)
		.WithArguments(new[] { "new", "sln", "-n", Path.GetFileNameWithoutExtension(solutionFile.Name) })
		.ExecuteAsync();

	// Check for errors in solution creation
	if (!createSlnResult.IsSuccess)
	{
		Console.WriteLine("Error creating solution: " + createSlnResult.ExitCode);
	}
	return solutionFile;
}