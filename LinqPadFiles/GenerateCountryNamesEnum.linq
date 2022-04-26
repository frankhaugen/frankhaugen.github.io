<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures).OrderBy(x => x.Name);
var countries = new Dictionary<string, string>();
var enumValues = new List<string>();

foreach (var culture in cultures)
{
	try
	{
		var regionInfo = new RegionInfo(culture.Name);
		countries.TryAdd(regionInfo.EnglishName.Replace(",", "").Replace("’", "").Replace(".", "").Replace("&", "And").Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", ""), regionInfo.NativeName);
	}
	catch (Exception ex)
	{
	}
}

var orderedCountries = countries.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

for (int i = 0; i < orderedCountries.Count; i++)
{
	var countryKvp = orderedCountries.ElementAt(i);
	var value = $"{countryKvp.Key} = {i},";
	enumValues.Add(value);
}

string.Join("\n", enumValues).Dump();