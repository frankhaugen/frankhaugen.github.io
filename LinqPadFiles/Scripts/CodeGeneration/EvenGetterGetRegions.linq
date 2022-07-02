<Query Kind="Expression">
  <Namespace>System.Globalization</Namespace>
</Query>

CultureInfo.GetCultures(CultureTypes.AllCultures)
			.Where(x => !x.IsNeutralCulture)
			.Where(x => x.Name != "")
			.Select(x => new RegionInfo(x.LCID))
			.OrderByDescending(x => x.Name)
			.DistinctBy(x => x.Name)
			.OrderBy(x => x.Name)
			.Where(x => x.Name.All(y => Char.IsLetter(y)))