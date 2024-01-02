<Query Kind="Program">
  <NuGetReference>Microsoft.EntityFrameworkCore.Design</NuGetReference>
  <NuGetReference>Microsoft.EntityFrameworkCore.Sqlite</NuGetReference>
  <NuGetReference>Microsoft.EntityFrameworkCore.SqlServer</NuGetReference>
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
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.Internal</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>Microsoft.Extensions.Logging.Abstractions</Namespace>
  <Namespace>Microsoft.Extensions.Options</Namespace>
  <Namespace>Microsoft.Extensions.Primitives</Namespace>
  <Namespace>Microsoft.VisualStudio.TextTemplating</Namespace>
  <Namespace>Mono.TextTemplating</Namespace>
  <Namespace>Mono.TextTemplating.CodeCompilation</Namespace>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

void Main()
{
	var args = Enumerable.Empty<string>().ToArray();
	
	var contextFactory = new ApplicationDbContextFactory();
	using (var context = contextFactory.CreateDbContext(args))
	{
		var migrator = context.GetService<IMigrator>();
		//migrator.Migrate(new CreateInvoiceViewMigration());
	}

	using (var context = contextFactory.CreateDbContext(args))
	{
		var invoiceViewData = context.InvoiceView.ToListAsync();
		invoiceViewData.Wait();
		invoiceViewData.Result.Dump();
	}
}

public class Invoice
{
	public int Id { get; set; }
	public ICollection<AdditionalField> AdditionalFields { get; set; }
}

public class AdditionalField
{
	public int Id { get; set; }
	public string Key { get; set; }
	public string Value { get; set; }
	public int InvoiceId { get; set; }
	public Invoice Invoice { get; set; }
}

[Keyless]
public class InvoiceViewModel
{
	public int Id { get; set; }
	public string ISBN { get; set; }
	public string DueDate { get; set; }
}

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Invoice> Invoices { get; set; }
	public DbSet<AdditionalField> AdditionalFields { get; set; }
	public DbSet<InvoiceViewModel> InvoiceView { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new InvoiceViewConfiguration());
	}
}

public class InvoiceViewConfiguration : IEntityTypeConfiguration<InvoiceViewModel>
{
	public void Configure(EntityTypeBuilder<InvoiceViewModel> builder)
	{
		builder.ToView("InvoiceView");
		builder.Property(iv => iv.ISBN).HasColumnName("ISBN");
		builder.Property(iv => iv.DueDate).HasColumnName("DueDate");
	}
}

public class CreateInvoiceViewMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.Sql(@"
            CREATE VIEW InvoiceView AS
            SELECT
                i.Id,
                MAX(CASE WHEN af.Key = 'ISBN' THEN af.Value END) AS ISBN,
                MAX(CASE WHEN af.Key = 'DueDate' THEN af.Value END) AS DueDate
            FROM
                Invoice i
            JOIN
                AdditionalField af
            ON
                i.Id = af.InvoiceId
            GROUP BY
                i.Id;
        ");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.Sql(@"DROP VIEW InvoiceView;");
	}
}


public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionString = "Data Source=D:\\LINQPad\\ChinookDemoDb.sqlite";
//        var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=YourDatabaseName;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .UseSqlServer(connectionString);
            .UseSqlite(connectionString);

        var context = new ApplicationDbContext(optionsBuilder.Options);
		
		context.Database.EnsureCreated();
		
		return context;
    }
}
