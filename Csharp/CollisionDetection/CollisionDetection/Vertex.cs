
using System.Diagnostics;

/// <summary>
/// Represents a 2D vertex with X and Y coordinates.
/// </summary>
/// <remarks>
/// This record is immutable and can be used for collision detection algorithms.
/// </remarks>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public record struct Vertex(float X, float Y)
{
    public override readonly string ToString() => $"({X}, {Y})";

    private readonly string GetDebuggerDisplay() => ToString();
}

