<Query Kind="Program">
  <NuGetReference>xunit</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Xunit</Namespace>
  <Namespace>Xunit.Runners</Namespace>
</Query>

#load "xunit"

void Main()
{
    //RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.

    // Set the path to the test project's DLL
    var testAssemblyPath = @"C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.Tests\bin\Debug\net7.0\Frank.Libraries.Tests.dll";
    
    // Discover and run all tests in the assembly
    using (var runner = AssemblyRunner.WithoutAppDomain(testAssemblyPath))
    {
        runner.OnDiagnosticMessage += args => args.Dump();
        
        runner.Start();
    }
}