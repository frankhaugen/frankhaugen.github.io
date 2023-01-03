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

var usingList = new UsingsList();

var generator = new Generator("MyClass", "MyNamespace");

generator._attributes.Add<AssemblyVersionAttribute>();

generator.ToString().Dump();










var writer = new CodegenTextWriter();

var testValues = new Dictionary<string, int>();
testValues.Add("A", 0);
testValues.Add("B", 1);
testValues.Add("C", 2);

GenerateSimpleEnum(testValues).Dump();

string GenerateSimpleEnum(Dictionary<string, int> names, string className = "MyEnum", string namespaceName = "MyNamespace")
{
	writer.WriteLine("using System.ComponentModel;");
	writer.WriteLine(" // ReSharper disable InconsistentNaming");
	writer.WriteLine(" ");

	writer.WriteLine($"namespace {namespaceName};");
	writer.WriteLine(" ");
	writer.WithCurlyBraces($"public enum {className}", () =>
	{
		foreach(var item in names) writer.WriteLine($"{item.Key} = {item.Value},");
	});

	return writer.GetContents();
}

var testClass = new Class("MyClass", "MyNamespace");

testClass.AddUsingNamespace(typeof(CultureInfo).Namespace);
testClass.AddProperty("Name", typeof(string));

testClass.ToString().Dump();



class UsingsList
{
	private readonly List<string> _namespaces = new();

	public IEnumerable<string> Get() => _namespaces
		.Where(x => !string.IsNullOrWhiteSpace(x))
		.Distinct()
		.OrderBy(x => x)
		.Select(x => $"using {x};");

	public void Add<T>() => _namespaces.Add(typeof(T).Namespace ?? "");
	public void Remove<T>() => _namespaces.Remove(typeof(T).Namespace ?? "");

	public override string ToString() => string.Join("\n", Get());
}


class AttributesList
{
	private readonly UsingsList _usings = new();
	private readonly List<string> _list = new();

	public string GetUsings() => _usings.ToString();

	public IEnumerable<string> Get() => _list
		.Where(x => !string.IsNullOrWhiteSpace(x))
		.Distinct()
		.OrderBy(x => x)
		.Select(x => $"using {x};");

	public void Add<T>(string? value = null) where T : System.Attribute
	{
		_usings.Add<T>();

		var stringBuilder = new StringBuilder();

		stringBuilder.Append('[');
		stringBuilder.Append(typeof(T).Name);
		if (value != null)
		{
			stringBuilder.Append('(');
			stringBuilder.Append('"');
			stringBuilder.Append(value);
			stringBuilder.Append('"');
			stringBuilder.Append(')');
		}
		stringBuilder.Append(']');

		var attribute = stringBuilder.ToString();
		_list.Add(attribute);
	}

	public override string ToString() => string.Join("\n", Get());
}


class PropertiesList
{
	private readonly UsingsList _usings = new();
	private readonly List<string> _list = new();

	public string GetUsings() => _usings.ToString();

	public IEnumerable<string> Get() => _list
		.Where(x => !string.IsNullOrWhiteSpace(x))
		.Distinct()
		.OrderBy(x => x)
		.Select(x => $"public {x}");

	public void Add<T>(string? name = null)
	{
		_usings.Add<T>();
		var type = typeof(T);
		var stringBuilder = new StringBuilder();

		stringBuilder.Append(type.Name);
		if (name != null)
		{
			stringBuilder.Append($" {name}");
		}
		else
		{
			stringBuilder.Append($" {type.Name}");
		}
		stringBuilder.Append(" { get; set; }");

		var property = stringBuilder.ToString();
		_list.Add(property);
	}

	public override string ToString() => string.Join("\n", Get());
}

class Generator
{
	
	public readonly AttributesList _attributes = new();
	public readonly PropertiesList _properties = new();
	
	private readonly string _name;
	private readonly string _nameSpace;

	public Generator(string name, string @namespace)
	{
		_name = name;
		_nameSpace = @namespace;
	}

	public override string ToString()
	{
		var stringBuider = new StringBuilder();

		var writer = new CodegenTextWriter();
		
		var usings = new List<string>();
		
		usings.Add(_attributes.GetUsings());
		usings.Add(_properties.GetUsings());
		
		writer.WriteLine(string.Join("\n", usings));

		writer.WriteLine($"namespace {_nameSpace};\n");

		writer.WithCurlyBraces($"public class {_name}", x => {
			writer.WriteLine(_properties.ToString());	
		});
		
		return writer.ToString();
	}




}