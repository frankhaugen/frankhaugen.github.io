<Query Kind="Statements">
  <Reference Relative="..\CodeGeneration\Roslyn - Get classes and types as seperate strings.linq">C:\repos\frankhaugen\frankhaugen.github.io\LinqPadFiles\Scripts\CodeGeneration\Roslyn - Get classes and types as seperate strings.linq</Reference>
  <NuGetReference>NJsonSchema.CodeGeneration.CSharp</NuGetReference>
  <Namespace>NJsonSchema</Namespace>
  <Namespace>NJsonSchema.CodeGeneration.CSharp</Namespace>
</Query>

#load "C:\repos\frankhaugen\frankhaugen.github.io\LinqPadFiles\Scripts\CodeGeneration\Roslyn - Get classes and types as seperate strings.linq"

var schemaPath = "C:/temp/launchsettings.json";
var schemaFileName = schemaPath.Split("/").LastOrDefault();
var @namespace = schemaPath.Split(".").FirstOrDefault();

if (string.IsNullOrWhiteSpace(@namespace))
{
    @namespace = "Unknown";
}

string json = File.ReadAllText(schemaPath);
var schemaFromFile = JsonSchema.FromSampleJson(json);
var classGenerator = new CSharpGenerator(schemaFromFile, new CSharpGeneratorSettings
{
    ClassStyle = CSharpClassStyle.Poco,
    GenerateOptionalPropertiesAsNullable = true,
    //GenerateNativeRecords = true,
    
    GenerateNullableReferenceTypes = true,
    //RequiredPropertiesMustBeDefined = false
    //GenerateDataAnnotations = false,
    ArrayType = "List"
});
var codeFile = classGenerator
        .GenerateFile()
        .Replace(", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore", "")
        .Replace(" = default!;", "")
        .Replace("Newtonsoft.Json.JsonProperty", typeof(System.Text.Json.Serialization.JsonPropertyNameAttribute).FullName)
        .Replace("\n        [Newtonsoft.Json.JsonExtensionData]", "\n")
        .Replace("10.8.0.0 (Newtonsoft.Json v9.0.0.0)", "System.Text.Json")
        .Replace("\n\n\n", "\n")
        ;
        
//codeFile.Dump();


var outputDirectory = new DirectoryInfo(Path.Combine(@"C:\temp\LaunchSettingsModels", @namespace));
if (!outputDirectory.Exists)
{
    outputDirectory.Create();
}

var results = SeparateCodeToFiles(codeFile, outputDirectory);

foreach (var result in results)
{
    File.WriteAllText(result.Key.FullName, result.Value.Replace("public partial class", "public class"));

}


