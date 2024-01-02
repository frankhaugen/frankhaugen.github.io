<Query Kind="Program">
  <NuGetReference>CliWrap</NuGetReference>
  <NuGetReference>Microsoft.Build</NuGetReference>
  <Namespace>Microsoft.Build.Construction</Namespace>
  <Namespace>CliWrap</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var solutionFilePath = @"D:\repos\Frank.Libraries\src\Frank.Libraries.sln";
	var solutionFile = new FileInfo(solutionFilePath);
	await UpdateSolutionFile(solutionFile);
}

async Task UpdateSolutionFile(FileInfo file)
{
	var rootDirectory = file.Directory;
	var solutionFile = SolutionFile.Parse(file.FullName);

	var subDirectories = rootDirectory.EnumerateDirectories();

	await ClearProjects(file);

	foreach (var directory in subDirectories)
	{
		var subSubDirectories = directory.EnumerateDirectories();
		foreach (var subDirectory in subSubDirectories)
		{
			var projectFiles = subDirectory.EnumerateFiles("*.csproj", SearchOption.AllDirectories);

			foreach (var projectFile in projectFiles)
			{
				Console.WriteLine($"Adding {projectFile} to {solutionFile}...");

				var result = await Cli.Wrap("dotnet")
					.WithWorkingDirectory(rootDirectory.FullName) // Set the working directory here
					.WithArguments(args => args
						.Add("sln")
						.Add(file.FullName)
						.Add("add")
						.Add(projectFile.FullName))
					.WithStandardErrorPipe(PipeTarget.ToDelegate(x =>
					{
						x.Dump();
					}))
					.ExecuteAsync();

				Console.WriteLine(result);
			}
		}
	}

	Console.WriteLine("Finished updating the solution file.");
}

async Task ClearProjects(FileInfo file)
{
	var rootDirectory = file.Directory;
	var solutionFile = SolutionFile.Parse(file.FullName);
	var projects = solutionFile.ProjectsByGuid.Where(x => x.Value.ProjectType != SolutionProjectType.SolutionFolder);

	foreach (var project in projects)
	{
		var result = await Cli.Wrap("dotnet")
					.WithWorkingDirectory(rootDirectory.FullName) // Set the working directory here
					.WithArguments(args => args
						.Add("sln")
						.Add(file.FullName)
						.Add("remove")
						.Add(project.Value.AbsolutePath))
					.WithStandardErrorPipe(PipeTarget.ToDelegate(x =>
					{
						x.Dump();
					}))
					.ExecuteAsync();
		Console.WriteLine(result);
	}
}
