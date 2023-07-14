<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
</Query>

// Define variables
List <Vector2> result = new();            

double x0 = 0;
double y0 = 0;
double v0 = 30;
double angle = 45;
double a = -9.8;

// Convert angle to radians
double radians = angle * Math.PI / 180;

// Calculate initial velocity in x and y directions
double v0x = v0 * Math.Cos(radians);
double v0y = v0 * Math.Sin(radians);

// Set the time step
double dt = 0.1;

// Loop over time
for (double t = 0; ; t += dt)
{
    // Calculate the position of the projectile at time t
    double x = x0 + v0x * t;
    double y = y0 + v0y * t + 0.5 * a * t * t;

    // Check if the projectile has hit the ground
    if (y < 0)
    {
        break;
    }
    
    // Add results
    result.Add(new ((float)x, (float)y));
}

// Output results
result.Count.Dump();
result.Chart(x => x.X, x => x.Y, LINQPad.Util.SeriesType.Spline).Dump();