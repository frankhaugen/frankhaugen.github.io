<Query Kind="Statements">
  <Connection>
    <ID>15d71fa3-87a3-45ce-b63f-7cc8caac7b0a</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.\SQLEXPRESS</Server>
    <Database>LoggingDatabase</Database>
  </Connection>
  <NuGetReference>RestSharp</NuGetReference>
  <NuGetReference>UriBuilder.Fluent</NuGetReference>
  <Namespace>RestSharp</Namespace>
  <Namespace>RestSharp.Extensions</Namespace>
  <Namespace>RestSharp.Serializers</Namespace>
  <Namespace>RestSharp.Serializers.Json</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

var builder = new UriBuilder()
	.WithHost("vg.no")
	.WithParameter("q", "12345");
	
var uri = builder.Uri;

var options = new RestClientOptions() {};

options.BaseUrl = uri;

var client = new RestClient(options);

var result = await client.GetAsync(new RestRequest());

result.ResponseUri.Dump();