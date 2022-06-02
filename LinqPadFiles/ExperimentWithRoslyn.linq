<Query Kind="Statements">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <NuGetReference>NSwag.CodeGeneration.CSharp</NuGetReference>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>NSwag</Namespace>
  <Namespace>NSwag.CodeGeneration.CSharp</Namespace>
  <Namespace>NJsonSchema.CodeGeneration.CSharp</Namespace>
</Query>

var document = await OpenApiDocument.FromUrlAsync("https://esi.evetech.net/latest/swagger.json");
var @namespace = "ReplaceMe";
var settings = new CSharpClientGeneratorSettings()
{
	ClassName = "ApiBase",
	WrapResponses = false,
	GenerateDtoTypes = true,
	GenerateExceptionClasses = false,
	WrapDtoExceptions = false,
	GenerateClientClasses = false,
	GenerateOptionalParameters = false,
	CSharpGeneratorSettings =
			{
				Namespace = @namespace,
				ClassStyle = CSharpClassStyle.Poco,
				ArrayType = "List",
				JsonLibrary = CSharpJsonLibrary.SystemTextJson,
				GenerateNativeRecords = false,
				GenerateDefaultValues = false,
				GenerateNullableReferenceTypes = true,
				GenerateDataAnnotations = false,
			}
};

var outputDirectory = new DirectoryInfo(Path.Combine(@"C:\temp\Models", @namespace));
if (!outputDirectory.Exists)
{
	outputDirectory.Create();
}

var generator = new CSharpClientGenerator(document, settings);
var text = generator.GenerateFile();
//var text = File.ReadAllText(@"C:\temp\SwaggerGeneratedCode.txt");

var syntaxTree = CSharpSyntaxTree.ParseText(text);
var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
var namespaceSyntax = root.Members.OfType<NamespaceDeclarationSyntax>().First();
var classes = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>();
var records = namespaceSyntax.Members.OfType<RecordDeclarationSyntax>();
var enums = namespaceSyntax.Members.OfType<EnumDeclarationSyntax>();
	
//var classesDictionary = classes.ToDictionary(x => x.Identifier.ToString(), x => x);
//var recordsDictionary = records.ToDictionary(x => x.Identifier.ToString(), x => x);
//var enumsDictionary = enums.ToDictionary(x => x.Identifier.ToString(), x => x);

foreach (var element in classes)
{
	var outputPath = Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + ".cs");
	var outputFile = new FileInfo(outputPath);
	
	if (outputFile.Exists)
	{
		outputPath = Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + "X" + ".cs");
		outputFile = new FileInfo(outputPath);
	}
	
	if (outputFile.Exists) continue;

	Console.WriteLine($"{element.Identifier.ToString()}\t=>\t{outputFile.Name}");
	File.WriteAllText(outputFile.FullName, $"namespace {namespaceSyntax.Name};\n" + element.ToFullString().Replace(" partial ", " "));
}
foreach (var element in records)
{
	var outputPath = Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + ".cs");
	var outputFile = new FileInfo(outputPath);

	if (outputFile.Exists)
	{
		outputPath = Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + "X" + ".cs");
		outputFile = new FileInfo(outputPath);
	}

	if (outputFile.Exists) continue;

	Console.WriteLine($"{element.Identifier.ToString()}\t=>\t{outputFile.Name}");
	File.WriteAllText(outputFile.FullName, $"namespace {namespaceSyntax.Name};\n" + element.ToFullString().Replace(" partial ", " "));
}
foreach (var element in enums)
{
	var outputPath = Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + ".cs");
	var outputFile = new FileInfo(outputPath);

	if (outputFile.Exists)
	{
		outputPath = Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + "X" + ".cs");
		outputFile = new FileInfo(outputPath);
	}

	if (outputFile.Exists) continue;

	Console.WriteLine($"{element.Identifier.ToString()}\t=>\t{outputFile.Name}");
	File.WriteAllText(outputFile.FullName, $"namespace {namespaceSyntax.Name};\n" + element.ToFullString().Replace(" partial ", " "));
}