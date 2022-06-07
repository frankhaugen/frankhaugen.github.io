<Query Kind="Statements">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <NuGetReference>NodaTime</NuGetReference>
  <Namespace>NodaTime</Namespace>
  <Namespace>NodaTime.Calendars</Namespace>
  <Namespace>NodaTime.Extensions</Namespace>
  <Namespace>NodaTime.Text</Namespace>
  <Namespace>NodaTime.TimeZones</Namespace>
  <Namespace>NodaTime.TimeZones.Cldr</Namespace>
  <Namespace>NodaTime.Utility</Namespace>
  <Namespace>NodaTime.Xml</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
</Query>

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var @namespace = "Localization";
var outputDirectory = new DirectoryInfo(Path.Combine("C:/repos/frankhaugen/frankhaugen.github.io/LinqPadFiles", @namespace));
outputDirectory.Create();

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
AddAttributes();
AddCultureExtensions();
stringBuilder.AppendLine("");
AddRegionExtensions();
stringBuilder.AppendLine("");
stringBuilder.AppendLine("///<summary></summary>");
stringBuilder.AppendLine("public readonly record struct LocalizationOptions(Culture Culture, Region Region);");
stringBuilder.AppendLine("public readonly record struct RegionLocation(Region Region, float Latitude, float Longitude, string Name);");
stringBuilder.AppendLine("");
AddRegion();
//AddCultures();
//AddTimezone();


RunRoslyAndOutput();

void WriteFile(string identifier, string code) => File.WriteAllText(Path.Combine(outputDirectory.FullName, identifier + ".cs"), code);


void RunRoslyAndOutput()
{
	var text = stringBuilder.ToString();
	text.Dump();
	var syntaxTree = CSharpSyntaxTree.ParseText(text);
	var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
	var enums = root.Members.OfType<EnumDeclarationSyntax>();
	var types = root.Members.OfType<TypeDeclarationSyntax>();


	var usings = new StringBuilder()
		.AppendLine("using System.Globalization;")
		.AppendLine("using System.ComponentModel;")
		.AppendLine("using System.Numerics;")
		//.AppendLine("")
		//.AppendLine($"namespace {@namespace};")
		//.AppendLine("")
		.ToString();

	foreach (var element in types)
	{
		WriteFile(element.Identifier.ToString(), usings + element.ToFullString());
	}
	foreach (var element in enums)
	{
		WriteFile(element.Identifier.ToString(), usings + element.ToFullString());
	}

}


void AddRegionExtensions()
{
	stringBuilder.AppendLine("public static class RegionExtensions");
	stringBuilder.AppendLine("{");
	stringBuilder.AppendLine(indent + "public static string GetNativeName(this Region code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(NativeNameAttribute), false)[0] as NativeNameAttribute)?.LocalName ?? string.Empty;");
	stringBuilder.AppendLine(indent + "public static string GetEnglishName(this Region code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(EnglishNameAttribute), false)[0] as EnglishNameAttribute)?.EnglishName ?? string.Empty;");
	stringBuilder.AppendLine(indent + "public static Vector2 GetCoordinates(this Region code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(CoordinatesAttribute), false)[0] as CoordinatesAttribute)?.Coordinates ?? Vector2.Zero;");
	stringBuilder.AppendLine("}");
}


void AddRegion()
{
	stringBuilder.AppendLine("///<summary>Contains a list of Two-letter country (region) codes stored in .net BCL. Enum integer value is based on universal GeoId to support a standardized numbersequence compatible with CultureInfo</summary>");
	stringBuilder.AppendLine("public enum Region");
	stringBuilder.AppendLine("{");

	var regions = GetRegions(GetCultures()).OrderBy(x => x.Value.Name);
	var csvFile = new FileInfo("C:/repos/frankhaugen/frankhaugen.github.io/LinqPadFiles/Localization/CountryLatitudeLongitudeName.csv");
	var csv = File.ReadAllLines(csvFile.FullName);

	foreach (var region in regions)
	{
		var row = csv.FirstOrDefault(x => x.StartsWith(region.Value.Name));
		
		stringBuilder.AppendLine($"{indent}/// <summary>{region.Value.EnglishName}</summary>");
		stringBuilder.AppendLine($"{indent}[NativeName(\"{region.Value.NativeName}\")]");
		stringBuilder.AppendLine($"{indent}[EnglishName(\"{region.Value.EnglishName}\")]");

		if (row != null)
		{
			var cells = row.Split("\t");
			stringBuilder.AppendLine($"{indent}[Coordinates({cells[1]}, {cells[2]})]");
		}
		
		stringBuilder.AppendLine($"{indent}[Description(\"{region.Value.EnglishName}\")]");
		stringBuilder.AppendLine($"{indent}{region.Value.Name} = {region.Value.GeoId},\n");
	}
	stringBuilder.AppendLine("}\n");

}

void AddCultures()
{
	var cultures = GetCultures().OrderBy(x => x.Name);
	stringBuilder.AppendLine("///<summary>Contains a list of Two-letter culture codes stored in .net BCL. Enum integer value is based on universal GeoId to support a standardized numbersequence compatible with regioninfo</summary>");
	stringBuilder.AppendLine("public enum Culture");
	stringBuilder.AppendLine("{");

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
			stringBuilder.AppendLine($"{indent}{culture.TwoLetterISOLanguageName.ToUpperInvariant()} = {regionInfo?.GeoId}, // LCID: {culture.LCID}\n");
			codes.Add(culture.TwoLetterISOLanguageName.ToUpperInvariant());
		}
	}
	stringBuilder.AppendLine("}\n");
}


List<CultureInfo> GetCultures() => CultureInfo.GetCultures(CultureTypes.AllCultures)
	.DistinctBy(x => x.Name)
	.ToList();

Dictionary<int, RegionInfo> GetRegions(List<CultureInfo> cultures)
{
	var regions = new Dictionary<int, RegionInfo>();

	CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures)
			.Select(x => new RegionInfo(x.LCID))
			.DistinctBy(x => x.Name)
			.OrderBy(x => x.Name)
			.Where(x => x.ThreeLetterISORegionName.Any());

	return regions;
}

bool TryGetRegionInfo(CultureInfo culture, out RegionInfo? regionInfo)
{
	var name = culture.Name;
	
	if (name.Equals("IV", StringComparison.InvariantCultureIgnoreCase))
	{
		name = "US";
	}
	
	try
	{
		regionInfo = new RegionInfo(name);
		return true;
	}
	catch (Exception ex)
	{
		regionInfo = null;
		return false;
	}
}

void AddCultureExtensions()
{
	stringBuilder.AppendLine("public static class CultureExtensions");
	stringBuilder.AppendLine("{");
	stringBuilder.AppendLine(indent + "public static string GetCultureInfoName(this Culture code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(CultureInfoNameAttribute), false)[0] as CultureInfoNameAttribute)?.CultureInfoName ?? string.Empty;");
	stringBuilder.AppendLine(indent + "public static string GetNativeName(this Culture code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(NativeNameAttribute), false)[0] as NativeNameAttribute)?.LocalName ?? string.Empty;");
	stringBuilder.AppendLine(indent + "public static string GetEnglishName(this Culture code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(EnglishNameAttribute), false)[0] as EnglishNameAttribute)?.EnglishName ?? string.Empty;");
	stringBuilder.AppendLine(indent + "public static CultureInfo GetCultureInfo(this Culture code) => CultureInfo.GetCultureInfoByIetfLanguageTag(code.GetCultureInfoName());");
	stringBuilder.AppendLine(indent + "public static RegionInfo GetRegionInfo(this Culture code) => new RegionInfo(code.GetCultureInfo().Name);");
	stringBuilder.AppendLine("}");

}


void AddTimezone()
{
	stringBuilder.AppendLine("///<summary>Contains a list of Two-letter country (region) codes stored in .net BCL. Enum integer value is based on universal GeoId to support a standardized numbersequence compatible with CultureInfo</summary>");
	stringBuilder.AppendLine("public enum Timezone");
	stringBuilder.AppendLine("{");

	var timezones = DateTimeZoneProviders.Tzdb.GetAllZones().OrderBy(x => x.Id);
	var derpzones = DateTimeZoneProviders.Bcl.GetAllZones().OrderBy(x => x.Id);

	foreach (var timezone in timezones)
	{
		var nameParts = timezone.Id.Split("/");
		stringBuilder.AppendLine($"{indent}/// <summary>{derpzones.FirstOrDefault(x => x.Equals(timezone))}</summary>");
		stringBuilder.AppendLine($"{indent}{nameParts.FirstOrDefault()}_{nameParts.LastOrDefault()},\n");
	}
	stringBuilder.AppendLine("}\n");

	var list = new List<(string, string, TimeSpan, TimeSpan)>();

	foreach (var timezone in timezones.Where(x => x.Id.Contains("/")))
	{
		var timeBuilder = new StringBuilder();
		var idSegments = timezone.Id.Split("/");

		//timeBuilder.Append(idSegments.First());

		(string, string, TimeSpan, TimeSpan) temp = new();

		if (idSegments.Length == 2)
		{
			temp.Item1 = idSegments.First();
			temp.Item2 = idSegments.Last();
			temp.Item3 = timezone.MaxOffset.ToTimeSpan();
			temp.Item4 = timezone.MinOffset.ToTimeSpan();

			list.Add(temp);
		}
	}

	list.Select(x => x.Item1).Distinct().Dump();

	list.Dump();

	//var typedTimezones = timezones.Select(x => new {RootClassName = x[0]}).Dump();




	timezones.Dump();


}


void AddAttributes()
{
	stringBuilder.AppendLine("public class CoordinatesAttribute : Attribute");
	stringBuilder.AppendLine("{");
	stringBuilder.AppendLine(indent + "public Vector2 Coordinates { get; }");
	stringBuilder.AppendLine(indent + "public CoordinatesAttribute(float latitude, float longitude) => Coordinates = new Vector2(latitude, longitude);");
	stringBuilder.AppendLine("}");
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
}
