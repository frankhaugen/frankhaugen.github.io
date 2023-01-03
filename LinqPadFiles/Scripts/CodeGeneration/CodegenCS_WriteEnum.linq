<Query Kind="Statements">
  <Reference Relative="..\..\..\company.IntegrationEngine\company.IntegrationEngine.UnusedCode\bin\Debug\net5.0\company.IntegrationEngine.UnusedCode.dll">C:\repos\company.IntegrationEngine\company.IntegrationEngine.UnusedCode\bin\Debug\net5.0\company.IntegrationEngine.UnusedCode.dll</Reference>
  <NuGetReference>CodegenCS</NuGetReference>
  <Namespace>CodegenCS</Namespace>
  <Namespace>CodegenCS.ControlFlow</Namespace>
  <Namespace>CodegenCS.DotNet</Namespace>
  <Namespace>CodegenCS.Extensions</Namespace>
  <Namespace>CodegenCS.InputModels</Namespace>
  <Namespace>CodegenCS.Utils</Namespace>
  <Namespace>Namotion.Reflection</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>NJsonSchema</Namespace>
  <Namespace>NJsonSchema.Annotations</Namespace>
  <Namespace>NJsonSchema.Converters</Namespace>
  <Namespace>NJsonSchema.Generation</Namespace>
  <Namespace>NJsonSchema.Generation.SchemaProcessors</Namespace>
  <Namespace>NJsonSchema.Generation.TypeMappers</Namespace>
  <Namespace>NJsonSchema.Infrastructure</Namespace>
  <Namespace>NJsonSchema.References</Namespace>
  <Namespace>NJsonSchema.Validation</Namespace>
  <Namespace>NJsonSchema.Validation.FormatValidators</Namespace>
  <Namespace>NJsonSchema.Visitors</Namespace>
  <Namespace>company.IntegrationEngine.UnusedCode</Namespace>
  <Namespace>company.IntegrationEngine.UnusedCode.CodeGeneration</Namespace>
  <Namespace>company.IntegrationEngine.UnusedCode.CodeGeneration.Models</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Runtime.Serialization.Formatters</Namespace>
</Query>

var dictionary = new Dictionary<string, int>();

dictionary.Add("A", 0);
dictionary.Add("B", 1);
dictionary.Add("C", 2);

var writer = new CodegenTextWriter();

WriteEnum(writer,"Letters","Alphabet",dictionary).Dump();

string WriteEnum(CodegenTextWriter writer, string name, string @namespace, Dictionary<string, int> values)
{
	writer.WriteLine("using System.ComponentModel;");
	writer.WriteLine(" ");

	writer.WriteLine($"namespace {@namespace};");
	writer.WriteLine(" ");
	writer.WriteLine(" // ReSharper disable InconsistentNaming");
	writer.WithCurlyBraces($"public enum {name}", () =>
	{
		foreach (var value in values)
		{
			writer.WriteLine($"{value.Key} = {value.Value},");
		}
	});

	return writer.GetContents();
}