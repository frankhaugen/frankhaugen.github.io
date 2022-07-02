<Query Kind="Expression">
  <Namespace>System.Globalization</Namespace>
</Query>

CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures)
			.Select(x => new RegionInfo(x.LCID))
			.DistinctBy(x => x.Name)
			.OrderBy(x => x.Name)
			.Where(x => x.ThreeLetterISORegionName.Any())