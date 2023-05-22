<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

#load ".\Mapping"
#load ".\Cloning"
#load ".\Converter"


public class MappingImplementation
{
    IMappingProvider provider = new MappingProvider();

    public void Run()
    {
        provider.RegisterProvider(new SourceDestinationMapping());

        var source = new Source()
        {
            FirstName = "Frank",
            LastName = "Sinatra"
        };

        var destination = provider.Map<Source, Destination>(source);

        destination.Dump();
    }
}

public class SourceDestinationMapping : IMappingDefinition<Source, Destination>
{
    public Destination Map(Source source)
    {
        return new() { FullName = $"{source.FirstName} {source.LastName}" };
    }

    public IEnumerable<Destination> Map(IEnumerable<Source> source)
    {
        foreach (var element in source)
        {
            yield return Map(element);
        }
    }
}

public class Source
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class Destination
{
    public string FullName { get; set; }
}