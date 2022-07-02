<Query Kind="Statements" />

var languages = File.ReadAllLines(@"C:\repos\frankhaugen\frankhaugen.github.io\YouTube\Episodes\001 - LinqPad\programminglanguages.csv");

var stringBuilder = new StringBuilder();

stringBuilder.AppendLine("public enum");
stringBuilder.AppendLine("{");

for (int i = 0; i < languages.Count(); i++)
{
	var language = languages[i];
	var enumName = language.Replace(" ", "");
	enumName = enumName.Replace("#", "Sharp");
	enumName = enumName.Replace("/", "_");
	enumName = enumName.Replace("+", "p");
	enumName = enumName.Replace("-", "");

	var indent = new StringBuilder().Append(" ").Append(" ").Append(" ").Append(" ").ToString();
	stringBuilder.AppendLine($"{indent}{enumName} = {i},");
}

stringBuilder.AppendLine("}");

stringBuilder.Dump();