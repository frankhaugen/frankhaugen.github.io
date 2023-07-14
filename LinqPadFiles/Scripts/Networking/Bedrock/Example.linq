<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.AspNetCore.Connections</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>








var builder = Host.CreateApplicationBuilder();

builder.Services.AddServer<MyServerConnectionHandler>(5000);

builder.Services.AddClient<MyClientConnectionHandler>(6000);

var app = builder.Build();

app.Start();