namespace Localization;

public class NativeNameAttribute : Attribute
{
    public string LocalName { get; }
    public NativeNameAttribute(string localName) => LocalName = localName;
}
