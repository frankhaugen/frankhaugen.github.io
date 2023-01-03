<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
  <RuntimeVersion>7.0</RuntimeVersion>
</Query>




public static class NumberExtensions
{
    
    public static T Add<T>(this T source, T value) where T: INumber<T>
    {
        return source + value;
    }
    
}