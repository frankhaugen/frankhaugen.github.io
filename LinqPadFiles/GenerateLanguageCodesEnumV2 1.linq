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
stringBuilder.AppendLine("");
stringBuilder.AppendLine("public class NativeNameAttribute : Attribute");
stringBuilder.AppendLine("{");
stringBuilder.AppendLine(indent + "public string LocalName { get; }");
stringBuilder.AppendLine(indent + "public NativeNameAttribute(string localName) => LocalName = localName;");
stringBuilder.AppendLine("}");
stringBuilder.AppendLine("");

// Extension to get cultureinfo from enum
stringBuilder.AppendLine("public static class CultureExtensions { public static CultureInfo GetCultureInfo(this Culture culture) => CultureInfo.GetCultureInfo(culture.ToString()); }");
stringBuilder.AppendLine("");

// Create Enum
stringBuilder.AppendLine("///<summary>Contains a list of .net cultures to simplify use of CultureInfo</summary>");
stringBuilder.AppendLine("public enum Culture");
stringBuilder.AppendLine("{");
for (int i = 0; i < cultures.Count(); i++)
{
	var culture = cultures[i];

	stringBuilder.AppendLine($"{indent}/// <summary>{culture.EnglishName}</summary>");
	stringBuilder.AppendLine($"{indent}[NativeName(\"{culture.NativeName}\")]");
	stringBuilder.AppendLine($"{indent}[EnglishName(\"{culture.EnglishName}\")]");
	stringBuilder.AppendLine($"{indent}{culture.TwoLetterISOLanguageName.ToUpperInvariant()} = {i},\n");
}
stringBuilder.AppendLine("}\n");

stringBuilder.Dump();