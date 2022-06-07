<Query Kind="Statements">
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
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Runtime.Serialization.Formatters</Namespace>
</Query>

var writer = new CodegenTextWriter();

var testValues = new Dictionary<string, int>();
testValues.Add("A", 0);
testValues.Add("B", 1);
testValues.Add("C", 2);

GenerateEnum(testValues).Dump();

string GenerateEnum(Dictionary<string, int> names, string className = "MyEnum", string namespaceName = "MyNamespace")
{
	writer.WriteLine("using System.ComponentModel;");
	writer.WriteLine(" // ReSharper disable InconsistentNaming");
	writer.WriteLine(" ");

	writer.WriteLine($"namespace {namespaceName};");
	writer.WriteLine(" ");
	writer.WithCurlyBraces($"public enum {className}", () =>
	{
		foreach(var item in names) GenerateEnumValue(item.Key, item.Value);
	});

	return writer.GetContents();
}

void GenerateEnumValue(string name, int value) => writer.WriteLine($"{name} = {value},");
