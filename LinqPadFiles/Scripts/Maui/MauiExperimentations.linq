<Query Kind="Program">
  <Reference Relative="ReferenceProject\ReferenceProject.csproj">C:\repos\frankhaugen\frankhaugen.github.io\LinqPadFiles\Scripts\Maui\ReferenceProject\ReferenceProject.csproj</Reference>
  <Namespace>Microsoft.Maui</Namespace>
  <Namespace>Microsoft.Maui.Hosting</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
    var app = MauiApp.CreateBuilder()
        .UseMauiApp<App>()
        .Build();

    app.Run();
}

public class App
{
    
}