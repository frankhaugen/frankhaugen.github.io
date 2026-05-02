---
title: WPF without XAML
tags:
  - WPF
  - dotnet
description: Worker + Generic Host + code-only windows. XAML optional; sanity less optional.
---

People assume WPF means XAML. It doesn't have to — you can drive the whole thing from C# if you accept a bit of ceremony and stop pretending `Application` is magic.

## Why would you use WPF without XAML

The easy answer: I don't like XAML. Even with MVVM it is fiddly XML that fights you the moment you want real nesting, dynamic trees, or anything that is not a designer-friendly toy demo.

## How to get started?
#### 1. Use the dotnet cli or Visual Studio project creator to create a "Worker project" and change the .csproj to look something like this:
```xml
<Project Sdk="Microsoft.NET.Sdk.Worker">
    <PropertyGroup>
        <TargetFramework>net5.0-windows</TargetFramework>
        <LangVersion>9.0</LangVersion>
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
            
            var button = new Button()
            {
                Content = "Log something"
            }
            
            button.Click += () => _logger.LogInformation("Something");
            
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
            window.Closed += (sender, args) => Environment.Exit(666); // use the code that you want/need

            app.Run(window);
        }
    }
}

```
#### 5. Then finally edit the the Program.cs to look like this
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
                    
                    // This MUST be last
                    services.AddHostedService<WindowHost>();
                });

            host.Build().Run();
        }
    }
}
```
