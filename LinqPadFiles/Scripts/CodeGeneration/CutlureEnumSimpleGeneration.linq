<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

var stringBuilder = new StringBuilder();
var indent = new StringBuilder()
	.Append(" ")
	.Append(" ")
	.Append(" ")
	.Append(" ")
	.ToString();

stringBuilder.AppendLine("using System.Globalization;");
stringBuilder.AppendLine("using System.ComponentModel;");
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
stringBuilder.AppendLine("public class CultureInfoNameAttribute : Attribute");
stringBuilder.AppendLine("{");
stringBuilder.AppendLine(indent + "public string CultureInfoName { get; }");
stringBuilder.AppendLine(indent + "public CultureInfoNameAttribute(string cultureInfoName) => CultureInfoName = cultureInfoName;");
stringBuilder.AppendLine("}");
stringBuilder.AppendLine("");

// Extension to get cultureinfo from enum
stringBuilder.AppendLine("public static class CultureCodeExtensions");
stringBuilder.AppendLine("{");
stringBuilder.AppendLine(indent + "public static string GetCultureInfoName(this CultureCode code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(CultureInfoNameAttribute), false)[0] as CultureInfoNameAttribute)?.CultureInfoName ?? string.Empty;");
stringBuilder.AppendLine(indent + "public static string GetNativeName(this CultureCode code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(NativeNameAttribute), false)[0] as NativeNameAttribute)?.LocalName ?? string.Empty;");
stringBuilder.AppendLine(indent + "public static string GetEnglishName(this CultureCode code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(EnglishNameAttribute), false)[0] as EnglishNameAttribute)?.EnglishName ?? string.Empty;");
stringBuilder.AppendLine(indent + "public static CultureInfo GetCultureInfo(this CultureCode code) => CultureInfo.GetCultureInfoByIetfLanguageTag(code.GetCultureInfoName());");
stringBuilder.AppendLine(indent + "public static RegionInfo GetRegionInfo(this CultureCode code) => new RegionInfo(code.GetCultureInfo().Name);");
stringBuilder.AppendLine("}");

stringBuilder.AppendLine("");

// Create Enum
stringBuilder.AppendLine("///<summary>Contains a list of Two-letter culture codes stored in .net BCL. Enum integer value is based on universal GeoId to support a standardized numbersequence compatible with regioninfo</summary>");
stringBuilder.AppendLine("public enum CultureCode");
stringBuilder.AppendLine("{");

var cultures = GetCultures();

var codes = new List<string>();

foreach (var culture in cultures)
{
	if (!codes.Contains(culture.TwoLetterISOLanguageName.ToUpperInvariant()) && TryGetRegionInfo(culture, out var regionInfo))
	{
		stringBuilder.AppendLine($"{indent}/// <summary>{culture.EnglishName}</summary>");
		stringBuilder.AppendLine($"{indent}[NativeName(\"{culture.NativeName}\")]");
		stringBuilder.AppendLine($"{indent}[EnglishName(\"{culture.EnglishName}\")]");
		stringBuilder.AppendLine($"{indent}[CultureInfoName(\"{culture.Name}\")]");
		stringBuilder.AppendLine($"{indent}[Description(\"{culture.EnglishName}\")]");
		stringBuilder.AppendLine($"{indent}{culture.TwoLetterISOLanguageName.ToUpperInvariant()} = {regionInfo?.GeoId}, // {regionInfo?.TwoLetterISORegionName} {culture.LCID}\n");
		codes.Add(culture.TwoLetterISOLanguageName.ToUpperInvariant());
	}
}
stringBuilder.AppendLine("}\n");

var file = new FileInfo("C:/repos/frankhaugen/frankhaugen.github.io/LinqPadFiles/Culture.cs");

file.FullName.Dump();
stringBuilder.ToString().Dump();

File.WriteAllText(file.FullName, stringBuilder.ToString());

List<CultureInfo> GetCultures() => CultureInfo.GetCultures(CultureTypes.SpecificCultures)
	.OrderBy(x => x.Name)
	.DistinctBy(x => x.Name)
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