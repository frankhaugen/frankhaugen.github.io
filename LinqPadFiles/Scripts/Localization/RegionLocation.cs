using System.Globalization;
using System.ComponentModel;
using System.Numerics;
public readonly record struct RegionLocation(RegionInfo Region, float Latitude, float Longitude, string Name);
