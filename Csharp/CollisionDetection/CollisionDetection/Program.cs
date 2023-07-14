
var boxA = PolygonFactory.CreateSquare("A", 7, 4);
boxA.Translate(new Vertex { X = 65, Y = 25 });

var boxB = PolygonFactory.CreateSquare("B", 7, 4);
boxB.Translate(new Vertex { X = 100, Y = 27 });

IRenderer Renderer = new ConsoleRenderer(225, 4f);
var sim = new Simulator();
sim.SimulationSpeed = 10;
sim.TimeIncrement = TimeSpan.FromMilliseconds(100);

sim.Run(60, x =>
{
    boxA.Translate(new((float)sim.TimeIncrement.TotalSeconds, 0));
    var result = Renderer.Render(boxA, boxB);
    PrintResult(result);

    if (CollisionDetector.DetectCollission(new[] { boxA, boxB }, out var collision) && collision != null)
    {
        Console.WriteLine($"Collision detected between {collision.Value.Polygon1} and {collision.Value.Polygon2} at {collision.Value.PointOfCollision} with a center distance of {collision.Value.CenterDistance}");
        sim.Stop();
    }
});

void PrintResult(string result)
{
    try {
        Console.Out.Flush();

        Console.Clear();
     } catch { }
    Console.Out.Write(result);
}