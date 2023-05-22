<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Xml.Schema</Namespace>
  <Namespace>System.CodeDom</Namespace>
  <Namespace>Microsoft.CSharp</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>


static void Main(string[] args)
{
    UblXsdSettings ublSettings = new UblXsdSettings
    {
        XsdValidationEvent = SchemaValidationEventHandler,
        UblXsdInputPath = @"C:\repos\ubllarsen\UblLarsen\UBL-2.1\xsd",  // Content originally from ubl zip download.
        CodeGenOutputPath = @"C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.Ubl\Models",
        OptionOptimizeCommonBasicComponents = true, // remove yagni types from schema before code generation. tests wont work when this is set to false
        CSharpDefaultNamespace = "Frank.Libaries.Ubl.Models",
        OptionMemberTypeToGenerate = UblXsdSettings.FieldTypesEnum.AutoProperty // or Field, or Property
    };

    XmlSchemaSet schemaSet = new XmlSchemaSet(new NameTable());
    schemaSet.ValidationEventHandler += SchemaValidationEventHandler;
    // Read and add the xsd documents in xsd/maindoc folder
    schemaSet.AddMaindocSchemas(ublSettings);
    schemaSet.Compile();

    if (ublSettings.OptionOptimizeCommonBasicComponents)
    {
        var cbcSchema = schemaSet.Schemas(Constants.CommonBasicComponentsTargetNamespace).Cast<XmlSchema>().Single();
        UblSchemaTypeSimplificationTool.SimplifyCommonBasicComponentsTypes(cbcSchema);
        schemaSet.Reprocess(cbcSchema);
        schemaSet.Compile();
    }

    UblSchemaTypeSimplificationTool.ResolveTypeNameClashesByRenaming(schemaSet);
    schemaSet.Compile();

    var abstractMaindocBaseSchema = UblSchemaInheritanceTools.ModifyMaindocSchemasForInheritance(schemaSet.MaindocSchemas());
    abstractMaindocBaseSchema = schemaSet.Add(abstractMaindocBaseSchema);
    schemaSet.MaindocSchemas().Where(s => s != abstractMaindocBaseSchema).ToList().ForEach(s => schemaSet.Reprocess(s));
    schemaSet.Compile();

    UblCodeGenerator gen = new UblCodeGenerator(ublSettings);
    // namespacelist parameter will drag in all other dependent types. New extensions shold probably be appended here... or...
    var allCodeDecls = gen.CreateCodeTypeDeclarations(schemaSet,
        new[] {
                    Constants.BaseDocumentTargetNamespace,
                    Constants.SignatureAggregateComponents,
                    Constants.CommonSignatureComponentsTargetNamespace,
                    Constants.Xadesv141TargetNamespace}
        );

    UblDocumentationTool.AddDocumentation(allCodeDecls);

    UblImplicitAssignmentTool.AddImplicitAssignmentOperatorsForXmlTextAttributedDecendants(allCodeDecls);

    XsdTimeTool.IgnoreDateTimeSerializeAsString(allCodeDecls.Where(c => c.Name == "TimeType").Single());

    var mainDocCodeDecls = allCodeDecls.Where(c => c.BaseTypes.Count == 1 && c.BaseTypes[0].BaseType == Constants.abstractBaseSchemaComplexTypeName);
    MainDocsAttributeTool.RenameTopLevelXmlType(mainDocCodeDecls);
    MainDocsAttributeTool.AddXmlRootAttributes(mainDocCodeDecls);

    gen.GenerateAndSaveCodeFilesBySchema(allCodeDecls, new UblNamespaceManager(schemaSet, ublSettings.CSharpDefaultNamespace, ublSettings.OptionOptimizeCommonBasicComponents));

    UblSchemaStatsTool.ShowStats(allCodeDecls);

    Console.WriteLine("DONE");
    Console.ReadLine();
}

private static void SchemaValidationEventHandler(object sender, ValidationEventArgs arg)
{
    if (arg.Severity == XmlSeverityType.Error)
    {
        //$"{arg.Severity.ToString()}: {arg.Message}".Dump();
    }
}











class UblCodeGenerator
{
    private readonly UblXsdSettings UblSettings;

    public UblCodeGenerator(UblXsdSettings ublSettings)
    {
        this.UblSettings = ublSettings;
    }

    public IEnumerable<CodeTypeDeclaration> CreateCodeTypeDeclarations(XmlSchemaSet schemaSet, string[] targetNamespaces)
    {
        XmlSchemas allSchemas = new XmlSchemas();
        foreach (XmlSchema schema in schemaSet.Schemas().Cast<XmlSchema>())
        {
            allSchemas.Add(schema);
        }

        allSchemas.Compile(UblSettings.XsdValidationEvent, true);
        if (!allSchemas.IsCompiled)
        {
            Console.WriteLine("Warning: allSchemas is not compiled!!! .NET BUG?");
        }
        XmlSchemaImporter importer = new XmlSchemaImporter(allSchemas);
        
        
        //importer.Extensions.Clear(); // Remove System.Data.SqlTypes.TypeXxxx stuff
        
        
        CodeNamespace bigBlobCodeNamespace = new CodeNamespace("UblDummyLibrary"); // temporary to hold everything

        CodeGenerationOptions opts = CodeGenerationOptions.GenerateOrder;
        if (UblSettings.OptionMemberTypeToGenerate == UblXsdSettings.FieldTypesEnum.Property)
        {
            opts |= CodeGenerationOptions.GenerateProperties;
        }
        
        //XmlCodeExporter exporter = new XmlCodeExporter(bigBlobCodeNamespace, null, opts);

        foreach (var ns in targetNamespaces)
        {
            XmlSchema schema = allSchemas[ns];
            foreach (XmlSchemaElement element in schema.Elements.Values)
            {
                XmlTypeMapping mapping = importer.ImportTypeMapping(element.QualifiedName);
                mapping.Dump();
                //exporter.ExportTypeMapping(mapping);
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

    public void GenerateAndSaveCodeFilesBySchema(IEnumerable<CodeTypeDeclaration> allCodeDecls, UblNamespaceManager nsMan)
    {
        var codeDeclsBySchema = (from t in allCodeDecls
                                 group t by t.GetSchema() into g
                                 select g)
                                 .ToDictionary(k => k.Key, v => v.ToArray());

        foreach (var schema in nsMan.Schemas)
        {
            CodeNamespace codeNamespace = nsMan.GetCodeNamespaceForXmltargetNamespace(schema.TargetNamespace);
            if (codeDeclsBySchema.ContainsKey(schema))
            {
                codeNamespace.Types.AddRange(codeDeclsBySchema[schema]);

                if (UblSettings.OptionMemberTypeToGenerate == UblXsdSettings.FieldTypesEnum.AutoProperty)
                {
                    ConvertFieldsToAutoProperties(codeNamespace); // Hack: append get/set to the Name of a field. 
                }
            }
            string csCodeFilename = schema.GetCSharpFilename(UblSettings.CodeGenOutputPath);
            SaveToFile(codeNamespace, csCodeFilename);
        }
    }

    private void ConvertFieldsToAutoProperties(CodeNamespace codeNamespace)
    {
        codeNamespace.Types.Cast<CodeTypeDeclaration>()
            .Where(c => c.IsClass)
            .SelectMany(c => c.Members.OfType<CodeMemberField>()) // Only apply to fields
            .ToList()
            .ForEach(c => c.Name += " { get; set; }//"); // Remove double backslash later on
    }

    public void SaveToFile(CodeNamespace ns, string filename)
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
}





















































public static class CodeTypeMemberExtensions
{
    public static CodeAttributeArgument GetNamespaceAttributeArgument(this CodeTypeMember @this)
    {
        return @this.CustomAttributes.OfType<CodeAttributeDeclaration>()
             .Where(d => d.Name == "System.Xml.Serialization.XmlTypeAttribute").Single().Arguments.Cast<CodeAttributeArgument>()
             .Where(a => a.Name == "Namespace").Single();
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

    public static bool HasAnyXmlTextAttribute(this CodeTypeMember @this)
    {
        return @this.CustomAttributes.Cast<CodeAttributeDeclaration>().Any(a => a.Name == "System.Xml.Serialization.XmlTextAttribute");
    }

    public static XmlSchema GetSchema(this CodeTypeMember @this)
    {
        return @this.UserData[Constants.XmlSchemaLabel] as XmlSchema;
    }

    public static XmlSchemaType GetXmlSchemaType(this CodeTypeMember @this)
    {
        return @this.UserData[Constants.XmlSchemaTypeLabel] as XmlSchemaType;
    }

    /// <summary>
    /// http://www.w3schools.com/xml/schema_simple_attributes.asp
    /// Look for attributes with use="required"
    /// </summary>
    public static bool HasAnyRequiredMembers(this CodeTypeMember @this)
    {
        XmlSchemaComplexType xmlComplexType = GetXmlSchemaType(@this) as XmlSchemaComplexType;
        var content = (xmlComplexType.ContentModel as XmlSchemaSimpleContent)?.Content;
        if (content != null)
        {
            var restriction = content as XmlSchemaSimpleContentRestriction;
            var extension = content as XmlSchemaSimpleContentExtension;
            return (restriction?.Attributes ?? extension.Attributes).Cast<XmlSchemaAttribute>().Any(a => a.Use == XmlSchemaUse.Required);
        }
        else
        {
            return xmlComplexType.Attributes.Cast<XmlSchemaAttribute>().Any(a => a.Use == XmlSchemaUse.Required);
        }
    }
}




/// <summary>
/// </summary>
public static class XmlSchemaDocumentationExtensions
{
    public static string[] GetDocumentation(this XmlSchemaDocumentation @this)
    {
        char[] trimChars = new[] { ' ', '\t', '\n', '\r' };
        List<string> res = null;
        if (@this != null && @this.Markup.Any())
        {
            var node = @this.Markup.First();
            if (node.NodeType == XmlNodeType.Text)
            {
                // return pure text
                res = @this.Markup.OfType<XmlText>()
                    .Select(s => s.Value.Trim(trimChars))
                    .ToList();
            }
            else if (node.NodeType == XmlNodeType.Element)
            {
                // xml to text
                string xml = string.Empty;
                if (node.Name == "ccts:Component")
                {
                    xml = node.OuterXml;
                }
                else if (node.Name.StartsWith("ccts:"))
                {
                    xml = $"<ccts:Component xmlns:ccts=\"urn:un:unece:uncefact:documentation:2\">{node.ParentNode.InnerXml}</ccts:Component>";
                }
                var comp = XElement.Parse(xml);
                res = comp.Elements().Select(e => $"{e.Name.LocalName}: {e.Value.Trim(trimChars)}").ToList(); // embedded '\n' might slip through
            }
            if (res != null)
            {
                for (int i = 1; i < res.Count; i++)
                {
                    res[i] = $"<para>{res[i]}</para>";
                }
                res.Insert(0, "<summary>");
                res.Add("</summary>");
                return res.ToArray();
            }
        }
        return new string[0];
    }
}

public static class XmlSchemaExtensions
{
    public static void Dump(this XmlSchema @this, string filename)
    {
        using (var writer = XmlWriter.Create(filename, new XmlWriterSettings { Indent = true }))
        {
            @this.Write(writer);
        }
    }

    /// <summary>
    /// Return "maindoc/somefile.xsd" or "common/someoterfile.xsd"
    /// </summary>
    /// <param name="this"></param>
    /// <returns>filname prefixed with common/ or maindoc/</returns>
    public static string GetFileNameWithSubDirectory(this XmlSchema @this)
    {
        string xsdFilename = new Uri(@this.SourceUri).LocalPath;
        FileInfo fi = new FileInfo(xsdFilename);
        return Path.Combine(fi.Directory.Name, fi.Name);
    }

    /// <summary>
    /// Construct a csharp filename for output code generation.
    /// Take the xsd filename and change exstension to .cs and append the filename to the settings outputDir.
    /// Maintains the "common" and "maindoc" sub dir structure.
    /// </summary>
    /// <param name="outputDir">csharp filename for output</param>
    /// <returns></returns>
    public static string GetCSharpFilename(this XmlSchema @this, string outputDir)
    {
        return Path.Combine(Path.Combine(outputDir, Path.ChangeExtension(GetFileNameWithSubDirectory(@this), ".cs")));
    }

}

public static class XmlSchemaSetExtensions
{
    public static void AddMaindocSchemas(this XmlSchemaSet @this, UblXsdSettings ublSettings)
    {
        XmlReaderSettings readerSettings = new XmlReaderSettings
        {
            ValidationType = ValidationType.Schema,
            DtdProcessing = DtdProcessing.Parse, // will crash without this
            NameTable = @this.NameTable,
        };

        // ubl2.1: Have to preload this file to avoid parsing error due to missing DtdProcessing.Parse setting for implicit includes/imports 
        string preloadFilename = ublSettings.CommonDirectory.GetFiles("UBL-xmldsig-core-schema-*.xsd").FirstOrDefault()?.FullName;
        if (!string.IsNullOrEmpty(preloadFilename))
        {
            @this.AddSchemaFile(preloadFilename, readerSettings, ublSettings.XsdValidationEvent);
        }

        foreach (var xsdFile in ublSettings.MaindocDirectory.GetFiles("*.xsd"))
        {
            var schema = @this.AddSchemaFile(xsdFile.FullName, readerSettings, ublSettings.XsdValidationEvent);
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

    // use to build XmlSerializerNamespaces for the serializer
    public static XmlQualifiedName[] GetNamespacePrefixes(this XmlSchemaSet @this)
    {
        List<XmlQualifiedName> nsList = new List<XmlQualifiedName>();
        foreach (XmlSchema schema in @this.Schemas())
        {
            nsList.AddRange(schema.Namespaces.ToArray().Where(ns => !string.IsNullOrEmpty(ns.Name)));
        }
        //return nsList.Distinct().ToArray();
        QNameComparer comparer = new QNameComparer();
        nsList.Sort(comparer.Compare);
        return nsList.ToArray();
    }

    public static ICollection<XmlSchema> MaindocSchemas(this XmlSchemaSet @this)
    {
        // TODO: Need a better way of separating maindocschemas from common ones imported/incuded explicitly.
        return @this.Schemas().Cast<XmlSchema>().Where(s => s.SourceUri.Contains("maindoc") && s.TargetNamespace.StartsWith(Constants.oasisUblTargetNamespacePrefix)).ToList();
    }
}

internal class QNameComparer : IComparer
{
    public int Compare(object o1, object o2)
    {
        XmlQualifiedName qn1 = (XmlQualifiedName)o1;
        XmlQualifiedName qn2 = (XmlQualifiedName)o2;
        int ns = String.Compare(qn1.Namespace, qn2.Namespace, StringComparison.Ordinal);
        if (ns == 0)
        {
            return String.Compare(qn1.Name, qn2.Name, StringComparison.Ordinal);
        }
        return ns;
    }
}

public static class Constants
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
public static class MainDocsAttributeTool
{
    public static void AddXmlRootAttributes(IEnumerable<CodeTypeDeclaration> mainDocCodeDecls)
    {
        var isNullableArgument = new CodeAttributeArgument { Name = "IsNullable", Value = new CodePrimitiveExpression(false) };

        foreach (var cdeclMaindoc in mainDocCodeDecls)
        {
            var indexOfTrailingType = cdeclMaindoc.Name.LastIndexOf("Type", StringComparison.InvariantCulture);
            if (indexOfTrailingType < 1)
                continue;

            string elementName = cdeclMaindoc.Name.Substring(0, indexOfTrailingType); // remove tailing "Type"
            var namespaceArgument = cdeclMaindoc.GetNamespaceAttributeArgument();
            CodeAttributeDeclaration xmlRootAtt = new CodeAttributeDeclaration
            {
                Name = "System.Xml.Serialization.XmlRootAttribute",
                Arguments =
                    {
                        new CodeAttributeArgument( new  CodePrimitiveExpression(elementName)),
                        namespaceArgument,
                        isNullableArgument
                    }
            };
            cdeclMaindoc.CustomAttributes.Add(xmlRootAtt);
        }
    }

    /// <summary>
    /// Add the correct name, excluding trailing "Type", to XmlTypeAttribute as first argument
    /// </summary>
    /// <param name="bigBlobCodeNamespace"></param>
    public static void RenameTopLevelXmlType(IEnumerable<CodeTypeDeclaration> mainDocCodeDecls)
    {
        foreach (var doc in mainDocCodeDecls)
        {
            var indexOfTrailingType = doc.Name.LastIndexOf("Type", StringComparison.InvariantCulture);
            if (indexOfTrailingType < 1)
                continue; // Should not happen

            string elementName = doc.Name.Substring(0, indexOfTrailingType); // remove trailing "Type"
            var xmlTypeAtt = doc.CustomAttributes.OfType<CodeAttributeDeclaration>().Where(v => v.Name == "System.Xml.Serialization.XmlTypeAttribute").Single();
            // Add correct name
            xmlTypeAtt.Arguments.Insert(0, new CodeAttributeArgument(new CodePrimitiveExpression(elementName)));
        }
    }
}


public static class UblDocumentationTool
{
    private static Dictionary<string, XmlSchemaDocumentation> emptyDocumentationDictionary = new Dictionary<string, XmlSchemaDocumentation>();

    /// <summary>
    /// Add comment to class and its members (properties or fields).
    /// </summary>
    public static void AddDocumentation(IEnumerable<CodeTypeDeclaration> codeDecls)
    {
        Func<CodeTypeDeclaration, IQueryable<CodeTypeMember>> GetPublicFieldPropertyMembers = cdecl => cdecl.Members.OfType<CodeTypeMember>()
            .Where(c => (c.GetType() == typeof(CodeMemberField) || c.GetType() == typeof(CodeMemberProperty)) &&
            (c.Attributes & MemberAttributes.Public) == MemberAttributes.Public).AsQueryable();

        Action<CodeTypeMember, XmlSchemaDocumentation> AddCommentsToCodeType = (ct, xd) =>
        {
            var textLines = xd.GetDocumentation();
            foreach (var line in textLines)
            {
                ct.Comments.Add(new CodeCommentStatement(line, true));
            }
        };

        foreach (CodeTypeDeclaration codeDecl in codeDecls)
        {
            XmlSchemaComplexType xsdComplexType = (XmlSchemaComplexType)codeDecl.GetXmlSchemaType();

            // TODO: Figure out why "{http://www.w3.org/2000/09/xmldsig#:ItemsChoiceType}" is missing
            if (xsdComplexType == null)
                continue;

            // Add comment to class
            XmlSchemaDocumentation xsdDocNode = xsdComplexType.Annotation?.Items.OfType<XmlSchemaDocumentation>().FirstOrDefault();
            if (xsdDocNode != null)
            {
                AddCommentsToCodeType(codeDecl, xsdDocNode);
            }

            // Get all members except XmlTextAttribute members. They do not have any doc and must be excluded
            var classMembers = GetPublicFieldPropertyMembers(codeDecl)
                .Where(m => !m.HasAnyXmlTextAttribute())
                .ToList();
            if (classMembers.Count == 0)
            {
                continue;
            }

            // Build a dictionary of docnodes from xsd attribute and element definitions
            Dictionary<string, XmlSchemaDocumentation> memberDocumentationDictionary = emptyDocumentationDictionary;
            switch (xsdComplexType.ContentType)
            {
                case XmlSchemaContentType.TextOnly: // simpleContent, DateType
                    XmlSchemaSimpleContentExtension sext = (xsdComplexType.ContentModel as XmlSchemaSimpleContent).Content as XmlSchemaSimpleContentExtension;
                    memberDocumentationDictionary = sext.Attributes.OfType<XmlSchemaAttribute>().ToDictionary(k => k.Name, v => v.Annotation?.Items.OfType<XmlSchemaDocumentation>().First());
                    break;
                case XmlSchemaContentType.Empty:
                    //continue;
                    break;
                case XmlSchemaContentType.ElementOnly:
                case XmlSchemaContentType.Mixed:
                    if (xsdComplexType.ContentTypeParticle is XmlSchemaSequence)
                    {
                        var schemaSequence = xsdComplexType.ContentTypeParticle as XmlSchemaSequence;
                        memberDocumentationDictionary = schemaSequence.Items.OfType<XmlSchemaElement>().ToDictionary(k => k.QualifiedName.Name, v => v.Annotation?.Items.OfType<XmlSchemaDocumentation>().FirstOrDefault());
                    }
                    else if (xsdComplexType.ContentTypeParticle is XmlSchemaChoice)
                    {
                        var schemaChoice = xsdComplexType.ContentTypeParticle as XmlSchemaChoice;
                        memberDocumentationDictionary = schemaChoice.Items.OfType<XmlSchemaElement>().ToDictionary(k => k.QualifiedName.Name, v => v.Annotation?.Items.OfType<XmlSchemaDocumentation>().FirstOrDefault());
                    }
                    break;
                default:
                    // Do never hit
                    break;
            }

            // Finally add comments to each member
            foreach (var member in classMembers)
            {
                var memberName = member.GetNameWithoutTrailingDigits();
                if (memberDocumentationDictionary.ContainsKey(memberName))
                {
                    AddCommentsToCodeType(member, memberDocumentationDictionary[memberName]);
                }
            }
        }
    }
}

/// <summary>
/// Generate implicit assignment for types that have
/// - XmlTextAttribute on a member
/// - Inherit from a type with the above condition
/// 
/// Do not generate imlicit assignment for types that have
/// - xsd use="required" on any member
/// - inherit from a type with the above condition
/// - is abstract
/// </summary>
public class UblImplicitAssignmentTool
{
    private class MemberTypeTuple
    {
        public Type BaseMemberType;
        public CodeTypeDeclaration CodeDecl;
    }

    // TODO: This is "secutity by obscurity"! Refactor for readability
    public static void AddImplicitAssignmentOperatorsForXmlTextAttributedDecendants(IEnumerable<CodeTypeDeclaration> codeDecls)
    {
        var classes = codeDecls.Where(c => c.IsClass).ToList();

        // public classes with "any" XmlTextAttribute on a field where all other fields are non-required
        var codeDeclsBase = classes.Where(c =>
                !c.HasAnyRequiredMembers() &&
                c.Members.OfType<CodeTypeMember>().Any(m =>
                    (m.GetType() == typeof(CodeMemberField) || m.GetType() == typeof(CodeMemberProperty)) &&
                    ((m.Attributes & MemberAttributes.Public) == MemberAttributes.Public) &&
                    m.HasAnyXmlTextAttribute())
                ).ToList();

        var accumulatedTupleList = (from cdecl in codeDeclsBase
                                    let xmlMember = cdecl.GetXmlSchemaType() as XmlSchemaComplexType
                                    where xmlMember.BaseXmlSchemaType.Datatype != null
                                    select new MemberTypeTuple { CodeDecl = cdecl, BaseMemberType = (Type)xmlMember.BaseXmlSchemaType.Datatype.ValueType }).ToList();

        IEnumerable<MemberTypeTuple> decendantsAtNextLevel = accumulatedTupleList;

        do
        {
            var baseTypeNameFilter = decendantsAtNextLevel.Select(d => d.CodeDecl.Name).ToArray();
            decendantsAtNextLevel = classes
                .Where(c =>
                    c.BaseTypes.Count > 0 &&
                    baseTypeNameFilter.Contains(c.BaseTypes[0].BaseType) &&
                    !c.HasAnyRequiredMembers()
                    )
                .Select(c => new MemberTypeTuple
                {
                    BaseMemberType = decendantsAtNextLevel.Where(d => d.CodeDecl.Name == c.BaseTypes[0].BaseType).Select(d => d.BaseMemberType).Single(),
                    CodeDecl = c
                }).ToList();
            accumulatedTupleList.AddRange(decendantsAtNextLevel);
        } while (decendantsAtNextLevel.Count() > 0);

        // Dont generate implicit assignment for abstract types at lowest level. Remove from list.
        foreach (var lowLevelAbstractTuple in accumulatedTupleList.Where(t => t.CodeDecl.GetQualifiedName().Namespace == Constants.CoreComponentTypeSchemaModuleTargetNamespace).ToList())
        {
            accumulatedTupleList.Remove(lowLevelAbstractTuple);
        }

        foreach (var tuple in accumulatedTupleList)
        {
            AddStaticImplicitAssignmentOperators(tuple.CodeDecl, tuple.BaseMemberType, "Value");
        }
    }


    /// <summary>
    /// 0 = string.IsNullOrEmpty(value) || value == default(type)
    /// 1 = ubltype
    /// 2 = clrtype
    /// 3 = property name ("Value")
    /// </summary>
    const string implicitAssignCodeStringFormat =
@"#if USE_IMPLICIT_ASSIGNMENT
        public static implicit operator {1}({2} value)
        {{
             return {0}new {1} {{ {3} = value }};
        }}

        public static implicit operator {2}({1} @this)
        {{
             return @this.{3};
        }}
#endif";

    private static void AddStaticImplicitAssignmentOperators(CodeTypeDeclaration codeDecl, Type parameterType, string propName)
    {
        string nullTest = parameterType == typeof(string) ? "string.IsNullOrEmpty(value) ? null : " : "";
        string snipCodeString = string.Format(implicitAssignCodeStringFormat, nullTest, codeDecl.Name, parameterType.FullName, propName);
        CodeSnippetTypeMember codeSnippet = new CodeSnippetTypeMember(snipCodeString);
        codeDecl.Members.Add(codeSnippet);
    }
}


/// <summary>
/// Change to: Give a list of possible cs filesnamespaces to generate. The ones that dont have any codeDelcs just save empty files.
/// Must join some files that have the same namespace prefix, like Xades.
/// What "usings" to add can also be an issue. cbcOptimized vs not.
/// Why not use just Common and Maindoc namespaces???
/// old comment:
/// What in short: given a xmlnamespace, should return a csharp namespace with a list of using directives. 
/// More or less the topmost part of a csharp code file.
/// </summary>
class UblNamespaceManager
{
    private readonly string nsHeaderComment = @"------------------------------------------------------------------------------
 <auto-generated>
     This code was generated by a tool.
     
     Changes to this file may cause incorrect behavior and will be lost if
     the code is regenerated.

     https://github.com/Gammern/ubllarsen
     {0}
 </auto-generated>
------------------------------------------------------------------------------";
    Dictionary<string, string[]> codeNamespaceUsings;
    // Hardcoded C# using statement resolver 
    Dictionary<string, string[]> codeNamespaceUsingsNonOptimized = new Dictionary<string, string[]>
    {
        [""] = new[] { "Cbc", "Cac", "Ext" },
        ["Cbc"] = new[] { "Udt" },
        ["Cac"] = new[] { "Udt", "Qdt", "Cbc" },
        ["Ext"] = new[] { "Udt", "Cbc" },
        ["Qdt"] = new[] { "Udt" },
        ["Udt"] = new[] { "Sbc", "Ext", "Cctscct", "Cbc" },
        ["Sbc"] = new[] { "Udt" }, // recursion
        ["Cctscct"] = new[] { "Udt", "Sbc", "Ext", "Cbc" },
        ["Abs"] = new[] { "Udt", "Ext", "Cbc" }, // Cbc for basedoc
        ["Xades"] = new[] { "DS" },
        ["Sac"] = new[] { "Udt", "Sbc", "DS" },
        ["Csc"] = new[] { "Sac" }
    };

    Dictionary<string, string[]> codeNamespaceUsingsOptimized = new Dictionary<string, string[]>
    {
        [""] = new[] { "Cac", "Udt" },
        ["Cac"] = new[] { "Udt" },
        ["Ext"] = new[] { "Udt" },
        ["Udt"] = new[] { "Sbc", "Ext", "Cctscct" },
        ["Sbc"] = new[] { "Udt" },
        ["Cctscct"] = new[] { "Udt", "Sbc", "Ext" },
        ["Abs"] = new[] { "Udt", "Ext" }, // Hack for basedoc
        ["Xades"] = new[] { "DS" },
        ["Sac"] = new[] { "Udt", "Sbc", "DS" },
        ["Csc"] = new[] { "Sac" }
    };

    Dictionary<string, string> xml2CSharpNamespaceDictionary;
    private XmlSchemaSet schemaSet;
    private string csDefaultNamespace;
    private bool OptionOptimizeCommonBasicComponents;
    static string[] unwantedPrefixes = new string[] { "", "xsd", "abs", "cct" }; //,"ccts-cct" ,"ds", "xades" };


    /// <summary>
    /// </summary>
    public UblNamespaceManager(XmlSchemaSet schemaSet, string csDefaultNamespace, bool optimizeCommonBasicComponents)
    {
        this.schemaSet = schemaSet;
        this.csDefaultNamespace = csDefaultNamespace;
        this.OptionOptimizeCommonBasicComponents = optimizeCommonBasicComponents;

        // Build a xml namespace(key) to csharp codenamespace/scope(value) dictionary by looking at all schema root xmlns attributes
        // Will bomb out on Distinct() if schemas use different namespace prefixes for the same namespace (empty ones are removed)
        xml2CSharpNamespaceDictionary = schemaSet.Schemas().Cast<XmlSchema>()
            .SelectMany(schema => schema.Namespaces.ToArray().Where(qname => !unwantedPrefixes.Contains(qname.Name)))
            .Select(qname => new { qname.Namespace, qname.Name })
            .Distinct()
            .ToDictionary(key => key.Namespace, val => $"{csDefaultNamespace}.{CodeIdentifier.MakePascal(val.Name)}");

        // missing references in 2.1. Is it unused?
        xml2CSharpNamespaceDictionary.Add(Constants.CommonSignatureComponentsTargetNamespace, $"{csDefaultNamespace}.Csc");
        xml2CSharpNamespaceDictionary[Constants.Xadesv132TargetNamespace] = $"{csDefaultNamespace}.Xades";
        xml2CSharpNamespaceDictionary[Constants.Xadesv141TargetNamespace] = $"{csDefaultNamespace}.Xades";// 141"; // Probably incorrect

        // add key:xmlns value:cSharpScope for all maindocs. They all point to the same scope value string
        foreach (XmlSchema schema in schemaSet.MaindocSchemas().Cast<XmlSchema>())
        {
            string targetNamespace = schema.TargetNamespace;
            if (!xml2CSharpNamespaceDictionary.ContainsKey(targetNamespace))
            {
                xml2CSharpNamespaceDictionary.Add(targetNamespace, csDefaultNamespace);
            }
        }

        // using directives in csharp files may vary depending on optimising of types
        if (this.OptionOptimizeCommonBasicComponents)
        {
            codeNamespaceUsings = codeNamespaceUsingsOptimized;
        }
        else
        {
            codeNamespaceUsings = codeNamespaceUsingsNonOptimized;
        }
        // prepend dictionary keys with default C# code namespace separated by a dot.
        codeNamespaceUsings = codeNamespaceUsings.ToDictionary(k => csDefaultNamespace + (k.Key == "" ? "" : ".") + k.Key, v => v.Value);
    }

    public CodeNamespace GetCodeNamespaceForXmltargetNamespace(string xmlNamespace)
    {
        if (xml2CSharpNamespaceDictionary.ContainsKey(xmlNamespace))
        {
            string csScopeName = xml2CSharpNamespaceDictionary[xmlNamespace];
            CodeNamespace codeNs = new CodeNamespace(csScopeName);
            string commentLine = GetCommentTextForScope(csScopeName);
            CodeComment headerComment = new CodeComment(string.Format(nsHeaderComment, commentLine));
            CodeCommentStatement fileHeader = new CodeCommentStatement(headerComment);
            codeNs.Comments.Add(fileHeader);

            // HACK! Do a hardcore swap if it is basedoc
            if (xmlNamespace.Equals(Constants.BaseDocumentTargetNamespace))
            {
                csScopeName += ".Abs";
            }

            // figure out what using statements to add
            if (codeNamespaceUsings.ContainsKey(csScopeName))
            {
                foreach (string usingNamespace in codeNamespaceUsings[csScopeName])
                {
                    codeNs.Imports.Add(new CodeNamespaceImport(usingNamespace));
                }
            }
            return codeNs;
        }
        else
        {
            //throw new ApplicationException(string.Format("Don't know how to handle xml namespace {0}", xmlNamespace));
            Console.WriteLine($"Don't know how to handle xml namespace {xmlNamespace}");
            return new CodeNamespace("BogusDontKnowHowToHandle");
        }
    }

    private string GetCommentTextForScope(string csScopeName)
    {
        string res = string.Empty;
        if (csScopeName.EndsWith(".Cbc"))
        {
            res = " UBL BBIEs (Basic Business Information Entities) are the leaf nodes of every UBL document structure.";
            if (this.OptionOptimizeCommonBasicComponents)
            {
                res = res + Environment.NewLine + " Types in this scope has been optimized/replaced by types from Udt."
                    + Environment.NewLine + " Members of maindocs streamed under Cbc namespace will in fact be Udt types.";
            }
            else
            {
                res = res + Environment.NewLine + " Yagni-types in this scope do not have any documentation present in xsd files.";
            }
        }
        else if (csScopeName.EndsWith(".Cac"))
        {
            res = " UBL ASBIEs (Association Business Information Entities) are substructures of an UBL document.";
        }
        else if (csScopeName.EndsWith(".Cctscct"))
        {
            res = " Types at the lowest level have been made abstract and prefixed with \"cctscct\" to avoid naming conflicts.";
        }
        else if (csScopeName.EndsWith(".Qdt"))
        {
            res = " --no qualified data types defined at this time--";
        }
        if (res != string.Empty) res = Environment.NewLine + res + Environment.NewLine;
        return res;
    }

    /// <summary>
    /// Return a list of all schemas for the purpose of generating matching c# files. 
    /// Some schemas will not contain any CodeTypeDeclaration/generated code, and hence, c# file will be empty with a header only.
    /// </summary>
    public IEnumerable<XmlSchema> Schemas
    {
        get
        {
            return this.schemaSet.Schemas().Cast<XmlSchema>();
        }
    }
}


public class UblSchemaInheritanceTools
{
    /// <summary>
    /// Find common elements amongst all maindocSchemas and put them in a new abstractBaseSchema.
    /// Modify maindocsSchemas to inherit from abstractBaseSchema and remove elements that now go
    /// into abstractBaseSchema.
    /// </summary>
    /// <param name="maindocSchemas"></param>
    /// <returns>Base schema</returns>
    public static XmlSchema ModifyMaindocSchemasForInheritance(ICollection<XmlSchema> maindocSchemas)
    {
        int sharedElementCount = GetSharedElementCount(maindocSchemas);
        if (0 == sharedElementCount)
        {
            throw new ApplicationException("Maindoc schemas do not seem to have any shared elements. Inheritance from a baseclass pointless."
                + "Have you mixed-up xsdfiles in common and maindoc folders?");
        }

        // Construct new abstract base from an arbitrary maindoc schema (use it as template). Just pick the first one and pray
        XmlSchema templateSchema = maindocSchemas.First();
        XmlSchema abstractBaseSchema = CreateAbstractBaseSchemaFromMaindocSchema(templateSchema, sharedElementCount);

        XmlSchemaImport abstactBaseSchemaImport = new XmlSchemaImport { Namespace = abstractBaseSchema.TargetNamespace, Schema = abstractBaseSchema };
        XmlQualifiedName abstactBaseSchemaQNameToInheritFrom = new XmlQualifiedName(Constants.abstractBaseSchemaComplexTypeName, abstractBaseSchema.TargetNamespace);

        foreach (XmlSchema maindocSchema in maindocSchemas)
        {
            maindocSchema.Namespaces.Add("abs", abstractBaseSchema.TargetNamespace);
            maindocSchema.Includes.Add(abstactBaseSchemaImport);

            XmlSchemaComplexType maindocSchemaComplexType = maindocSchema.Items.OfType<XmlSchemaComplexType>().Single(); // Single is safe. Should only be one.
            XmlSchemaSequence nonSharedElementSequence = maindocSchemaComplexType.Particle as XmlSchemaSequence;
            //maindocSchemaComplexType.Particle = null; // do I have to do this? Has no effect

            // remove shared elements (they are now in the base we inherit from)
            for (int i = 0; i < sharedElementCount; i++) nonSharedElementSequence.Items.RemoveAt(0);

            maindocSchemaComplexType.ContentModel = new XmlSchemaComplexContent
            {
                Content = new XmlSchemaComplexContentExtension
                {
                    BaseTypeName = abstactBaseSchemaQNameToInheritFrom,
                    Particle = nonSharedElementSequence
                }
            };
        }

        return abstractBaseSchema;
    }

    /// <summary>
    ///  TODO: Create new XmlSchema instead of DeepCopy
    /// </summary>
    /// <param name="templateSchema">Arbitrary maindoc schema used as a template for our new base class schema</param>
    /// <param name="sharedElementCount">Number of elements that shoud be copied over to the new base class schema complex type</param>
    /// <returns></returns>
    private static XmlSchema CreateAbstractBaseSchemaFromMaindocSchema(XmlSchema templateSchema, int sharedElementCount)
    {
        XmlSchema abstractBaseSchema = DeepCopy(templateSchema);
        var abstractBaseElement = abstractBaseSchema.Items.OfType<XmlSchemaElement>().Single();          // Single. There can only be one
        var abstractBaseComplexType = abstractBaseSchema.Items.OfType<XmlSchemaComplexType>().Single();  // Christopher Lambert again

        // overwrite template props
        abstractBaseSchema.TargetNamespace = abstractBaseSchema.TargetNamespace.Replace(abstractBaseElement.Name, Constants.abstractBaseSchemaName);
        abstractBaseSchema.Namespaces.Add("", abstractBaseSchema.TargetNamespace);
        abstractBaseSchema.SourceUri = templateSchema.SourceUri.Replace(abstractBaseElement.Name, Constants.abstractBaseSchemaName);

        abstractBaseComplexType.IsAbstract = true;
        abstractBaseComplexType.Annotation.Items.Clear();
        XmlSchemaDocumentation doc = new XmlSchemaDocumentation();
        var nodeCreaterDoc = new XmlDocument();
        doc.Markup = new XmlNode[] { nodeCreaterDoc.CreateTextNode("This is a custom generated class that holds all the props/fields common to all UBL maindocs."),
                                         nodeCreaterDoc.CreateTextNode("You won't find a matching xsd file where it originates from.") };
        abstractBaseComplexType.Annotation.Items.Add(doc);
        abstractBaseComplexType.Name = Constants.abstractBaseSchemaComplexTypeName;

        // remove non-shared tailing elements.
        XmlSchemaObjectCollection elementCollection = (abstractBaseComplexType.Particle as XmlSchemaSequence).Items;
        while (sharedElementCount < elementCollection.Count) elementCollection.RemoveAt(sharedElementCount);

        abstractBaseElement.Name = Constants.abstarctBaseSchemaElementName;
        abstractBaseElement.SchemaTypeName = new XmlQualifiedName(Constants.abstractBaseSchemaComplexTypeName, abstractBaseSchema.TargetNamespace);

        // Don't need schemaLocation for loaded schemas. Will generate schemasetcompile warnings if not removed
        foreach (var baseSchemaImports in abstractBaseSchema.Includes.OfType<XmlSchemaImport>()) baseSchemaImports.SchemaLocation = null;

        return abstractBaseSchema;
    }

    /// <summary>
    /// Maintain an ever shinking array of common elements(acc) as we aggLINQerate through each maindoc schema elementlist
    /// Both position and QualifiedName must match: "s.QualifiedName == next[i].QualifiedName"
    /// Start with the maindoc complextype with fewest elements first, hence the OrderBy
    /// Note: will not work once we have added abstactBaseSchema to the schemaset in main()
    /// </summary>
    /// <returns>Number of elements shared by all maindocs</returns>
    private static int GetSharedElementCount(ICollection<XmlSchema> maindocSchemas)
    {
        return maindocSchemas
            .Select(s => s.Items.OfType<XmlSchemaComplexType>().Single())
            .Select(c => (c.ContentTypeParticle as XmlSchemaSequence).Items.Cast<XmlSchemaElement>().ToArray())
            .OrderBy(c => c.Length)
            .Aggregate((acc, next) => acc.AsEnumerable().TakeWhile((s, i) => s.QualifiedName == next[i].QualifiedName).ToArray())
            .Count();
    }

    private static XmlSchema DeepCopy(XmlSchema sourceSchema)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            sourceSchema.Write(stream);
            stream.Position = 0;
            return XmlSchema.Read(stream, null);
        }
    }

}

class UblSchemaStatsTool
{
    Dictionary<string, string> prefixLookup;

    public static void ShowHiearachy(XmlSchemaSet schemaSet)
    {
        UblSchemaStatsTool tool = new UblSchemaStatsTool();
        using (var file = File.CreateText("../../hierarchy.txt"))
        {
            tool.ShowHierarchy(schemaSet, file);// Console.Out);
        }
    }

    private void ShowHierarchy(XmlSchemaSet schemaSet, TextWriter writer)
    {
        var schema = schemaSet.Schemas(Constants.InvoiceTargetNamespace).Cast<XmlSchema>().Single();
        prefixLookup = schema.Namespaces.ToArray().Where(v => !string.IsNullOrEmpty(v.Name)).ToDictionary(k => k.Namespace, v => v.Name);
        PrintSchemaDetail(schema, writer);
        var prefixes = schemaSet.GetNamespacePrefixes();
        Console.WriteLine();
        foreach (var prefix in prefixes)
        {
            writer.WriteLine($"{prefix.Name,9}: -> \"{prefix.Namespace}\"");
        }
    }

    private void PrintSchemaDetail(XmlSchema schema, TextWriter writer, int level = 0)
    {
        Func<string, string> lastpart = s => (s.Substring(s.Substring(0, s.Length - 2).LastIndexOf(':') + 1));
        string tab = new string(' ', level * 2);
        var newPrefixes = schema.Namespaces.ToArray().Where(v => !string.IsNullOrEmpty(v.Name)).ToDictionary(k => k.Namespace, v => v.Name);
        foreach (var item in newPrefixes)
        {
            prefixLookup[item.Key] = item.Value;
        }
        string prefix = prefixLookup.ContainsKey(schema.TargetNamespace) ? prefixLookup[schema.TargetNamespace] + ":" : "";
        string org = (schema.TargetNamespace.Contains("oasis") ? "oasis" : schema.TargetNamespace.Contains("unece") ? "unece" : "");
        string name = Path.GetFileName(schema.GetFileNameWithSubDirectory()) + " \t" + lastpart(schema.TargetNamespace);

        writer.WriteLine($"{tab}{name} ({org} {prefix})");

        var imports = schema.Includes.Cast<XmlSchemaExternal>().ToList();
        foreach (var import in imports)
        {
            string s = import.GetType().Name.Substring("XmlSchema".Length);
            writer.Write($"{s}");
            PrintSchemaDetail(import.Schema, writer, level + 1);
        }
    }

    public static void ShowStats(IEnumerable<CodeTypeDeclaration> types)
    {
        int totalTypes = types.Count();
        int countClass = types.Where(t => t.IsClass).Count();
        int countEnum = types.Where(t => t.IsEnum).Count();
        int countStruct = types.Where(t => t.IsStruct).Count();

        Console.WriteLine("Stats:");
        Console.WriteLine($"Classes: {countClass}");
        Console.WriteLine($"  Enums: {countEnum}");
        Console.WriteLine($"Structs: {countStruct}");
        Console.WriteLine($"  Total: {totalTypes}");

        // You Ain't Gonna Need It
        var yagnis = types.Where(c => c.IsClass && c.BaseTypes.Count == 1 &&
                c.Members.Cast<CodeTypeMember>().Where(d => d.GetType() == typeof(CodeMemberField) || d.GetType() == typeof(CodeMemberProperty)).Count() == 0)
            .Count();
        Console.WriteLine($"Yagni classes:\t {yagnis}");
    }

}

internal class UblSchemaTypeSimplificationTool
{
    /// <summary>
    /// Complextypes in CommonBasicComponents are just pointers to other types. Removes that indirection.
    ///
    /// E.g:
    ///    <xsd:element name="AcceptedIndicator" type="AcceptedIndicatorType"/>
    ///    <xsd:complexType name="AcceptedIndicatorType">
    ///        <xsd:simpleContent>
    ///            <xsd:extension base="udt:IndicatorType"/>
    ///        </xsd:simpleContent>
    ///    </xsd:complexType>
    ///
    /// Becomes:
    ///    <xsd:element name="AcceptedIndicator" type="udt:IndicatorType"/>
    ///    
    /// ubl 2.1: Number of generated C# classes are reduced from 1202 to 329!
    /// </summary>
    public static void SimplifyCommonBasicComponentsTypes(XmlSchema schema)
    {
        // 
        var typeLookup = schema.Items.OfType<XmlSchemaComplexType>()
            .ToDictionary(k => k.QualifiedName, v => (v.ContentModel.Content as XmlSchemaSimpleContentExtension).BaseTypeName);
        var repaceElements = schema.Items.OfType<XmlSchemaElement>()
            .Select(e => new XmlSchemaElement { Name = e.Name, SchemaTypeName = typeLookup[e.SchemaTypeName] }).ToList();
        schema.Items.Clear();
        repaceElements.ForEach(r => schema.Items.Add(r));
    }

    // .NET C# cannot have types with same name in the Codenamespace. If that happens a digit is appended to the typename.
    // Eg. CoreComponents and UnqualifiedDataTypes both have "IdentifierType", one gets named "IdentifierType1" :-(
    // Solution: Prepend types in CoreComponents with "cctscct" and modify references in UnqualifiedComponents
    public static void ResolveTypeNameClashesByRenaming(XmlSchemaSet schemaSet)
    {
        string ccts_cctPrefix = "cctscct";
        XmlSchema coreCompSchema = schemaSet.Schemas(Constants.CoreComponentTypeSchemaModuleTargetNamespace).OfType<XmlSchema>().Single();
        XmlSchema unqualSchema = schemaSet.Schemas(Constants.UnqualifiedDataTypesTargetNamespace).OfType<XmlSchema>().Single();

        foreach (var complexType in coreCompSchema.Items.OfType<XmlSchemaComplexType>())
        {
            complexType.Name = ccts_cctPrefix + complexType.Name;
            complexType.IsAbstract = true; // Make it abstract as well. Ain't gonna use the base, only the one derived from this type.
        }

        foreach (var complexType in unqualSchema.Items.OfType<XmlSchemaComplexType>()
                        .Where(t => t.BaseXmlSchemaType.QualifiedName.Namespace.Equals(Constants.CoreComponentTypeSchemaModuleTargetNamespace)))
        {
            var name = new XmlQualifiedName(ccts_cctPrefix + complexType.BaseXmlSchemaType.QualifiedName.Name, complexType.BaseXmlSchemaType.QualifiedName.Namespace);
            var content = complexType.ContentModel as XmlSchemaSimpleContent;
            if (content.Content is XmlSchemaSimpleContentRestriction)
            {
                (content.Content as XmlSchemaSimpleContentRestriction).BaseTypeName = name;
            }
            else if (content.Content is XmlSchemaSimpleContentExtension)
            {
                (content.Content as XmlSchemaSimpleContentExtension).BaseTypeName = name;
            }
        }
        schemaSet.Reprocess(coreCompSchema);
        schemaSet.Reprocess(unqualSchema);
    }
}

internal class UblXmlSchemaOrderComparer : IComparer<XmlSchema>
{
    // Returns:
    //     A signed integer that indicates the relative values of x and y, as shown in the
    //     following table.Value Meaning 
    //      Less than zero -> x is less than y.
    //      Zero -> x equals y
    //      Greater than zero -> x is greater than y
    //  -1: x comes before y
    //  0: order of x and y don't matter
    //  1 : x comes after y
    public int Compare(XmlSchema x, XmlSchema y)
    {
        if (IncludesContain(x.Includes, y))
        {
            return 1;
        }
        else if (IncludesContain(y.Includes, x))
        {
            return -1;
        }
        else
        {
            return 0; // x.TargetNamespace.Contains("BaseDocument") || y.TargetNamespace.Contains("BaseDocument")
        }
    }

    // recurse includes until we find y
    private bool IncludesContain(XmlSchemaObjectCollection includes, XmlSchema y)
    {
        bool res = false;
        foreach (XmlSchemaExternal include in includes)
        {
            if (include.Schema == y)
            {
                return true;
            }
            if (include.Schema != null)
            {
                foreach (XmlSchemaExternal item in include.Schema.Includes)
                {
                    if (item.Schema != null)
                        res = IncludesContain(item.Schema.Includes, y);
                    if (res) return res;
                }
            }
        }

        return res;
    }
}

public class UblXsdSettings
{
    DirectoryInfo baseDir;
    DirectoryInfo commonDir;
    DirectoryInfo maindocDir;

    /// <summary>
    /// Parent of common and maindoc directories from the downloaded/unziped ubl package
    /// </summary>
    public string UblXsdInputPath
    {
        set
        {
            this.baseDir = new DirectoryInfo(value);
            try
            {
                this.commonDir = baseDir.GetDirectories("common")[0];
                this.maindocDir = baseDir.GetDirectories("maindoc")[0];
            }
            catch
            {
                throw new ArgumentException($"{nameof(this.UblXsdInputPath)} must point to xsd folder that have common and maindoc subfolders.", nameof(this.UblXsdInputPath));
            }
            if (this.commonDir.GetFiles("*.xsd").Length == 0 || this.maindocDir.GetFiles("*.xsd").Length == 0)
                throw new ArgumentException($"{nameof(UblXsdInputPath)} Can't find *.xsd files in common and maindoc subfolders.", nameof(this.UblXsdInputPath));
        }
    }

    public enum FieldTypesEnum { Field, Property, AutoProperty };

    public FieldTypesEnum OptionMemberTypeToGenerate { get; set; }

    public DirectoryInfo MaindocDirectory => maindocDir;

    public DirectoryInfo CommonDirectory => commonDir;

    public ValidationEventHandler XsdValidationEvent { get; set; }

    /// <summary>
    /// Generated codefiles are written to this path
    /// </summary>
    public string CodeGenOutputPath { get; set; }

    /// <summary>
    /// Default code namespace used in generated code
    /// </summary>
    public string CSharpDefaultNamespace { get; set; }

    public bool OptionOptimizeCommonBasicComponents { get; internal set; }
}

public static class XsdTimeTool
{
    public static void IgnoreDateTimeSerializeAsString(CodeTypeDeclaration codeDeclTimeType)
    {
        // Remove "time" member codeattributes and add XmlIgnore
        var valueMember = codeDeclTimeType.Members.Cast<CodeTypeMember>().Where(m => m.Name == "Value").Single();
        valueMember.CustomAttributes.Clear();
        valueMember.CustomAttributes.Add(new CodeAttributeDeclaration("System.Xml.Serialization.XmlIgnore"));

        string commentLine = "New serialized string type is declared in file UBL-UnqualifiedDataTypes-2.1.partial.cs";
        CodeCommentStatement memberCommentStatement = new CodeCommentStatement(commentLine, false);
        valueMember.Comments.Add(memberCommentStatement);

        // Modify class attributes. Make it possible to step into partial class with debugger
        var debuggerStepThroughAttribute = codeDeclTimeType.CustomAttributes.OfType<CodeAttributeDeclaration>()
            .Where(a => a.Name == "System.Diagnostics.DebuggerStepThroughAttribute").SingleOrDefault();
        if (debuggerStepThroughAttribute != null)
        {
            codeDeclTimeType.CustomAttributes.Remove(debuggerStepThroughAttribute);
        }
    }
}