<Query Kind="Statements">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <NuGetReference>Raylib-cs</NuGetReference>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

var queryPath = Util.CurrentQueryPath;

var queryFile = new FileInfo(queryPath); 
var queryDirectory = queryFile.Directory;
var csvFile = queryFile.Directory?.EnumerateFiles("*.csv").First();

var csvData = File.ReadAllText(csvFile?.FullName!);

var stars = CsvUtil.ReadCsvData<Star, StarClassMap>(csvData, ",");

// Raylib rendering




public class Star
{
	public int Id { get; set; }
	public int? Hip { get; set; }
	public int? Hd { get; set; }
	public int? Hr { get; set; }
	public string Gl { get; set; }
	public string Bf { get; set; }
	public string Proper { get; set; }
	public decimal Ra { get; set; }
	public decimal Dec { get; set; }
	public decimal Dist { get; set; }
	public decimal Pmra { get; set; }
	public decimal Pmdec { get; set; }
	public decimal Rv { get; set; }
	public decimal Mag { get; set; }
	public decimal Absmag { get; set; }
	public string? Spect { get; set; }
	public string Ci { get; set; }
	public decimal X { get; set; }
	public decimal Y { get; set; }
	public decimal Z { get; set; }
	public decimal Vx { get; set; }
	public decimal Vy { get; set; }
	public decimal Vz { get; set; }
	public decimal Rarad { get; set; }
	public decimal Decrad { get; set; }
	public decimal Pmrarad { get; set; }
	public decimal Pmdecrad { get; set; }
	public string Bayer { get; set; }
	public int? Flam { get; set; }
	public string Con { get; set; }
	public int Comp { get; set; }
	public int CompPrimary { get; set; }
	public string Base { get; set; }
	public decimal Lum { get; set; }
	public string Var { get; set; }
	public string VarMin { get; set; }
	public string VarMax { get; set; }
}

public class StarClassMap : ClassMap<Star>
{
	public StarClassMap()
	{
		Map(m => m.Id).Name("id");
		Map(m => m.Hip).Name("hip");
		Map(m => m.Hd).Name("hd");
		Map(m => m.Hr).Name("hr");
		Map(m => m.Gl).Name("gl");
		Map(m => m.Bf).Name("bf");
		Map(m => m.Proper).Name("proper");
		Map(m => m.Ra).Name("ra");
		Map(m => m.Dec).Name("dec");
		Map(m => m.Dist).Name("dist");
		Map(m => m.Pmra).Name("pmra");
		Map(m => m.Pmdec).Name("pmdec");
		Map(m => m.Rv).Name("rv");
		Map(m => m.Mag).Name("mag");
		Map(m => m.Absmag).Name("absmag");
		Map(m => m.Spect).Name("spect");
		Map(m => m.Ci).Name("ci");
		Map(m => m.X).Name("x");
		Map(m => m.Y).Name("y");
		Map(m => m.Z).Name("z");
		Map(m => m.Vx).Name("vx");
		Map(m => m.Vy).Name("vy");
		Map(m => m.Vz).Name("vz");
		Map(m => m.Rarad).Name("rarad");
		Map(m => m.Decrad).Name("decrad");
		Map(m => m.Pmrarad).Name("pmrarad");
		Map(m => m.Pmdecrad).Name("pmdecrad");
		Map(m => m.Bayer).Name("bayer");
		Map(m => m.Flam).Name("flam");
		Map(m => m.Con).Name("con");
		Map(m => m.Comp).Name("comp");
		Map(m => m.CompPrimary).Name("comp_primary");
		Map(m => m.Base).Name("base");
		Map(m => m.Lum).Name("lum");
		Map(m => m.Var).Name("var");
		Map(m => m.VarMin).Name("var_min");
		Map(m => m.VarMax).Name("var_max");
	}
}
