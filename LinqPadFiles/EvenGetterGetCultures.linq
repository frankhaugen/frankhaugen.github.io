<Query Kind="Expression">
  <Namespace>System.Globalization</Namespace>
</Query>

CultureInfo.GetCultures(CultureTypes.AllCultures)
			.OrderBy(x => x.Name)
			.Where(x => x.TwoLetterISOLanguageName != "iv")
			.Select(x => new { 
				Name = x.Name.Replace("-", "_").Replace("001", "World").ToUpperInvariant(), 
				x.TwoLetterISOLanguageName, 
				x.ThreeLetterISOLanguageName, 
				x.ThreeLetterWindowsLanguageName, 
				x.EnglishName, 
				x.NativeName, 
				x.LCID, 
				x.IsNeutralCulture, 
				Country = x.IsNeutralCulture ? "" : new RegionInfo(x.LCID).Name.ToUpperInvariant() 
			})
