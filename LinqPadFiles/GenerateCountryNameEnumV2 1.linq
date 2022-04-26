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
stringBuilder.AppendLine("public static class CountryNameExtensions");
stringBuilder.AppendLine("{");
stringBuilder.AppendLine(indent + "public static string GetNativeName(this CountryName countryName) => (countryName.GetType().GetMember(countryName.ToString())[0].GetCustomAttributes(typeof(NativeNameAttribute), false)[0] as NativeNameAttribute)?.LocalName ?? string.Empty;");
stringBuilder.AppendLine(indent + "public static string GetEnglishName(this CountryName countryName) => (countryName.GetType().GetMember(countryName.ToString())[0].GetCustomAttributes(typeof(EnglishNameAttribute), false)[0] as EnglishNameAttribute)?.EnglishName ?? string.Empty;");
stringBuilder.AppendLine("}");


stringBuilder.AppendLine("");

// Create Enum
stringBuilder.AppendLine("///<summary>Contains a list of cleaned country names stored in .net BCL. Enum integer value is based on universal GeoId to support a standardized numbersequence</summary>");
stringBuilder.AppendLine("public enum CountryName");
stringBuilder.AppendLine("{");

var cultures = GetCultures();
var regions = GetRegions(cultures);
foreach (var region in regions.Values.DistinctBy(x => x.EnglishName).OrderBy(x => x.GeoId))
{
	stringBuilder.AppendLine($"{indent}/// <summary>{region?.EnglishName}</summary>");
	stringBuilder.AppendLine($"{indent}[NativeName(\"{region?.NativeName}\")]");
	stringBuilder.AppendLine($"{indent}[EnglishName(\"{region?.EnglishName}\")]");
	stringBuilder.AppendLine($"{indent}{region?.EnglishName.Replace(",", "").Replace("’", "").Replace(".", "").Replace("&", "And").Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "")} = {region?.GeoId},\n");
}
stringBuilder.AppendLine("}\n");

stringBuilder.Dump();

List<CultureInfo> GetCultures() => CultureInfo.GetCultures(CultureTypes.AllCultures)
	.OrderBy(x => x.Name)
	.DistinctBy(x => x.Name)
	.ToList();

Dictionary<int, RegionInfo> GetRegions(List<CultureInfo> cultures)
{
	var regions = new Dictionary<int, RegionInfo>();

	foreach (var culture in cultures)
	{
		if (TryGetRegionInfo(culture, out var regionInfo) && regionInfo != null && regionInfo.TwoLetterISORegionName.All(Char.IsLetter))
		{
			regions.TryAdd(regionInfo.GeoId, regionInfo);
		}
	}
	
	return regions;
}

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