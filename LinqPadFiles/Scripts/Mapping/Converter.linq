<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
</Query>

public class GenericConverter<TSource, TDestination> : IConverter<TSource, TDestination>
    where TSource : INumber<TSource>
    where TDestination : INumber<TDestination>
{
    public TDestination Convert(TSource source)
    {
        return source.As<TSource, TDestination>();
    }
}

public interface IConverter<TSource, TDestination>
{
    TDestination Convert(TSource source);
}

public class ConversionProvider
{
    private readonly Dictionary<ConversionIdentity, object> _converters;

    public ConversionProvider()
    {
        _converters = new Dictionary<ConversionIdentity, object>();
    }

    public void Register<TSource, TDestination>(IConverter<TSource, TDestination> converter)
    {
        var key = new ConversionIdentity(typeof(TSource), typeof(TDestination));
        _converters[key] = converter;
    }

    public TDestination Convert<TSource, TDestination>(TSource source)
    {
        var converter = GetConverter<TSource, TDestination>();
        return converter.Convert(source);
    }

    private IConverter<TSource, TDestination> GetConverter<TSource, TDestination>()
    {
        var key = new ConversionIdentity(typeof(TSource), typeof(TDestination));
        if (_converters.TryGetValue(key, out var converter))
        {
            return (IConverter<TSource, TDestination>)converter;
        }
        throw new InvalidOperationException("No converter was found for the given types.");
    }
}

public static class ConversionExtensions
{
    public static T As<TSource, T>(this TSource source)
        where T : INumber<T>  
        where TSource : INumber<TSource>  
        => T.CreateTruncating(source);
    
    public static ConversionIdentity GetIdentity<TSource, TDestination>(this IConverter<TSource, TDestination> definition) => new ConversionIdentity(typeof(TSource), typeof(TDestination));
}

public struct ConversionIdentity
{
    public Type SourceType { get; }
    public Type DestinationType { get; }

    public ConversionIdentity(Type sourceType, Type destinationType)
    {
        SourceType = sourceType;
        DestinationType = destinationType;
    }

    public override bool Equals(object? obj) => obj is ConversionIdentity identity && SourceType == identity.SourceType && DestinationType == identity.DestinationType;
    public override int GetHashCode() => HashCode.Combine(SourceType, DestinationType);
    public static bool operator ==(ConversionIdentity left, ConversionIdentity right) => left.Equals(right);
    public static bool operator !=(ConversionIdentity left, ConversionIdentity right) => !(left == right);
}
