<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "ChatGPT\HttpClientWrapper\Url"
#load "ChatGPT\HttpClientWrapper\IUrl"
#load "ChatGPT\HttpClientWrapper\HttpSchema"

#load "xunit"

void Main()
{
    RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.
}

// You can define other methods, fields, classes and namespaces here

#region private::Tests

[Theory]
[InlineData(HttpScheme.Http, "example.com", 81, "/path/to/resource", "key1", "value1", "fragment", "username:password", "http://username:password@example.com:81/path/to/resource?key1=value1#fragment")]
[InlineData(HttpScheme.Http, "example.com", 80, "/path/to/resource", "key1", "value1", "fragment", "username:password", "http://username:password@example.com/path/to/resource?key1=value1#fragment")]
[InlineData(HttpScheme.Https, "example.com", 443, "/path/to/resource", "key1", "value1", "fragment", "username:password", "https://username:password@example.com/path/to/resource?key1=value1#fragment")]
public void TestUrl(HttpScheme scheme, string host, int port, string path, string queryParamsKey, string queryParamsValue, string fragment, string userInfo, string expected)
{

    var url = new Url(host)
        .AddScheme(scheme)
        .AddHost(host)
        .AddPort(port)
        .AddPath(path)
        .AddQueryParam(queryParamsKey, queryParamsValue)
        .AddFragment(fragment)
        .AddUserInfo(userInfo);

    Assert.Equal(expected, url.ToString());
}

[Fact]
public void TestUrlDefaults()
{
    var url = new Url("vg.no");
    url.ToString().Dump();
}

#endregion