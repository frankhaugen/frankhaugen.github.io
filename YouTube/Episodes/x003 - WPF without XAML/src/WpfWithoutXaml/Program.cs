using System.Globalization;
using System.Runtime.InteropServices;

namespace WpfWithoutXaml;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        AllocConsole();

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddScoped<App>();
                services.AddHostedService<Worker>();
                services.AddHostedService<WindowHost>();
                services.AddScoped<MainWindow>();
            })
            .Build();

        host.Run();
    }

    [DllImport("kernel32")]
    static extern bool AllocConsole();
}