/// <summary>
/// Represents the intersection of two edges in a two-dimensional space.
/// </summary>
public readonly struct EdgeIntersection
{
    public Vertex? IntersectionPoint { get; }
    public bool IsIntersection { get; }

    public EdgeIntersection(Edge edgeA, Edge edgeB)
    {
        // Calculate the intersection point of two line segments
        float x1 = edgeA.Start.X, y1 = edgeA.Start.Y, x2 = edgeA.End.X, y2 = edgeA.End.Y;
        float x3 = edgeB.Start.X, y3 = edgeB.Start.Y, x4 = edgeB.End.X, y4 = edgeB.End.Y;

        float denominator = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
        if (denominator == 0)
        {
            // The lines are parallel or coincident
            IntersectionPoint = null;
            IsIntersection = false;
            return;
        }

        float ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / denominator;
        float ub = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / denominator;

        if (ua < 0 || ua > 1 || ub < 0 || ub > 1)
        {
            // The intersection point is outside the line segments
            IntersectionPoint = null;
            IsIntersection = false;
            return;
        }

        float x = x1 + ua * (x2 - x1);
        float y = y1 + ua * (y2 - y1);
        IntersectionPoint = new Vertex(x, y);
        IsIntersection = true;
    }
}

