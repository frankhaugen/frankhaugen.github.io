<Query Kind="Statements">
  <NuGetReference>UblSharp.Generator.Core</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>UblSharp.Generator</Namespace>
  <Namespace>UblSharp.Generator.CodeFixers</Namespace>
  <Namespace>UblSharp.Generator.ConditionalFeatures</Namespace>
  <Namespace>UblSharp.Generator.Extensions</Namespace>
  <Namespace>UblSharp.Generator.Internal</Namespace>
  <Namespace>UblSharp.Generator.XsdFixers</Namespace>
  <Namespace>System.Xml.Schema</Namespace>
</Query>

var generator = new UblGenerator();
generator.Generate(
    new UblGeneratorOptions()
    {
        XsdBasePath = Path.Combine(Util.CurrentQueryPath, "xsd/maindoc"),
        OutputPath = Path.Combine(Util.CurrentQueryPath, "output"),
        Namespace = "Frank.Ubl",
        ValidationHandler = ValidationHandler,
        GenerateCommonFiles = true
    });


static void ValidationHandler(object sender, ValidationEventArgs e)
{
    //Console.WriteLine($"{e.Severity}: {e.Message}");
    if (e.Severity == XmlSeverityType.Error)
    {
        e.Dump(e.Severity.ToString());
    }
}