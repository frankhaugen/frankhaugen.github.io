<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures).OrderBy(x => x.Name);
var enumValues = new List<string>();

foreach (var culture in cultures)
{
	try 
	{
		var regionInfo = new RegionInfo(culture.Name);
		enumValues.Add(regionInfo.TwoLetterISORegionName);
	}
	catch (Exception ex)
	{
	}
}

var ordered = enumValues.Distinct().OrderBy(x => x).Where(x => x.All(Char.IsLetter));
var result = new List<string>();

for (int i = 0; i < ordered.Count(); i++)
{
	var value = ordered.ElementAt(i);
	var line = $"{value} = {i},";
	result.Add(line);
}

string.Join("\n", result).Dump();