void Main()
{
    var boxA = PolygonFactor.CreateSquare("A", 10, 10);
    boxA.Translate(new Vertex { X = -5, Y = -5 });

    var boxB = PolygonFactor.CreateSquare("B", 10, 10);
    boxB.Translate(new Vertex { X = 5, Y = 5 });

    var svg = Renderer.ToSVG(boxA, boxB);

    svg.Dump();
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
}

public static class Renderer
{
    private static Random random = new Random();

    public static Svg ToSVG(params Polygon[] polygons)
    {
        var svgBuilder = new StringBuilder();

        // Calculate the grid size based on the polygons' size
        var gridSize = CalculateGridSize(polygons);

        // Calculate the offset for positioning the origin in the middle
        var xOffset = -gridSize / 2;
        var yOffset = -gridSize / 2;

        var scale = 1;
        //var scale = gridSize * 50; // Adjust the scale based on the grid size

        svgBuilder.AppendLine("<svg xmlns='http://www.w3.org/2000/svg' version='1.1'>");

        // Add background grid
        svgBuilder.AppendLine($"<rect x='{xOffset}' y='{yOffset}' width='{gridSize}' height='{gridSize}' fill='#FFF' />");
        for (int i = 0; i <= gridSize; i += 10)
        {
            svgBuilder.AppendLine($"<line x1='{xOffset + i}' y1='{yOffset}' x2='{xOffset + i}' y2='{yOffset + gridSize}' stroke='#DDD' />");
            svgBuilder.AppendLine($"<line x1='{xOffset}' y1='{yOffset + i}' x2='{xOffset + gridSize}' y2='{yOffset + i}' stroke='#DDD' />");
        }

        foreach (var polygon in polygons)
        {
            string color = GenerateRandomColor();
            svgBuilder.AppendLine($"<polygon points='");
            foreach (var vertex in polygon)
            {
                svgBuilder.Append($"{(vertex.X * scale) + xOffset},{(vertex.Y * scale) + yOffset} ");
            }
            svgBuilder.AppendLine($"' fill='{color}' stroke='black' />");
        }

        svgBuilder.AppendLine("</svg>");

        return new Svg(svgBuilder.ToString(), (int)gridSize, (int)gridSize);
    }

    private static float CalculateGridSize(params Polygon[] polygons)
    {
        float maxSize = 0;
        foreach (var polygon in polygons)
        {
            foreach (var vertex in polygon)
            {
                var size = Math.Max(Math.Abs(vertex.X), Math.Abs(vertex.Y));
                maxSize = Math.Max(maxSize, size);
            }
        }
        var gridSize = 2 * maxSize;
        return gridSize;
    }

    private static string GenerateRandomColor()
    {
        byte[] colorBytes = new byte[3];
        random.NextBytes(colorBytes);
        string color = "#" + BitConverter.ToString(colorBytes).Replace("-", string.Empty);
        return color;
    }
}


public class Vertex
{
    public float X { get; set; }
    public float Y { get; set; }
}