<Query Kind="Statements">
  <Reference Relative="..\..\..\..\..\Frank.Libraries\src\Frank.Libraries.DataStorage\bin\Debug\net7.0\Frank.Libraries.DataStorage.dll">C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.DataStorage\bin\Debug\net7.0\Frank.Libraries.DataStorage.dll</Reference>
  <Reference Relative="..\..\..\..\..\Frank.Libraries\src\Frank.Libraries.DataStorage\bin\Debug\net7.0\Frank.Libraries.Json.dll">C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.DataStorage\bin\Debug\net7.0\Frank.Libraries.Json.dll</Reference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Frank.Libraries.DataStorage.Json</Namespace>
  <Namespace>Frank.Libraries.DataStorage.Abstractions</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
</Query>

var builder = AppUtil.GetHostApplicationBuilder();

builder.Services.AddJsonDataStore(repos => {
    repos.AddRepository<Person>();
}, config => {
    config.Folder = @"C:\repos\frankhaugen\frankhaugen.github.io\LinqPadFiles\Scripts\Frank.Libraries\DataStorage";
});

builder.Services.AddHostedService<PersonService>();

var app = builder.Build();

try
{	        
    await app.StartAsync();
}
catch (Exception ex)
{
    builder.Services.DumpServices();
    ex.ToString().Dump();
}


public class PersonService : BackgroundService
{
    private readonly IRepository<Person> _repository;

    private readonly ILogger<PersonService> _logger;

    public PersonService(IRepository<Person> repository, ILogger<PersonService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PersonService is running.");

        var peopleInitial = await _repository.GetAllAsync();

        _logger.LogInformation("Retrieved {Count} people from the repository.", peopleInitial.Count());

        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "John Doe"
        };

        await _repository.AddAsync(person);

        _logger.LogInformation("Added person {Name} to the repository.", person.Name);

        var peopleUpdated = await _repository.GetAllAsync();

        _logger.LogInformation("Retrieved {Count} people from the repository.", peopleUpdated.Count());

        var personToUpdate = peopleUpdated.FirstOrDefault(p => p.Id == person.Id);
        personToUpdate.Name = "Jane Doe";
        await _repository.UpdateAsync(personToUpdate);
        _logger.LogInformation("Updated person {Name} in the repository.", personToUpdate.Name);
    }
}


public class Person : IEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}