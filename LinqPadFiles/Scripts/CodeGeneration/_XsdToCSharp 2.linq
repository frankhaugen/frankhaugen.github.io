<Query Kind="Program">
  <Reference Relative="..\..\..\..\Frank.Libraries\src\Frank.Libraries.Ubl\bin\Debug\net7.0\Frank.Libraries.Ubl.dll">C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.Ubl\bin\Debug\net7.0\Frank.Libraries.Ubl.dll</Reference>
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <NuGetReference>System.Xml.ReaderWriter</NuGetReference>
  <NuGetReference>XmlSchemaClassGenerator-beta</NuGetReference>
  <Namespace>Frank.Libraries.Ubl.Validation.Internals</Namespace>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>Microsoft.CodeAnalysis.Text</Namespace>
  <Namespace>System.CodeDom</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Xml.Schema</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
  <Namespace>XmlSchemaClassGenerator</Namespace>
</Query>

void Main()
{
    var xsdManifest = new XsdManifest();

    var xsd = xsdManifest.XsdMetadata.Values.ToSchemaSet();

    var generator = new Generator()
    {
        Log = s => s.Dump(),
        NamingScheme = NamingScheme.PascalCase,
        GenerateNullables = true,
        GenerateInterfaces = true,
        SeparateClasses = true,
        DisableComments = false,
        UniqueTypeNamesAcrossNamespaces = false,
        NetCoreSpecificCode = true,
        EnableNullableReferenceAttributes = true,
        SeparateSubstitutes = true,
        OutputWriter = new CodeWriter(new DirectoryInfo(@"C:\temp\CodeGeneration\")),
        CollectionImplementationType = typeof(List<>),
        CollectionType = typeof(List<>),
        NamespaceProvider = new XsdNamespaceProvider(),
        UseXElementForAny = true
    };

    generator.Generate(xsd);
}

public class XsdNamespaceProvider : NamespaceProvider
{
    protected override string OnGenerateNamespace(NamespaceKey key)
    {
        return NamespaceUtility.ExtractNamespace(key);
    }

    public static class NamespaceUtility
    {
        private const string pattern = @"(?<=xsd:)\w+";

        private readonly static Regex regex = new Regex(pattern);

        public static string ExtractNamespace(NamespaceKey namespaceKey)
        {
            Match match = regex.Match(namespaceKey.XmlSchemaNamespace);
            if (match.Success)
            {
                return match.Value;
            }
            if (namespaceKey.XmlSchemaNamespace =="http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")
            {
                return "StandardBusinessDocumentHeaders";
            }

            return "Common";
        }
    }
}

public class CodeWriter : OutputWriter
{
    private readonly DirectoryInfo _outputDirectory;
    
    public CodeWriter(DirectoryInfo outputDirectory)
    {
        _outputDirectory = outputDirectory;
        
        if (!_outputDirectory.Exists)
        {
            _outputDirectory.Create();
        }
    }
    
    public override void Write(CodeNamespace cn)
    {
        var compileUnit = new CodeCompileUnit();
        compileUnit.Namespaces.Add(cn);
        var code = GetCode(compileUnit);
        code = code.Replace("};" + Environment.NewLine, "}"+ Environment.NewLine);
        var syntaxTree = GetSyntaxTree(code);
        var codeFiles = SeparateCodeToFiles(syntaxTree);
        
        foreach (var kvp in codeFiles)
        {
            if (!kvp.Key.Directory!.Exists)
            {
                kvp.Key.Directory.Create();
            }
            kvp.Key.DumpAsync(kvp.Value);
        }
    }

    SyntaxTree GetSyntaxTree(string code)
    {
        return CSharpSyntaxTree.ParseText(SourceText.From(code));
    }

    string GetCode(CodeCompileUnit compileUnit)
    {
        var provider = CodeDomProvider.CreateProvider("C#");
        var options = new CodeGeneratorOptions
        {
            BracingStyle = "CSharp",
            IndentString = "    "
        };
        var code = new StringWriter();
        provider.GenerateCodeFromCompileUnit(compileUnit, code, options);
        return code.ToString();
    }

    Dictionary<FileInfo, string> SeparateCodeToFiles(SyntaxTree syntaxTree)
    {
        var output = new Dictionary<FileInfo, string>();
        var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
        var namespaceSyntax = root!.Members.OfType<NamespaceDeclarationSyntax>().First();
        var classes = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>();
        var records = namespaceSyntax.Members.OfType<RecordDeclarationSyntax>();
        var enums = namespaceSyntax.Members.OfType<EnumDeclarationSyntax>();
        var interfaces = namespaceSyntax.Members.OfType<InterfaceDeclarationSyntax>();
        var structs = namespaceSyntax.Members.OfType<StructDeclarationSyntax>();
        var leadingTriviaCode = GetLeadingTriviaCode(root);
        var trailingTriviaCode = GetTrailingTriviaCode(root);

        AddToOutput(output, classes, namespaceSyntax.Name.ToString(), leadingTriviaCode, trailingTriviaCode);
        AddToOutput(output, records, namespaceSyntax.Name.ToString(), leadingTriviaCode, trailingTriviaCode);
        AddToOutput(output, enums, namespaceSyntax.Name.ToString(), leadingTriviaCode, trailingTriviaCode);
        AddToOutput(output, interfaces, namespaceSyntax.Name.ToString(), leadingTriviaCode, trailingTriviaCode);
        AddToOutput(output, structs, namespaceSyntax.Name.ToString(), leadingTriviaCode, trailingTriviaCode);
        return output;
    }

    private string GetLeadingTriviaCode(SyntaxNode root)
    {   
        var header = GetHeader("Franks 2000inch TV", DateTime.UtcNow);
        return string.Join("\n", header
            .Select(x => x.ToFullString())
            .Where(x => x.Any()))
            .Replace(Environment.NewLine, "\n")
            .Replace("\n\n\n", "\n")
            .Replace("\n\n", "\n");
    }
    
    private string GetTrailingTriviaCode(SyntaxNode root)
    {
        return string.Join("\n", root
            .GetTrailingTrivia()
            .Select(x => x.ToFullString())
            .Where(x => x.Any()))
            .Replace(Environment.NewLine, "\n")
            .Replace("\n\n\n", "\n")
            .Replace("\n\n", "\n");
    }
    
    private void AddToOutput<T>(IDictionary<FileInfo, string> filesAndContent, IEnumerable<T> elements, string namespaceName, string leadingTriviaCode, string trailingTriviaCode)
        where T : BaseTypeDeclarationSyntax
    {
        foreach (var element in elements)
        {
            var outputFile = new FileInfo(Path.Combine(_outputDirectory.FullName, namespaceName, element.Identifier.ToString() + ".cs"));
            filesAndContent.Add(outputFile, $"{leadingTriviaCode}\nnamespace {namespaceName};\n" + element.ToFullString() + $"\n{trailingTriviaCode}");
        }
    }

    SyntaxTriviaList GetHeader(string toolName, DateTime time) =>
        SyntaxFactory.TriviaList(
            SyntaxFactory.Comment("//----------------------------------------------------------------------------"),
            SyntaxFactory.Comment("// <auto-generated>"),
            SyntaxFactory.Comment("//     This code was generated by a tool."),
            SyntaxFactory.Comment($"//     Tool: {toolName}"),
            SyntaxFactory.Comment($"//     Version: {new Version(time.Year, time.Month, time.Day, int.Parse(time.ToString("HHmmss")))}"),
            SyntaxFactory.Comment($"//     Date: {time.ToString("s")}"),
            SyntaxFactory.Comment("//     Changes to this file may cause incorrect behavior and will be lost if"),
            SyntaxFactory.Comment("//     the code is regenerated."),
            SyntaxFactory.Comment("// </auto-generated>"),
            SyntaxFactory.Comment("//----------------------------------------------------------------------------"));
}
