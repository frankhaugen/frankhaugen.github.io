<Query Kind="Expression">
  <Namespace>System.Globalization</Namespace>
</Query>

CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures)
			.Select(x => new {Name = x.Name.ToUpperInvariant(), x.TwoLetterISOLanguageName})
			.DistinctBy(x => x.Name)
			.OrderBy(x => x.Name)
			//.Where(x => x.ThreeLetterISORegionName.Any())