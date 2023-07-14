<Query Kind="Statements">
  <Reference Relative="..\..\..\..\..\Frank.Networking\Frank.Networking\bin\Debug\net7.0\Frank.Networking.dll">C:\repos\frankhaugen\Frank.Networking\Frank.Networking\bin\Debug\net7.0\Frank.Networking.dll</Reference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Frank.Networking.Common</Namespace>
  <Namespace>Frank.Networking.Server</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
</Query>



var builder = Host.CreateApplicationBuilder();

builder.Logging.ClearProviders().AddProvider(new LinqPadLoggerProvider());
builder.Services.AddNetworkServer<MyDataReceivedHandler>(x => {});

var app = builder.Build();

await app.RunAsync(CancellationTokenUtil.Get(TimeSpan.FromSeconds(25)));

public class MyDataReceivedHandler : IOnDataReceivedHandler
{
    public async Task<ReadOnlyMemory<byte>> OnDataReceivedAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken)
    {
        var text = Encoding.UTF8.GetString(data.ToArray());
        
        text.Dump(DateTime.Now.ToString("s"));
        
        return ReadOnlyMemory<byte>.Empty;
    }
}