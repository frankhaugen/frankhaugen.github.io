<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Xml.Schema</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>



IEnumerable<XmlTypeMapping> GetTypeMappingsFromDirectory(DirectoryInfo directory)
{
    var schemaSet = ReadDirectory(directory);

    schemaSet.Compile();

    var namespaces = new List<XmlQualifiedName>();

    var schemas = SchemaSetToSchemas(schemaSet);

    schemas.Compile(HandleErrors, true);

    var mappings = new List<XmlTypeMapping>();

    foreach (var name in namespaces)
    {
        mappings.Add(GetMapping(schemas, name));
    }

    return mappings;
}

XmlTypeMapping GetMapping(XmlSchemas schemas, XmlQualifiedName name)
{
    var schemaImporter = new XmlSchemaImporter(schemas);
    
    var mapping = schemaImporter.ImportSchemaType(name);
    return mapping;
}

XmlSchemaSet ReadDirectory(DirectoryInfo directory)
{
    XmlSchemaSet schemaSet = new XmlSchemaSet();

    var xsds = directory.EnumerateFiles("*.xsd", SearchOption.AllDirectories);

    foreach (var xsd in xsds)
    {
        var xmlSchema = ReadFile(xsd);
        if (xmlSchema != null) schemaSet.Add(xmlSchema);
    }

    return schemaSet;
}

XmlSchema? ReadFile(FileInfo file)
{
    return XmlSchema.Read(new XmlTextReader(file.OpenRead()), (e, args) => HandleErrors(e, args));
}

XmlSchemas SchemaSetToSchemas(XmlSchemaSet schemaSet)
{
    var schemas = new XmlSchemas();
    foreach (XmlSchema schema in schemaSet.Schemas())
    {
        var cleanedSchema = CleanupSchemaNamespace(schema);
        schemas.Add(schema);
    }
    return schemas;
}

void HandleErrors(object? sender, ValidationEventArgs e)
{
    if (e.Severity == XmlSeverityType.Error)
    {
        e.Dump();
    }
}


XmlSchema CleanupSchemaNamespace(XmlSchema schema)
{
    return schema;
//
//    var cleanNamespace = CleanUpNamespace(schema.TargetNamespace);
//    schema.TargetNamespace = cleanNamespace;
//    return schema;
}

string CleanUpNamespace(string @namespace)
{
    var segments = @namespace.Split(':');
    var lastSegment = segments.Last();
    lastSegment = new string(lastSegment.Where(c => char.IsLetter(c)).ToArray());
    if (!lastSegment.Any())
    {
        lastSegment = new string(segments[^2].Where(c => char.IsLetter(c)).ToArray());
    }
    return lastSegment;
}

