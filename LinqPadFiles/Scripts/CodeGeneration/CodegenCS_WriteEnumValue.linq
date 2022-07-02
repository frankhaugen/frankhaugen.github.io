<Query Kind="Statements">
  <Reference Relative="..\..\..\Semine.IntegrationEngine\Semine.IntegrationEngine.UnusedCode\bin\Debug\net5.0\Semine.IntegrationEngine.UnusedCode.dll">C:\repos\Semine.IntegrationEngine\Semine.IntegrationEngine.UnusedCode\bin\Debug\net5.0\Semine.IntegrationEngine.UnusedCode.dll</Reference>
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
  <Namespace>Semine.IntegrationEngine.UnusedCode</Namespace>
  <Namespace>Semine.IntegrationEngine.UnusedCode.CodeGeneration</Namespace>
  <Namespace>Semine.IntegrationEngine.UnusedCode.CodeGeneration.Models</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Runtime.Serialization.Formatters</Namespace>
</Query>

var attributes = new Dictionary<string, string?>();
var value = new EnumValue("A", 0, attributes);

attributes.Add(nameof(ObsoleteAttribute).Replace(nameof(System.Attribute), ""), null);
attributes.Add(nameof(DebuggableAttribute).Replace(nameof(System.Attribute), ""), "This is not in use");

WriteEnumValue(new CodegenTextWriter(), value).Dump();

string WriteEnumValue(CodegenTextWriter writer, EnumValue enumValue)
{
	if (enumValue.Attributes != null && enumValue.Attributes.Any() && enumValue.Attributes.Any(x => !string.IsNullOrWhiteSpace(x.Key)))
	{
		foreach (var attribute in enumValue.Attributes)
		{

			writer.Write('[');
			writer.Write(attribute.Key);
			if (attribute.Value != null)
			{
				writer.Write('(');
				writer.Write('"');
				writer.Write(attribute.Value);
				writer.Write('"');
				writer.Write(')');
			}
			writer.Write(']');
			writer.WriteLine();
		}
	}
	writer.WriteLine($"{enumValue.Name} = {enumValue.Value},");
	return writer.GetContents();
}

readonly record struct EnumValue(string Name, int Value, Dictionary<string, string?>? Attributes = null);