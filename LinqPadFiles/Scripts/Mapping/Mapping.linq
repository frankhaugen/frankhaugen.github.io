<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

public interface IMappingDefinition<TSource, TDestination>
{
    TDestination Map(TSource source);
    IEnumerable<TDestination> Map(IEnumerable<TSource> source);
}


public abstract class MappingDefinitionBase<TSource, TDestination> : IMappingDefinition<TSource, TDestination>
{
    public TDestination Map(TSource source) => MapInternal(source);
    public IEnumerable<TDestination> Map(IEnumerable<TSource> source) => source.Select(MapInternal);
    protected abstract TDestination MapInternal(TSource source);
}

public interface IMappingProvider
{
    void RegisterProvider<TSource, TDestination>(IMappingDefinition<TSource, TDestination> provider);
    TDestination Map<TSource, TDestination>(TSource source);
    IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source);
}

public class MappingProvider : IMappingProvider
{
    private readonly Dictionary<MappingIdentity, object> _providers;

    public MappingProvider() => _providers = new Dictionary<MappingIdentity, object>();

    public void RegisterProvider<TSource, TDestination>(IMappingDefinition<TSource, TDestination> provider)
        => _providers[provider.GetIdentity()] = provider;

    public TDestination Map<TSource, TDestination>(TSource source)
        => ((IMappingDefinition<TSource, TDestination>)_providers[new MappingIdentity(typeof(TSource), typeof(TDestination))]).Map(source);

    public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        => source.Select(Map<TSource, TDestination>);
}

public struct MappingIdentity
{
    private readonly Type _sourceType;
    private readonly Type _destinationType;

    public MappingIdentity(Type sourceType, Type destinationType)
    {
        _sourceType = sourceType;
        _destinationType = destinationType;
    }

    public override bool Equals(object? obj)
    {
        if (obj is MappingIdentity other)
        {
            return _sourceType == other._sourceType && _destinationType == other._destinationType;
        }

        return false;
    }

    public override int GetHashCode() => HashCode.Combine(_sourceType, _destinationType);

    public override string ToString() => $"{_sourceType.FullName}=>{_destinationType.FullName}";
}

public static class MappingDefinitionExtensions
{
    public static MappingIdentity GetIdentity<TSource, TDestination>(this IMappingDefinition<TSource, TDestination> definition)
        => new MappingIdentity(typeof(TSource), typeof(TDestination));
}