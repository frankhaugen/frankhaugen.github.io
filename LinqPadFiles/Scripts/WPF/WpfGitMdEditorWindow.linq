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


var serviceProvider = new ServiceCollection()
				.AddSingleton<GitConfig>(new GitConfig
				{
					Branch = "main",
					RepoPath = "https://github.com/frankhaugen/Frank.Notes.git",
					Username = "frank.haugen@gmail.com",
					Password = "54VpIo6fGg",
					CommitterEmail = "frank.haugen@gmail.com",
					CommitterName = "Frank R. Haugen",
				})
				.AddSingleton<IHostApplicationLifetime>(new CustomHostApplicationLifetime(QueryCancelToken))
				.AddSingleton<IGitService, GitService>()
				.BuildServiceProvider();

var mainWindow = ActivatorUtilities.CreateInstance<MainWindow>(serviceProvider);
new Application().Run(mainWindow);


public class MainWindow : Window
{
	private readonly IGitService _gitService;
	private readonly IHostApplicationLifetime _hostApplicationLifetime;

	protected override void OnClosing(CancelEventArgs e)
	{
		_hostApplicationLifetime.StopApplication();
		base.OnClosing(e);
	}

	public MainWindow(IGitService gitService, IHostApplicationLifetime hostApplicationLifetime)
	{
		_gitService = gitService;
		_hostApplicationLifetime = hostApplicationLifetime;

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

public class GitService : IGitService
{
	private readonly GitConfig _config;

	public GitService(GitConfig config)
	{
		_config = config;
	}

	public void CommitAndPush(FileInfo file, string content)
	{
		File.WriteAllText(file.FullName, content);

		using (var repo = new Repository(_config.RepoPath))
		{
			Commands.Stage(repo, file.FullName);

			var author = new Signature(_config.CommitterName, _config.CommitterEmail, DateTimeOffset.Now);
			repo.Commit($"Updated {file}", author, author);

			var options = new PushOptions
			{
				CredentialsProvider = (_url, _user, _cred) =>
					new UsernamePasswordCredentials { Username = _config.Username, Password = _config.Password }
			};

			repo.Network.Push(repo.Branches[_config.Branch], options);
		}
	}

	public IEnumerable<CommitInfo> GetFileCommitHistory(FileInfo file)
	{
		var commitInfos = new List<CommitInfo>();


		using (var repo = new Repository(_config.RepoPath))
		{
			var filter = new CommitFilter
			{
				SortBy = CommitSortStrategies.Time,
			};

			foreach (var commit in repo.Commits.QueryBy(filter))
			{
				if (commit.Tree[file.FullName] != null && commit.Parents.Count() > 0 && commit.Parents.First().Tree[file.FullName] == null)
				{
					var commitInfo = new CommitInfo
					{
						Message = commit.Message,
						Author = commit.Author.Name,
						Timestamp = commit.Author.When
					};
					commitInfos.Add(commitInfo);
				}
			}
		}

		return commitInfos;
	}

	public void CreateCommitFromHash(FileInfo file, string commitHash)
	{
		using (var repo = new Repository(_config.RepoPath))
		{
			var commit = repo.Commits.FirstOrDefault(c => c.Id.Sha == commitHash);
			if (commit == null) return;

			var blob = commit.Tree[file.Name]?.Target as Blob;
			if (blob == null) return;

			var content = blob.GetContentText();
			File.WriteAllText(file.FullName, content);

			CommitAndPush(file, content);
		}
	}
}

public interface IGitService
{
	void CommitAndPush(FileInfo file, string content);
	IEnumerable<CommitInfo> GetFileCommitHistory(FileInfo file);
	void CreateCommitFromHash(FileInfo file, string commitHash);
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