<Query Kind="Statements">
  <NuGetReference>CliWrap</NuGetReference>
  <NuGetReference>Microsoft.Build</NuGetReference>
  <Namespace>Microsoft.Build.Construction</Namespace>
  <Namespace>CliWrap</Namespace>
</Query>

// Replace this with the path to your .sln file.
var solutionPath = @"D:\repos\Frank.Libraries\src\Frank.Libraries.sln";

// Get the directory of the solution
var solutionDir = new DirectoryInfo(Path.GetDirectoryName(solutionPath));

var projectDirectories = solutionDir.EnumerateDirectories().Where(x => x.EnumerateFiles("*.csproj").Any());

foreach (var projectDirectory in projectDirectories)
{
	var projectPath = projectDirectory.GetFiles("*.csproj").FirstOrDefault()?.FullName;

	if (projectPath != null)
	{
		await Cli
			.Wrap("dotnet")
			.WithWorkingDirectory(projectDirectory.FullName)
			.WithArguments("new sln --force")
			.WithStandardErrorPipe(PipeTarget.ToDelegate(x =>
			{
				x.Dump();
			}))
			.ExecuteAsync();

		await Cli
			.Wrap("dotnet")
			.WithWorkingDirectory(projectDirectory.FullName)
			.WithArguments($"sln {projectDirectory.Name}.sln add {projectPath}")
			.WithStandardErrorPipe(PipeTarget.ToDelegate(x => {
				x.Dump();
			}))
			.ExecuteAsync();

	}
}