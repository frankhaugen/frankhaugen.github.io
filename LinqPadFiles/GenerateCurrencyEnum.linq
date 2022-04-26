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
		enumValues.Add(regionInfo.ISOCurrencySymbol);
	}
	catch (Exception ex)
	{
	}
}

var orderedCurrencies = enumValues.Distinct().OrderBy(x => x).Where(x => x.Count() == 3);
var result = new List<string>();

for (int i = 0; i < orderedCurrencies.Count(); i++)
{
	var currency = orderedCurrencies.ElementAt(i);
	var value = $"{currency} = {i},";
	result.Add(value);
}

string.Join("\n", result).Dump();