<Query Kind="Statements">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <Namespace>System.Globalization</Namespace>
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
stringBuilder.AppendLine("public static class RegionExtensions");
stringBuilder.AppendLine("{");
stringBuilder.AppendLine(indent + "public static string GetNativeName(this Region code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(NativeNameAttribute), false)[0] as NativeNameAttribute)?.LocalName ?? string.Empty;");
stringBuilder.AppendLine(indent + "public static string GetEnglishName(this Region code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(EnglishNameAttribute), false)[0] as EnglishNameAttribute)?.EnglishName ?? string.Empty;");
stringBuilder.AppendLine("}");

stringBuilder.AppendLine("");

// Create Enum
stringBuilder.AppendLine("///<summary>Contains a list of Two-letter country (region) codes stored in .net BCL. Enum integer value is based on universal GeoId to support a standardized numbersequence compatible with CultureInfo</summary>");
stringBuilder.AppendLine("public enum Region");
stringBuilder.AppendLine("{");

var regions = GetRegions(GetCultures());

var codes = new List<string>();

foreach (var region in regions)
{
	stringBuilder.AppendLine($"{indent}/// <summary>{region.Value.EnglishName}</summary>");
	stringBuilder.AppendLine($"{indent}[NativeName(\"{region.Value.NativeName}\")]");
	stringBuilder.AppendLine($"{indent}[EnglishName(\"{region.Value.EnglishName}\")]");
	stringBuilder.AppendLine($"{indent}[Description(\"{region.Value.EnglishName}\")]");
	stringBuilder.AppendLine($"{indent}{region.Value.Name} = {region.Value.GeoId},\n");
}
stringBuilder.AppendLine("}\n");

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

var text = stringBuilder.ToString();
var syntaxTree = CSharpSyntaxTree.ParseText(text);
var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
var enums = root.Members.OfType<EnumDeclarationSyntax>();
var types = root.Members.OfType<TypeDeclarationSyntax>();

foreach (var element in types)
{
	WriteFile( element.Identifier.ToString(), element.ToFullString());
}
foreach (var element in enums)
{
	WriteFile(element.Identifier.ToString(), element.ToFullString());
}

void WriteFile(string identifier, string code) => File.WriteAllText(Path.Combine(outputDirectory.FullName, identifier + ".cs"), $"namespace {@namespace};\n" + code);