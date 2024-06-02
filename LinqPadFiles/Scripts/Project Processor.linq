<Query Kind="Statements">
  <NuGetReference>Dotnet.ProjInfo</NuGetReference>
  <NuGetReference>Microsoft.Build.Locator</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.CSharp.Workspaces</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.Workspaces.MSBuild</NuGetReference>
  <NuGetReference>MvsSln</NuGetReference>
  <NuGetReference>NuGet.PackageManagement</NuGetReference>
  <NuGetReference>NuGet.Packaging</NuGetReference>
  <NuGetReference>NuGet.ProjectManagement</NuGetReference>
  <NuGetReference>NuGet.ProjectModel</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <RuntimeVersion>7.0</RuntimeVersion>
</Query>





var projectFiles = new DirectoryInfo("C:/repos/frankhaugen/Frank.Libraries/src").EnumerateFiles("*.csproj", SearchOption.AllDirectories);

var workspace = new AdhocWorkspace();
workspace.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId("Frank.Libs"), VersionStamp.Create(), "C:/repos/frankhaugen/Frank.Libraries/src"));

workspace.WorkspaceFailed += OnWorkspaceFailed;

var solution = workspace.CurrentSolution;
var projects = projectFiles.Select(x => ProjectInfo.Create(ProjectId.CreateNewId(Path.GetFileNameWithoutExtension(x.FullName)), VersionStamp.Default, Path.GetFileNameWithoutExtension(x.FullName), Path.GetFileNameWithoutExtension(x.FullName), LanguageNames.CSharp, x.FullName));

workspace.AddProjects(projects);

solution.Projects.Select(x => x.GetCompilationAsync().GetAwaiter().GetResult()).ToList();
workspace.CurrentSolution.Projects.Dump();

projects.Dump();

void OnWorkspaceFailed(object? sender, WorkspaceDiagnosticEventArgs e)
{
    Console.WriteLine(e.Diagnostic.Message);
}
