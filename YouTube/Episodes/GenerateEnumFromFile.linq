<Query Kind="Statements" />

var languages = File.ReadAllLines(@"C:\repos\frankhaugen\frankhaugen.github.io\YouTube\Episodes\001 - LinqPad\programminglanguages.csv");

var stringBuilder = new StringBuilder();

stringBuilder.AppendLine("public enum Language");
stringBuilder.AppendLine("{");

for (int i = 0; i < languages.Count(); i++)
{
	var language = languages[i];
	var enumName = language
		.Replace(" ", "")
		.Replace("#", "Sharp")
		.Replace("/", "_")
		.Replace("+", "p")
		.Replace("-", "");

	var indent = new StringBuilder()
		.Append(" ")
		.Append(" ")
		.Append(" ")
		.Append(" ")
		.ToString();

	stringBuilder.AppendLine($"{indent}///<summary>{language}</summary>");
	stringBuilder.AppendLine($"{indent}[Description(\"{language}\")]");
	stringBuilder.AppendLine($"{indent}{enumName} = {i},\n");
}

stringBuilder.AppendLine("}");

stringBuilder.Dump();