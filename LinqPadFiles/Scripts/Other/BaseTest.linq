<Query Kind="Statements">
  <NuGetReference>AutoMapper</NuGetReference>
  <NuGetReference>Microsoft.EntityFrameworkCore</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Options.ConfigurationExtensions</NuGetReference>
  <NuGetReference>NSubstitute</NuGetReference>
  <NuGetReference>xunit</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>


public abstract class TestBaseBase
{
    /// <summary>No logic has run before this</summary>
    public IServiceCollection InitialServices { get; set; }
    
    /// <summary>Services that the Subject Under Test is dependant on and has been dynamically instanciated usong NSubstitute</summary>
    public IServiceCollection SutDependentServices { get; set; }
    
    public TestBaseBase()
    {
        
    }
    
    public IServiceProvider ServiceProvider { get; }
}

public abstract class TestBase<T> : TestBaseBase
{
    public T Sut { get; set; }
 
    
}

































