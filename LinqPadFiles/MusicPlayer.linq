<Query Kind="Statements">
  <NuGetReference>ID3</NuGetReference>
  <NuGetReference>LINQPadQueryPlanVisualizer</NuGetReference>
  <NuGetReference>NAudio.Asio</NuGetReference>
  <NuGetReference>NAudio.Midi</NuGetReference>
  <NuGetReference>NAudio.Wasapi</NuGetReference>
  <NuGetReference>NAudio.WinMM</NuGetReference>
  <Namespace>Id3</Namespace>
  <Namespace>Id3.Frames</Namespace>
  <Namespace>Id3.InfoFx</Namespace>
  <Namespace>Id3.v2</Namespace>
  <Namespace>NAudio</Namespace>
  <Namespace>NAudio.Codecs</Namespace>
  <Namespace>NAudio.CoreAudioApi</Namespace>
  <Namespace>NAudio.CoreAudioApi.Interfaces</Namespace>
  <Namespace>NAudio.Dmo</Namespace>
  <Namespace>NAudio.Dmo.Effect</Namespace>
  <Namespace>NAudio.Dsp</Namespace>
  <Namespace>NAudio.FileFormats.Mp3</Namespace>
  <Namespace>NAudio.FileFormats.Wav</Namespace>
  <Namespace>NAudio.MediaFoundation</Namespace>
  <Namespace>NAudio.Midi</Namespace>
  <Namespace>NAudio.Mixer</Namespace>
  <Namespace>NAudio.SoundFont</Namespace>
  <Namespace>NAudio.Utils</Namespace>
  <Namespace>NAudio.Wave</Namespace>
  <Namespace>NAudio.Wave.Asio</Namespace>
  <Namespace>NAudio.Wave.Compression</Namespace>
  <Namespace>NAudio.Wave.SampleProviders</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var file = new FileInfo(@"\\PERSONALCLOUD\Public\AudioBooks\Harry Potter Audio Books 1-7; Read by Jim Dale [MP3]\Book 01 - Harry Potter And The Sorcerer's Stone\Chapter 01 - The Boy Who Lived.mp3");
var directory = new DirectoryInfo(@"\\PERSONALCLOUD\Public\");

//directory.EnumerateFiles("*.mp4", SearchOption.AllDirectories).Take(100).Dump();
//directory.EnumerateFileSystemInfos().Dump();

var player = new Player(file);

//await player.Play();

//Thread.Sleep(3000);

//await player.Stop();

var ntfsDrives = DriveInfo.GetDrives().Where(d => d.DriveFormat == "NTFS").ToList();

ntfsDrives.Dump();

record AudioFile(FileInfo File);

class Player
{
	private readonly FileInfo _file;
	private readonly WaveOutEvent _output;
	private readonly MediaFoundationReader _audioFile;
	private readonly Mp3 _mp3;
	
	public Player(FileInfo file)
	{
		if(!file.Exists) throw new FileNotFoundException();

		_file = file;

		_mp3 = new Mp3(_file);
		
			
		_output = new WaveOutEvent();
		_audioFile = new MediaFoundationReader(_file.FullName);
	}

	public async Task Stop() => _output.Stop();
	
	public async Task Play()
	{
		//Task.Factory.StartNew(() =>
		Task.Run(() =>
		{
			_output.Init(_audioFile);
			_output.Play();
			while (_output.PlaybackState == PlaybackState.Playing) { Thread.Sleep(1000); }

			//Thread.Sleep(TimeSpan.FromMilliseconds(_audioFile.Length));
		});
		
		//while (_output.PlaybackState == PlaybackState.Playing) { Thread.Sleep(1000); }
	}
	
	
}