<Query Kind="Statements">
  <NuGetReference>DecimalMath.DecimalEx</NuGetReference>
  <Namespace>DecimalMath</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
</Query>


var positions = new List<Vector2>();

var origin = new Position(1, new Vector2(1, 1), 60, 90);

var currentPosition = origin.Coordinates;

for (int i = 1; i < 100; i++)
{
    
    currentPosition = Trajectory(new Position(i, currentPosition, origin.Angle, origin.Velocity));
    positions.Add(currentPosition);
}

positions.Dump();
positions.Chart(x => x.X, x => x.Y).Dump();

Vector2 Trajectory(Position position)
{
    var vector2 = position.Coordinates;
    vector2.X = CalculateHorizontalVelocity(position.Instant, position.Velocity, position.Angle);
    vector2.Y = CalculateVerticalVelocity(position.Instant, position.Velocity, position.Angle);
    return vector2;
}

static float CalculateHorizontalVelocity(int instant, float initialVelocity, float launchAngle)
{
    return initialVelocity * MathF.Cos(launchAngle) * instant;
}

static float CalculateVerticalVelocity(int instant, float initialVelocity, float launchAngle)
{
    return initialVelocity * MathF.Sin(launchAngle) * instant - 0.5F * 9.81F * instant * instant;
}

readonly record struct Position(int Instant, Vector2 Coordinates, float Angle, float Velocity);
