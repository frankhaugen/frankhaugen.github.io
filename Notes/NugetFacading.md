# Facading NuGet Dependencies in .NET

When building a library or application in .NET, you may have dependencies on external libraries that you don't want to expose to the consumers of your library. This can be achieved by using the `internal` keyword and creating a facade that exposes only the necessary functionality to the consumers.

In .NET, you can manage your dependencies in such a way that they are not exposed to the consumers of your library. This involves making use of the `internal` keyword and managing project references carefully.

## Internal Dependencies in .NET

1. **Using `internal` Keyword**: Mark classes and methods you don’t want to expose to other assemblies as `internal`.

2. **Project File Configuration**: You can modify the project file to control which dependencies are exposed.

Here's a detailed explanation:

## Step 1: Marking Classes and Methods as `internal`

This limits their visibility to within the same assembly.

```csharp
// FileScoped namespace
namespace MyProject.Internal;

using Humanizer; // Internal usage

internal static class HumanizerWrapper
{
    internal static string HumanizeDate(DateTime date)
    {
        return date.Humanize();
    }

    internal static string HumanizeNumber(int number)
    {
        return number.ToWords();
    }
}
```

## Step 2: Exposing a Facade

Create a public facade that only exposes what you want:

```csharp
// FileScoped namespace
namespace MyProject.Facade;

using MyProject.Internal;

public static class HumanizerFacade
{
    public static string HumanizeDate(DateTime date)
    {
        return HumanizerWrapper.HumanizeDate(date);
    }

    public static string HumanizeNumber(int number)
    {
        return HumanizerWrapper.HumanizeNumber(number);
    }
}
```

## Step 3: Configuring Your Project File

Ensure that the Humanizer dependency is not directly exposed in your NuGet package by setting the `PrivateAssets` attribute in your project file.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net7.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Humanizer" Version="2.10.1">
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
  </ItemGroup>

</Project>
```

The `PrivateAssets` attribute ensures that the Humanizer package is used internally and not exposed to any project that consumes your library.

## Step 4: Using the Facade

Consumers of your library will use the facade without directly interacting with the Humanizer library:

```csharp
using MyProject.Facade;

class Program
{
    static void Main()
    {
        Console.WriteLine(HumanizerFacade.HumanizeDate(DateTime.Now));
        Console.WriteLine(HumanizerFacade.HumanizeNumber(12345));
    }
}
```

## Summary

By marking the internal implementation as `internal` and carefully managing your project dependencies, you can effectively control which parts of your library are exposed to the consumers. This way, you provide a clean and controlled API while keeping the internal dependencies hidden.