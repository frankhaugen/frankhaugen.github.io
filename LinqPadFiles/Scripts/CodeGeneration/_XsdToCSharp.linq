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
    var generator = new XmlSchemaSetGenerator(Merge(xsdManifest.CommonXsdMetadata, xsdManifest.MaindocXsdMetadata), "Frank");
    
    var name = xsdManifest.MaindocXsdMetadata.First(x => x.Key.Contains("Catalogue")).Key;
    
    var syntaxTree = generator.Generate(name);
    
    syntaxTree.ToString().Dump();
    
    
//    var compilation = generator.GenerateCompilation();
//    var tempFilePath = Path.GetTempFileName();
//
//    var syntaxTrees = compilation
//        .GetSymbolsWithName(name => true, SymbolFilter.Type)
//        .OfType<INamedTypeSymbol>()
//        .Where(type => type.TypeKind == TypeKind.Class)
//        .Select(type => type.DeclaringSyntaxReferences.First().SyntaxTree)
//        .Distinct()
//        .Select(tree => tree.WithFilePath(tempFilePath))
//        .ToList();

    //await Parallel.ForEachAsync(syntaxTrees, async (syntaxTree, cancellationToken) =>
    //{
    //    var typename = syntaxTree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>().First().Identifier.ValueText;
    //    var file = new FileInfo($"C:/temp/codegen/ubl/{typename}.cs");
    //    var text = await syntaxTree.GetTextAsync();
    //    await text.ToString().DumpToFileAsync(file);
    //});
}

public static IDictionary<TKey, TValue> Merge<TKey, TValue>(IDictionary<TKey, TValue> dictionary1, IDictionary<TKey, TValue> dictionary2)
{
    return dictionary1.Concat(dictionary2).ToDictionary(x => x.Key, x => x.Value);
}

public class XmlSchemaSetGenerator
{
    
    private readonly string _namespace;
    private readonly XmlSchemaSet _schemaSet;
    private readonly Dictionary<XmlQualifiedName, XmlSchemaComplexType> _baseTypes = new();
    
    public XmlSchemaSetGenerator(IDictionary<string, XsdMetadata> xsds, string @namespace)
    {
        _namespace = @namespace;
        XSDs = xsds;
        
        _schemaSet = xsds.Values.ToSchemaSet();
        if (!_schemaSet.IsCompiled) _schemaSet.Compile();

        var globalTypes = _schemaSet.GlobalTypes.As<XmlSchemaObjectTable>();
        foreach (DictionaryEntry globalType in globalTypes)
        {
            _baseTypes.Add(globalType.Key.As<XmlQualifiedName>(), globalType.Value.As<XmlSchemaComplexType>());
        }
    }
    
    public IDictionary<string, XsdMetadata> XSDs { get; }
    
    public SyntaxTree Generate(string name)
    {
        var schemaMetadata = XSDs[name];
        
        var schema = schemaMetadata.Schema;

        XmlSchemaToCodeConverter.Convert(schema, _schemaSet, _namespace, out var codeCompileUnit, out var nameType);
        var syntaxTree = GetSyntaxTree(codeCompileUnit);
        return syntaxTree;
    }
    
    
    private CodeCompileUnit GetCompileUnit(XmlNameType nameType)
    {
        var compileUnit = new CodeCompileUnit();
        var codeNamespace = new CodeNamespace($"{_namespace}.Generated");

        compileUnit.Namespaces.Add(codeNamespace);

        var typeDeclaration = new CodeTypeDeclaration(nameType.QualifiedName.Name);

        if (nameType.ComplexType?.ContentTypeParticle is XmlSchemaParticle particle)
        {
            XmlSchemaToCodeConverter.XmlSchemaParticleToCodeTypeDeclaration(particle, _baseTypes, typeDeclaration);
        }
        
        codeNamespace.Types.Add(typeDeclaration);

        if (nameType.QualifiedName.Name == "BinaryObjectType") Debugger.Break();
        return compileUnit;
    }


//    private Compilation GenerateCompilation()
//    {
//        var compilationUnits = GetCompileUnits();
//        var syntaxTrees = GetSyntaxTrees(compilationUnits);
//        var compilation = CSharpCompilation.Create(_namespace)
//            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
//            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
//            .AddSyntaxTrees(syntaxTrees);
//
//        return compilation;
//    }

    //private CodeCompileUnit[] GetCompileUnits()
    //{
    //    var compileUnits = new List<CodeCompileUnit>();
    //    foreach (var kvp in _baseTypes)
    //    {
    //        compileUnits.Add(GetCompileUnit(new(kvp.Key, kvp.Value)));
    //    }
    //    return compileUnits.ToArray();
    //}

//    private SyntaxTree[] GetSyntaxTrees(CodeCompileUnit[] compileUnits)
//    {
//        var trees = new List<SyntaxTree>();
//
//        foreach (var compileUnit in compileUnits)
//        {
//            trees.Add(GetSyntaxTree(compileUnit));
//        }
//
//        return trees.ToArray();
//    }

    private SyntaxTree GetSyntaxTree(CodeCompileUnit compileUnit)
    {
        var code = GetCode(compileUnit);
        return CSharpSyntaxTree.ParseText(SourceText.From(code.ToString()));
    }

    private string GetCode(CodeCompileUnit compileUnit)
    {
        var provider = CodeDomProvider.CreateProvider("C#");
        var options = new CodeGeneratorOptions
        {
            BracingStyle = "C",
            IndentString = "    "
        };
        var code = new StringWriter();
        provider.GenerateCodeFromCompileUnit(compileUnit, code, options);
        return code.ToString();
    }

}

public record XmlNameType(XmlQualifiedName QualifiedName, XmlSchemaComplexType ComplexType);

public static class XmlSchemaToCodeConverter
{

    public static void XmlSchemaParticleToCodeTypeDeclaration(XmlSchemaParticle particle, IDictionary<XmlQualifiedName, XmlSchemaComplexType> baseTypes, in CodeTypeDeclaration typeDeclaration)
    {
        if (particle is XmlSchemaSequence sequence)
        {
            foreach (var item in sequence.Items as XmlSchemaObjectCollection)
            {
                if (item is XmlSchemaElement element)
                {
                    typeDeclaration.Members.Add(XmlSchemaToCodeConverter.XmlSchemaElementToCodeSnippetTypeMember(element, baseTypes));
                }
                else if (item is XmlSchemaParticle childParticle)
                {
                    XmlSchemaParticleToCodeTypeDeclaration(childParticle, baseTypes, typeDeclaration);
                }
            }
        }
        else if (particle is XmlSchemaChoice choice)
        {
            foreach (var item in choice.Items as XmlSchemaObjectCollection)
            {
                if (item is XmlSchemaElement element)
                {
                    typeDeclaration.Members.Add(XmlSchemaToCodeConverter.XmlSchemaElementToCodeSnippetTypeMember(element, baseTypes));
                }
                else if (item is XmlSchemaParticle childParticle)
                {
                    XmlSchemaParticleToCodeTypeDeclaration(childParticle, baseTypes, typeDeclaration);
                }
            }
        }
    }
    
    public static CodeSnippetTypeMember XmlSchemaElementToCodeSnippetTypeMember(XmlSchemaElement element, IDictionary<XmlQualifiedName, XmlSchemaComplexType> baseTypes)
    {
        return new CodeSnippetTypeMember($"        public {XmlSchemaElementToCodeTypeReference(element, baseTypes).BaseType} {element.Name ?? element.QualifiedName.Name} {{ get; set; }}");
    }

    public static CodeTypeReference XmlSchemaElementToCodeTypeReference(XmlSchemaElement element, IDictionary<XmlQualifiedName, XmlSchemaComplexType> baseTypes)
    {
        var type = element.ElementSchemaType?.Datatype?.ValueType;

        if (type != null)
            return new CodeTypeReference(type);

        var typename = element?.ElementSchemaType?.QualifiedName;

        if (typename == null)
            return new CodeTypeReference();

        var thing = baseTypes[typename];
        if (thing.Datatype?.ValueType != null)
            return new CodeTypeReference(thing.Datatype?.ValueType);

        return new CodeTypeReference(thing.Name);
    }

    internal static void Convert(XmlSchema schema, XmlSchemaSet schemaSet, string @namespace, out CodeCompileUnit codeCompileUnit, out object nameType)
    {
        var codeNamespace = new CodeNamespace(@namespace);
        codeCompileUnit = new CodeCompileUnit();
        codeCompileUnit.Namespaces.Add(codeNamespace);

        var baseTypes = schemaSet.Schemas().Cast<XmlSchema>().SelectMany(x => x.SchemaTypes.Values.OfType<XmlSchemaComplexType>()).ToDictionary(x => x.QualifiedName);

        foreach (var item in schema.Items)
        {
            if (item is XmlSchemaElement element)
            {
                var typeDeclaration = new CodeTypeDeclaration(element.Name ?? element.QualifiedName.Name);
                typeDeclaration.IsClass = true;
                typeDeclaration.TypeAttributes = System.Reflection.TypeAttributes.Public;
                if (element.ElementSchemaType is XmlSchemaComplexType complexType)
                {
                    XmlSchemaParticleToCodeTypeDeclaration(complexType.Particle, baseTypes, typeDeclaration);
                }

                codeNamespace.Types.Add(typeDeclaration);
            }
        }

        nameType = codeNamespace;
    }
}