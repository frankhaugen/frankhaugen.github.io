<Query Kind="Statements">
  <NuGetReference>AvalonEdit</NuGetReference>
  <NuGetReference>Extended.Wpf.Toolkit</NuGetReference>
  <NuGetReference>LiveCharts.Wpf</NuGetReference>
  <NuGetReference>OxyPlot.Wpf</NuGetReference>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Windows.Controls</Namespace>
</Query>


var window = new MainWindow();

window.Show();

public class MainWindow : Window
{
    public MainWindow()
    {
        Title = "Hello World";
        Width = 400;
        Height = 300;

        var button = new Button
        {
            Content = "Click me!",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        button.Click += Button_Click;

        Content = button;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Button clicked!");
    }
}
