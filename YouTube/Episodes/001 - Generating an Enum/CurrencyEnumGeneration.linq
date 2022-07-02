<Query Kind="Statements">
  <NuGetReference>CodegenCS</NuGetReference>
  <Namespace>CodegenCS</Namespace>
  <Namespace>CodegenCS.ControlFlow</Namespace>
  <Namespace>CodegenCS.DotNet</Namespace>
  <Namespace>CodegenCS.Extensions</Namespace>
  <Namespace>CodegenCS.InputModels</Namespace>
  <Namespace>CodegenCS.Utils</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.ComponentModel</Namespace>
</Query>

var @namespace = "Enums";
var enumName = "Currency";

var codeWriter = new CodegenTextWriter();
codeWriter.WriteLine($"using {typeof(DescriptionAttribute).Namespace};");
codeWriter.WriteLine("");
codeWriter.WithCurlyBraces($"namespace {@namespace}", () =>
{
    codeWriter.WriteLine($"// ReSharper disable InconsistentNaming");
    codeWriter.WriteLine($"/// <summary>Three-letter currency codes from RegionInfo -class in the BCL</summary>");
    codeWriter.WithCurlyBraces($"public enum {enumName}", () =>
    {
        var currencies = GetCurrencyInfo();

        for (int i = 0; i < currencies.Count; i++)
        {
            var currency = currencies[i];
            codeWriter.WriteLine($"/// <summary>{currency.Name}</summary>");
            codeWriter.WriteLine($"[Description(\"{currency.Name}\")]");
            codeWriter.Write($"{currency.Code} = {i}");
            if (i < currencies.Count - 1)
            {
                codeWriter.WriteLine(",");
                codeWriter.WriteLine("");
            }
        }
    });
});

codeWriter.ToString().Dump();
codeWriter.SaveToFile(Path.Combine(new FileInfo(Util.CurrentQueryPath).Directory!.FullName, $"{enumName}.cs"));

List<CurrencyInfo> GetCurrencyInfo()
{
    var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

    var regions = cultures.Select(x => new RegionInfo(x.LCID));
    var currencies = regions.Select(x => new CurrencyInfo(x.ISOCurrencySymbol, x.CurrencyEnglishName));

    return currencies
        .DistinctBy(x => x.Code)
        .Where(x => x.Name.Any())
        .OrderBy(x => x.Code)
        .ToList();
}

public readonly record struct CurrencyInfo(string Code, string Name);