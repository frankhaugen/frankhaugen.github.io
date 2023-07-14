<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

#load ".\IUrl"
#load ".\HttpSchema"

public class Url : IUrl
{
    public HttpScheme Scheme { get; private set; }
    public string Host { get; private set; }
    public int Port { get; private set; }
    public string Path { get; private set; }
    public IList<KeyValuePair<string, string>> QueryParams { get; private set; } = new List<KeyValuePair<string, string>>();
    public string Fragment { get; private set; }
    public string UserInfo { get; private set; }

    public Url(string host, HttpScheme scheme = HttpScheme.Http, int port = 80)
    {
        AddScheme(scheme);
        AddHost(host);
        AddPort(port);
    }
    
    public IUrl AddScheme(HttpScheme scheme)
    {
        Scheme = scheme;
        return this;
    }

    public IUrl AddHost(string host)
    {
        Host = host;
        return this;
    }

    public IUrl AddPort(int port)
    {
        Port = port;
        return this;
    }

    public IUrl AddPath(string path)
    {
        Path = path;
        return this;
    }

    public IUrl AddQueryParam(string key, string value)
    {
        QueryParams.Add(new KeyValuePair<string, string>(key, value));
        return this;
    }

    public IUrl AddFragment(string fragment)
    {
        Fragment = fragment;
        return this;
    }

    public IUrl AddUserInfo(string userInfo)
    {
        UserInfo = userInfo;
        return this;
    }

    public IUrl AddQueryParams(IList<KeyValuePair<string, string>> queryParams)
    {
        foreach (var queryParam in queryParams)
        {
            QueryParams.Add(queryParam);
        }

        return this;
    }

    public Uri ToUri()
    {
        var builder = new UriBuilder
        {
            Scheme = Scheme.ToString().ToLowerInvariant(),
            Host = Host,
            Port = Port,
            Path = Path,
            Fragment = Fragment
        };

        if (!string.IsNullOrEmpty(UserInfo))
        {
            builder.UserName = UserInfo;
        }

        if (QueryParams.Any())
        {
            builder.Query = string.Join("&", QueryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }

        return builder.Uri;
    }
    
    public string Build()
    {
        return ToUri().ToString();
    }
    
    public override string ToString()
    {
        return Build();
    }
}
