<Query Kind="Statements">
  <NuGetReference Version="1.0.30">MonoTorrent</NuGetReference>
  <Namespace>MonoTorrent</Namespace>
  <Namespace>MonoTorrent.BEncoding</Namespace>
  <Namespace>MonoTorrent.Client</Namespace>
  <Namespace>MonoTorrent.Client.Connections</Namespace>
  <Namespace>MonoTorrent.Client.Listeners</Namespace>
  <Namespace>MonoTorrent.Client.PiecePicking</Namespace>
  <Namespace>MonoTorrent.Client.PieceWriters</Namespace>
  <Namespace>MonoTorrent.Client.PortForwarding</Namespace>
  <Namespace>MonoTorrent.Client.Tracker</Namespace>
  <Namespace>MonoTorrent.Dht</Namespace>
  <Namespace>MonoTorrent.Dht.Listeners</Namespace>
  <Namespace>MonoTorrent.Streaming</Namespace>
  <Namespace>MonoTorrent.TorrentWatcher</Namespace>
  <Namespace>MonoTorrent.Tracker</Namespace>
  <Namespace>MonoTorrent.Tracker.Listeners</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

(string ProgressBar , ConcurrentBag<string>  Messages) output = new (){ ProgressBar = "", Messages = new ConcurrentBag<string>() };

// Specify the path where you want to save the downloaded torrent file

string torrentsPath = @"D:\torrents\";
string downloadsPath = Path.Combine(torrentsPath, "downloads");
string torrentName = @"C:\Users\frank\Downloads\Star.Trek.Lower.Decks.S04.1080p.x265-ELiTE.torrent";
string torrentPath = Path.Combine(torrentsPath, torrentName);
//var torrentDownloadBaseUrl = "https://zoink.ch/torrent/";
//var torrentDownloadUrl = torrentDownloadBaseUrl + torrentName;
//
//var file = await Downloader.DownloadAsync(torrentDownloadUrl);
//
//File.WriteAllBytes(torrentPath, file);

//https://zoink.ch/torrent/star.trek.lower.decks.s04e03.multi.1080p.web.h264-higgsboson[eztv.re].mkv.torrent

var torrent = Torrent.Load(torrentPath);
var manager = new TorrentManager(torrent, downloadsPath);

// Create a new ClientEngine instance
var settings = new EngineSettings();
var engine = new ClientEngine(settings);

// Register events for download progress and completion
manager.PieceHashed += (o, e) =>
{
	var progress = (int)(manager.Progress);
	Util.Progress = progress;
	var progressBar = GetProgressBar(progress);
	var status = $"Piece {e.PieceIndex} hashed. Progress: {progress}%";
	OutputStatus(status, progressBar);
};

manager.TorrentStateChanged += async (o, e) =>
{
	var status = $"State changed from {e.OldState} to {e.NewState}";
	OutputStatus(status);
	if (e.NewState == TorrentState.Seeding)
	{
		status = "Download completed!";
		OutputStatus(status);
		// Optionally, stop the torrent or the engine
		//await manager.StopAsync();
		//engine.Dispose();
	}
};

// Add the torrent to the engine and start it
engine.Register(manager).Wait();
manager.StartAsync().Wait();

// Helper method to output status with optional progress bar
void OutputStatus(string status, string progressBar = null)
{
	var coloredStatus = $"\u001b[32m{status}\u001b[0m"; // Green color
	if (progressBar != null)
	{
		output.ProgressBar = $"{coloredStatus}\n{progressBar}";
	}
	else
	{
		output.Messages.Add(coloredStatus);
	}

	Util.ClearResults();

	var bar = new Util.ProgressBar()
	{
		Percent = (int)Util.Progress,
		Caption = Util.Progress.ToString()
	}.Dump();
	//output.ProgressBar.Dump();
	output.Messages.ToList().ForEach(m => $"    {m}".Dump());
}

// Helper method to generate a progress bar
string GetProgressBar(int progress)
{
	const int progressBarWidth = 50;

	// Ensure progress is within valid range
	if (progress < 0)
		progress = 0;
	else if (progress > 100)
		progress = 100;

	int completedWidth = (int)(progressBarWidth * (progress / 100.0));
	int remainingWidth = progressBarWidth - completedWidth;
	return $"[{new string('#', completedWidth)}{new string('-', remainingWidth)}] {progress}%";
}