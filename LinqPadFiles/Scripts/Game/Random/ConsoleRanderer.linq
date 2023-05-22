<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
</Query>

void Main()
{
    IRenderer<double> renderer = new ConsoleRenderer<double>(80, 40);
    var polygon = new Polygon<double>(new[]
    {
        new Vector3<double>(-1, -1, 0),
        new Vector3<double>(-1, 1, 0),
        new Vector3<double>(1, 1, 0),
        new Vector3<double>(1, -1, 0),
        new Vector3<double>(0, 0, 1)
    });

    var frame = new Frame<double>(polygon, new Vector3<double>(0, 0, 5), new Vector3<double>(0, 0, 0));

    renderer.Render(frame);
    
}

public struct Vector3<T> where T : INumber<T>
{
    public T X { get; }
    public T Y { get; }
    public T Z { get; }

    public Vector3(T x, T y, T z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}

public class Polygon<T> where T : INumber<T>
{
    private readonly List<Vector3<T>> _points;

    public Polygon(IEnumerable<Vector3<T>> points)
    {
        _points = new List<Vector3<T>>(points);
    }

    public IReadOnlyList<Vector3<T>> Points => _points;
}



public class Frame<T> where T : INumber<T>
{
    public Polygon<T> Polygon { get; }
    public Vector3<T> Position { get; }
    public Vector3<T> Rotation { get; }

    public Frame(Polygon<T> polygon, Vector3<T> position, Vector3<T> rotation)
    {
        Polygon = polygon;
        Position = position;
        Rotation = rotation;
    }
}



public interface IRenderer<T> where T : INumber<T>
{
    void Render(Frame<T> frame);
}

public class ConsoleRenderer<T> : IRenderer<T> where T : INumber<T>
{
    private readonly T _width;
    private readonly T _height;

    public ConsoleRenderer(T width, T height)
    {
        _width = width;
        _height = height;
    }

    public void Render(Frame<T> frame)
    {
        // Determine the scaling factor based on the size of the polygon and the size of the console window
        var xScale = _width / (frame.Polygon.Points.Max(p => p.X) - frame.Polygon.Points.Min(p => p.X));
        var yScale = _height / (frame.Polygon.Points.Max(p => p.Y) - frame.Polygon.Points.Min(p => p.Y));
        var scale = T.Min(xScale, yScale);

        // Translate and rotate the polygon based on the frame's position and rotation
        var translatedPoints = frame.Polygon.Points.Select(p => new Vector3<T>(p.X - frame.Position.X, p.Y - frame.Position.Y, p.Z - frame.Position.Z));
        var rotatedPoints = translatedPoints.Select(p => new Vector3<T>(
            p.X * frame.Rotation.X + p.Z * frame.Rotation.Y,
            p.Y,
            p.X * -frame.Rotation.Y + p.Z * frame.Rotation.X
        ));

        // Convert the polygon's points to screen coordinates and draw them as ASCII art
        Console.Clear();
        foreach (var point in rotatedPoints)
        {
            var x = (int)Math.Round(Convert.ToDouble(point.X * scale + _width) / 2);
            var y = (int)Math.Round(Convert.ToDouble(point.Y * scale + _height) / 2);
            Console.SetCursorPosition(x, y);
            Console.Write("*");
        }
    }
}
