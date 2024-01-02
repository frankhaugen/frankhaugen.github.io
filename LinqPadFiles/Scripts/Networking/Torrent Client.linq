<Query Kind="Statements">
  <NuGetReference Prerelease="true">TorrentCore</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>TorrentCore</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>TorrentCore.Modularity</Namespace>
  <Namespace>TorrentCore.Extensions.ExtensionProtocol</Namespace>
  <Namespace>TorrentCore.Extensions.PeerExchange</Namespace>
  <Namespace>TorrentCore.Extensions.SendMetadata</Namespace>
</Query>

// Specify the path where you want to save the downloaded torrent file
string downloadsPath = @"D:\torrents\downloads";
//string torrentsPath = @"D:\torrents";
string torrentPath = @"D:\torrents\Star.Trek.Lower.Decks.S04E08.1080p.WEB.h264-ETHEL[eztv.re].mkv.torrent";

// Specify the URL of the torrent file you want to download
//string torrentUrl = "https://zoink.ch/torrent/Star.Trek.Lower.Decks.S04E01.1080p.WEB.h264-ETHEL[eztv.re].mkv.torrent";
//var fileData = await Downloader.DownloadAsync(torrentUrl);
//await File.WriteAllBytesAsync(torrentPath, fileData);




var client = TorrentClientBuilder
	.CreateDefaultBuilder()
	.ConfigureServices(x => {
		x.AddLogging(y => y.ClearProviders().AddProvider(new LinqPadLoggerProvider()));
		x.AddScoped<IModule>(s =>
				{
					// TODO: Handle construction of message handlers inside ExtensionProtocolModule
					var extensionProtocolModule = ActivatorUtilities.CreateInstance<ExtensionProtocolModule>(s);
					extensionProtocolModule.RegisterMessageHandler(ActivatorUtilities.CreateInstance<PeerExchangeMessageHandler>(s));
					extensionProtocolModule.RegisterMessageHandler(ActivatorUtilities.CreateInstance<MetadataMessageHandler>(s));
					return extensionProtocolModule;
				});
	})
	.AddTcpTransportProtocol()
	.AddDefaultPipeline()
	.Build();
var download = client.Add(torrentPath, downloadsPath);
download.Start();

while (download.State != DownloadState.Seeding)
{
	await Task.Delay(1000);

	Console.WriteLine(download.Progress.ToString("P"));
	Console.WriteLine(download.State.ToString());
}

await download.WaitForDownloadCompletionAsync();

Console.WriteLine("Download complete!");