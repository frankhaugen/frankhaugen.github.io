<Query Kind="Statements">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>System.Xml.Schema</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
  <Namespace>System.CodeDom</Namespace>
  <Namespace>Microsoft.CSharp</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
</Query>





var scriptFolder = new FileInfo(Util.CurrentQueryPath).Directory;
var scriptFolderPath = new FileInfo(Util.CurrentQueryPath).Directory!.FullName;
XmlSchemaSet schemaSet = new XmlSchemaSet(new NameTable());
//schemaSet.Add("http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader", Path.Combine(scriptFolderPath, "SBDH20040506-02", "StandardBusinessDocumentHeader.xsd"));
//schemaSet.Add("http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader", Path.Combine(scriptFolderPath, "SBDH20040506-02", "DocumentIdentification.xsd"));
//schemaSet.Add("http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader", Path.Combine(scriptFolderPath, "SBDH20040506-02", "Partner.xsd"));
//schemaSet.Add("http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader", Path.Combine(scriptFolderPath, "SBDH20040506-02", "Manifest.xsd"));
//schemaSet.Add("http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader", Path.Combine(scriptFolderPath, "SBDH20040506-02", "BusinessScope.xsd"));

schemaSet.AddMaindocSchemas(scriptFolder.EnumerateDirectories().First());

schemaSet.Compile();


IEnumerable<CodeTypeDeclaration> CreateCodeTypeDeclarations(XmlSchemaSet schemaSet, string[] targetNamespaces)
{
    XmlSchemas allSchemas = new XmlSchemas();
    foreach (XmlSchema schema in schemaSet.Schemas().Cast<XmlSchema>())
    {
        allSchemas.Add(schema);
    }

    //allSchemas.Compile(UblSettings.XsdValidationEvent, true);
    if (!allSchemas.IsCompiled)
    {
        Console.WriteLine("Warning: allSchemas is not compiled!!! .NET BUG?");
    }
    XmlSchemaImporter importer = new XmlSchemaImporter(allSchemas);
  //  importer.Extensions.Clear(); // Remove System.Data.SqlTypes.TypeXxxx stuff
    CodeNamespace bigBlobCodeNamespace = new CodeNamespace("UblDummyLibrary"); // temporary to hold everything

    CodeGenerationOptions opts = CodeGenerationOptions.GenerateOrder;

bigBlobCodeNamespace.Dump();

    foreach (var ns in targetNamespaces)
    {
        XmlSchema schema = allSchemas[ns];
        foreach (XmlSchemaElement element in schema.Elements.Values)
        {
            XmlTypeMapping mapping = importer.ImportTypeMapping(element.QualifiedName);
           // exporter.ExportTypeMapping(mapping);
        }
    }

    IEnumerable<CodeTypeDeclaration> allCodeDecls = bigBlobCodeNamespace.Types.Cast<CodeTypeDeclaration>().ToList();

    foreach (var codeDecl in allCodeDecls)
    {
        // clear auto generated comment "<remarks/>"
        codeDecl.Comments.Clear();
        foreach (var item in codeDecl.Members.OfType<CodeTypeMember>())
        {
            item.Comments.Clear();
        }

        // populate userdata with things we often query from xsd files
        XmlQualifiedName qname = codeDecl.GetQualifiedName();
        XmlSchema schema = allSchemas[qname.Namespace];
        codeDecl.UserData[Constants.XmlSchemaLabel] = schema;
        XmlSchemaType xmlSchemaType = (XmlSchemaType)schema.SchemaTypes[qname];
        codeDecl.UserData[Constants.XmlSchemaTypeLabel] = xmlSchemaType;

        if (xmlSchemaType == null)
        {
            Console.WriteLine($"Warning: {codeDecl.Name} is missing schema information {qname}");
        }
    }

    return allCodeDecls;
}

//void GenerateAndSaveCodeFilesBySchema(IEnumerable<CodeTypeDeclaration> allCodeDecls, UblNamespaceManager nsMan)
//{
//    var codeDeclsBySchema = (from t in allCodeDecls
//                             group t by t.GetSchema() into g
//                             select g)
//                             .ToDictionary(k => k.Key, v => v.ToArray());
//
//    foreach (var schema in nsMan.Schemas)
//    {
//        CodeNamespace codeNamespace = nsMan.GetCodeNamespaceForXmltargetNamespace(schema.TargetNamespace);
//        if (codeDeclsBySchema.ContainsKey(schema))
//        {
//            codeNamespace.Types.AddRange(codeDeclsBySchema[schema]);
//
//            ConvertFieldsToAutoProperties(codeNamespace); // Hack: append get/set to the Name of a field. 
//        }
//        string csCodeFilename = schema.GetCSharpFilename(UblSettings.CodeGenOutputPath);
//        SaveToFile(codeNamespace, csCodeFilename);
//    }
//}

void ConvertFieldsToAutoProperties(CodeNamespace codeNamespace)
{
    codeNamespace.Types.Cast<CodeTypeDeclaration>()
        .Where(c => c.IsClass)
        .SelectMany(c => c.Members.OfType<CodeMemberField>()) // Only apply to fields
        .ToList()
        .ForEach(c => c.Name += " { get; set; }//"); // Remove double backslash later on
}

void SaveToFile(CodeNamespace ns, string filename)
{
    CSharpCodeProvider provider = new CSharpCodeProvider();
    CodeGeneratorOptions cOpts = new CodeGeneratorOptions { BracingStyle = "C", BlankLinesBetweenMembers = true };
    StringBuilder sb = new StringBuilder();
    using (StringWriter sw = new StringWriter(sb))
    {
        provider.GenerateCodeFromNamespace(ns, sw, cOpts);
    }

    sb = sb.Replace(" { get; set; }//;", " { get; set; }")
        .Replace("Namespace=\"", "Namespace = \"");

    // Enclose XmlRootAttribute with #if and #endif
    const string rootAttributeFragment = "[System.Xml.Serialization.XmlRootAttribute(";
    if (!filename.Contains(Constants.abstractBaseSchemaName)) // skip the abstract base doc
    {
        var fileContent = sb.ToString();
        if (fileContent.Contains(rootAttributeFragment))
        {
            var lines = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            var lineNum = lines.FindIndex(s => s.Contains(rootAttributeFragment));
            lines.Insert(lineNum + 1, "#endif");
            lines.Insert(lineNum, "#if USE_UBL_XMLROOTATTRIBUTE");
            sb = new StringBuilder(string.Join(Environment.NewLine, lines));
        }
    }

    using (StreamWriter writer = File.CreateText(filename))
    {
        writer.Write(sb.ToString());
        Console.WriteLine(filename);
    }
}


void RunRoslyAndOutput(string code, DirectoryInfo outputDirectory, string @namespace)
{
    var text = code;
    var syntaxTree = CSharpSyntaxTree.ParseText(text);
    var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
    if (root == null) throw new NullReferenceException();
    var enums = root.Members.OfType<EnumDeclarationSyntax>();
    var types = root.Members.OfType<TypeDeclarationSyntax>();

    var usings = new StringBuilder()
    .AppendLine($"namespace {@namespace};")
    .ToString();

    foreach (var element in types) File.WriteAllText(Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + ".cs"), usings + element.ToFullString());
    foreach (var element in enums) File.WriteAllText(Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + ".cs"), usings + element.ToFullString());
}

static class Extensions
{

    public static void AddMaindocSchemas(this XmlSchemaSet @this, DirectoryInfo directory)
    {
        XmlReaderSettings readerSettings = new XmlReaderSettings
        {
            ValidationType = ValidationType.Schema,
            DtdProcessing = DtdProcessing.Parse, // will crash without this
            NameTable = @this.NameTable,
        };

        // ubl2.1: Have to preload this file to avoid parsing error due to missing DtdProcessing.Parse setting for implicit includes/imports 
        string preloadFilename = directory!.GetFiles("*.xsd").FirstOrDefault()?.FullName;
        if (!string.IsNullOrEmpty(preloadFilename))
        {
            @this.AddSchemaFile(preloadFilename, readerSettings, null);
        }

        foreach (var xsdFile in directory.GetFiles("*.xsd"))
        {
            var schema = @this.AddSchemaFile(xsdFile.FullName, readerSettings, null);
        }
    }
    private static XmlSchema AddSchemaFile(this XmlSchemaSet @this, string filename, XmlReaderSettings settings, ValidationEventHandler validationEventHandler)
    {
        using (var reader = XmlReader.Create(filename, settings))
        {
            XmlSchema schema = XmlSchema.Read(reader, validationEventHandler);
            return @this.Add(schema);
        }
    }
    public static XmlQualifiedName GetQualifiedName(this CodeTypeMember @this)
    {
        if (@this.UserData[Constants.QNameLabel] == null)
        {
            string name = GetNameWithoutTrailingDigits(@this);
            string codeDeclTargetNamespace = ((CodePrimitiveExpression)GetNamespaceAttributeArgument(@this).Value).Value as string;
            @this.UserData[Constants.QNameLabel] = new XmlQualifiedName(name, codeDeclTargetNamespace);
        }
        return @this.UserData[Constants.QNameLabel] as XmlQualifiedName;
    }

    public static CodeAttributeArgument GetNamespaceAttributeArgument(this CodeTypeMember @this)
    {
        return @this.CustomAttributes.OfType<CodeAttributeDeclaration>()
             .Where(d => d.Name == "System.Xml.Serialization.XmlTypeAttribute").Single().Arguments.Cast<CodeAttributeArgument>()
             .Where(a => a.Name == "Namespace").Single();
    }
    public static string GetNameWithoutTrailingDigits(this CodeTypeMember @this)
    {
        string codeDeclName = @this.Name;
        while (char.IsDigit(codeDeclName.Last())) // most likely single trailing "1"
        {
            // Doing guesswork here. Geting lucky!? Will eventually break...
            codeDeclName = codeDeclName.Substring(0, codeDeclName.Length - 1);
        }
        return codeDeclName;
    }

}

static class Constants
{
    public static readonly string oasisUblTargetNamespacePrefix = "urn:oasis:names:specification:ubl:schema:xsd:";

    public static readonly string CommonBasicComponentsTargetNamespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
    public static readonly string UnqualifiedDataTypesTargetNamespace = "urn:oasis:names:specification:ubl:schema:xsd:UnqualifiedDataTypes-2";
    public static readonly string QualifiedDataTypesTargetNamespace = "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDataTypes-2";
    public static readonly string CoreComponentTypeSchemaModuleTargetNamespace = "urn:un:unece:uncefact:data:specification:CoreComponentTypeSchemaModule:2";
    public static readonly string CommonAggregateComponentsTargetNamespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
    public static readonly string CommonSignatureComponentsTargetNamespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonSignatureComponents-2";
    public static readonly string Xadesv132TargetNamespace = "http://uri.etsi.org/01903/v1.3.2#";
    public static readonly string Xadesv141TargetNamespace = "http://uri.etsi.org/01903/v1.4.1#";
    public static readonly string InvoiceTargetNamespace = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2";
    public static readonly string SignatureAggregateComponents = "urn:oasis:names:specification:ubl:schema:xsd:SignatureAggregateComponents-2";

    public static readonly string BaseDocumentTargetNamespace = "urn:oasis:names:specification:ubl:schema:xsd:BaseDocument-2";
    public static readonly string abstractBaseSchemaName = "BaseDocument";
    public static readonly string abstractBaseSchemaComplexTypeName = "UblBaseDocumentType";
    public static readonly string abstarctBaseSchemaElementName = "UblBaseDocument";
    public static readonly string QNameLabel = "qname";
    public static readonly string XmlSchemaTypeLabel = "xsdtype";
    public static readonly string XmlSchemaLabel = "xsdschema";
}