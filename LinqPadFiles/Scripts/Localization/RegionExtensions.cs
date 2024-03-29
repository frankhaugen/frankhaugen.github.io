#load ".\CoordinatesAttribute.cs"
#load ".\NativeNameAttribute.cs"
#load ".\EnglishNameAttribute.cs"




using System.Globalization;
using System.ComponentModel;
using System.Numerics;

public static class RegionExtensions
{
    public static string GetNativeName(this RegionInfo code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(NativeNameAttribute), false)[0] as NativeNameAttribute)?.LocalName ?? string.Empty;
    public static string GetEnglishName(this RegionInfo code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(EnglishNameAttribute), false)[0] as EnglishNameAttribute)?.EnglishName ?? string.Empty;
    public static Vector2 GetCoordinates(this RegionInfo code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(CoordinatesAttribute), false)[0] as CoordinatesAttribute)?.Coordinates ?? Vector2.Zero;
}
