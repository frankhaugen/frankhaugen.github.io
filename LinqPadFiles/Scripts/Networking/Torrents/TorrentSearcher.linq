<Query Kind="Program">
  <NuGetReference>HtmlAgilityPack</NuGetReference>
  <NuGetReference>System.ServiceModel.Syndication</NuGetReference>
  <Namespace>HtmlAgilityPack</Namespace>
  <Namespace>System.ServiceModel.Syndication</Namespace>
</Query>






void Main()
{
	var searcher = new TorrentSearcher(new EztvScraper());
	var search = "Star Trek Lower Decks";
	
	var result = searcher.Search(search);
	
	result.Dump();
}

public class TorrentSearcher
{
	private readonly IEnumerable<IScraper> _scrapers;

	public TorrentSearcher(params IScraper[] scrapers)
	{
		_scrapers = scrapers;
	}
	
	public TorrentSearcher(IEnumerable<IScraper> scrapers)
	{
		_scrapers = scrapers;
	}
	
	public IEnumerable<string> Search(string search)
	{
		return _scrapers.SelectMany(scraper => scraper.GetTorrentLinks(search));
	}
}

public class EztvScraper : IScraper
{
	public IEnumerable<string> GetTorrentLinks(string query)
	{
		var url = $"https://eztv.re/search/{query.Replace(" ", "-")}";
		var web = new HtmlWeb();
		var doc = web.Load(url);

		var links = doc.DocumentNode.SelectNodes("//a[@href]")
			.Where(node => node.Attributes["href"].Value.EndsWith(".torrent"))
			.Select(node => node.Attributes["href"].Value);

		return links;
	}
}

public interface IScraper
{
	IEnumerable<string> GetTorrentLinks(string query);
}