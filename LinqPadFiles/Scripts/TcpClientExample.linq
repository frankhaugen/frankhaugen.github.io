<Query Kind="Statements">
  <NuGetReference>Frank.BedrockSlim.Client</NuGetReference>
  <Namespace>Frank.BedrockSlim.Client</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
</Query>

  var builder = Host.CreateEmptyApplicationBuilder(new ());

builder.Services.AddTcpClient();

builder.Logging.AddConsole();
  
  var app = builder.Build();
  
  var client = app.Services.GetRequiredService<ITcpClient>();


//var hostEntry = await Dns.GetHostEntryAsync("irc.libera.chat");
//hostEntry.Dump();
//var ipAddress = hostEntry.AddressList[4];



var text = "PING\r\n";
var data = Encoding.UTF8.GetBytes(text);

var response = await client.SendAsync(IPAddress.Parse("127.0.0.1"), 6667, data);

Encoding.UTF8.GetString(response.ToArray()).Dump();