<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
    var boxA = PolygonFactor.CreateSquare("A", 10, 10);
    boxA.Translate(new Vertex { X = 15, Y = 15 });

    var boxB = PolygonFactor.CreateSquare("B", 10, 10);
    boxB.Translate(new Vertex { X = 55, Y = 15 });

    var sim = new Simulator();
    sim.SimulationSpeed = 10;

    sim.Run(60, x =>
                {
                    sim.MoveRight(boxA);
                    var svg = Renderer.ToSVG(boxA, boxB);
                    Util.ClearResults();
                    svg.Dump(x.ToString());

                    if (CollisionDetector.DetectCollission(new[] { boxA, boxB }, out var collision))
                    {
                        collision.Dump("BOOM!!!");
                        sim.Stop();
                    }
                });
}

public class Simulator
{
    public readonly TimeSpan TimeIncrement = TimeSpan.FromSeconds(1);
    private readonly DateTime _started;
    private TimeSpan _runningTime = TimeSpan.Zero;

    private bool _stopSimulation;

    public Simulator()
    {
        _started = DateTime.UtcNow;
    }

    public float SimulationSpeed { get; set; } = 5;

    public void Stop() => _stopSimulation = true;

    public void Run(int iterations, Action<TimeSpan> action)
    {
        for (int i = 0; i < iterations; i++)
        {
            if (_stopSimulation)
            {
                _stopSimulation = false;
                break;
            }
            Run(action);
        }
    }

    public void Run(Action<TimeSpan> action)
    {
        _runningTime += TimeIncrement;
        Task.Delay(TimeIncrement / SimulationSpeed).Wait();
        action.Invoke(_runningTime);
    }

    public void MoveRight(Polygon polygon)
    {
        polygon.Translate(new((float)TimeIncrement.TotalSeconds, 0));
    }
}

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
                        PointOfCollision = pointOfCollision
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

public struct Collission
{
    public string Polygon1 { get; set; }
    public string Polygon2 { get; set; }

    public float CenterDistance { get; set; }
    public Vertex PointOfCollision { get; set; }
}


public static class PolygonFactor
{
    public static Polygon CreateSquare(string name, float with, float height)
    {
        var polygon = new Polygon(name);
        polygon.Add(new Vertex { X = 0, Y = 0 });
        polygon.Add(new Vertex { X = with, Y = 0 });
        polygon.Add(new Vertex { X = with, Y = height });
        polygon.Add(new Vertex { X = 0, Y = height });
        return polygon;
    }
}

public class Polygon : List<Vertex>
{
    public Polygon(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public void Translate(Vertex position)
    {
        foreach (var vertex in this)
        {
            vertex.X += position.X;
            vertex.Y += position.Y;
        }
    }

    public Vertex Center => new Vertex(this.Average(v => v.X), this.Average(v => v.Y));

    public Edge[] Edges => this.Select((v, i) => new Edge(v, this[(i + 1) % this.Count])).ToArray();
}

public readonly struct Edge
{
    public Edge(Vertex start, Vertex end)
    {
        Start = start;
        End = end;
    }

    public Vertex Start { get; }
    public Vertex End { get; }

    internal static bool Intersect(Edge edge1, Edge edge2, out EdgeIntersection? intersection)
    {
        intersection = new EdgeIntersection(edge1, edge2);
        return intersection.Value.IsIntersection;
    }
}

public static class Renderer
{
    public static LINQPad.Controls.Svg ToSVG(params Polygon[] polygons)
    {
        var svgBuilder = new StringBuilder();

        svgBuilder.AppendLine($"<rect x='-100' y='-100' width='200' height='200' fill='#FFF' />");

        foreach (var polygon in polygons)
        {
            var polygonElement = Convert(polygon);
            svgBuilder.AppendLine(polygonElement);
        }

        var rawSvg = svgBuilder.ToString();

        return new LINQPad.Controls.Svg(rawSvg, 100, 100);
    }

    private static string Convert(Polygon polygon)
    {
        var polygonBuilder = new StringBuilder();
        polygonBuilder.Append($"<polygon points=\"{string.Join(" ", polygon.Select(v => $"{v.X},{v.Y}"))}\" style=\"fill:lime;stroke:purple;stroke-width:1\" />");

        return polygonBuilder.ToString();
    }
}

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

public class Vertex
{
    public Vertex()
    {

    }

    public Vertex(float x, float y)
    {
        X = x;
        Y = y;
    }

    public float X { get; set; }
    public float Y { get; set; }
}