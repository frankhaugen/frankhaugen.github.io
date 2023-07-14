<Query Kind="Program">
  <NuGetReference>Microsoft.EntityFrameworkCore</NuGetReference>
  <NuGetReference>Microsoft.EntityFrameworkCore.Sqlite</NuGetReference>
  <Namespace>Microsoft.EntityFrameworkCore</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "xunit"

void Main()
{
    RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.
}

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IEntityChangeEventHandler _changeHandler;

    public UserRepository(DataContext context, IEntityChangeEventHandler changeHandler)
    {
        _context = context;
        _changeHandler = changeHandler;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
}

public class DataContext : DbContext
{
    public DbSet<Business> Businesses { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=myDb.db");
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
    }
}

public class EntityChangeEventHandler : IEntityChangeEventHandler
{
    public void HandleEntityAddEvent<T>(T entity) where T : IEntity
    {
        var changeEvent = new ChangeEvent<T>
        {
            EntityId = entity.Id,
            ChangeType = ChangeType.Create,
            Entity = entity
        };
        changeEvent.Dump();
    }

    public void HandleEntityUpdateEvent<T>(T entity) where T : IEntity
    {
        var changeEvent = new ChangeEvent<T>
        {
            EntityId = entity.Id,
            ChangeType = ChangeType.Update,
            Entity = entity
        };
        changeEvent.Dump("rrr");
        Console.WriteLine(entity.Id);
    }

    public void HandleEntityDeleteEvent<T>(T entity) where T : IEntity
    {
        var changeEvent = new ChangeEvent<T>
        {
            EntityId = entity.Id,
            ChangeType = ChangeType.Delete,
            Entity = entity
        };
        changeEvent.Dump();
    }
}

public class ChangeEvent<T> : ChangeEvent where T : IEntity
{
    public T Entity { get; set; }
}

public abstract class ChangeEvent
{
    public ChangeType ChangeType { get; set; }
    public Guid EntityId { get; set; }
}

public interface IEntityChangeEventHandler
{
    void HandleEntityAddEvent<T>(T entity) where T : IEntity;
    void HandleEntityUpdateEvent<T>(T entity) where T : IEntity;
    void HandleEntityDeleteEvent<T>(T entity) where T : IEntity;
}

public enum ChangeType
{
    Create,
    Update,
    Delete
}

public interface IEntity
{
    Guid Id {get; set;}
}

public class Business : IEntity
{
    public Guid Id { get; set; }
}

public class User : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Birthdate { get; set; }
}

#region private::Tests

[Fact]
public async Task GetByIdAsync_ReturnsCorrectUser()
{
    // Arrange
    var user = new User { Id = Guid.NewGuid(), Name = "John Doe", Birthdate = new DateTime(1990, 1, 1) };

    using (var context = new DataContext())
    {
        await context.Database.EnsureCreatedAsync();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        
        var repository = new UserRepository(context, new EntityChangeEventHandler());

        // Act
        var result = await repository.GetByIdAsync(user.Id);

        // Assert
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Birthdate, result.Birthdate);
        await context.Database.EnsureDeletedAsync();
    }
}

[Fact]
public async Task GetAllAsync_ReturnsAllUsers()
{
    // Arrange
    var users = new List<User>
    {
        new User { Id = Guid.NewGuid(), Name = "John Doe", Birthdate = new DateTime(1990, 1, 1) },
        new User { Id = Guid.NewGuid(), Name = "Jane Doe", Birthdate = new DateTime(1995, 1, 1) },
        new User { Id = Guid.NewGuid(), Name = "Bob Smith", Birthdate = new DateTime(1985, 1, 1) }
    };

    using (var context = new DataContext())
    {
        await context.Database.EnsureCreatedAsync();
        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
        
        var repository = new UserRepository(context, new EntityChangeEventHandler());

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(users.Count, result.Count());
        foreach (var user in users)
        {
            Assert.Contains(result, u => u.Id == user.Id && u.Name == user.Name && u.Birthdate == user.Birthdate);
        }

        await context.Database.EnsureDeletedAsync();
    }
}

[Fact]
public async Task AddAsync_AddsUserToDatabase()
{
    // Arrange
    var user = new User { Id = Guid.NewGuid(), Name = "John Doe", Birthdate = new DateTime(1990, 1, 1) };

    using (var context = new DataContext())
    {
        await context.Database.EnsureCreatedAsync();
        var repository = new UserRepository(context, new EntityChangeEventHandler());

        // Act
        await repository.AddAsync(user);
        
        // Assert
        var result = await context.Users.FindAsync(user.Id);
        Assert.NotNull(result);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Birthdate, result.Birthdate);
        await context.Database.EnsureDeletedAsync();
    }
}

[Fact]
public async Task UpdateAsync_UpdatesUserInDatabase()
{
    // Arrange
    var user = new User { Id = Guid.NewGuid(), Name = "John Doe", Birthdate = new DateTime(1990, 1, 1) };

    using (var context = new DataContext())
    {
        await context.Database.EnsureCreatedAsync();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        
        var repository = new UserRepository(context, new EntityChangeEventHandler());

        // Act
        user.Name = "Jane Doe";
        await repository.UpdateAsync(user);
        
        // Assert
        var result = await context.Users.FindAsync(user.Id);
        Assert.NotNull(result);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Birthdate, result.Birthdate);
        await context.Database.EnsureDeletedAsync();
    }
}

[Fact]
public async Task DeleteAsync_DeletesUserFromDatabase()
{
    // Arrange
    var user = new User { Id = Guid.NewGuid(), Name = "John Doe", Birthdate = new DateTime(1990, 1, 1) };

    using (var context = new DataContext())
    {
        await context.Database.EnsureCreatedAsync();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        
        var repository = new UserRepository(context, new EntityChangeEventHandler());

        // Act
        await repository.DeleteAsync(user);

        // Assert
        var result = await context.Users.FindAsync(user.Id);
        Assert.Null(result);
        await context.Database.EnsureDeletedAsync();
    }
}
#endregion