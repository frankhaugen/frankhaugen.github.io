using System.Globalization;
using System.ComponentModel;
using System.Numerics;

public class CoordinatesAttribute : Attribute
{
    public Vector2 Coordinates { get; }
    public CoordinatesAttribute(float latitude, float longitude) => Coordinates = new Vector2(latitude, longitude);
}
