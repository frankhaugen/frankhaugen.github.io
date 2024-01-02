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
</Query>



// Specify the path where you want to save the downloaded torrent file
string downloadsPath = @"D:\torrents\downloads";
//string torrentsPath = @"D:\torrents";
string torrentPath = @"D:\torrents\kali-linux-2023.2-live-everything-amd64.iso.torrent";

var torrent = Torrent.Load(torrentPath);
var manager = new TorrentManager(torrent, downloadsPath);

// Create a new ClientEngine instance
var settings = new EngineSettings();
var engine = new ClientEngine(settings);

// Register events for download progress and completion
manager.PieceHashed += (o, e) => {
  Console.WriteLine("Piece {0} hashed. Progress: {1}%", e.PieceIndex, manager.Progress);
};

manager.TorrentStateChanged += async (o, e) => {
  Console.WriteLine("State changed from {0} to {1}", e.OldState, e.NewState);
  if (e.NewState == TorrentState.Seeding) {
    Console.WriteLine("Download completed!");
		// Optionally, stop the torrent or the engine
		await manager.StopAsync();
		engine.Dispose();
	}
};

// Add the torrent to the engine and start it
await engine.Register(manager);
await manager.StartAsync();
