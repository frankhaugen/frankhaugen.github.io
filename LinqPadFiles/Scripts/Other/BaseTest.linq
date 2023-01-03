<Query Kind="Statements">
  <NuGetReference>Divergic.Logging.Xunit</NuGetReference>
  <NuGetReference>Microsoft.EntityFrameworkCore</NuGetReference>
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Logging.Abstractions</NuGetReference>
  <NuGetReference>SpecFlow.xUnit</NuGetReference>
  <Namespace>System.Globalization</Namespace>
</Query>


public abstract class TestBaseBase
{
    public TestBaseBase()
    {
        
    }
    
    public IServiceProvider ServiceProvider { get; }
}

public abstract class TestBase<T> : TestBaseBase
{
    public T Sut { get; set; }
    
}

































