

/// <summary>
/// Represents a polygon defined by a list of vertices.
/// </summary>
/// <param name="Name"></param>
public class Polygon : List<Vertex>
{
    public Polygon(string name, params Vertex[] vertices) : base(vertices)
    {
        Name = name;
    }

    public string Name { get; set; }

    public void Translate(Vertex position)
    {
        for (int i = 0; i < Count; i++)
        {
            this[i] = new Vertex(this[i].X + position.X, this[i].Y + position.Y);
        }
    }

    public Vertex Center => new Vertex(this.Average(v => v.X), this.Average(v => v.Y));

    public Edge[] Edges => this.Select((v, i) => new Edge(v, this[(i + 1) % this.Count])).ToArray();


    public static ViewBox CalculateViewBox(Polygon[] polygons, int padding = 25)
    {
        if (polygons.Length == 0)
        {
            return new ViewBox(-100, -100, 200, 200);
        }

        var minX = polygons.SelectMany(polygon => polygon).Min(vertex => vertex.X);
        var minY = polygons.SelectMany(polygon => polygon).Min(vertex => vertex.Y);
        var maxX = polygons.SelectMany(polygon => polygon).Max(vertex => vertex.X);
        var maxY = polygons.SelectMany(polygon => polygon).Max(vertex => vertex.Y);

        var width = maxX - minX + padding * 2;
        var height = maxY - minY + padding * 2;

        var viewBox = new ViewBox((int)minX, (int)minY, (int)width, (int)height);
        return viewBox;
    }
}

