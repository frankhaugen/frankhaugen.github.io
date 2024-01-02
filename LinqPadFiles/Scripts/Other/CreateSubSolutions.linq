<Query Kind="Statements">
  <NuGetReference>CliWrap</NuGetReference>
  <Namespace>CliWrap</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

string srcPath = @"D:\repos\Frank.Libraries\src\"; // Change this to your src path

var srcDirectory = new DirectoryInfo(srcPath);
var directories = srcDirectory.EnumerateDirectories("Frank.Libraries.*");

foreach (var directory in directories)
{
	try
	{
		string testDirectoryName = directory.Name + ".Tests";
		var testDirectory = Directory.CreateDirectory(Path.Combine(directory.FullName, testDirectoryName));

		await CreateSolution(directory);
		await AddProjectToSolution(directory);
		await CreateTestProject(testDirectory);
		await AddTestProjectToSolution(directory);
	}
	catch (Exception ex)
	{
		Console.WriteLine($"An error occurred while processing directory {directory}: {ex.Message}");
	}
}



async Task CreateSolution(DirectoryInfo workingDirectory)
{
	await Cli
	.Wrap("dotnet")
	.WithWorkingDirectory(workingDirectory.FullName)
	.WithArguments("new sln --force")
	.WithStandardErrorPipe(PipeTarget.ToDelegate(x =>
	{
		x.Dump();
	}))
	.ExecuteAsync();
}

async Task AddProjectToSolution(DirectoryInfo workingDirectory)
{
	await Cli
		.Wrap("dotnet")
		.WithWorkingDirectory(workingDirectory.FullName)
		.WithArguments($"sln {workingDirectory.Name}.sln add {workingDirectory.Name}\\{workingDirectory.Name}.csproj")
		.WithStandardErrorPipe(PipeTarget.ToDelegate(x =>
		{
			x.Dump();
		}))
		.ExecuteAsync();
}

async Task AddTestProjectToSolution(DirectoryInfo workingDirectory)
{
	string testDirectoryName = workingDirectory.Name + ".Tests";
	await Cli
		.Wrap("dotnet")
		.WithWorkingDirectory(workingDirectory.FullName)
		.WithArguments($"sln {workingDirectory.Name}.sln add {Path.Combine(workingDirectory.FullName, testDirectoryName)}\\{Path.Combine(workingDirectory.FullName, testDirectoryName)}.csproj")
		.WithStandardErrorPipe(PipeTarget.ToDelegate(x =>
		{
			x.Dump();
		}))
		.ExecuteAsync();
}

async Task CreateTestProject(DirectoryInfo workingDirectory)
{
	await Cli
		.Wrap("dotnet")
		.WithArguments($"new xunit")
		.WithWorkingDirectory(workingDirectory.FullName)
		.WithStandardErrorPipe(PipeTarget.ToDelegate(x =>
		{
			x.Dump();
		}))
		.ExecuteAsync();
}
