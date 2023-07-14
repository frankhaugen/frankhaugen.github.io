
public static class PolygonFactory
{
    public static Polygon CreateCircle(string name, float radius, int numberOfVertices)
    {
        var polygon = new Polygon(name);

        var angle = 360f / numberOfVertices;

        for (var i = 0; i < numberOfVertices; i++)
        {
            var vertex = new Vertex
            {
                X = radius * MathF.Cos(angle * i * MathF.PI / 180f),
                Y = radius * MathF.Sin(angle * i * MathF.PI / 180f)
            };

            polygon.Add(vertex);
        }

        return polygon;
    }

    public static Polygon CreateSquare(string name, float with, float height)
    {
        var polygon = new Polygon(name);
        polygon.Add(new Vertex { X = 0, Y = 0 });
        polygon.Add(new Vertex { X = with, Y = 0 });
        polygon.Add(new Vertex { X = with, Y = height });
        polygon.Add(new Vertex { X = 0, Y = height });
        return polygon;
    }

    public static Polygon CreateTriangle(string name, float with, float height)
    {
        var polygon = new Polygon(name);
        polygon.Add(new Vertex { X = 0, Y = 0 });
        polygon.Add(new Vertex { X = with, Y = 0 });
        polygon.Add(new Vertex { X = with / 2, Y = height });
        return polygon;
    }

    public static Polygon CreatePentagon(string name, float with, float height)
    {
        var polygon = new Polygon(name);
        polygon.Add(new Vertex { X = 0, Y = 0 });
        polygon.Add(new Vertex { X = with, Y = 0 });
        polygon.Add(new Vertex { X = with, Y = height });
        polygon.Add(new Vertex { X = with / 2, Y = height * 1.5f });
        polygon.Add(new Vertex { X = 0, Y = height });
        return polygon;
    }

    public static Polygon CreateHexagon(string name, float with, float height)
    {
        var polygon = new Polygon(name);
        polygon.Add(new Vertex { X = 0, Y = 0 });
        polygon.Add(new Vertex { X = with, Y = 0 });
        polygon.Add(new Vertex { X = with * 1.5f, Y = height });
        polygon.Add(new Vertex { X = with, Y = height * 2 });
        polygon.Add(new Vertex { X = 0, Y = height * 2 });
        polygon.Add(new Vertex { X = -with / 2, Y = height });
        return polygon;
    }


}

