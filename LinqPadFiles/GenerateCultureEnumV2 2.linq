<Query Kind="Statements" />

using System.Globalization;

var stringBuilder = new StringBuilder();
var indent = new StringBuilder()
	.Append(" ")
	.Append(" ")
	.Append(" ")
	.Append(" ")
	.ToString();

stringBuilder.AppendLine("");
stringBuilder.AppendLine("public class NativeNameAttribute : Attribute");
stringBuilder.AppendLine("{");
stringBuilder.AppendLine(indent + "public string LocalName { get; }");
stringBuilder.AppendLine(indent + "public NativeNameAttribute(string localName) => LocalName = localName;");
stringBuilder.AppendLine("}");
stringBuilder.AppendLine("");
stringBuilder.AppendLine("public class EnglishNameAttribute : Attribute");
stringBuilder.AppendLine("{");
stringBuilder.AppendLine(indent + "public string EnglishName { get; }");
stringBuilder.AppendLine(indent + "public EnglishNameAttribute(string englishName) => EnglishName = englishName;");
stringBuilder.AppendLine("}");
stringBuilder.AppendLine("");

// Extension to get cultureinfo from enum
stringBuilder.AppendLine("public static class CultureExtensions");
stringBuilder.AppendLine("{");
stringBuilder.AppendLine(indent + "public static CultureInfo GetCultureInfo(this Culture culture) => CultureInfo.GetCultureInfo(culture.ToString());");
stringBuilder.AppendLine(indent + "public static string GetNativeName(this Culture culture) => (culture.GetType().GetMember(culture.ToString())[0].GetCustomAttributes(typeof(NativeNameAttribute), false)[0] as NativeNameAttribute)?.LocalName ?? string.Empty;");
stringBuilder.AppendLine(indent + "public static string GetEnglishName(this Currency culture) => (culture.GetType().GetMember(culture.ToString())[0].GetCustomAttributes(typeof(EnglishNameAttribute), false)[0] as EnglishNameAttribute)?.EnglishName ?? string.Empty;");
stringBuilder.AppendLine("}");


stringBuilder.AppendLine("");

// Create Enum
stringBuilder.AppendLine("///<summary>Contains a list of culture codes used in .net BCL from CultureInfo</summary>");
stringBuilder.AppendLine("public enum Culture");
stringBuilder.AppendLine("{");

var cultures = GetCultures();
var counter = 0;
foreach (var culture in cultures)
{
	stringBuilder.AppendLine($"{indent}/// <summary>{culture.EnglishName}</summary>");
	stringBuilder.AppendLine($"{indent}[NativeName(\"{culture.NativeName}\")]");
	stringBuilder.AppendLine($"{indent}[EnglishName(\"{culture.EnglishName}\")]");
	stringBuilder.AppendLine($"{indent}{culture.TwoLetterISOLanguageName.ToUpperInvariant()} = {counter},\n");
	counter++;
}
stringBuilder.AppendLine("}\n");

stringBuilder.Dump();

List<CultureInfo> GetCultures() => CultureInfo.GetCultures(CultureTypes.AllCultures)
	.OrderBy(x => x.Name)
	.DistinctBy(x => x.TwoLetterISOLanguageName)
	.ToList();

bool TryGetRegionInfo(CultureInfo culture, out RegionInfo? regionInfo)
{
	try
	{
		regionInfo = new RegionInfo(culture.Name);
		return true;
	}
	catch (Exception ex)
	{
		regionInfo = null;
		return false;
	}
}