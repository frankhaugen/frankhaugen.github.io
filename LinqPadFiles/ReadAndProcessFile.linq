<Query Kind="Statements" />

var languages = File.ReadAllLines(@"C:\repos\frankhaugen\frankhaugen.github.io\YouTube\Episodes\001 - LinqPad\programminglanguages.csv");

var stringBuilder = new StringBuilder();

for (int i = 0; i < languages.Count(); i++)
{
	var language = languages[i];
	var enumName = language.Replace(" ", "");
	enumName = enumName.Replace("#", "Sharp");
	enumName = enumName.Replace("/", "_");
	enumName = enumName.Replace("+", "p");

	stringBuilder.AppendLine($"{enumName}");
}

stringBuilder.Dump();