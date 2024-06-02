<Query Kind="Statements">
  <NuGetReference>AutoMapper</NuGetReference>
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Http</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Http.Polly</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Logging</NuGetReference>
  <NuGetReference>Polly</NuGetReference>
</Query>












record RequestOptions(Uri Endpoint, );

public record Headers
{
	private readonly Dictionary<string, string> _headers = new();

	public bool Exist(string key) => _headers.ContainsKey(key);
	public void Add(string key, string value) => _headers.Add(key, value);
	public void Update(string key, string value) => _headers[key] = value;
	public void Delete(string key) => _headers.Remove(key);
	public KeyValuePair<string, string> Get(string key) => _headers.Single(x => x.Equals(key));
}
public record Segments
{
	private readonly Dictionary<int, string> _segments = new();

	public bool Exist(int key) => _segments.ContainsKey(key);
	public void Add(string value) => _segments.Add(key, value);
	public void Update(int key, string value) => _segments[key] = value;
	public void Delete(int key) => _segments.Remove(key);
	public KeyValuePair<int, string> Get(int key) => _segments.Single(x => x.Key.Equals(key));
}


public interface IUrl
{
	IUrl AppendMyEmployerIdSegments(Guid organizationId, Guid? clientId = null);
	IUrl AppendSegments(params string[] segments);
	IUrl AppendParameters(params KeyValuePair<string, string>[] parameters);
	Uri ToUri();
	string ToString();
}

public class Url : IUrl
{
	private readonly string _baseUrl;
	private readonly StringBuilder _urlBuilder = new();
	private readonly StringBuilder _parametersBuilder = new();

	private Url(string baseUrl = "")
	{
		if (baseUrl.EndsWith('/'))
		{
			baseUrl.Remove(baseUrl.Length);
		}
		_baseUrl = baseUrl;
	}

	public static Url CreateInstance(string baseUrl = "") => new(baseUrl);

	public IUrl AppendMyEmployerIdSegments(Guid organizationId, Guid? clientId = null)
	{
		if (organizationId == Guid.Empty) throw new ArgumentException("Guid cannot be of value 'Empty'", nameof(organizationId));

		_urlBuilder.Append("organizations");
		_urlBuilder.Append('/');
		_urlBuilder.Append(organizationId);

		if (clientId == null || clientId == Guid.Empty) return this;

		_urlBuilder.Append('/');
		_urlBuilder.Append("clients");
		_urlBuilder.Append('/');
		_urlBuilder.Append(clientId);

		return this;
	}

	public IUrl AppendSegments(params string[] segments)
	{
		foreach (var segment in segments)
		{
			if (_urlBuilder.Length > 1 && _urlBuilder[0] != '/')
			{
				_urlBuilder.Append('/');
			}
			_urlBuilder.Append(segment);
		}

		return this;
	}

	public IUrl AppendParameters(params KeyValuePair<string, string>[] parameters)
	{
		if (!parameters.Any()) return this;

		foreach (var parameter in parameters)
		{
			_parametersBuilder.Append(parameter.Key);
			_parametersBuilder.Append('=');
			_parametersBuilder.Append(parameter.Value);
			_parametersBuilder.Append('&');
		}

		return this;
	}

	public Uri ToUri()
	{
		var url = ToString();
		return !string.IsNullOrWhiteSpace(_baseUrl) ? new Uri(url) : new Uri(url, UriKind.Relative);
	}

	public override string ToString()
	{
		var url = _urlBuilder.ToString();

		if (!string.IsNullOrWhiteSpace(_baseUrl)) url = _baseUrl + (!_baseUrl.EndsWith("/") ? "/" : "") + url;

		if (_parametersBuilder.Length > 0)
		{
			url += "?" + _parametersBuilder;
		}

		if (url.EndsWith('&')) url = url.TrimEnd('&');

		return url;
	}
}