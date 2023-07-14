
public static class CollisionDetector
{
    public static bool DetectCollission(IEnumerable<Polygon> polygons, out Collission? collision)
    {
        foreach (var polygon1 in polygons)
        {
            foreach (var polygon2 in polygons)
            {
                if (polygon1 != polygon2 && CheckCollision(polygon1, polygon2, out var pointOfCollision))
                {
                    collision = new Collission
                    {
                        Polygon1 = polygon1.Name,
                        Polygon2 = polygon2.Name,
                        CenterDistance = CalculateCenterDistance(polygon1, polygon2),
                        PointOfCollision = pointOfCollision.GetValueOrDefault()
                    };
                    return true;
                }
            }
        }

        collision = null;
        return false;
    }

    private static bool CheckCollision(Polygon polygon1, Polygon polygon2, out Vertex? pointOfCollision)
    {
        pointOfCollision = FindPointOfCollision(polygon1, polygon2);
        return pointOfCollision != null;
    }

    static Vertex? FindPointOfCollision(Polygon polygon1, Polygon polygon2)
    {
        var edges1 = polygon1.Edges;
        var edges2 = polygon2.Edges;

        foreach (var edge1 in edges1)
        {
            foreach (var edge2 in edges2)
            {
                if (CheckEdgeCollision(edge1, edge2, out var intersection) && intersection != null)
                {
                    return intersection.Value.IntersectionPoint;
                }
            }
        }

        return null;
    }

    static bool CheckEdgeCollision(Edge edge1, Edge edge2, out EdgeIntersection? intersection)
    {
        return Edge.Intersect(edge1, edge2, out intersection);
    }

    public static float CalculateCenterDistance(Polygon polygon1, Polygon polygon2)
    {
        return MathF.Sqrt(MathF.Pow(polygon2.Center.X - polygon1.Center.X, 2) + MathF.Pow(polygon2.Center.Y - polygon1.Center.Y, 2));
    }
}
