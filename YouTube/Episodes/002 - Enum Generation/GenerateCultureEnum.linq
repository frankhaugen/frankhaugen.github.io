<Query Kind="Statements" />

using System.Globalization;

var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
.DistinctBy(x => x.TwoLetterISOLanguageName)
.ToArray();
var stringBuilder = new StringBuilder();
var indent = new StringBuilder()
	.Append(" ")
	.Append(" ")
	.Append(" ")
	.Append(" ")
	.ToString();

// Using statements and namespace declaration
stringBuilder.AppendLine("using System.ComponentModel;");
stringBuilder.AppendLine("using System.Globalization;");
stringBuilder.AppendLine("");
stringBuilder.AppendLine("namespace GeneratedEnums;");

// To make it runnable in LinqPad
stringBuilder.AppendLine(indent + "// Only for LinqPad debugging purposes");
stringBuilder.AppendLine(indent + "public class Program { public static void Main() => Culture.NB.GetCultureInfo().NumberFormat.Dump();}\n");

// Extension to get cultureinfo from enum
stringBuilder.AppendLine(indent + "public static class CultureExtensions { public static CultureInfo GetCultureInfo(this Culture culture) => CultureInfo.GetCultureInfo(culture.ToString()); }\n");

// Create Enum
stringBuilder.AppendLine(indent + "///<summary>Contains a list of .net cultures to simplify use of CultureInfo</summary>");
stringBuilder.AppendLine(indent + "public enum Culture");
stringBuilder.AppendLine(indent + "{");
for (int i = 0; i < cultures.Count(); i++)
{
	var culture = cultures[i];

	stringBuilder.AppendLine($"{indent}{indent}/// <summary>{culture.EnglishName}</summary>");
	stringBuilder.AppendLine($"{indent}{indent}[Description(\"{culture.NativeName}\")]");
	stringBuilder.AppendLine($"{indent}{indent}{culture.TwoLetterISOLanguageName.ToUpperInvariant()} = {i},\n");
}
stringBuilder.AppendLine(indent + "}\n");

stringBuilder.Dump();