<Query Kind="Program">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

void Main()
{
	var colorContainer = new ColorContainer { Color = Color.Green };

	// Serialize and deserialize without any special configuration (defaults to enum names)
	var optionsDefault = new JsonSerializerOptions();
	Console.WriteLine("Default:");
	var serializedDefault = JsonSerializer.Serialize(colorContainer, optionsDefault);
	Console.WriteLine(serializedDefault);
	Console.WriteLine(JsonSerializer.Deserialize<ColorContainer>(serializedDefault, optionsDefault).Color);

	// Serialize and deserialize using the JsonStringEnumConverter with a naming policy (e.g., camel case)
	var optionsWithNamingPolicy = new JsonSerializerOptions
	{
		Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
	};
	Console.WriteLine("\nWith Naming Policy:");
	var serializedNamingPolicy = JsonSerializer.Serialize(colorContainer, optionsWithNamingPolicy);
	Console.WriteLine(serializedNamingPolicy);
	Console.WriteLine(JsonSerializer.Deserialize<ColorContainer>(serializedNamingPolicy, optionsWithNamingPolicy).Color);

	// Serialize and deserialize using the JsonStringEnumConverter allowing numbers
	var optionsWithNumbers = new JsonSerializerOptions
	{
		Converters = { new JsonStringEnumConverter(allowIntegerValues: true) }
	};
	Console.WriteLine("\nWith Numbers Allowed:");
	var serializedWithNumbers = JsonSerializer.Serialize(colorContainer, optionsWithNumbers);
	Console.WriteLine(serializedWithNumbers);
	Console.WriteLine(JsonSerializer.Deserialize<ColorContainer>("{\"Color\":1}", optionsWithNumbers).Color);
}

public class ColorContainer
{
	public Color Color { get; set; }
}

// Example enum
public enum Color
{
	Red,
	Green,
	Blue
}