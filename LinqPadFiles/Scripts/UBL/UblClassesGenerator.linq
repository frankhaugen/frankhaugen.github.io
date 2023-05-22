<Query Kind="Statements">
  <NuGetReference>Microsoft.CodeDom.Providers.DotNetCompilerPlatform</NuGetReference>
  <NuGetReference>UblSharp.Generator.Core</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Xml.Schema</Namespace>
  <Namespace>UblSharp.Generator</Namespace>
  <Namespace>UblSharp.Generator.CodeFixers</Namespace>
  <Namespace>UblSharp.Generator.ConditionalFeatures</Namespace>
  <Namespace>UblSharp.Generator.Extensions</Namespace>
  <Namespace>UblSharp.Generator.Internal</Namespace>
  <Namespace>UblSharp.Generator.XsdFixers</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>


var walker = new XsdWalker();

var xsdPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "xsd/maindoc");

var schemaFiles = Directory.GetFiles(xsdPath, "*.xsd");
var schemas = new List<XmlSchema>();


foreach (var schemaFile in schemaFiles)
{
    var xmlReaderSettings = new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse };
    using var reader = XmlReader.Create(new FileInfo(schemaFile).OpenRead(), xmlReaderSettings);
    var schema = XmlSchema.Read(reader, (s, e) => throw new Exception($"Error reading schema file {schemaFile}: [{e.Severity}] {e.Message}"));
    
    //schemas.Add(schema);
    walker.Walk(schema).Dump();
}










//var generator = new UblGenerator();
//
//generator.Generate(
//    new UblGeneratorOptions()
//    {
//        XsdBasePath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "xsd/maindoc"),
//        OutputPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "output"),
//        Namespace = "Frank.Ubl",
//        ValidationHandler = ValidationHandler,
//        GenerateCommonFiles = true
//    });
//
//
static void ValidationHandler(object? sender, ValidationEventArgs e)
{
    e.Dump(e.Severity.ToString());
}

public class XsdWalker
{
    private StringBuilder? _stringBuilder;
    
    public string Walk(XmlSchema schema)
    {
        _stringBuilder = new StringBuilder();
        
        foreach (var item in schema.Elements.Values)
        {
            switch (item)
            {

                case XmlSchemaSimpleType simpleType:
                    Walk(simpleType);
                    break;
                case XmlSchemaElement element:
                    Walk(element);
                    break;
                case XmlSchemaComplexType complexType:
                    Walk(complexType);
                    break;
                default:
                    throw new NotSupportedException("Unsupported element type.");
            }
        }

        return _stringBuilder.ToString();
    }

    private void Walk(XmlSchemaElement element)
    {
        if (element.SchemaType is XmlSchemaComplexType complexType)
        {
            Walk(complexType);
        }
        else if (element.SchemaType is XmlSchemaSimpleType simpleType)
        {
            Walk(simpleType);
        }
    }

    private void Walk(XmlSchemaComplexType complexType)
    {

        if (complexType.Attributes.Count > 0)
        {
            _stringBuilder.AppendLine("using System;");
            _stringBuilder.AppendLine("using System.Xml.Serialization;");
            _stringBuilder.AppendLine();

        }
        _stringBuilder.AppendLine($"namespace {complexType.QualifiedName.Namespace};");

        foreach (var attribute in complexType.Attributes)
        {
            
            _stringBuilder.AppendLine($"    [XmlAttribute(\"{attribute}\")]");
        }
        _stringBuilder.AppendLine($"public class {complexType.Name}");
        _stringBuilder.AppendLine("{");

            switch (complexType.Particle)
            {
                case XmlSchemaElement element:
                    Walk(element);
                    break;
                case XmlSchemaSequence sequence:
                    Walk(sequence);
                    break;
                case XmlSchemaChoice choice:
                    Walk(choice);
                    break;
                case XmlSchemaAny any:
                    Walk(any);
                    break;
                default:
                    throw new NotSupportedException("Unsupported particle type.");
            }
            complexType.Dump();
        
        _stringBuilder.AppendLine("}");
    }

    private void Walk(XmlSchemaAny any)
    {
        _stringBuilder.AppendLine($"    [XmlAnyElement]");
        _stringBuilder.AppendLine($"    public XmlElement[] {any.Id} {{ get; set; }}");
    }

    private void Walk(XmlSchemaChoice choiceInput)
    {
        foreach (var item in choiceInput.Items)
        {
            switch(item)
            {
                case XmlSchemaElement element:
                    Walk(element);
                    break;
                case XmlSchemaChoice choice:
                    Walk(choice);
                    break;
                case XmlSchemaSequence sequence:
                    Walk(sequence);
                    break;
                case XmlSchemaAny any:
                    Walk(any);
                    break;
                default:
                    throw new NotSupportedException("Unsupported particle type.");
            }
        }
    }

    private void Walk(XmlSchemaSequence sequenceInput)
    {
        foreach (var item in sequenceInput.Items)
        {
            switch (item)
            {
                case XmlSchemaElement element:
                    Walk(element);
                    break;
                case XmlSchemaChoice choice:
                    Walk(choice);
                    break;
                case XmlSchemaSequence sequence:
                    Walk(sequence);
                    break;
                case XmlSchemaAny any:
                    Walk(any);
                    break;
                default:
                    throw new NotSupportedException("Unsupported particle type.");
            }
        }
    }

    private void Walk(XmlSchemaSimpleType simpleType)
    {
        _stringBuilder.AppendLine($"public {simpleType.Name} {simpleType.Name} {{ get; set; }}");
    }
}