<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures).OrderBy(x => x.Name);
var countries = new List<Country>();
var counter = 0;
foreach (var culture in cultures)
{
	var info = new Dictionary<string, string>();

	info.Add(nameof(Country.CultureName), culture.Name);
	info.Add(nameof(Country.TwoLetterLanguageCode), culture.TwoLetterISOLanguageName.ToUpperInvariant());
	info.Add(nameof(Country.ThreeLetterLanguageCode), culture.ThreeLetterISOLanguageName.ToUpperInvariant());
	info.Add(nameof(Country.LanguageName), culture.EnglishName);
	info.Add(nameof(Country.LocalLanguageName), culture.NativeName);
	info.Add(nameof(Country.CalenderName), culture.Calendar.AlgorithmType.ToString());

	try
	{
		var regionInfo = new RegionInfo(culture.Name);
		info.Add(nameof(Country.Name), regionInfo.EnglishName);
		info.Add(nameof(Country.LocalName), regionInfo.NativeName);
		info.Add(nameof(Country.CountryCode), regionInfo.TwoLetterISORegionName);
		info.Add(nameof(Country.Currency), regionInfo.CurrencyEnglishName);
		info.Add(nameof(Country.CurrencyCode), regionInfo.ISOCurrencySymbol);
	}
	catch (Exception ex)
	{
		info.Add(nameof(Country.Name), "");
		info.Add(nameof(Country.CountryCode), "");
		info.Add(nameof(Country.LocalName), "");
		info.Add(nameof(Country.Currency), "");
		info.Add(nameof(Country.CurrencyCode), "");
	}

	countries.Add(new Country(
		counter,
		info[nameof(Country.CultureName)],
		info[nameof(Country.Name)],
		info[nameof(Country.LocalName)],
		info[nameof(Country.CountryCode)],
		info[nameof(Country.LanguageName)],
		info[nameof(Country.LocalLanguageName)],
		info[nameof(Country.TwoLetterLanguageCode)],
		info[nameof(Country.ThreeLetterLanguageCode)],
		info[nameof(Country.Currency)],
		info[nameof(Country.CurrencyCode)],
		info[nameof(Country.CalenderName)]
	));

	counter++;
}

var countryCodeEnumValues = new List<string>();

foreach (var country in countries.DistinctBy(x => x.Name))
{
	try
	{
		var countryName = country.Name.Replace(",", "").Replace("’", "").Replace(".", "").Replace("&", "And").Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");

		if (string.IsNullOrWhiteSpace(countryName)) countryName = "InvariantCountry";

		var value = $"{countryName} = {country.CultureId},";
		countryCodeEnumValues.Add(value);
	}
	catch (Exception ex)
	{
	}
}

//string.Join("\n", countryCodeEnumValues).Dump();

var countryNameEnumValues = new List<string>();

foreach (var country in countries.DistinctBy(x => x.Name))
{
	try
	{
		var countryName = country.Name.Replace(",", "").Replace("’", "").Replace(".", "").Replace("&", "And").Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");

		if (string.IsNullOrWhiteSpace(countryName)) countryName = "InvariantCountry";

		var value = $"{countryName} = {country.CultureId},";
		countryNameEnumValues.Add(value);
	}
	catch (Exception ex)
	{
	}
}

//string.Join("\n", countryNameEnumValues).Dump();

countries.OrderBy(x => x.CultureName).Dump();

record Country(int CultureId, string CultureName, string Name, string LocalName, string CountryCode, string LanguageName, string LocalLanguageName, string TwoLetterLanguageCode, string ThreeLetterLanguageCode, string Currency, string CurrencyCode, string CalenderName);