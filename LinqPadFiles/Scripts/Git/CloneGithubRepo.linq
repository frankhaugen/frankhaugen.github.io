<Query Kind="Statements">
  <NuGetReference>LibGit2Sharp</NuGetReference>
  <Namespace>LibGit2Sharp</Namespace>
  <Namespace>LibGit2Sharp.Handlers</Namespace>
</Query>

string repoUrl = "https://github.com/frankhaugen/Frank.Notes.git";
string rootDir = "D:\\temp\\repo";
string repoName = "Frank.Notes";

string localPath = Path.Combine(rootDir, repoName);

var co = new CloneOptions();
co.CredentialsProvider = (_url, _user, _cred) =>
	new UsernamePasswordCredentials
	{
		Username = "frank.haugen@gmail.com",
		Password = Util.GetPassword("GitHub_PAT")
	};

try
{
	new DirectoryInfo(localPath).Create();
	Repository.Clone(repoUrl, localPath, co);
	Console.WriteLine($"Cloned to {localPath}.");
}
catch (Exception ex)
{
	Console.WriteLine($"Error: {ex.Message}");
}