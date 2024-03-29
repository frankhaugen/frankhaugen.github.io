<Query Kind="Statements">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <NuGetReference>NodaTime</NuGetReference>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>NodaTime</Namespace>
  <Namespace>NodaTime.Calendars</Namespace>
  <Namespace>NodaTime.Extensions</Namespace>
  <Namespace>NodaTime.Text</Namespace>
  <Namespace>NodaTime.TimeZones</Namespace>
  <Namespace>NodaTime.TimeZones.Cldr</Namespace>
  <Namespace>NodaTime.Utility</Namespace>
  <Namespace>NodaTime.Xml</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
</Query>


var driver = CSharpGeneratorDriver.Create(new HelloSourceGenerator());
var compilation = CSharpCompilation.Create("My");
driver.RunGenerators(compilation).GetRunResult().Dump();


[Generator]
public class HelloSourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        // Find the main method
        var @namespace = "MyNamespace";
        var mainMethod = "Start";

        // Build up the source code
        string source = 
        $$"""
        // <auto-generated/>
        using System;

        namespace {{@namespace}};

        public static partial class {mainMethod.ContainingType.Name}
        {
            static partial void HelloFrom(string name) => Console.WriteLine($"Generator says: Hi from '{name}'");
        }
        """;
        var typeName = mainMethod;

        // Add the source code to the compilation
        context.AddSource($"{typeName}.g.cs", source);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required for this one
    }
}