using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WpfWithoutXaml.Controls;
using WpfWithoutXaml.Extensions;

namespace WpfWithoutXaml;

public class MainWindow : Window
{
    private readonly ILogger<MainWindow> _logger;

    private readonly TabControl _tabControl = new();

    public MainWindow(ILogger<MainWindow> logger)
    {
        _logger = logger;

        var easy = new EasyStackPanel();

        easy.Add(
            new TextInput("Hey World!", (o, args) => { }, "Change me"),
            new TextInput("Hey World!", (o, args) => { }, "Change me"),
            new TextInput("Hey World!", (o, args) => { }, "Change me"),
            new TextInput("Hey World!", (o, args) => { }, "Change me"),
            new TextInput("Hey World!", (o, args) => { }, "Change me"),
            new TextInput("Hey World!", (o, args) => { }, "Change me")
            );



        _tabControl.Items.Add(new TabItem()
        {
            Header = "Page 1"
        });
        _tabControl.Items.Add(new TabItem()
        {
            Header = "Page 2",
            Content = easy
        });

        ConfigureWindow();

        Content = _tabControl;
    }

    private void ConfigureWindow()
    {
        var size = this.GetScreenWorkingAreaSize();
        MinWidth = 512;
        MinHeight = 256;
        MaxHeight = size.Height;
        MaxWidth = size.Width;

        SizeToContent = SizeToContent.WidthAndHeight;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _logger.LogInformation("Closing");
        base.OnClosing(e);
    }
}