namespace Localization;

public class EnglishNameAttribute : Attribute
{
    public string EnglishName { get; }
    public EnglishNameAttribute(string englishName) => EnglishName = englishName;
}
