<Query Kind="Program">
  <NuGetReference>AutoMapper</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>AutoMapper</Namespace>
</Query>

#load "BenchmarkDotNet"
#load ".\Implementation"
#load ".\Mapping"
#load ".\Converter"


void Main()
{
    RunBenchmark(); return;  // Uncomment this line to initiate benchmarking.
}

MappingImplementation MappingImplementation { get; set; }
IMapper AutoMapper { get; set; }
Source Source = new Source() { FirstName = "Frank", LastName = "Sinatra" };
IConverter<float, decimal> FloatToDecimaleConverter = new GenericConverter<float, decimal>();
IConverter<decimal, float> DecimalToFloatConverter = new GenericConverter<decimal, float>();

[Benchmark]
public void BenchmarkFloatToDecimaleConverter()
{
    FloatToDecimaleConverter.Convert(float.MaxValue);
}

[Benchmark]
public void BenchmarkDecimalToFloatConverter()
{
    DecimalToFloatConverter.Convert(decimal.MaxValue);
}

[Benchmark]
public void BenchmarkMapping()
{
    MappingImplementation.Run();
}

[Benchmark]
public void BenchmarkAutoMapper()
{
    AutoMapper.Map<Source, Destination>(Source);
}

[GlobalSetup]
public void BenchmarkSetup()
{
    MappingImplementation = new();
    AutoMapper = new Mapper(new MapperConfiguration(x => x.AddProfile<TestProfile>()));
}

public class TestProfile : Profile
{
    public TestProfile()
    {
        CreateMap<Source, Destination>()
            .ForMember(x => x.FullName, opt => opt.MapFrom<FullNameResolver>()) // Fails
                                                                                //.ForMember(x => x.FullName, opt => opt.MapFrom(y => $"{y.FirstName} {y.LastName}")) // Passes
            ;
    }
}

public class FullNameResolver : IValueResolver<Source, Destination, string>
{
    public string Resolve(Source source, Destination destination, string destMember, ResolutionContext context)
    {
        return $"{source.FirstName} {source.LastName}";
    }
}