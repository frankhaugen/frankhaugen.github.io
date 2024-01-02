<Query Kind="Statements">
  <NuGetReference>XmlSchemaClassGenerator-beta</NuGetReference>
  <Namespace>XmlSchemaClassGenerator</Namespace>
</Query>


var inputFolder = @"C:\repos\ubllarsen\UblLarsen\UBL-2.1\xsd";
var files = new DirectoryInfo(inputFolder).EnumerateFiles("*.xsd", SearchOption.AllDirectories);


var generator = new Generator
{
	SeparateClasses = true,
	OutputFolder = @"C:\temp\ubl",
	Log = s => Console.Out.WriteLine(s),
	GenerateNullables = true,
	NetCoreSpecificCode = true,
	EntityFramework = true,
	CollectionType = typeof(List<>),
	NamespaceProvider = new NamespaceProvider
	{
		GenerateNamespace = key => "UBL"
	}
};

generator.Generate(files.Select(x => x.FullName));