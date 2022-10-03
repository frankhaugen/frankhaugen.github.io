<Query Kind="Statements">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
</Query>

Dictionary<FileInfo, string> SeparateCodeToFiles(string code, DirectoryInfo outputDirectory)
{
    var output = new Dictionary<FileInfo, string>();
    
    var syntaxTree = CSharpSyntaxTree.ParseText(code);
    var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
    var namespaceSyntax = root!.Members.OfType<NamespaceDeclarationSyntax>().First();
    var classes = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>();
    var records = namespaceSyntax.Members.OfType<RecordDeclarationSyntax>();
    var enums = namespaceSyntax.Members.OfType<EnumDeclarationSyntax>();

    foreach (var element in classes)
    {
        var outputFile = new FileInfo(Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + ".cs"));
        output.Add(outputFile, $"namespace {namespaceSyntax.Name};\n" + element.ToFullString());
    }
    foreach (var element in records)
    {
        var outputFile = new FileInfo(Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + ".cs"));
        output.Add(outputFile, $"namespace {namespaceSyntax.Name};\n" + element.ToFullString());
    }
    foreach (var element in enums)
    {
        var outputFile = new FileInfo(Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + ".cs"));
        output.Add(outputFile, $"namespace {namespaceSyntax.Name};\n" + element.ToFullString());
    }
    return output;
}