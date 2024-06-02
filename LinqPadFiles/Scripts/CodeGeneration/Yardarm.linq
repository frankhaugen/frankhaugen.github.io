<Query Kind="Statements">
  <NuGetReference>Microsoft.OpenApi.Readers</NuGetReference>
  <NuGetReference>Yardarm</NuGetReference>
  <NuGetReference>Yardarm.SystemTextJson</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Yardarm</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
</Query>


var reader = new Microsoft.OpenApi.Readers.OpenApiStreamReader();
var document = reader.Read(Downloader.Download("https://api.MyEmployer.no/swagger/v1.0/swagger.json"), out _);
//var document = reader.Read(Downloader.Download("https://petstore.swagger.io/v2/swagger.json"), out _);
//var outputDirectory= new DirectoryInfo(@"C:\temp\");

//document.DumpCSharp(outputDirectory: outputDirectory);

//var settings = JsonSerializer.Deserialize<YardarmGenerationSettings>("{\"InputFile\":\"C:\\\\repos\\\\frankhaugen\\\\Yardarm\\\\src\\\\main\\\\output\\\\TestJson\\\\swagger.json\",\"Version\":\"1.0.0\",\"KeyFile\":null,\"KeyContainerName\":null,\"PublicSign\":false,\"EmbedAllSources\":false,\"NoRestore\":false,\"References\":[],\"OutputFile\":\"C:\\\\repos\\\\frankhaugen\\\\Yardarm\\\\src\\\\main\\\\output\\\\TestJson\\\\generated\\\\TestJson.dll\",\"OutputXmlFile\":null,\"NoXmlFile\":false,\"OutputDebugSymbols\":null,\"NoDebugSymbols\":false,\"OutputReferenceAssembly\":null,\"NoReferenceAssembly\":false,\"DelaySign\":false,\"OutputPackageFile\":null,\"OutputSymbolsPackageFile\":null,\"NoSymbolsPackageFile\":false,\"RepositoryType\":\"git\",\"RepositoryUrl\":null,\"RepositoryBranch\":null,\"RepositoryCommit\":null,\"AssemblyName\":\"TestJson\",\"RootNamespace\":null,\"TargetFrameworks\":[],\"ExtensionFiles\":[],\"IntermediateOutputPath\":\"C:\\\\repos\\\\frankhaugen\\\\Yardarm\\\\src\\\\main\\\\output\\\\TestJson\\\\intermediate\"}");

var rootDirectory = new DirectoryInfo("C:/temp/Yardarm/");

//var dllOutput = new MemoryStream();
var dllOutput = new FileInfo(Path.Combine(rootDirectory.FullName, "output.dll")).Create();
var pdbOutput = new FileInfo(Path.Combine(rootDirectory.FullName, "output.pdb")).Create();
var xmlOutput = new FileInfo(Path.Combine(rootDirectory.FullName, "output.xml")).Create();

var settings = GetSettings(dllOutput, pdbOutput, xmlOutput);

settings.DumpCSharp();

var generator = new Yardarm.YardarmGenerator(document, settings);

var restoreResult = await generator.RestoreAsync();

restoreResult.Dump();

var result = await generator.EmitAsync();

result.GetAllDiagnostics().Dump();

YardarmGenerationSettings GetSettings(Stream dllOutput, Stream pdbOutput, Stream xmlDocumentationOutput)
{
    return new YardarmGenerationSettings
    {
        AssemblyName = "TestJson",
        Version = new Version(),
        Author = "anonymous",
        BasePath = "C:\\Users\\frank\\AppData\\Local\\Temp\\LINQPad7\\_hbaxgecd\\shadow-1",
        DllOutput = dllOutput,
        PdbOutput = pdbOutput,
        XmlDocumentationOutput = xmlDocumentationOutput,
        IntermediateOutputPath = "C:\\repos\\frankhaugen\\Yardarm\\src\\main\\output\\TestJson\\intermediate",
        TargetFrameworkMonikers = new string[]
        {
            "netstandard2.0"
        }.ToImmutableArray(),
        CompilationOptions = new CSharpCompilationOptions(Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary)
        {
            //Usings = new string[0].ToImmutableArray(),
            //NullableContextOptions = NullableContextOptions.Enable,
            //OutputKind = OutputKind.DynamicallyLinkedLibrary,
            //ScriptClassName = "Script",
            //CryptoPublicKey = new byte[0].ToImmutableArray(),
            //OptimizationLevel = OptimizationLevel.Release,
            //WarningLevel = 4,
            //ConcurrentBuild = true,
            //Deterministic = true,
            //SpecificDiagnosticOptions = new Dictionary<string, ReportDiagnostic>
            //{
            //    {
            //        "CS1702",
            //        ReportDiagnostic.Suppress
            //    },
            //    {
            //        "CS1701",
            //        ReportDiagnostic.Suppress
            //    }
            //}.ToImmutableDictionary(),
            //AssemblyIdentityComparer = new DesktopAssemblyIdentityComparer()
        }
    };
}