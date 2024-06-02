<Query Kind="Program">
  <NuGetReference>bzTorrent</NuGetReference>
  <NuGetReference>HtmlAgilityPack</NuGetReference>
  <NuGetReference>System.ServiceModel.Syndication</NuGetReference>
  <Namespace>bzTorrent</Namespace>
  <Namespace>bzTorrent.Data</Namespace>
  <Namespace>HtmlAgilityPack</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.ServiceModel.Syndication</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>






async Task Main()
{
	Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
	var tempFilesHelper = new TempFilesHelper();
	var searcher = new TorrentSearcher(new EztvScraper());
	var search = "Star Trek Lower Decks";
	
	var links = searcher.Search(search);
	
	var files = new List<FileInfo>();
	var metadata = new List<IMetadata>();
	
	foreach (var link in links)
	{
		var file = await tempFilesHelper.GetFileFromUriAsync(link);
		files.Add(file);
		metadata.Add(Metadata.FromFile(file.FullName));
	}
	
	var torrent = metadata.First();
	
	// Print tracker URLs
	foreach (var announce in torrent.AnnounceList)
	{
		announce.Dump();  // Using Dump() instead of Console.WriteLine()
	}
	
	torrent.Dump();
	
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
	
	public IEnumerable<Uri> Search(string search)
	{
		return _scrapers.SelectMany(scraper => scraper.GetTorrentLinks(search));
	}
}

public class EztvScraper : IScraper
{
	public IEnumerable<Uri> GetTorrentLinks(string query)
	{
		var url = $"https://eztv.to/search/{query.Replace(" ", "-")}";
		var web = new HtmlWeb();
		var doc = web.Load(url);

		var links = doc.DocumentNode.SelectNodes("//a[@href]")
			.Where(node => node.Attributes["href"].Value.EndsWith(".torrent"))
			.Select(node => node.Attributes["href"].Value);

		return links.Select(link => new Uri(link, UriKind.Absolute));
	}
}

public interface IScraper
{
	IEnumerable<Uri> GetTorrentLinks(string query);
}

public class TempFilesHelper : IDisposable
{
	private readonly string tempDirectory;

	public TempFilesHelper()
	{
		tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
		Directory.CreateDirectory(tempDirectory);
	}

	public async Task<System.IO.FileInfo> GetFileFromUriAsync(Uri uri)
	{
		var httpClient = new HttpClient();
		var response = await httpClient.GetAsync(uri);
		response.EnsureSuccessStatusCode();

		var tempFilePath = Path.Combine(tempDirectory, Path.GetFileName(uri.LocalPath));
		await File.WriteAllBytesAsync(tempFilePath, await response.Content.ReadAsByteArrayAsync());

		return new System.IO.FileInfo(tempFilePath);
	}

	public void Dispose()
	{
		Directory.Delete(tempDirectory, true);
	}
}