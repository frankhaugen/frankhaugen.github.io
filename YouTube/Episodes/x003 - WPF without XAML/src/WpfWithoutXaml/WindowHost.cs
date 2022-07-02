using System.Runtime.InteropServices;
using System.Windows;

namespace WpfWithoutXaml;

public class WindowHost : BackgroundService
{
    [DllImport("kernel32")]
    static extern bool FreeConsole();

    private readonly ILogger<WindowHost> _logger;
    private readonly IServiceProvider _serviceProvider;

    public WindowHost(ILogger<WindowHost> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting");

        using var scope = _serviceProvider.CreateScope();
        var window = scope.ServiceProvider.GetRequiredService<MainWindow>();
        var app = scope.ServiceProvider.GetRequiredService<App>();

        app.ShutdownMode = ShutdownMode.OnMainWindowClose;
        app.Exit += (sender, args) =>
        {
            FreeConsole();
            app.Shutdown();
            Environment.Exit(0);
        };

        app.Run(window);
    }
}