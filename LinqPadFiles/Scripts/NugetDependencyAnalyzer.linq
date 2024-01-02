<Query Kind="Program">
  <NuGetReference>Buildalyzer</NuGetReference>
  <NuGetReference>NuGet.Protocol.Core.v3</NuGetReference>
  <Namespace>Buildalyzer</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.ComponentModel.DataAnnotations</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>NuGet.Protocol.Core.Types</Namespace>
  <Namespace>NuGet.Configuration</Namespace>
  <Namespace>NuGet.Protocol</Namespace>
</Query>

async Task Main()
{
    await OnExecuteAsync();
}


//[Argument(0, Description = "The solution file to analyze.")]
//[Required]
public string? Solution { get; set; } = @"D:\repos\Frank.Libraries\src\Frank.Libraries.sln";

//[Argument(1, Description = "The output path for file results")]
//[Required]
public string? OutputPath { get; set; } = @"D:\temp\";

//[Option("-g|--create-graph-image", Description = "Runs dot to create a png from the dotfile. Make sure to have dot installed before activating this option")]
public bool WriteGraph { get; set; } = false;

private ValidationResult OnValidate()
{
    if (Solution != null && Solution.EndsWith("sln", StringComparison.InvariantCultureIgnoreCase) && File.Exists(Solution))
    {
        if (!string.IsNullOrEmpty(OutputPath) && !Directory.Exists(OutputPath))
        {
            return new ValidationResult("Output file invalid");
        }
        return ValidationResult.Success;
    }
    else
    {
        return new ValidationResult("Solution file invalid");
    }
}

private async Task OnExecuteAsync()
{
    var dependencyAnalyzer = new DependencyAnalyzer(Solution!);
    await dependencyAnalyzer.AnalyzeAsync().ConfigureAwait(false);

    var markdownWriter = new MarkdownWriter(OutputPath!);
    var dotWriter = new DotWriter(OutputPath!);
    var tasks = new Task[]
    {
                dotWriter.WriteProjectDependencyGraph(dependencyAnalyzer.ProjectResults, Path.GetFileNameWithoutExtension(Solution!), WriteGraph),
                markdownWriter.WritePackages(dependencyAnalyzer.PackageResults),
                markdownWriter.WritePackagesDependenciesByProject(dependencyAnalyzer.PackagesByProject),
                markdownWriter.WriteProjectDependenciesByPackage(dependencyAnalyzer.ProjectsByPackage)
    };
    await Task.WhenAll(tasks).ConfigureAwait(false);
    Console.WriteLine("Done");
}

public sealed class DependencyAnalyzer
{
    public string Solution { get; }

    /// <summary>
    /// Contains the package ID as key, and its version as value
    /// </summary>
    public ConcurrentDictionary<string, string> PackageResults { get; } = new ConcurrentDictionary<string, string>();

    /// <summary>
    /// A dictionary in which the key is a project, and the values are its project dependencies
    /// </summary>
    public ConcurrentDictionary<string, IList<string>> ProjectResults { get; } = new ConcurrentDictionary<string, IList<string>>();

    /// <summary>
    /// A dictionary in which the key is a project, and the values are its package dependencies
    /// </summary>
    public ConcurrentDictionary<string, IList<string>> PackagesByProject { get; } = new ConcurrentDictionary<string, IList<string>>();

    /// <summary>
    /// A dictionary in which the key is a package, and the values are its project dependencies
    /// </summary>
    public ConcurrentDictionary<string, IList<string>> ProjectsByPackage { get; private set; } = new ConcurrentDictionary<string, IList<string>>();

    /// <summary>
    ///
    /// </summary>
    /// <param name="solutionPath">The full solution path</param>
    public DependencyAnalyzer(string solutionPath)
    {
        if (solutionPath != null && solutionPath.EndsWith("sln", StringComparison.InvariantCultureIgnoreCase) && File.Exists(solutionPath))
        {
            Solution = solutionPath;
        }
        else
        {
            throw new ArgumentException($"Invalid Solution: {solutionPath}", nameof(solutionPath));
        }
    }

    /// <summary>
    /// Runs the analysis on the solution projects. At the end of this method, these properties will be filled:
    /// - <see cref="PackageResults"/>
    /// - <see cref="ProjectResults"/>
    /// - <see cref="PackagesByProject"/>
    /// - <see cref="ProjectsByPackage"/>
    /// </summary>
    public async Task AnalyzeAsync()
    {
        var analyzerManager = new AnalyzerManager(Solution);
        var tasks = new List<Task>();
        foreach (var project in analyzerManager.Projects)
        {
            tasks.Add(AnalyzeProject(project));
        }

        await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);
        ProjectsByPackage = GetProjectsByPackage(PackagesByProject);
    }

    private async Task AnalyzeProject(KeyValuePair<string, IProjectAnalyzer> project)
    {
        await Task.Run(() =>
        {
            var projectName = Path.GetFileNameWithoutExtension(project.Key);
            Console.WriteLine($"Building Project {projectName}");
            var results = project.Value.Build().First();
            PackagesByProject.TryAdd(projectName, new List<string>());
            foreach (var (packageId, attributes) in results.PackageReferences.Where(p => p.Value.ContainsKey("Version")))
            {
                PackagesByProject[projectName].Add(packageId + " " + attributes["Version"]);
                PackageResults.TryAdd(packageId, attributes["Version"]);
            }
            ProjectResults.TryAdd(projectName, results.ProjectReferences.Select(s => Path.GetFileNameWithoutExtension(s!)).ToList());
            Console.WriteLine($"Project {projectName} done");
        }).ConfigureAwait(false);
    }

    private ConcurrentDictionary<string, IList<string>> GetProjectsByPackage(ConcurrentDictionary<string, IList<string>> packageDepByProject)
    {
        var result = new ConcurrentDictionary<string, IList<string>>();
        foreach (var (project, packages) in packageDepByProject)
        {
            foreach (var package in packages)
            {
                if (!result.ContainsKey(package))
                {
                    result.TryAdd(package, new List<string>());
                }
                result[package].Add(project);
            }
        }
        return result;
    }
}

public sealed class MarkdownWriter : AWriter
{
    public MarkdownWriter(string outputPath) : base(outputPath)
    {
    }

    /// <summary>
    /// Lists all provides packages in the packages.md file
    /// </summary>
    /// <param name="packages">The packages to write</param>
    public async Task WritePackages(IDictionary<string, string> packages)
    {
        if (packages == null || !packages.Any())
        {
            return;
        }
        var providers = new List<Lazy<INuGetResourceProvider>>();
        providers.AddRange(Repository.Provider.GetCoreV3());
        var packageSource = new PackageSource("https://api.nuget.org/v3/index.json");
        var sourceRepository = new SourceRepository(packageSource, providers);
        var packageSearchResource = await sourceRepository.GetResourceAsync<PackageSearchResource>().ConfigureAwait(false);

        var fileName = Path.Combine(OutputPath, "packages.md");
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
        using (var file = File.CreateText(fileName))
        {
            await file.WriteLineAsync("# Nuget dependencies").ConfigureAwait(false);
            foreach (var (packageId, version) in packages.OrderBy(p => p.Key))
            {
                var searchMetadata = await packageSearchResource.SearchAsync(packageId, new SearchFilter(true), 0, 1, null, CancellationToken.None).ConfigureAwait(false);
                var metadata = searchMetadata.FirstOrDefault();
                if (metadata != null && metadata.Identity.Id == packageId && !string.IsNullOrWhiteSpace(metadata.ProjectUrl?.ToString()))
                {
                    var url = metadata.ProjectUrl?.ToString();
                    await file.WriteLineAsync($" - [{packageId}]({url}) {version}").ConfigureAwait(false);
                }
                else
                {
                    await file.WriteLineAsync($" - {packageId} {version}").ConfigureAwait(false);
                }
            }
        }
    }

    /// <summary>
    /// Lists the package dependencies by project in the packagesByProject.md file
    /// </summary>
    /// <param name="packagesByProject">A dictionary in which the key is a project, and the values are its package dependencies</param>
    public async Task WritePackagesDependenciesByProject(IDictionary<string, IList<string>> packagesByProject)
    {
        if (packagesByProject == null)
        {
            throw new ArgumentNullException(nameof(packagesByProject));
        }
        await WriteDependencyFile(packagesByProject, "packagesByProject.md", "Package dependencies by project").ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the project dependencies by package in the projectsByPackage.md file
    /// </summary>
    /// <param name="projectsByPackage">A dictionary in which the key is a package, and the values are its project dependencies</param>
    /// <returns></returns>
    public async Task WriteProjectDependenciesByPackage(IDictionary<string, IList<string>> projectsByPackage)
    {
        if (projectsByPackage == null)
        {
            throw new ArgumentNullException(nameof(projectsByPackage));
        }
        await WriteDependencyFile(projectsByPackage, "projectsByPackage.md", "Project dependencies by package").ConfigureAwait(false);
    }

    private async Task WriteDependencyFile(IDictionary<string, IList<string>> dependencies, string fileName, string title)
    {
        var filePath = Path.Combine(OutputPath, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        using (var file = File.CreateText(filePath))
        {
            await file.WriteLineAsync($"# {title}").ConfigureAwait(false);
            foreach (var (node, leafs) in dependencies.OrderBy(t => t.Key))
            {
                await file.WriteLineAsync($"### {node}").ConfigureAwait(false);
                foreach (var leaf in leafs.OrderBy(v => v))
                {
                    await file.WriteLineAsync($" - {leaf}").ConfigureAwait(false);
                }
            }
        }
    }
}
public abstract class AWriter
{
    public string OutputPath { get; }

    public AWriter(string outputPath)
    {
        OutputPath = outputPath;
    }
}

public sealed class DotWriter : AWriter
{
    public DotWriter(string outputPath) : base(outputPath)
    {
    }

    /// <summary>
    /// Creates a dot file and the associated png graph of project dependencies
    /// </summary>
    /// <param name="projectDependencies">A dictionary in which the key is a project, and the values are its dependencies</param>
    /// <param name="graphTitle">Title of the generated dot graph</param>
    /// <param name="createImage">true if the graph image should be created</param>
    public async Task WriteProjectDependencyGraph(IDictionary<string, IList<string>> projectDependencies, string graphTitle, bool createImage)
    {
        if (projectDependencies == null)
        {
            throw new ArgumentNullException(nameof(projectDependencies));
        }
        var fileName = Path.Combine(OutputPath, "projectDependencyGraph.dot");
        var graphFileName = Path.Combine(OutputPath, "projectDependencyGraph.png");
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
        if (File.Exists(graphFileName))
        {
            File.Delete(graphFileName);
        }
        using (var file = File.CreateText(fileName))
        {
            await file.WriteLineAsync($"digraph \"{graphTitle}\" {{").ConfigureAwait(false);
            await file.WriteLineAsync("splines=ortho;").ConfigureAwait(false);
            foreach (var (project, dependencies) in projectDependencies)
            {
                // if edges a -> b and b -> c exist, remove a -> c to have a readable graph
                var toExclude = dependencies.SelectMany(v => projectDependencies.ContainsKey(v) ? projectDependencies[v] : new List<string>()).Distinct();

                foreach (var dependency in dependencies.Except(toExclude))
                {
                    await file.WriteLineAsync($"\"{project}\" -> \"{dependency}\"").ConfigureAwait(false);
                }
            }
            await file.WriteLineAsync("}").ConfigureAwait(false);
        }

        if (createImage)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("dot");
            startInfo.Arguments = $"-Tpng {fileName} -o {graphFileName}";
            var process = Process.Start(startInfo);
            process.WaitForExit();
        }
    }
}