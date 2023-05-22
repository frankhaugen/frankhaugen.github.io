<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Hosting</NuGetReference>
  <NuGetReference>NuGet.Configuration</NuGetReference>
  <NuGetReference>NuGet.DependencyResolver.Core</NuGetReference>
  <NuGetReference>NuGet.Protocol</NuGetReference>
  <NuGetReference>NuGet.Resolver</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>NuGet.Protocol.Core.Types</Namespace>
  <Namespace>NuGet.Protocol</Namespace>
  <Namespace>NuGet.Configuration</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>NuGet.Common</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>NuGet.Packaging.Core</Namespace>
  <Namespace>NuGet.Versioning</Namespace>
  <Namespace>NuGet.Frameworks</Namespace>
</Query>

async Task Main()
{
    var builder = Host.CreateDefaultBuilder();

    builder.ConfigureServices((context, services) =>
    {
        services.AddSingleton<INugetService, NugetService>();
        services.AddSingleton<NuGet.Common.ILogger>(new LinqPadNugetLogger(NuGet.Common.LogLevel.Verbose));
    });

    var app = builder.Build();


    await app.StartAsync();

    var service = app.Services.GetRequiredService<INugetService>();

    var searchResult = await service.SearchAsync("SemineConnect");
    var package = searchResult.First();
    
    await service.GetStuffAsync(package);

    await app.StopAsync();
}










public interface INugetService
{
    Task GetStuffAsync(IPackageSearchMetadata packageSearchMetadata);
    Task<NugetPackage> GetDetailsAsync(IPackageSearchMetadata packageSearchMetadata);
    Task<FileInfo> DownloadAsync(DirectoryInfo downloadDirectory, IPackageSearchMetadata packageSearchMetadata);
    Task<IEnumerable<IPackageSearchMetadata>> SearchAsync(string search, int skip = 0, int take = 50);
}

public class NugetPackage
{
    public PackageIdentity Identity { get; set; }
    public IEnumerable<NugetPackage> Dependencies { get; set; }
}

public class NugetService : INugetService
{
    private readonly SourceCacheContext _cache = new SourceCacheContext();
    CancellationToken _cancellationToken = CancellationToken.None;

    private readonly SourceRepository _defaultRepository;
    private readonly SourceRepository _repository;
    private readonly NuGet.Common.ILogger _logger;

    public NugetService(NuGet.Common.ILogger logger)
    {
        _logger = logger;
        
        _repository = ConfigureAuthenticatedRepository();
        _defaultRepository = ConfigureDefaultRepository();
    }

    private SourceRepository ConfigureAuthenticatedRepository()
    {

        var sourceUri = "https://semine.pkgs.visualstudio.com/_packaging/Semine.Integration/nuget/v3/index.json";
        var source = new PackageSource(sourceUri)
        {
            Credentials = new PackageSourceCredential(
                source: sourceUri,
                username: "frank.haugen",
                passwordText: Util.GetPassword("azure-devops-PAT"),
                isPasswordClearText: true,
                validAuthenticationTypesText: null)
        };
        return Repository.Factory.GetCoreV3(source);
    }
    
    private SourceRepository ConfigureDefaultRepository()
    {
        var sourceUri = "https://api.nuget.org/v3/index.json";
        var source = new PackageSource(sourceUri);

        return Repository.Factory.GetCoreV3(source);
    }

    public async Task<NugetPackage> GetDetailsAsync(IPackageSearchMetadata packageSearchMetadata)
    {
        var result = await GetNugetPackageAsync(packageSearchMetadata.Identity);
        return result;
    }

    public async Task GetStuffAsync(IPackageSearchMetadata packageSearchMetadata)
    {
        static void Main()
        {
            var repo = new LocalPackageRepository(@"C:\Code\Common\Group\Business-Logic\packages");
            IQueryable<IPackage> packages = repo.GetPackages();
            OutputGraph(repo, packages, 0);
        }

        static void OutputGraph(LocalPackageRepository repository, IEnumerable<IPackage> packages, int depth)
        {
            foreach (IPackage package in packages)
            {
                Console.WriteLine("{0}{1} v{2}", new string(' ', depth), package.Id, package.Version);

                IList<IPackage> dependentPackages = new List<IPackage>();
                foreach (var dependency in package.Dependencies)
                {
                    dependentPackages.Add(repository.FindPackage(dependency.Id, dependency.VersionSpec.ToString()));
                }

                OutputGraph(repository, dependentPackages, depth += 3);
            }
        }

    }
    public async Task GetStuffAsync(IPackageSearchMetadata packageSearchMetadata)
    {
        var resource = await _repository.GetResourceAsync<NuGet.Protocol.Core.Types.DependencyInfoResource>();
        var result = await resource.ResolvePackage(packageSearchMetadata.Identity, new NuGetFramework(".NETCoreApp", new Version(7, 0, 0)), _cache, _logger, _cancellationToken);
        result.Dump();
    }

    public async Task<NugetPackage> GetNugetPackageAsync(PackageIdentity identity)
    {
        var dependencyInfo = await GetDependenciesAsync(identity.Id);
        
        var dependencies = await GetDependenciesAsNugetsAsync(identity);
        var nuget = new NugetPackage();
        
        if (dependencyInfo != null && dependencyInfo.PackageIdentity != null && dependencyInfo.PackageIdentity.Id != string.Empty)
        {
            nuget.Identity = dependencyInfo.PackageIdentity;
        }
        else
        {
            nuget.Identity = identity;
        }
        
        if (dependencies != null && dependencies.Any())
        {
            nuget.Dependencies = dependencies.ToList();
        }


        return nuget;
    }
    
    public async Task<IEnumerable<NugetPackage>> GetDependenciesAsNugetsAsync(PackageIdentity identity)
    {
        var output = new List<NugetPackage>();
        var dependencyInfo = await GetDependenciesAsync(identity.Id);

        if (dependencyInfo == null || dependencyInfo.DependencyGroups == null || !dependencyInfo.DependencyGroups.Any())
        {
            
        }
        else
        {
            foreach (var dependency in dependencyInfo.DependencyGroups.Where(x => x.Packages != null && x.Packages.Any()).SelectMany(x => x.Packages).Take(10))
            {
                output.Add(await GetNugetPackageAsync(new PackageIdentity(dependency.Id, dependency.VersionRange.MaxVersion)));
            }
        }
        return output;
    }

    public async Task<FindPackageByIdDependencyInfo> GetDependenciesAsync(string id)
    {
        var resource = await _repository.GetResourceAsync<FindPackageByIdResource>();
        var latestVersion = await GetLatestVersionAsync(id);
        var dependencyInfo = await resource.GetDependencyInfoAsync(id, latestVersion, _cache, _logger, _cancellationToken);
        return dependencyInfo;
    }

    public async Task<NuGetVersion> GetLatestVersionAsync(string id)
    {
        var resource = await _repository.GetResourceAsync<FindPackageByIdResource>();
        var versions = await resource.GetAllVersionsAsync(id, _cache, _logger, _cancellationToken);
        
        if (versions == null)
        {
            var defaultResource = await _defaultRepository.GetResourceAsync<FindPackageByIdResource>();
            versions = await defaultResource.GetAllVersionsAsync(id, _cache, _logger, _cancellationToken);
        }
        
        return versions.OrderDescending().FirstOrDefault() ?? new NuGetVersion(1, 0, 0);
    }

    public async Task<FileInfo> DownloadAsync(DirectoryInfo downloadDirectory, IPackageSearchMetadata packageSearchMetadata)
    {
        var resource = await _repository.GetResourceAsync<FindPackageByIdResource>();
        
        var downloader = await resource.GetPackageDownloaderAsync(packageSearchMetadata.Identity, _cache, _logger, _cancellationToken);
        
        var filename = packageSearchMetadata.Identity.ToString();
        
        var file = new FileInfo(Path.Combine(downloadDirectory.FullName, filename));
        
        var success = await downloader.CopyNupkgFileToAsync(file.FullName, _cancellationToken);
        success.Dump();
        
        return file;
    }

    public async Task<IEnumerable<IPackageSearchMetadata>> SearchAsync(string search, int skip = 0, int take = 50)
    {
        var searchResource = await _repository.GetResourceAsync<PackageSearchResource>();

        var packages = await searchResource.SearchAsync(
            search,
            new SearchFilter(true, SearchFilterType.IsLatestVersion),
            skip,
            take,
            _logger,
            _cancellationToken);

        return packages;
    }
}

public class LinqPadNugetLogger : NuGet.Common.ILogger
{
    private readonly NuGet.Common.LogLevel _logLevel;

    public LinqPadNugetLogger(NuGet.Common.LogLevel logLevel = NuGet.Common.LogLevel.Verbose)
    {
        _logLevel = logLevel;
    }

    public void Log(NuGet.Common.LogLevel level, string data)
    {
        if (_logLevel >= level)
        {
            data.Dump();
        }
    }

    public void Log(ILogMessage message)
    {
        Log(message.Level, $"[{message.Time}]\t{message.Level}\t{message.Code}=>{message.Message}\t{message.ProjectPath}");
    }

    public async Task LogAsync(NuGet.Common.LogLevel level, string data)
    {
        throw new NotImplementedException();
    }

    public async Task LogAsync(ILogMessage message)
    {
        Log(message.Level, $"[{message.Time}]\t{message.Level}\t{message.Code}=>{message.Message}\t{message.ProjectPath}");
    }

    public void LogDebug(string data)
    {
        Log(NuGet.Common.LogLevel.Debug, data);
    }

    public void LogError(string data)
    {
        Log(NuGet.Common.LogLevel.Error, data);
    }

    public void LogInformation(string data)
    {
        Log(NuGet.Common.LogLevel.Information, data);
    }

    public void LogInformationSummary(string data)
    {
        Log(NuGet.Common.LogLevel.Information, data);
    }

    public void LogMinimal(string data)
    {
        Log(NuGet.Common.LogLevel.Minimal, data);
    }

    public void LogVerbose(string data)
    {
        Log(NuGet.Common.LogLevel.Verbose, data);
    }

    public void LogWarning(string data)
    {
        Log(NuGet.Common.LogLevel.Warning, data);
    }
}