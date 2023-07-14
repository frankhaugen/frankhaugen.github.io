<Query Kind="Statements">
  <Reference Relative="..\..\..\..\Csharp\CollisionDetection\CollisionDetection\bin\Debug\net7.0\CollisionDetection.dll">C:\repos\frankhaugen\frankhaugen.github.io\Csharp\CollisionDetection\CollisionDetection\bin\Debug\net7.0\CollisionDetection.dll</Reference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
</Query>

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

var boxA = PolygonFactory.CreateSquare("A", 7, 4);
boxA.Translate(new Vertex { X = 15, Y = 25 });

var boxB = PolygonFactory.CreateCircle("B", 5, 32);
boxB.Translate(new Vertex { X = 55, Y = 25 });

var consoleRenderer = new ConsoleRenderer(150, 4f);

var viewBox = new ViewBox(0, 0, 600, 400);
var renderer = new SvgRenderer(viewBox);

var sim = new Simulator();
sim.SimulationSpeed = 10;
sim.TimeIncrement = TimeSpan.FromMilliseconds(100);

sim.Run(600, x =>
{
    boxA.Translate(new((float)sim.TimeIncrement.TotalSeconds, 0));
    var result = renderer.Render(boxA, boxB);

    Print(new Svg(result, (int)viewBox.Width, (int)viewBox.Height, viewBox.ToString()));

    if (CollisionDetector.DetectCollission(new[] { boxA, boxB }, out var collision) && collision != null)
    {
        $"Collision detected between {collision.Value.Polygon1} and {collision.Value.Polygon2} at {collision.Value.PointOfCollision} with a center distance of {collision.Value.CenterDistance}".Dump("Boom!");
        sim.Stop();
    }
});

void Print(Svg svg, string heading = "")
{
    Util.ClearResults();
    svg.Dump(heading);

    svg.HtmlElement.ToString().Dump("Raw");

    var result2 = consoleRenderer.Render(boxA, boxB);
    result2.Dump("Console");
}


viewBox.GetType().Assembly.GetName().Dump("Assembly");