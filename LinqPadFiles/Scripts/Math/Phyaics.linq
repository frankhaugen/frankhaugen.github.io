<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
  <RuntimeVersion>7.0</RuntimeVersion>
</Query>

var positions = new List<Vector2>();
var velocities = new List<Vector2>();
var position = new Vector2(0, 8);
var startSpeed = 12.0f; // m/s
var startAngle = 45.0f; // degrees

var time = TimeSpan.Zero;
var stopwatch = Stopwatch.StartNew();
var frameLength = TimeSpan.FromMilliseconds(1000);

var initialVelocity = Trajectory(position, Convert.ToSingle(time.TotalSeconds), startAngle, startSpeed);

var velocity = initialVelocity;

while(position.Y > -0.00001 && position.Y < 1000)
{
    time = time.Add(frameLength);
    var seconds = Convert.ToSingle(time.TotalSeconds);
    //velocity = Trajectory(position, seconds, startAngle, startSpeed);
    position = TrajectoryV2(position, seconds, startAngle);
    
    //position = position + velocity * seconds;
    
    if (position.Y > -0.0000001)
    {
        positions.Add(position);
    }
    else
    {
        break;
    }
}

positions.Chart(x => x.X, y => y.Y, Util.SeriesType.Spline).Dump();
positions.Dump();

Vector2 TrajectoryV2(Vector2 origin, float time, float angle)
{
    var gravity = 9.80665F;
    
    var Sx = origin.X * MathF.Cos(angle) * time;
    var Sy = origin.Y * MathF.Sin(angle) * time - 0.5F * gravity * MathF.Pow(time, 2);
    
    return new Vector2(Sx, Sy);
}

Vector2 Trajectory(Vector2 origin, float instant, float launchAngle, float initialVelocity)
{
    origin.X = CalculateHorizontalVelocity(instant, initialVelocity, launchAngle);
    origin.Y = CalculateVerticalVelocity(instant, initialVelocity, launchAngle);
    return origin;
}

float CalculateHorizontalVelocity(float instant, float initialVelocity, float launchAngle) => initialVelocity * MathF.Cos(ToRadian(launchAngle)) * instant;
float CalculateVerticalVelocity(float instant, float initialVelocity, float launchAngle) => initialVelocity * MathF.Sin(ToRadian(launchAngle)) * instant - 0.5F * 9.81F * instant * instant;

float ToRadian(float angle) => angle * (MathF.PI / 180);
