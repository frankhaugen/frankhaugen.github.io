<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

public static class PropertyMapper
{
    public static void Map(object source, object destination) =>
        source.GetType().GetProperties()
        .Where(s => IsMatchingProperty(s, destination))
        .ToList()
        .ForEach(s =>
        {
            var value = s.GetValue(source);
            var destinationProperty = GetFirstMatchingProperty(s, destination);
            var dto = new PropertyMappingDto(destination, destinationProperty, value);
            SetPropertyValue(dto);
        });

    private static bool IsMatchingProperty(PropertyInfo source, object destination) =>
        destination.GetType().GetProperties()
        .Any(x => x.Name.Equals(source.Name, StringComparison.InvariantCultureIgnoreCase) && x.CanWrite);

    private static PropertyInfo GetFirstMatchingProperty(PropertyInfo source, object destination) =>
        destination.GetType().GetProperties()
        .First(x => x.Name.Equals(source.Name, StringComparison.InvariantCultureIgnoreCase) && x.CanWrite);

    private static void SetPropertyValue(PropertyMappingDto dto)
    {
        if (IsAssignable(dto))
        {
            dto.DestinationProperty.SetValue(dto.Destination, dto.Value);
        }
        else if (IsNullable(dto))
        {
            dto.DestinationProperty.SetValue(dto.Destination, dto.Value);
        }
        else
        {
            dto.DestinationProperty.SetValue(dto.Destination, ConvertValue(dto));
        }
    }

    private static bool IsAssignable(PropertyMappingDto dto) =>
        dto.DestinationValueType.IsAssignableFrom(dto.SourceValueType);

    private static bool IsNullable(PropertyMappingDto dto) =>
        (dto.DestinationValueType.IsGenericType && dto.DestinationValueType.GetGenericTypeDefinition() == typeof(Nullable<>))
        && dto.DestinationValueType.GetGenericArguments()[0].IsAssignableFrom(dto.SourceValueType);

    private static object ConvertValue(PropertyMappingDto dto) => Convert.ChangeType(dto.Value, dto.DestinationValueType);
    
    private class PropertyMappingDto
    {
        public PropertyMappingDto(object destination, PropertyInfo destinationProperty, object value)
        {
            Destination = destination;
            DestinationProperty = destinationProperty;
            Value = value;
            DestinationValueType = destinationProperty.PropertyType;
            SourceValueType = value.GetType();
        }

        public object Destination { get; }
        public PropertyInfo DestinationProperty { get; }
        public object Value { get; }
        public Type DestinationValueType { get; }
        public Type SourceValueType { get; }
    }
}

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
    public void RegisterProvider<TSource, TDestination>(IMappingDefinition<TSource, TDestination> provider) => _providers[provider.GetIdentity()] = provider;
    public TDestination Map<TSource, TDestination>(TSource source) => ((IMappingDefinition<TSource, TDestination>)_providers[new MappingIdentity(typeof(TSource), typeof(TDestination))]).Map(source);
    public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source) => source.Select(Map<TSource, TDestination>);

    private IMappingDefinition<TSource, TDestination> GetConverter<TSource, TDestination>()
    {
        var key = new MappingIdentity(typeof(TSource), typeof(TDestination));
        if (_providers.TryGetValue(key, out var mapping))
        {
            return (IMappingDefinition<TSource, TDestination>)mapping;
        }
        throw new InvalidOperationException("No converter was found for the given types.");
    }
}

public static class MappingDefinitionExtensions
{
    public static MappingIdentity GetIdentity<TSource, TDestination>(this IMappingDefinition<TSource, TDestination> definition) => new MappingIdentity(typeof(TSource), typeof(TDestination));
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

    public override bool Equals(object? obj) => obj is MappingIdentity other && _sourceType == other._sourceType && _destinationType == other._destinationType;
    public override int GetHashCode() => HashCode.Combine(_sourceType, _destinationType);
    public override string ToString() => $"{_sourceType.FullName}=>{_destinationType.FullName}";
    public static bool operator ==(MappingIdentity left, MappingIdentity right) => left.Equals(right);
    public static bool operator !=(MappingIdentity left, MappingIdentity right) => !(left == right);
}