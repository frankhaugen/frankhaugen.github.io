<Query Kind="Statements">
  <NuGetReference>Microsoft.OpenApi</NuGetReference>
  <NuGetReference>Microsoft.OpenApi.Readers</NuGetReference>
  <Namespace>System.Globalization</Namespace>
</Query>

var swaggerFile = await Downloader.DownloadAsync("https://api.MyEmployer.no/swagger/v1.0/swagger.json");

var reader = new Microsoft.OpenApi.Readers.OpenApiStringReader();

var result = reader.Read(Encoding.UTF8.GetString(swaggerFile), out var diagnostic);


diagnostic.Dump();

result.Dump();

// Generate an OpenApoSchema to CSharpSyntaxTree method
var generator = new Microsoft.OpenApi.CSharpAnnotations.CSharpGenerator(result.Value);
var syntaxTree = generator.GenerateFile();
syntaxTree.Dump();