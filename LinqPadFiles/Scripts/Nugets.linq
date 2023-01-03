<Query Kind="Statements">
  <NuGetReference>RestSharp</NuGetReference>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
  <Namespace>RestSharp</Namespace>
  <RemoveNamespace>System.Collections</RemoveNamespace>
  <RemoveNamespace>System.Data</RemoveNamespace>
  <RemoveNamespace>System.Diagnostics</RemoveNamespace>
  <RemoveNamespace>System.Linq.Expressions</RemoveNamespace>
  <RemoveNamespace>System.Reflection</RemoveNamespace>
  <RemoveNamespace>System.Text</RemoveNamespace>
  <RemoveNamespace>System.Text.RegularExpressions</RemoveNamespace>
  <RemoveNamespace>System.Threading</RemoveNamespace>
  <RemoveNamespace>System.Transactions</RemoveNamespace>
  <RemoveNamespace>System.Xml</RemoveNamespace>
  <RemoveNamespace>System.Xml.Linq</RemoveNamespace>
  <RemoveNamespace>System.Xml.XPath</RemoveNamespace>
</Query>

var client = new RestClient();
var request = PrepareSearchRequest("Math.Units");
var response = await client.ExecuteGetAsync<SearchResponse>(request);

response.Data.TotalHits.Dump();
response.Data.Data.Dump();

var packages = response.Data.Data;
var package = packages.ElementAt(0);

var data = await client.DownloadDataAsync(package.DowloadUrl.GetRequest());

HandleDownload(data, package);

void HandleDownload(byte[]? data, Package package) => File.WriteAllBytes($"C:/temp/{package.FileName}", data!);

RestRequest PrepareSearchRequest(string query, int skip = 0, int take = 10, bool includePreRelease = false)
{
    var request = new RestRequest("https://api-v2v3search-0.nuget.org/query");

    request.AddQueryParameter("q", query);
    request.AddQueryParameter("skip", skip);
    request.AddQueryParameter("take", take);
    request.AddQueryParameter("prerelease", includePreRelease);

    return request;
}

public class SearchResponse
{
    public int TotalHits { get; set; }
    public IEnumerable<Package> Data { get; set; }
}

public class Package
{
    [JsonPropertyName("@id")]
    public string ApiId { get; set; }

    [JsonPropertyName("@package")]
    public string Type { get; set; }

    public string Registration { get; set; }
    public string Id { get; set; }
    public Version Version { get; set; }
    public string Description { get; set; }
    public string Summary { get; set; }
    public string Title { get; set; }
    public Uri IconUrl { get; set; }
    public Uri LicenseUrl { get; set; }
    public Uri ProjectUrl { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public IEnumerable<string> Authors { get; set; }
    public int TotalDownloads { get; set; }
    public bool Verified { get; set; }
    public IEnumerable<PackageVersion> Versions { get; set; }

    [JsonIgnore]
    public Uri DowloadUrl => new Uri($"https://www.nuget.org/api/v2/package/{Id}/{Version}", UriKind.Absolute);
    
    [JsonIgnore]
    public string FileName => $"{Id}.{Version}.nupkg";
}

public class PackageVersion
{
    [JsonPropertyName("@id")]
    public Uri Id { get; set; }
    public Version Version { get; set; }
    public int Downloads { get; set; }
}

public static class RestResponseExtensions
{
    public static FileInfo WriteContentToFile(this RestResponse source, FileInfo file)
    {
        File.WriteAllBytes(file.FullName, source.RawBytes);
        if (!file.Exists) throw new FileNotFoundException("File not found", file.Name);
        return file;
    }
}
public static class UriExtensions
{
    public static RestRequest GetRequest(this Uri source) => new RestRequest(source);
}