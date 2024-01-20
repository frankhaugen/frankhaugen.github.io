<Query Kind="Program">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

void Main()
{
	var list = new List<Piece>();
	list.Add(new Piece { Title = "Piece 1", Description = "Description 1", Value = 10 });
	list.Add(new Piece { Title = "Piece 2", Description = "Description 2", Value = 20 });
	list.Add(new Piece { Title = "Piece 3", Description = "Description 3", Value = 30 });
	list.Add(new Piece { Title = "Piece 3", Description = "Description 3", Value = 30, Version = new Version(1, 0, 0, 0) });

}

public record Piece
{
	public string Title { get; init; }
	public string Description { get; init; }
	public int Value { get; init; }
	public Version Version { get; init; }
}

public static class MarkdownTableFormatter
{
	public static string Format<T>(IEnumerable<T> items)
	{
		var type = typeof(T);
		var properties = type.GetProperties();

		// Create header
		var header = "| " + string.Join(" | ", properties.Select(prop => prop.Name)) + " |";
		var separator = "|" + string.Join("|", properties.Select(_ => "---")) + "|";
		var lines = new List<string> { header, separator };

		// Create each row
		foreach (var item in items)
		{
			var row = "| " + string.Join(" | ", properties.Select(prop =>
				FormatCellValue(prop.GetValue(item), prop.PropertyType))) + " |";
			lines.Add(row);
		}

		return string.Join(Environment.NewLine, lines);
	}

	private static string FormatCellValue(object value, Type type)
	{
		if (value == null)
			return string.Empty;

		if (IsSimpleType(type) || HasCustomToString(value))
		{
			// Replace newline characters to avoid breaking Markdown table format
			return value.ToString().Replace("\n", " ").Replace("\r", " ");
		}

		return "{}";
	}

	private static bool HasCustomToString(object obj)
	{
		var toString = obj.ToString();
		return !string.IsNullOrEmpty(toString) && toString != obj.GetType().ToString() && toString.Length< 128;
	}

	private static bool IsSimpleType(Type type)
	{
		return
			type.IsPrimitive ||
			new Type[] {
			typeof(Enum),
			typeof(String),
			typeof(Decimal),
			typeof(DateTime),
			typeof(DateTimeOffset),
			typeof(TimeSpan),
			typeof(Guid)
			}.Contains(type) ||
			Convert.GetTypeCode(type) != TypeCode.Object;
	}

}

