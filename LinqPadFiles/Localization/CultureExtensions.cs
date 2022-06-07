using System.Globalization;
using System.ComponentModel;
using System.Numerics;

public static class CultureExtensions
{
    public static string GetCultureInfoName(this Culture code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(CultureInfoNameAttribute), false)[0] as CultureInfoNameAttribute)?.CultureInfoName ?? string.Empty;
    public static string GetNativeName(this Culture code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(NativeNameAttribute), false)[0] as NativeNameAttribute)?.LocalName ?? string.Empty;
    public static string GetEnglishName(this Culture code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(EnglishNameAttribute), false)[0] as EnglishNameAttribute)?.EnglishName ?? string.Empty;
    public static CultureInfo GetCultureInfo(this Culture code) => CultureInfo.GetCultureInfoByIetfLanguageTag(code.GetCultureInfoName());
    public static RegionInfo GetRegionInfo(this Culture code) => new RegionInfo(code.GetCultureInfo().Name);
}
