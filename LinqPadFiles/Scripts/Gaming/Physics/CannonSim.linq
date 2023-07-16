<Query Kind="Statements">
  <Reference Relative="..\..\..\..\Csharp\CollisionDetection\CollisionDetection\bin\Debug\net7.0\CollisionDetection.dll">C:\repos\frankhaugen\frankhaugen.github.io\Csharp\CollisionDetection\CollisionDetection\bin\Debug\net7.0\CollisionDetection.dll</Reference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Numerics</Namespace>
</Query>

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
float cannonBallMass = 5; // Cannon ball mass in kg
float launchAngleDegrees = 45; // Launch angle in degrees
float gravityStrength = 9.81f; // Gravity strength in m/s²

var pointsInTime = new Dictionary<TimeSpan, Vector3>();
var ball = PolygonFactory.CreateCircle("Ball", 3, 6);
var viewBox = new ViewBox(0, 0, 6000, 4000);
var renderer = new SvgRenderer(viewBox);
var sim = new Simulator();
var cannonSim = new CannonSimulator(0.05, cannonBallMass, EulerAnglesToVector3(launchAngleDegrees, 0, 0), new(0, gravityStrength, 0));

sim.SimulationSpeed = 100;
sim.TimeIncrement = TimeSpan.FromMilliseconds(10);


sim.Run(5000, x =>
{
    var newPosition = cannonSim.GetPosition(x);
    
    if (newPosition.Y < 0)
    {
        sim.Stop();
        
        var lastPosition = pointsInTime.Last().Value;
        var lastTime = pointsInTime.Last().Key;
        var impactPoint = Midpoint(lastPosition, newPosition);
        
        var impact = new {
            Time = MidpointTime(x, lastTime),
            Distance = Distance(Vector3.Zero, impactPoint),
            Point = impactPoint
        };
        
        impact.Dump("Impact!");
    }

    pointsInTime.Add(x, newPosition);
    ball.Translate(new Vertex(newPosition.X, newPosition.Y));
});

void Print(Svg svg, string heading = "")
{
    Util.ClearResults();
    svg.Dump(heading);

    svg.HtmlElement.ToString().Dump("Raw");
}

viewBox.GetType().Assembly.GetName().Dump("Assembly");
//pointsInTime.Dump();

PlotHelper.PlotDictionaryV1(pointsInTime, "Cannon Ball");

static Vector3 EulerAnglesToVector3(float pitch, float yaw, float roll)
{
    // Convert degrees to radians
    float pitchRadians = (float)(Math.PI / 180 * pitch);
    float yawRadians = (float)(Math.PI / 180 * yaw);
    float rollRadians = (float)(Math.PI / 180 * roll);

    float x = (float)Math.Cos(yawRadians) * (float)Math.Cos(pitchRadians);
    float y = (float)Math.Sin(pitchRadians);
    float z = (float)Math.Sin(yawRadians) * (float)Math.Cos(pitchRadians);

    return new Vector3(x, y, z);
}

static Vector3 Midpoint(Vector3 pointA, Vector3 pointB)
{
    return (pointA + pointB) / 2;
}

static TimeSpan MidpointTime(TimeSpan timeA, TimeSpan timeB)
{
    return timeA + ((timeB - timeA) / 2);
}

static float Distance(Vector3 pointA, Vector3 pointB)
{
    return (float)Math.Sqrt(Math.Pow((pointB.X - pointA.X), 2)
                   + Math.Pow((pointB.Y - pointA.Y), 2)
                   + Math.Pow((pointB.Z - pointA.Z), 2));
}

public class CannonSimulator
{
    public const float GunpowderEnergyPerGramInJoules = 3000;
    
    private readonly Vector3 _initialVelocity;
    private readonly Vector3 _gravity;

    public CannonSimulator(double gramsOfGunpowder, float mass, Vector3 launchDirection, Vector3 gravity)
    {
        _gravity = gravity;
        var initialForce = gramsOfGunpowder * GunpowderEnergyPerGramInJoules; // Convert grams to joules (energy)
        _initialVelocity = GetInitialVelocity(initialForce, mass, launchDirection);
    }

    public Vector3 GetPosition(TimeSpan time)
    {
        float t = (float)time.TotalSeconds;
        float x = _initialVelocity.X * t;
        float y = _initialVelocity.Y * t - 0.5f * _gravity.Y * t * t;
        float z = _initialVelocity.Z * t - 0.5f * _gravity.Z * t * t;

        return new Vector3(x, y, z);
    }

    public static Vector3 GetInitialVelocity(double force, float mass, Vector3 direction)
    {
        var initialSpeed = (float)(force / mass);
        //direction = Vector3.Normalize(direction); // Normalize direction vector
        return direction * initialSpeed;
    }
}