<Query Kind="Program">
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
  <Namespace>Xunit</Namespace>
</Query>

#load "xunit"

#load "C:\repos\frankhaugen\frankhaugen.github.io\LinqPadFiles\Scripts\Roslyn\Codefiles\*.cs"
#load "Scripts\Roslyn\CreateProject"

void Main()
{
    //RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.

    var path = "C:\\repos\\frankhaugen\\Frank.Libraries\\src\\Frank.Libraries.Calculators";
    var directory = new DirectoryInfo(path);
    var files = directory.EnumerateFiles("*.cs", SearchOption.AllDirectories);
    var code = string.Join("\n",files.Select(x => File.ReadAllText(x.FullName)));
    
    
    var syntaxTree = CSharpSyntaxTree.ParseText(code);
    var root = syntaxTree.GetRoot();
    
    var analyzer = new LineLengthAnalyzer();
   // analyzer.Initialize(
   //var project = CreateProject("My Project", files);
    
    //analyzer.Initialize();
}

public class LineLengthAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => 
        new List<DiagnosticDescriptor>().ToImmutableArray();

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationAction(CompilationAction);
    }

    private void CompilationAction(CompilationAnalysisContext obj)
    {
        var illegalTrees = obj.Compilation.SyntaxTrees.AsParallel().Where(x => x.TryGetText(out var resultText) && resultText.Lines.Count(line => line.Text.ToString().Contains($";")) > 150).ToList();
        var walker = new ListTypesWalker();

        foreach (var illegalTree in illegalTrees)
        {
            walker.Visit(illegalTree.GetRoot());
        }
    }
}

public class ListTypesWalker : CSharpSyntaxWalker
{
    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        Write(node);
        base.VisitClassDeclaration(node);
    }

    public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
    {
        Write(node);
        base.VisitInterfaceDeclaration(node);
    }

    private static void Write(BaseTypeDeclarationSyntax node)
    {
        var namespaceDeclarationSyntax = node.FirstAncestorOrSelf<NamespaceDeclarationSyntax>();
        var typeName = node.Identifier.Text;
        var namespaceName = namespaceDeclarationSyntax?.Name.ToString();
        Console.WriteLine($"{namespaceName}.{typeName}");
    }
}
