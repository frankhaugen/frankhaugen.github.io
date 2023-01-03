<Query Kind="Statements">
  <NuGetReference>Microsoft.Build.Locator</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.CSharp.Workspaces</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.Workspaces.MSBuild</NuGetReference>
  <NuGetReference>NuGet.ProjectModel</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.Build.Locator</Namespace>
  <Namespace>Microsoft.CodeAnalysis.MSBuild</Namespace>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>RoslynDemo</Namespace>
  <Namespace>Microsoft.CodeAnalysis.Diagnostics</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>Microsoft.CodeAnalysis.Text</Namespace>
</Query>

MetadataReference SystemCoreReference() => MetadataReference.CreateFromFile(typeof(System.Text.RegularExpressions.Regex).Assembly.Location);
MetadataReference CorlibReference() => MetadataReference.CreateFromFile(typeof(System.Numerics.INumber<>).Assembly.Location);
MetadataReference CSharpSymbolsReference() => MetadataReference.CreateFromFile(typeof(Microsoft.CodeAnalysis.CSharp.CSharpSyntaxWalker).Assembly.Location);
MetadataReference CodeAnalysisReference() => MetadataReference.CreateFromFile(typeof(Microsoft.CodeAnalysis.Compilation).Assembly.Location);

Project CreateProject(string projectName, IEnumerable<FileInfo> sources, string language = LanguageNames.CSharp, string fileNamePrefix = "db")
{
    string fileExt = language == LanguageNames.CSharp ? ".cs" : ".vb";

    var projectId = ProjectId.CreateNewId(debugName: projectName);

    var solution = new AdhocWorkspace()
        .CurrentSolution
        .AddProject(projectId, projectName, projectName, language)
        .AddMetadataReference(projectId, CorlibReference())
        .AddMetadataReference(projectId, SystemCoreReference())
        .AddMetadataReference(projectId, CSharpSymbolsReference())
        .AddMetadataReference(projectId, CodeAnalysisReference());

    int count = 0;
    foreach (var source in sources)
    {
        var newFileName = fileNamePrefix + count + "." + fileExt;
        var documentId = DocumentId.CreateNewId(projectId, debugName: newFileName);
        solution = solution.AddDocument(documentId, newFileName, SourceText.From(source.ReadAllText()));
        count++;
    }
    return solution.GetProject(projectId);
}