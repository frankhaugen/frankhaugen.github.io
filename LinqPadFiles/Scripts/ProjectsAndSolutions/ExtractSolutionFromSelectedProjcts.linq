<Query Kind="Program">
  <NuGetReference>CliWrap</NuGetReference>
  <NuGetReference>DotNetProjectParser</NuGetReference>
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <NuGetReference>Microsoft.Build</NuGetReference>
  <NuGetReference>Microsoft.Build.Framework</NuGetReference>
  <NuGetReference>Microsoft.Build.Locator</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.CSharp.Workspaces</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.Workspaces.MSBuild</NuGetReference>
  <NuGetReference>SlnParser</NuGetReference>
  <Namespace>CliWrap</Namespace>
  <Namespace>CliWrap.Buffered</Namespace>
  <Namespace>DotNetProjectParser</Namespace>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.MSBuild</Namespace>
  <Namespace>SlnParser.Contracts</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Windows.Controls</Namespace>
  <Namespace>System.Windows.Data</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

async Task Main()
{
	var rootDirectory = new DirectoryInfo(@"D:\repos");
	var outputDirectory = new DirectoryInfo(@"D:\temp");
	
	var ui = new MyUserInterface(rootDirectory, outputDirectory);
	
	ui.Show();
}

public class MyUserInterface
{
	private Window window;
	private TreeView _treeView;
	private Button button;
	private DirectoryInfo _rootDirectory;
	private DirectoryInfo _outputDirectory;

	public MyUserInterface(DirectoryInfo rootDir, DirectoryInfo outputDir)
	{
		_rootDirectory = rootDir;
		_outputDirectory = outputDir;
	}

	public void Show()
	{
		window = new Window() {
			Title = "Project Selector",
			MinWidth = 512,
			SizeToContent = SizeToContent.WidthAndHeight,
		};
		var stackPanel = new StackPanel { Orientation = Orientation.Vertical };
		_treeView = new TreeView()
		{
			IsTextSearchEnabled = true,
			IsTextSearchCaseSensitive = false,
		};
		
		button = new Button { Content = "Create Solution", Height = 30 };

		button.Click += async (sender, args) => await SolutionHelper.CreateSolution(_treeView, _outputDirectory); ;

		TreeViewHelper.PopulateTreeViewFromFoldersGroupByTop(_treeView, _rootDirectory); // Initial population

		var scrollView = new ScrollViewer
		{
			Content = _treeView,
			HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
			VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
			MinHeight = 256,
			MaxHeight = 500,
			CanContentScroll = true
		};

		stackPanel.Children.Add(scrollView);
		stackPanel.Children.Add(button);

		window.Content = stackPanel;
		window.ShowDialog();
	}
}

public class TreeViewCheckBoxItem : TreeViewItem
{
	private readonly StackPanel _stackPanel = new()
	{
		Orientation = Orientation.Horizontal
	};

	private readonly CheckBox _checkBox;
	private readonly TextBlock _label;

	public TreeViewCheckBoxItem(FileInfo projectFile)
	{
		ProjectFile = projectFile;
		_checkBox = new CheckBox();
		_label = new TextBlock()
		{
			Text = Path.GetFileNameWithoutExtension(projectFile.Name)
		};
		_stackPanel.Children.Add(_checkBox);
		_stackPanel.Children.Add(_label);
		Header = _stackPanel;
		ToolTip = new TextBlock() { Text = projectFile.FullName };
	}

	public bool IsChecked => _checkBox.IsChecked ?? false;

	public FileInfo ProjectFile { get; }
}


public class TreeViewHelper
{
	public static void PopulateTreeViewFromSolutions(TreeView treeView, DirectoryInfo rootDirectory)
	{
		var rootPath = rootDirectory.FullName;
		foreach (var solutionPath in Directory.EnumerateFiles(rootPath, "*.sln", SearchOption.AllDirectories))
		{
			var solutionParser = new SlnParser.SolutionParser();
			var solution = solutionParser.Parse(solutionPath);

			var rootNode = new TreeViewItem() {
				Header = solution.Name
			};

			var projects = solution.AllProjects;
			var solutionProjects = projects
				.Where(p => p.Type == ProjectType.CSharp
				|| p.Type == ProjectType.Test
				|| p.Type == ProjectType.PortableClassLibrary
				|| p.Type == ProjectType.AspNet5)
				.Cast<SolutionProject>().ToList();
			foreach (var project in solutionProjects)
			{
				var projectFile = project.File;
				rootNode.Items.Add(new TreeViewCheckBoxItem(projectFile)
				{
					Tag = projectFile
				});
			}
			treeView.Items.Add(rootNode);
		}
	}
	
	public static void PopulateTreeViewFromFoldersGroupByTop(TreeView treeView, DirectoryInfo rootDirectory)
	{
		var childDirectories = rootDirectory.EnumerateDirectories();

		foreach (var directory in childDirectories)
		{
			var rootNode = new TreeViewItem() {
				Header = directory.Name
			};
			var projectFiles = directory.EnumerateFiles("*.csproj", SearchOption.AllDirectories);
			foreach (var projectFile in projectFiles)
			{
				rootNode.Items.Add(new TreeViewCheckBoxItem(projectFile));
			}
			if (rootNode.Items.Count > 0)
			{
				treeView.Items.Add(rootNode);
			}
		}
	}

	public static List<FileInfo> GetSelectedProjects(TreeView treeView)
	{
		var selectedProjects = new List<FileInfo>();

		foreach (var item in treeView.Items)
		{
			if (item is TreeViewItem solutionNode)
			{
				foreach (TreeViewCheckBoxItem projectNode in solutionNode.Items)
				{
					if (projectNode.IsChecked)
					{
						var tag = projectNode.Tag;
						if (tag != null)
						{
							var fileInfo = tag as FileInfo;
							if (fileInfo != null)
							{
								selectedProjects.Add(fileInfo);
							}
						}
					}
				}

			}
		}

		return selectedProjects;
	}
}

public class SolutionHelper
{
	public static async Task CreateSolution(System.Windows.Controls.TreeView treeView, DirectoryInfo outputDirectory, string? solutionName = null)
	{
		if (solutionName == null)
			solutionName = "MySolution " + DateTime.Now.ToString("yyyy-MM-ddTHHmmss");
		var newSolutionPath = Path.Combine(outputDirectory.FullName, Path.GetFileNameWithoutExtension(solutionName) + ".sln"); // Specify your new solution path
		var solutionFile = new FileInfo(newSolutionPath);

		// Create new solution
		var createSlnResult = await Cli.Wrap("dotnet")
			.WithWorkingDirectory(solutionFile.Directory!.FullName)
			.WithArguments(new[] { "new", "sln", "-n", Path.GetFileNameWithoutExtension(solutionFile.Name) })
			.ExecuteBufferedAsync();

		// Check for errors in solution creation
		if (!string.IsNullOrEmpty(createSlnResult.StandardError))
		{
			Console.WriteLine("Error creating solution: " + createSlnResult.StandardError);
			return;
		}

		// Add each project to the solution
		var projectFiles = TreeViewHelper.GetSelectedProjects(treeView);
		foreach (var projectFile in projectFiles)
		{
			var addProjectResult = await Cli.Wrap("dotnet")
				.WithArguments(new[] { "sln", solutionFile.FullName, "add", projectFile.FullName })
				.ExecuteBufferedAsync();

			// Check for errors in adding project
			if (!string.IsNullOrEmpty(addProjectResult.StandardError))
			{
				Console.WriteLine($"Error adding project {projectFile.Name}: " + addProjectResult.StandardError);
			}
			else
			{
				Console.WriteLine($"Added project {projectFile.Name} to solution.");
			}
		}
	}
}
