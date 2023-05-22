<Query Kind="Statements">
  <NuGetReference>Buildalyzer</NuGetReference>
  <NuGetReference>Microsoft.Build.Framework</NuGetReference>
  <NuGetReference>Microsoft.Build.Locator</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.CSharp.Analyzer.Testing.XUnit</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.CSharp.Workspaces</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.Workspaces.Common</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.Workspaces.MSBuild</NuGetReference>
  <Namespace>Microsoft.Build.Locator</Namespace>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.Diagnostics</Namespace>
  <Namespace>Microsoft.CodeAnalysis.MSBuild</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.CodeAnalysis.Testing</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Testing.XUnit</Namespace>
</Query>

#load "Roslyn\RoslynAnalyzers"

var solutionFile = new FileInfo(@"C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.sln");
if (!MSBuildLocator.IsRegistered)
    MSBuildLocator.RegisterDefaults();

//var adHocWorspace = GetAdWorkspaceFromSolution(solution);
var workspace = await GetMsBuildWorkspaceFromSolution(solutionFile);
workspace.LoadMetadataForReferencedProjects = true;

var solution = workspace.CurrentSolution;

var projects = solution.Projects;

var analyzer = new FrankAnalyzersCodeLengthAnalyzer();

foreach (var project in projects)
{
    project.Dump();
    foreach (var document in project.Documents)
    {
        var expected = new DiagnosticResult(FrankAnalyzersCodeLengthAnalyzer.Rule).WithArguments(document.Name, FrankAnalyzersCodeLengthAnalyzer.MaxCodeLine);

        var code = await document.GetTextAsync();
        
        code.Dump();
        
        await AnalyzerVerifier<FrankAnalyzersCodeLengthAnalyzer>.VerifyAnalyzerAsync(code.ToString().Dump(document.Name), expected);


    }

}







//var ircProject = workspace.CurrentSolution.Projects.First(x => x.Name == "Frank.Libraries.Logging");

//var compilation = await ircProject.GetCompilationAsync();

//ircProject.Dump();

async Task<MSBuildWorkspace> GetMsBuildWorkspaceFromSolution(FileInfo solutionFile)
{
    var workspace = MSBuildWorkspace.Create();

    await workspace.OpenSolutionAsync(solutionFile.FullName);

    return workspace;
}

AdhocWorkspace GetAdWorkspaceFromSolution(FileInfo solutionFile)
{
    var workspace = new AdhocWorkspace();
    workspace.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId(Path.GetFileNameWithoutExtension(solutionFile.Name)), VersionStamp.Create(), solutionFile.FullName));

    var projectFiles = solutionFile.Directory.EnumerateDirectories().Where(x => !x.Name.StartsWith(".")).SelectMany(x => x.EnumerateFiles("*.csproj"));
    workspace.WorkspaceFailed += OnWorkspaceFailed;

    var projects = projectFiles.Select(x => ProjectInfo.Create(ProjectId.CreateNewId(Path.GetFileNameWithoutExtension(x.Name)), VersionStamp.Default, Path.GetFileNameWithoutExtension(x.FullName), Path.GetFileNameWithoutExtension(x.FullName), LanguageNames.CSharp, x.FullName));
    workspace.AddProjects(projects);

    void OnWorkspaceFailed(object? sender, WorkspaceDiagnosticEventArgs e)
    {
        e.Dump();
    }

    return workspace;
}