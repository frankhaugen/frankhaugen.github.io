<Query Kind="Statements" />

var cultures = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures)
.DistinctBy(x => x.TwoLetterISOLanguageName)
.ToArray();

var stringBuilder = new StringBuilder();

stringBuilder.AppendLine("public enum Culture");
stringBuilder.AppendLine("{");

for (int i = 0; i < cultures.Count(); i++)
{
	var culture = cultures[i];
	var indent = new StringBuilder()
		.Append(" ")
		.Append(" ")
		.Append(" ")
		.Append(" ")
		.ToString();

	stringBuilder.AppendLine($"{indent}///<summary>{culture.EnglishName}</summary>");
	stringBuilder.AppendLine($"{indent}[Description(\"{culture.NativeName}\")]");
	stringBuilder.AppendLine($"{indent}{culture.TwoLetterISOLanguageName.ToUpperInvariant()} = {i},\n");
}

stringBuilder.AppendLine("}");

stringBuilder.Dump();