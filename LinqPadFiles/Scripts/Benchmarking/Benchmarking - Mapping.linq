<Query Kind="Program">
  <NuGetReference>AutoBogus</NuGetReference>
  <NuGetReference>AutoMapper</NuGetReference>
  <NuGetReference Version="0.13.2">BenchmarkDotNet</NuGetReference>
  <Namespace>AutoMapper</Namespace>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Configs</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>BenchmarkDotNet.Diagnosers</Namespace>
</Query>

// To use BenchmarkDotNet directly, add a NuGet reference to BenchmarkDotNet.
// Then you can use BenchmarkDotNet much as you would in Visual Studio.

// Although you don't get LINQPad's live result visualizer, BenchmarkDotNet recognizes
// that it's running inside LINQPad, and emits nicely colored output.

// Note that you must explicitly enable compiler optimizations:
#LINQPad optimize+

void Main()
{
	Util.AutoScrollResults = true;
	BenchmarkRunner.Run<Mapping>();
}

[ShortRunJob]
[ThreadingDiagnoser]
[MemoryDiagnoser]
public class Mapping
{
    [Params(10, 100, 1_000, 10_000, 100_000, 1_000_000)]
    public int N { get; set; }
    private readonly IMapper _mapper;
	private readonly IMappingProvider<Source, Destination> _mappingProvider;
	private readonly IMappingProvider<Source, Destination> _mappingProviderV2;
	private List<Source> _data;

	public Mapping()
    {
        _mapper = new AutoMapper.Mapper(new MapperConfiguration(x => x.AddProfile<MapppingProfile>()));
	    _mappingProvider = new MappingProvider();
	    _mappingProviderV2 = new MappingProviderV2();
    }

    [IterationSetup]
    public void Setup() => _data = new AutoBogus.AutoFaker<Source>()
            .RuleFor(x => x.Dob, faker => DateOnly.FromDateTime(faker.Date.Past(15)))
            .RuleFor(x => x.FirstName, faker => faker.Name.FirstName())
            .RuleFor(x => x.LastName, faker => faker.Name.LastName())
            .Generate(N);

    [Benchmark]
    public List<Destination> AutoMapper() => _mapper.Map<List<Source>, List<Destination>>(_data);

    [Benchmark]
    public List<Destination> MappingProvider() => _mappingProvider.Map(_data).ToList();
    
    [Benchmark]
    public List<Destination> MappingProviderV2() => _mappingProviderV2.Map(_data).ToList();
}

public class MapppingProfile : Profile
{
    public MapppingProfile()
    {
        CreateMap<Source, Destination>()

        .ForMember(x => x.Name, opt => opt.MapFrom(y => $"{y.FirstName} {y.LastName}"))
        .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(y => y.Dob))
        ;
    }
}

public interface IMappingProvider<TSource, TDestination>
{
    TDestination Map(TSource source);
    IEnumerable<TDestination> Map(IEnumerable<TSource> source);
}

public class MappingProviderV2 : IMappingProvider<Source, Destination>
{
    public IEnumerable<Destination> Map(IEnumerable<Source> source)
    {
        return source.Select(Map);
    }

    public Destination Map(Source source)
    {
        return new Destination()
        {
            Name = $"{source.FirstName} {source.LastName}",
            DateOfBirth = source.Dob
        };
    }
}

public class MappingProvider : IMappingProvider<Source, Destination>
{
    public IEnumerable<Destination> Map(IEnumerable<Source> source)
    {
        foreach (var element in source)
        {
            yield return Map(element);
        }
    }

    public Destination Map(Source source)
    {
        return new Destination()
        {
            Name = $"{source.FirstName} {source.LastName}",
            DateOfBirth = source.Dob
        };
    }
}

public class Source
{
    public DateOnly Dob { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class Destination
{
    public Destination()
    {
        
    }
    public DateOnly DateOfBirth { get; set; }
    public string Name { get; set; }
}