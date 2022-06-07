using System.Globalization;
using System.ComponentModel;
using System.Numerics;

public class NativeNameAttribute : Attribute
{
    public string LocalName { get; }
    public NativeNameAttribute(string localName) => LocalName = localName;
}
