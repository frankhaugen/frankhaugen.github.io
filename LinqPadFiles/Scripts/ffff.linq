<Query Kind="Statements" />


var trajectoryPoints = CalculateTrajectory(100, 45);  // 100 m/s initial velocity, 45 degree angle
var chart = Util.Chart(trajectoryPoints, 
	//.WithXAxisTitle("Distance (m)")
	//.WithYAxisTitle("Height (m)")
	//.WithLegend(false)
	;
chart.Dump();

static IEnumerable<(double X, double Y)> CalculateTrajectory(double initialVelocity, double angle, double gravity = 9.81)
{
	double radianAngle = Math.PI * angle / 180;
	double totalTime = (2 * initialVelocity * Math.Sin(radianAngle)) / gravity;
	for (double t = 0; t <= totalTime; t += 0.01)
	{
		double x = initialVelocity * t * Math.Cos(radianAngle);
		double y = initialVelocity * t * Math.Sin(radianAngle) - 0.5 * gravity * t * t;
		yield return (x, y);
	}
}