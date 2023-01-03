<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>FlatPhysics</Namespace>
</Query>

#load "C:\repos\FlatPhysics\*.cs"

var origin = new FlatVector(0, 100);

FlatBody.CreateCircleBody(10, origin, 1, false, 1, out var ball, out var errors);

var world = new FlatWorld();
world.AddBody(ball);
var time = TimeSpan.Zero;

for (int i = 0; i <= 8; i++)
{
    time = time.Add(TimeSpan.FromSeconds(i));
    //world.Step(Convert.ToSingle(time.TotalSeconds), i);
    //ball.Position.Y.Dump();
    
    AltCalc(i, ball.LinearVelocity.Y).Dump();
}

double AltCalc(double time, double speed)
{
    var gravity = 9.80665;
    
    var velocity = speed + (gravity * time);
    
    return velocity;
}