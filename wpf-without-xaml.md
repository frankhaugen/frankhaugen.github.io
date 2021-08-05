# WPF without XAML

## Why would you use WPF without XAML
The easy answer is: I don't like XAML! Even using MVVM, it's just bad HTML/XML that is slow to work with, if you want some nesting and dynamic component adding

## How to get started?
1. Use the dotnet cli or Visual Studio project creator to create a "Worker project" and change the .csproj to look something like this:
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
2. Add an App.cs that extends `Application`
```c#
using System.Windows;

namespace Demo.WpfApplication
{
    public class App : Application
    {
    }
}
```
3. Create a MainWindow.cs to be your main window
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
5. Create a WindowHost.cs -file that injects the `IServiceProvider`
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
