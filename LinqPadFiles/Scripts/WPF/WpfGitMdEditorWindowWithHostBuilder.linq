<Query Kind="Statements">
  <NuGetReference>AvalonEdit</NuGetReference>
  <NuGetReference>Extended.Wpf.Toolkit</NuGetReference>
  <NuGetReference>LibGit2Sharp</NuGetReference>
  <NuGetReference>LiveCharts.Wpf</NuGetReference>
  <NuGetReference>Markdig</NuGetReference>
  <NuGetReference>OxyPlot.Wpf</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Windows.Controls</Namespace>
  <Namespace>LibGit2Sharp</Namespace>
  <Namespace>ICSharpCode.AvalonEdit</Namespace>
  <Namespace>Markdig</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.Hosting.Internal</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>System.ComponentModel</Namespace>
</Query>


var builder = AppUtil.GetBuiler();

builder.ConfigureServices((context, services) =>
{
	services.AddSingleton<GitConfig>(new GitConfig
	{
		Branch = "main",
		RepoPath = "https://github.com/frankhaugen/Frank.Notes.git",
		Username = "frank.haugen@gmail.com",
		Password = "54VpIo6fGg",
		CommitterEmail = "frank.haugen@gmail.com",
		CommitterName = "Frank R. Haugen",
	});
	services.AddScoped<Application>();
	services.AddScoped<MainWindow>();
	services.AddHostedService<WindowHost<MainWindow>>();
});

var app = builder.Build();

await app.RunAsync(QueryCancelToken);

public class MainWindow : Window
{

	public MainWindow()
	{
		var grid = new Grid();
		var col1 = new ColumnDefinition();
		var col2 = new ColumnDefinition();
		grid.ColumnDefinitions.Add(col1);
		grid.ColumnDefinitions.Add(col2);

		var textEditor = new TextEditor();
		Grid.SetColumn(textEditor, 0);

		textEditor.TextArea.TextEntered += (sender, e) =>
		{
			if (e.Text == "*")
			{
				var caret = textEditor.TextArea.Caret.Offset;
				textEditor.Document.Insert(caret, "*");
				textEditor.TextArea.Caret.Offset = caret;
			}
		};

		var webBrowser = new WebBrowser();
		Grid.SetColumn(webBrowser, 1);

		textEditor.TextChanged += (sender, e) =>
		{
			var markdown = textEditor.Text;
			var html = Markdown.ToHtml(markdown);
			try
			{
				webBrowser.NavigateToString(html);
			}
			catch (Exception ex)
			{
				ex.Message.Dump();
			}
		};

		grid.Children.Add(textEditor);
		grid.Children.Add(webBrowser);

		this.Content = grid;
	}
}

public struct CommitInfo
{
	public string Hash { get; set; }
	public string Message { get; set; }
	public string Author { get; set; }
	public DateTimeOffset Timestamp { get; set; }
}

public class GitConfig
{
	public string RepoPath { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public string Branch { get; set; }
	public string CommitterName { get; set; }
	public string CommitterEmail { get; set; }
}

public class CustomHostApplicationLifetime : IHostApplicationLifetime
{
	private readonly CancellationTokenSource _startedSource = new CancellationTokenSource();

	private readonly CancellationTokenSource _stoppingSource = new CancellationTokenSource();

	private readonly CancellationTokenSource _stoppedSource = new CancellationTokenSource();

	private readonly CancellationTokenSource _linkedSource;

	private readonly CancellationToken _cancellationToken;


	public CustomHostApplicationLifetime(CancellationToken cancellationToken)
	{
		_cancellationToken = cancellationToken;
	
		
		_linkedSource = CancellationTokenSource.CreateLinkedTokenSource(_cancellationToken);
		_linkedSource.Token.Register(StopApplication);
	}

	public CancellationToken ApplicationStarted => _startedSource.Token;
	public CancellationToken ApplicationStopping => _stoppingSource.Token;
	public CancellationToken ApplicationStopped => _stoppedSource.Token;

	public void StopApplication()
	{
		_stoppedSource.Cancel();
		_stoppingSource.Cancel();
	}
}