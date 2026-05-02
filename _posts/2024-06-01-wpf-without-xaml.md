---
title: WPF without XAML
tags:
  - WPF
  - dotnet
description: Worker + Generic Host + code-only windows. XAML optional; sanity less optional.
---

People assume WPF means XAML. It doesn't have to — you can drive the whole thing from C# if you accept a bit of ceremony and stop pretending `Application` is magic.

## Facts: how WPF expects an application to boot

Microsoft documents `System.Windows.Application` as the entry point for WPF UI—a message pump (`Run`) tied to Windows message semantics and **STA threading**.[^wpf-app]

.NET console templates historically used `[STAThread]` on `Main` because COM and classic UI frameworks expected STA; WPF shares that heritage even when you host inside `Microsoft.Extensions.Hosting`.[^stathread]

The **Worker SDK** (`Microsoft.NET.Sdk.Worker`) pulls in hosting primitives (`BackgroundService`, configuration, DI) documented under *.NET Generic Host*.[^generic-host][^worker-template]

This pattern pairs:

| Piece | Responsibility |
|-------|----------------|
| `Host.CreateDefaultBuilder` | Logging, config, DI wiring |
| `BackgroundService` (`WindowHost`) | Keeps process alive while UI runs |
| `Application.Run(Window)` | Starts WPF dispatcher loop |

## Why would you use WPF without XAML

The easy answer: I don't like XAML. Even with MVVM it is fiddly XML that fights you the moment you want real nesting, dynamic trees, or anything that is not a designer-friendly toy demo.

**Tradeoff snapshot:**

| Approach | Upside | Downside |
|----------|--------|----------|
| XAML + MVVM | Designer tooling, markup/styles separation | Verbosity + merge dictionaries rabbit holes |
| Pure C# trees | Full refactor tooling; dynamic UI trivial | Verbose construction code |
| Worker + Host | Unified DI/config with headless services | Extra hop vs classic `App.xaml` bootstrap |

## Pitfalls worth naming

1. **Lifetime mismatch** — treating `MainWindow` like a singleton when your DI scope says scoped invites ghosts after window close.
2. **Async void on UI thread** — still the easiest footgun in event handlers.
3. **Shutdown semantics** — forcing `Environment.Exit` (as below) is blunt; production apps usually coordinate `CancellationToken` + `IHostApplicationLifetime`.

## How to get started?

#### 1. Use the dotnet CLI or Visual Studio project creator to create a "Worker project" and change the .csproj to look something like this:

Prefer a modern `TargetFramework` (`net8.0-windows` or newer LTS) unless you truly need `net5.0-windows`.

```xml
<Project Sdk="Microsoft.NET.Sdk.Worker">
    <PropertyGroup>
        <TargetFramework>net8.0-windows</TargetFramework>
        <LangVersion>latest</LangVersion>
        <Nullable>enable</Nullable>
        <OutputType>Exe</OutputType>
        <UseWpf>true</UseWpf>
    </PropertyGroup>
</Project>
```

#### 2. Add an `App.cs` that extends `Application`

```c#
using System.Windows;

namespace Demo.WpfApplication
{
    public class App : Application
    {
    }
}
```

#### 3. Create a `MainWindow.cs` to be your main window

```c#
using System.Windows;
using System.Windows.Controls;

namespace Demo.WpfApplication
{
    public class MainWindow : Window
    {
        private readonly ILogger<MainWindow> _logger;

        public MainWindow(ILogger<MainWindow> logger)
        {
            _logger = logger;

            var button = new Button { Content = "Log something" };
            button.Click += (_, _) => _logger.LogInformation("Something");

            Content = button;
        }
    }
}
```

#### 4. Rename `Worker.cs` to `WindowHost.cs` then inject only the `IServiceProvider`

```c#
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.WpfApplication
{
    public class WindowHost : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public WindowHost(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var window = scope.ServiceProvider.GetRequiredService<MainWindow>();
            var app = scope.ServiceProvider.GetRequiredService<App>();
            window.Closed += (_, _) => Environment.Exit(666); // replace with graceful shutdown in real apps

            app.Run(window);
        }
    }
}

```

#### 5. Then finally edit Program.cs to look like this

```c#
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.WpfApplication
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                    services.AddScoped<App>();
                    services.AddScoped<MainWindow>();

                    services.AddHostedService<WindowHost>();
                });

            host.Build().Run();
        }
    }
}
```

[^wpf-app]: Microsoft Learn — `Application` class (WPF application model). https://learn.microsoft.com/dotnet/api/system.windows.application

[^stathread]: Microsoft Learn — `STAThreadAttribute`. https://learn.microsoft.com/dotnet/api/system.stathreadattribute

[^generic-host]: Microsoft Learn — *.NET Generic Host*. https://learn.microsoft.com/dotnet/core/extensions/generic-host

[^worker-template]: Microsoft Learn — `BackgroundService` base class for long-running hosted services. https://learn.microsoft.com/dotnet/api/microsoft.extensions.hosting.backgroundservice

## References

- [Microsoft Learn — WPF overview](https://learn.microsoft.com/dotnet/desktop/wpf/overview/)
- [Microsoft Learn — Application](https://learn.microsoft.com/dotnet/api/system.windows.application)
- [Microsoft Learn — Generic Host](https://learn.microsoft.com/dotnet/core/extensions/generic-host)
- [Microsoft Learn — Worker Service template](https://learn.microsoft.com/dotnet/core/extensions/workers)
