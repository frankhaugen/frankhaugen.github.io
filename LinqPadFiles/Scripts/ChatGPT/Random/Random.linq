<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
</Query>

var generator = new SystemRandomNumberGenerator();

var result = generator.Next<decimal>();

result.Dump();

public interface IRandomNumberGenerator
{
    T Next<T>() where T : INumber<T>, IConvertible;
    T Next<T>(T maxValue) where T : INumber<T>;
    T Next<T>(T minValue, T maxValue) where T : INumber<T>;
}

public class SystemRandomNumberGenerator : IRandomNumberGenerator
{
    private readonly Random _random = new Random();
    
    public T Next<T>() where T : INumber<T>, IConvertible
    {
        var values = new byte[1024];
        _random.NextBytes(values);
        
        var typeCode = Type.GetTypeCode(typeof(T));
        
        switch (typeCode)
        {
            case TypeCode.Boolean:
                return Convert.To.ToBoolean(values);
        }
        
        return (T)Convert.ChangeType(values, );
    }

    public T Next<T>(T maxValue) where T : INumber<T>
    {
        throw new NotImplementedException();
    }

    public T Next<T>(T minValue, T maxValue) where T : INumber<T>
    {
        throw new NotImplementedException();
    }
}