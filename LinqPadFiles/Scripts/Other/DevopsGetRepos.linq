<Query Kind="Statements">
  <NuGetReference>Microsoft.TeamFoundationServer.Client</NuGetReference>
  <NuGetReference>Microsoft.VisualStudio.Services.Client</NuGetReference>
  <NuGetReference>Microsoft.VisualStudio.Services.Release.Client</NuGetReference>
  <Namespace>Microsoft.TeamFoundation.SourceControl.WebApi</Namespace>
  <Namespace>Microsoft.VisualStudio.Services.Common</Namespace>
  <Namespace>Microsoft.VisualStudio.Services.ReleaseManagement.WebApi</Namespace>
  <Namespace>Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Clients</Namespace>
  <Namespace>Microsoft.VisualStudio.Services.WebApi</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Web</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
</Query>

#nullable enable

var devops= new DevopsWrap();

var repos = await devops.GetRepos();

repos.Dump();

public class DevopsWrap
{
	const string url = "https://dev.azure.com/ProjectName/";
	private readonly string PAT = Util.GetPassword("azure-devops-PAT");

	private VssConnection connection;

	private GitHttpClient? gitHttpClient;
	public GitHttpClient GitHttpClient => gitHttpClient ??= connection.GetClient<GitHttpClient>();

	private ReleaseHttpClient? releaseHttpClient;
	private ReleaseHttpClient ReleaseHttpClient => releaseHttpClient ??= connection.GetClient<ReleaseHttpClient>();

	private ReleaseHttpClient2? releaseHttpClient2;
	public ReleaseHttpClient2 ReleaseHttpClient2 => releaseHttpClient2 ??= connection.GetClient<ReleaseHttpClient2>();

	public DevopsWrap()
	{
		connection = new VssConnection(new Uri(url), new VssBasicCredential(string.Empty, PAT));


	}

	public async Task<IEnumerable<GitRepository>> GetRepos()
	{
		return await GitHttpClient.GetRepositoriesAsync(includeLinks: false, includeAllUrls: false, includeHidden: false);Util.Cmd (@"");
	}

	internal Repo[] GetSelectedRepos()
		=> new[] {
		};

	internal async Task PickRepos()
	{
		var repos = await GitHttpClient.GetRepositoriesAsync("company");
		var dc = new DumpContainer();
		var selected = GetSelectedRepos();
		var repoView = repos.OrderBy(a => a.Name).Select(a => new { a.Id, Name = new CheckBox(a.Name, selected.Any(b => b.Name == a.Name)) }).ToList();

		void UpdateList(object? _, EventArgs _2) => dc.Content = Util.WithHeading(Util.WithStyle(
			$"\t\t\t{string.Join("\r\n\t\t\t", repoView.Where(a => a.Name.Checked).Select(a => $"new Repo(\"{a.Name.Text}\", Guid.Parse(\"{a.Id}\")),"))}\r\n",
			"font-family:'Cascadia Mono'"), "Paste into GetSelectedRepos() above"
			);
		repoView.ForEach(a => a.Name.Click += UpdateList);
		Util.HorizontalRun(true, repoView, dc).Dump();
		UpdateList(null, EventArgs.Empty);
	}

	internal record Repo(string Name, Guid Id);
	internal record RepoPrs(Repo Repo, List<GitPullRequest> LastActivePrs, object ActivePrsView);
	
}
