<Query Kind="Expression">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
</Query>

public void WriteCodeToFiles(string code, DirectoryInfo outputDirectory)
{
    var syntaxTree = CSharpSyntaxTree.ParseText(code);
    var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
    var namespaceSyntax = root.Members.OfType<NamespaceDeclarationSyntax>().First();
    var classes = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>();
    var records = namespaceSyntax.Members.OfType<RecordDeclarationSyntax>();
    var enums = namespaceSyntax.Members.OfType<EnumDeclarationSyntax>();

    foreach (var element in classes)
    {
        var outputPath = Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + ".cs");
        var outputFile = new FileInfo(outputPath);
        Console.WriteLine($"{element.Identifier.ToString()}\t=>\t{outputFile.Name}");
        File.WriteAllText(outputFile.FullName, $"namespace {namespaceSyntax.Name};\n" + element.ToFullString());
    }
    foreach (var element in records)
    {
        var outputPath = Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + ".cs");
        var outputFile = new FileInfo(outputPath);
        Console.WriteLine($"{element.Identifier.ToString()}\t=>\t{outputFile.Name}");
        File.WriteAllText(outputFile.FullName, $"namespace {namespaceSyntax.Name};\n" + element.ToFullString());
    }
    foreach (var element in enums)
    {
        var outputPath = Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + ".cs");
        var outputFile = new FileInfo(outputPath);
        Console.WriteLine($"{element.Identifier.ToString()}\t=>\t{outputFile.Name}");
        File.WriteAllText(outputFile.FullName, $"namespace {namespaceSyntax.Name};\n" + element.ToFullString());
    }
}