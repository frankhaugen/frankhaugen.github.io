<Query Kind="Statements">
  <NuGetReference>XmlSchemaClassGenerator-beta</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>XmlSchemaClassGenerator</Namespace>
</Query>



var generator = new Generator
{
    OutputFolder = "C:/temp/UBL-Gen",
    Log = s => s.Dump(),
    GenerateNullables = true,
    //NamespaceProvider = new Dictionary<NamespaceKey, string>
    //{
    //    { new NamespaceKey("http://wadl.dev.java.net/2009/02"), "Wadl" }
    //}
    //.ToNamespaceProvider(new GeneratorConfiguration { NamespacePrefix = "Wadl" }.NamespaceProvider.GenerateNamespace)
};

generator.Generate(new DirectoryInfo("C:/repos/ubllarsen/UblLarsen/UBL-2.1/xsd").EnumerateFiles("*.xsd", SearchOption.AllDirectories).Select(x => x.FullName));