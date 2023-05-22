<Query Kind="Program">
  <Reference Relative="..\..\..\..\Frank.Libraries\src\Frank.Libraries.Ubl\bin\Debug\net7.0\Frank.Libraries.Ubl.dll">C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.Ubl\bin\Debug\net7.0\Frank.Libraries.Ubl.dll</Reference>
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <NuGetReference>System.Xml.ReaderWriter</NuGetReference>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.Text</Namespace>
  <Namespace>System.CodeDom</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Xml.Schema</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
  <Namespace>Frank.Libraries.Ubl.Validation.Internals</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
</Query>

async Task Main()
{
    var xsdManifest = new XsdManifest();
    var set = xsdManifest.XsdMetadata.Values.ToSchemaSet();
    var generator = new XmlSchemaSetGenerator(set, "Frank");
    var compilation = generator.GenerateCompilation();
    var tempFilePath = Path.GetTempFileName();

    var syntaxTrees = compilation
        .GetSymbolsWithName(name => true, SymbolFilter.Type)
        .OfType<INamedTypeSymbol>()
        .Where(type => type.TypeKind == TypeKind.Class)
        .Select(type => type.DeclaringSyntaxReferences.First().SyntaxTree)
        .Distinct()
        .Select(tree => tree.WithFilePath(tempFilePath))
        .ToList();

    await Parallel.ForEachAsync(syntaxTrees, async (syntaxTree, cancellationToken) =>
    {
        var typename = syntaxTree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>().First().Identifier.ValueText;
        var file = new FileInfo($"C:/temp/codegen/{typename}.cs");
        var text = await syntaxTree.GetTextAsync();
        await text.ToString().DumpToFileAsync(file);
    });
}

public class XmlSchemaSetGenerator
{
    private readonly string _namespace;
    private readonly XmlSchemaSet _schemaSet;
    private readonly Dictionary<XmlQualifiedName, XmlSchemaComplexType> _baseTypes = new();

    public XmlSchemaSetGenerator(XmlSchemaSet schemaSet, string @namespace)
    {
        if (!schemaSet.IsCompiled) schemaSet.Compile();

        _schemaSet = schemaSet;
        _namespace = @namespace;

        var globalTypes = schemaSet.GlobalTypes.As<XmlSchemaObjectTable>();

        foreach (DictionaryEntry globalType in globalTypes)
        {
            _baseTypes.Add(globalType.Key.As<XmlQualifiedName>(), globalType.Value.As<XmlSchemaComplexType>());
        }
    }

    public Compilation GenerateCompilation()
    {
        // Parse the C# code into a SyntaxTree
        var compilationUnits = GetCompileUnits();
        var syntaxTrees = GetSyntaxTrees(compilationUnits);

        // Create a compilation with the parsed syntax tree
        var compilation = CSharpCompilation.Create(_namespace)
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(syntaxTrees);

        return compilation;
    }

    private CodeCompileUnit[] GetCompileUnits()
    {
        var compileUnits = new List<CodeCompileUnit>();
        foreach (var kvp in _baseTypes)
        {
            compileUnits.Add(GetCompileUnit(new(kvp.Key, kvp.Value)));
        }
        return compileUnits.ToArray();
    }

    private SyntaxTree[] GetSyntaxTrees(CodeCompileUnit[] compileUnits)
    {
        var trees = new List<SyntaxTree>();

        foreach (var compileUnit in compileUnits)
        {
            trees.Add(GetSyntaxTree(compileUnit));
        }

        return trees.ToArray();
    }

    private SyntaxTree GetSyntaxTree(CodeCompileUnit compileUnit)
    {
        var code = GetCode(compileUnit);
        return CSharpSyntaxTree.ParseText(SourceText.From(code.ToString()));
    }

    private string GetCode(CodeCompileUnit compileUnit)
    {
        // Create a CodeDom provider for C#
        var provider = CodeDomProvider.CreateProvider("C#");

        // Generate the C# code for the XmlSchemaSet
        var options = new CodeGeneratorOptions
        {
            BracingStyle = "C",
            IndentString = "    "
        };
        var code = new StringWriter();
        provider.GenerateCodeFromCompileUnit(compileUnit, code, options);
        return code.ToString();
    }

    private CodeCompileUnit GetCompileUnit(XmlNameType nameType)
    {
        var compileUnit = new CodeCompileUnit();
        var codeNamespace = new CodeNamespace($"{_namespace}.Generated");

        compileUnit.Namespaces.Add(codeNamespace);

        var typeDeclaration = new CodeTypeDeclaration(nameType.QualifiedName.Name)
        {
            
        };

        if (nameType.ComplexType?.ContentTypeParticle is XmlSchemaParticle particle)
        {
            GeneratePropertiesForContentModel(particle, typeDeclaration);
        }
        //else if (nameType.ComplexType?.ContentTypeParticle is XmlSchemaSequence sequence)
        //{
        //    foreach (var item in sequence.Items as XmlSchemaObjectCollection)
        //    {
        //        if (item is XmlSchemaElement element)
        //        {
        //            var property = XmlSchemaElementToCodeSnippetTypeMember(element);
        //            typeDeclaration.Members.Add(property);
        //        }
        //    }
        //}

        codeNamespace.Types.Add(typeDeclaration);

        if (nameType.QualifiedName.Name == "BinaryObjectType") Debugger.Break();
        return compileUnit;
    }

    public CodeTypeReference XmlSchemaElementToCodeTypeReference(XmlSchemaElement element)
    {
        var type = element.ElementSchemaType?.Datatype?.ValueType;

        if (type != null)
            return new CodeTypeReference(type);

        var typename = element?.ElementSchemaType?.QualifiedName;

        if (typename == null)
            return new CodeTypeReference();

        var thing = _baseTypes[typename];
        if (thing.Datatype?.ValueType != null)
            return new CodeTypeReference(thing.Datatype?.ValueType);

        return new CodeTypeReference(thing.Name);
    }
    
    private void GeneratePropertiesForContentModel(XmlSchemaParticle particle, CodeTypeDeclaration typeDeclaration)
    {
        if (particle is XmlSchemaSequence sequence)
        {
            foreach (var item in sequence.Items as XmlSchemaObjectCollection)
            {
                if (item is XmlSchemaElement element)
                {
                    var property = XmlSchemaElementToCodeSnippetTypeMember(element);
                    typeDeclaration.Members.Add(property);
                }
                else if (item is XmlSchemaParticle childParticle)
                {
                    GeneratePropertiesForContentModel(childParticle, typeDeclaration);
                }
            }
        }
        else if (particle is XmlSchemaChoice choice)
        {
            foreach (var item in choice.Items as XmlSchemaObjectCollection)
            {
                if (item is XmlSchemaElement element)
                {
                    var property = XmlSchemaElementToCodeSnippetTypeMember(element);
                    typeDeclaration.Members.Add(property);
                }
                else if (item is XmlSchemaParticle childParticle)
                {
                    GeneratePropertiesForContentModel(childParticle, typeDeclaration);
                }
            }
        }
    }
        
    public CodeSnippetTypeMember XmlSchemaElementToCodeSnippetTypeMember(XmlSchemaElement element)
    {
        return new CodeSnippetTypeMember($"        public {XmlSchemaElementToCodeTypeReference(element).BaseType} {element.Name ?? element.QualifiedName.Name} {{ get; set; }}");
    }
    
    public record XmlNameType(XmlQualifiedName QualifiedName, XmlSchemaComplexType ComplexType);
}

