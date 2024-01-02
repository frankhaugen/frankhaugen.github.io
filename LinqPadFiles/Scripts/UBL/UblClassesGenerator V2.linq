<Query Kind="Statements">
  <NuGetReference>XmlSchemaClassGenerator-beta</NuGetReference>
  <Namespace>XmlSchemaClassGenerator</Namespace>
  <Namespace>XmlSchemaClassGenerator.NamingProviders</Namespace>
</Query>

var inputFolder = @"D:\repos\Frank.Libraries\src\Frank.Libraries.Ubl\Frank.Libraries.Ubl\xsd";
var outputFolder = "D:/temp/generated";

var files = new List<string>();

var generator = new Generator
{
	OutputFolder = outputFolder,
	Log = s => files.Add(s),
	GenerateNullables = true,
	SeparateNamespaceHierarchy = true,
	CollectionImplementationType = typeof(List<>),
	CollectionType = typeof(List<>),
	DoNotForceIsNullable = false,
	EmitOrder = false,
	NamespaceProvider = new NamespaceProvider()
	{
		GenerateNamespace = x => {
			var output = "Ubl";
			var namespaceRaw = x.XmlSchemaNamespace;
			
			return output;
		}
	},
	GenerateInterfaces = true,
	NetCoreSpecificCode = true,
	UseXElementForAny = true,
	SeparateClasses = false,
	EntityFramework = true,
	EnumAsString = false,
	EnableNullableReferenceAttributes = true,
	CodeTypeReferenceOptions = System.CodeDom.CodeTypeReferenceOptions.GenericTypeParameter
};

var directory = new DirectoryInfo(inputFolder);

generator.Generate(directory.EnumerateFiles("*.xsd", SearchOption.AllDirectories).Select(x => x.FullName));

var fileInfos = files.Select(x => new FileInfo(x)).OrderBy(x => x.Name);

var content = new StringBuilder();
foreach (var fileInfo in fileInfos)
{
	content.AppendLine(File.ReadAllText(fileInfo.FullName));
}
var outFile = Path.Combine(outputFolder, "Output.cs");

File.WriteAllText(outFile, content.ToString());

content.ToString().Dump();