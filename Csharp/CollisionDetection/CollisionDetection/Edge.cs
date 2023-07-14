/// <summary>
/// Represents an edge in a 2D space, defined by its start and end vertices.
/// </summary>
public readonly record struct Edge(Vertex Start, Vertex End)
{
    internal static bool Intersect(Edge edge1, Edge edge2, out EdgeIntersection? intersection)
    {
        intersection = new EdgeIntersection(edge1, edge2);
        return intersection.Value.IsIntersection;
    }
}

