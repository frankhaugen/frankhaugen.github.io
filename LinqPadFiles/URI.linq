<Query Kind="Statements">
  <NuGetReference>UriBuilder.Fluent</NuGetReference>
  <Namespace>System.Globalization</Namespace>
</Query>

var uri = new Uri("https://vg.no/home");

uri.Segments.Append("away");

uri.Segments.Dump();
uri.Dump();

uri = new Uri(uri, "away");
uri = new Uri(uri, "gone");
uri.Dump();

var one = new UriBuilder()
	.WithPathSegment("v1")
	.WithPathSegment("API")
	.WithParameter("Age", "11")
	.WithHost("_")
	.Uri;