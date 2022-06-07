using System.Globalization;
using System.ComponentModel;
using System.Numerics;

public class EnglishNameAttribute : Attribute
{
    public string EnglishName { get; }
    public EnglishNameAttribute(string englishName) => EnglishName = englishName;
}
