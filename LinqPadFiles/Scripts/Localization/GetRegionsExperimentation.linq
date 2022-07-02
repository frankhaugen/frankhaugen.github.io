<Query Kind="Expression">
  <Namespace>System.Globalization</Namespace>
</Query>

CultureInfo.GetCultures(CultureTypes.AllCultures & CultureTypes.NeutralCultures)
			.Select(x => new {Name = x.Name.ToUpperInvariant().Replace("-", "_"), Code = x.TwoLetterISOLanguageName.ToUpper(), x.EnglishName, x.NativeName x.})
			.Where(x => !x.Name.Contains("_"))
			.DistinctBy(x => x.Name)
			.OrderBy(x => x.Name)
