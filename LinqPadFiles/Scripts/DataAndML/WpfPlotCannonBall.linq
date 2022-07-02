<Query Kind="Statements">
  <NuGetReference>LinqToRegex</NuGetReference>
  <NuGetReference>ScottPlot.WPF</NuGetReference>
  <Namespace>ScottPlot</Namespace>
  <Namespace>ScottPlot.Control</Namespace>
  <Namespace>ScottPlot.Control.EventProcess</Namespace>
  <Namespace>ScottPlot.Control.EventProcess.Events</Namespace>
  <Namespace>ScottPlot.DataStructures</Namespace>
  <Namespace>ScottPlot.Drawing</Namespace>
  <Namespace>ScottPlot.Drawing.Colormaps</Namespace>
  <Namespace>ScottPlot.Drawing.Colorsets</Namespace>
  <Namespace>ScottPlot.MinMaxSearchStrategies</Namespace>
  <Namespace>ScottPlot.Plottable</Namespace>
  <Namespace>ScottPlot.Renderable</Namespace>
  <Namespace>ScottPlot.Statistics</Namespace>
  <Namespace>ScottPlot.Statistics.Interpolation</Namespace>
  <Namespace>ScottPlot.Styles</Namespace>
  <Namespace>ScottPlot.Ticks</Namespace>
  <Namespace>ScottPlot.Ticks.DateTimeTickUnits</Namespace>
  <Namespace>ScottPlot.WinForms.Events</Namespace>
  <Namespace>ScottPlot.WPF</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Windows.Controls</Namespace>
</Query>

var plot = new ScottPlot.WpfPlot();

var list = new Dictionary<DateTime, double>();
int pointCount = 20;
var firstDay = new DateTime(2020, 1, 22);

var position = new Vector2();

for (int i = 0; i < pointCount; i++)
{
	var date = firstDay.AddDays(i);
	list.Add(date, i);
}

plot.Plot.AddScatter(list.Keys.Select(x => x.ToOADate()).ToArray(), list.Values.ToArray());
plot.Plot.XAxis.DateTimeFormat(true);
plot.Plot.XAxis.ManualTickSpacing(1, ScottPlot.Ticks.DateTimeUnit.Day);
plot.Plot.XAxis.TickLabelStyle(rotation: 45);
plot.Plot.XAxis.SetSizeLimit(min: 50);

plot.Render();
var window = new Window();
window.Content = plot;
window.Show();


public readonly record struct DateTimeValue(DateTime DateTime, double Value);


public static class Physics
{
	public static Vector2 Trajectory(Vector2 origin, int instant, float launchAngle, float initialVelocity)
	{
		var vector2 = origin;
		vector2.X = CalculateHorizontalVelocity(instant, initialVelocity, launchAngle);
		vector2.Y = CalculateVerticalVelocity(instant, initialVelocity, launchAngle);
		return vector2;
	}
	public static float CalculateHorizontalVelocity(int instant, float initialVelocity, float launchAngle)
	{
		return initialVelocity * MathF.Cos(launchAngle) * instant;
	}
	public static float CalculateVerticalVelocity(int instant, float initialVelocity, float launchAngle)
	{
		return initialVelocity * MathF.Sin(launchAngle) * instant - 0.5F * 9.81F * instant * instant;
	}
}