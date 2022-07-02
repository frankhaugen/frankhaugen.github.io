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


//var plot = new BarPlot(new double[2],new double[2],new double[2],new double[2]);

var plot = new ScottPlot.WpfPlot();
//plot.pl = new ScottPlot.Plot();

double[] values = DataGen.RandomWalk(100);
var sig = plot.Plot.AddSignal(values);
sig.YAxisIndex = 1;

plot.Plot.YAxis.Ticks(false);
plot.Plot.YAxis.Grid(false);
plot.Plot.YAxis2.Ticks(true);
plot.Plot.YAxis2.Grid(true);
plot.Plot.YAxis2.Label("Value");
plot.Plot.XAxis.Label("Sample Number");

plot.Render();

var window = new Window();
window.Content = plot;
window.Show();