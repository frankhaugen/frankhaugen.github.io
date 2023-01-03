<Query Kind="Program">
  <Namespace>System.Numerics</Namespace>
</Query>

void Main()
{
	// Write code to test your extensions here. Press F5 to compile and run.
}


public static class MyExtensions
{
	// Write custom extension methods here. They will be available to all queries.
	public static string ReadAllText(this FileInfo file) => File.ReadAllText(file.FullName);
	public static Guid ToGuid(this string value) => Guid.Parse(value);
	public static string Join(this IEnumerable<string> source, string separator = "\n") => string.Join(separator, source);

    public static float ToRadian(this float angle) => angle * (MathF.PI / 180);
    public static float Round(this float value, int precision = 1) => MathF.Round(value, precision);

    public static Vector2 Copy(this Vector2 source) => new(source.X, source.Y);
}

// You can also define namespaces, non-static classes, enums, etc.

#region Advanced - How to multi-target

// The NETx symbol is active when a query runs under .NET x or later.

#if NET7
// Code that requires .NET 7 or later
#endif

#if NET6
// Code that requires .NET 6 or later
#endif

#if NET5
// Code that requires .NET 5 or later
#endif

#endregion