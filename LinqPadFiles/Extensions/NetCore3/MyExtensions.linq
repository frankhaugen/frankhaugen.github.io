<Query Kind="Program" />

void Main()
{
	// Write code to test your extensions here. Press F5 to compile and run.
}

public static class MyExtensions
{
	// Write custom extension methods here. They will be available to all queries.
	public static Guid ToGuid(this string value) => Guid.Parse(value);
}

// You can also define namespaces, non-static classes, enums, etc.

#region Advanced - How to multi-target

// The NET6 symbol is active when the query runs on .NET 6 or later.

#if NET6
// Code that requires .NET 6 or later
#endif

#if NET5
// Code that requires .NET 5 or later
#endif

#endregion