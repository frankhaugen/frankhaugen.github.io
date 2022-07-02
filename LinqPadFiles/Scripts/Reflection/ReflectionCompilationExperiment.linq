<Query Kind="Statements">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <NuGetReference>NSwag.CodeGeneration.CSharp</NuGetReference>
  <NuGetReference>System.CodeDom</NuGetReference>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>Microsoft.CSharp</Namespace>
  <Namespace>NJsonSchema.CodeGeneration.CSharp</Namespace>
  <Namespace>NSwag</Namespace>
  <Namespace>NSwag.CodeGeneration.CSharp</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.CodeAnalysis.Emit</Namespace>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
</Query>

var document = await OpenApiDocument.FromUrlAsync("https://esi.evetech.net/latest/swagger.json");
var @namespace = "EveOnlineApi";
var settings = new CSharpClientGeneratorSettings()
{
	ClassName = "Test",
	WrapResponses = false,
	GenerateDtoTypes = true,
	GenerateExceptionClasses = false,
	WrapDtoExceptions = false,
	GenerateClientClasses = false,
	GenerateOptionalParameters = true,
	CSharpGeneratorSettings =
			{
				Namespace = @namespace,
				ClassStyle = CSharpClassStyle.Record,
				ArrayType = "IList",
				JsonLibrary = CSharpJsonLibrary.SystemTextJson,
				GenerateNativeRecords = true,
				GenerateDefaultValues = false,
				GenerateNullableReferenceTypes = true,
				GenerateDataAnnotations = false
			}
};

var generator = new CSharpClientGenerator(document, settings);
var text = generator.GenerateFile();

var syntaxTree = CSharpSyntaxTree.ParseText(text);
var root = syntaxTree.GetRoot() as CompilationUnitSyntax;

MetadataReference[] references = new MetadataReference[]
{
	MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
	MetadataReference.CreateFromFile(typeof(IList<>).Assembly.Location),
	MetadataReference.CreateFromFile(typeof(ValueTuple).Assembly.Location),
	MetadataReference.CreateFromFile(typeof(System.Threading.Tasks.Task).Assembly.Location),
	MetadataReference.CreateFromFile(typeof(Type).Assembly.Location),
	MetadataReference.CreateFromFile(typeof(System.Runtime.JitInfo).Assembly.Location),
	MetadataReference.CreateFromFile(typeof(System.Runtime.CompilerServices.CallConvFastcall).Assembly.Location),
	MetadataReference.CreateFromFile(typeof(System.Attribute).Assembly.Location),
	MetadataReference.CreateFromFile(typeof(System.Collections.Generic.IEnumerable<>).Assembly.Location),
	MetadataReference.CreateFromFile(typeof(System.Runtime.Serialization.EnumMemberAttribute).Assembly.Location),
	MetadataReference.CreateFromFile(typeof(System.Text.Json.JsonDocument).Assembly.Location),
	MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
};

var parameters = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

var compilation = Microsoft.CodeAnalysis.CSharp.CSharpCompilation.Create("TestAssembly", new[] { syntaxTree }, references, parameters);

using (var ms = new MemoryStream())
{
	EmitResult result = compilation.Emit(ms);

	if (!result.Success)
	{
		IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
			diagnostic.IsWarningAsError ||
			diagnostic.Severity == DiagnosticSeverity.Error);

		foreach (Diagnostic diagnostic in failures)
		{
			Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
		}
	}
	else
	{
		ms.Seek(0, SeekOrigin.Begin);
		Assembly assembly = Assembly.Load(ms.ToArray());
		assembly.GetTypes().Select(x => x.Name).Dump();
	}
}



/*
var parameters = new CompilerParameters();
parameters.GenerateExecutable = false;
parameters.OutputAssembly = "Something.dll";

CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
var result = provider.CompileAssemblyFromSource(parameters, text);


//CSharpCodeProvider provider = new CSharpCodeProvider();
ICodeCompiler compiler = provider.CreateCompiler();
CompilerParameters compilerparams = new CompilerParameters();
compilerparams.GenerateExecutable = false;
CompilerResults results = compiler.CompileAssemblyFromSource(compilerparams, text);

results.CompiledAssembly.GetTypes().Select(x => x.Name).Dump();
*/