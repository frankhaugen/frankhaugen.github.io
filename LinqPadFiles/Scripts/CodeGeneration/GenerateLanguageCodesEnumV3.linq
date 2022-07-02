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
            stringBuilder.AppendLine("public static class LanguageCodeExtensions");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine(indent + "public static string GetNativeName(this LanguageCode code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(NativeNameAttribute), false)[0] as NativeNameAttribute)?.LocalName ?? string.Empty;");
            stringBuilder.AppendLine(indent + "public static string GetEnglishName(this LanguageCode code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(EnglishNameAttribute), false)[0] as EnglishNameAttribute)?.EnglishName ?? string.Empty;");
            stringBuilder.AppendLine("}");


            stringBuilder.AppendLine("");

            // Create Enum
            stringBuilder.AppendLine("///<summary>Contains a list of Two-letter language codes stored in .net BCL. Enum integer value is based on universal GeoId to support a standardized numbersequence</summary>");
            stringBuilder.AppendLine("public enum LanguageCode");
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
                    stringBuilder.AppendLine($"{indent}[Description(\"{culture.TwoLetterISOLanguageName.ToLowerInvariant()}\")]");
		stringBuilder.AppendLine($"{indent}{culture.TwoLetterISOLanguageName.ToUpperInvariant()} = {regionInfo?.GeoId},\n");
		codes.Add(culture.TwoLetterISOLanguageName.ToUpperInvariant());
	}
}
stringBuilder.AppendLine("}\n");
stringBuilder.ToString().Dump();


List<CultureInfo> GetCultures() => CultureInfo.GetCultures(CultureTypes.AllCultures)
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