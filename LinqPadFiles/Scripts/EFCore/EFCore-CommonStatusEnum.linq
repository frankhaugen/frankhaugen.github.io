<Query Kind="Statements">
  <NuGetReference>Microsoft.EntityFrameworkCore.Design</NuGetReference>
  <NuGetReference>Microsoft.EntityFrameworkCore.Sqlite</NuGetReference>
  <Namespace>Humanizer</Namespace>
  <Namespace>Humanizer.Bytes</Namespace>
  <Namespace>Humanizer.Configuration</Namespace>
  <Namespace>Humanizer.DateTimeHumanizeStrategy</Namespace>
  <Namespace>Humanizer.Inflections</Namespace>
  <Namespace>Humanizer.Localisation</Namespace>
  <Namespace>Humanizer.Localisation.CollectionFormatters</Namespace>
  <Namespace>Humanizer.Localisation.DateToOrdinalWords</Namespace>
  <Namespace>Humanizer.Localisation.Formatters</Namespace>
  <Namespace>Humanizer.Localisation.NumberToWords</Namespace>
  <Namespace>Humanizer.Localisation.Ordinalizers</Namespace>
  <Namespace>Humanizer.Localisation.TimeToClockNotation</Namespace>
  <Namespace>Microsoft.Data.Sqlite</Namespace>
  <Namespace>Microsoft.DotNet.PlatformAbstractions</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.ChangeTracking</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.ChangeTracking.Internal</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Design</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Design.Internal</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Diagnostics</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Diagnostics.Internal</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Infrastructure</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Infrastructure.Internal</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Internal</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Metadata</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Metadata.Builders</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Metadata.Conventions</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Metadata.Internal</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Migrations</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Migrations.Design</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Migrations.Internal</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Migrations.Operations</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Migrations.Operations.Builders</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Query</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Query.Internal</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Query.SqlExpressions</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Scaffolding</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Scaffolding.Internal</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Scaffolding.Metadata</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Scaffolding.Metadata.Internal</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Storage</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Storage.Internal</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Storage.ValueConversion</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Update</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Update.Internal</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.ValueGeneration</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.ValueGeneration.Internal</Namespace>
  <Namespace>Microsoft.Extensions.Caching.Distributed</Namespace>
  <Namespace>Microsoft.Extensions.Caching.Memory</Namespace>
  <Namespace>Microsoft.Extensions.Configuration</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection.Extensions</Namespace>
  <Namespace>Microsoft.Extensions.DependencyModel</Namespace>
  <Namespace>Microsoft.Extensions.DependencyModel.Resolution</Namespace>
  <Namespace>Microsoft.Extensions.Internal</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>Microsoft.Extensions.Logging.Abstractions</Namespace>
  <Namespace>Microsoft.Extensions.Options</Namespace>
  <Namespace>Microsoft.Extensions.Primitives</Namespace>
  <Namespace>Microsoft.VisualStudio.TextTemplating</Namespace>
  <Namespace>Mono.TextTemplating</Namespace>
  <Namespace>Mono.TextTemplating.CodeCompilation</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
</Query>



var builder = Host.CreateDefaultBuilder();

builder.ConfigureServices((hostContext, services) =>
{
	services.AddDbContext<BaseSemineDbContext>(options =>
		options.UseSqlite(hostContext.Configuration.GetConnectionString("InMemory")));
});

public class MyDataContext : BaseSemineDbContext
{
	public MyDataContext(DbContextOptions<BaseSemineDbContext> options) : base(options)
	{
	}

	public DbSet<MyEntity> MyEntities { get; set; }
}

public class MyEntity : IStatusFilterable
{
	public int Id { get; set; }
	public string Name { get; set; }
	public EntityStatus Status { get; set; }
}

public interface IStatusFilterable
{
	EntityStatus Status { get; set; }
}

public abstract class BaseSemineDbContext : DbContext
{
	// CTOR
	public BaseSemineDbContext(IDbContextOptions<BaseSemineDbContext> options) : base(options)
	{
	}
	
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseSqlite("Data Source=database.db");
		}
	}
	
	public BaseSemineDbContext(DbContextOptions options) : base(options)
	{
		
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IStatusFilterable>()
            .HasQueryFilter(e => e.Status != EntityStatus.Deleted);
    }
}

public enum EntityStatus
{
    Active,
    Inactive,
    Deleted
}