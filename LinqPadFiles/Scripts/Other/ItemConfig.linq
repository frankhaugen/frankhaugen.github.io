<Query Kind="Statements">
  <NuGetReference>MonoGame.Extended</NuGetReference>
  <NuGetReference>MonoGame.Framework.DesktopGL</NuGetReference>
  <Namespace>System.Globalization</Namespace>
</Query>

var className = "User";

var thing = $"public class {className}EntityConfiguration : IEntityTypeConfiguration<{className}>" +
"{"+
$"	public void Configure(EntityTypeBuilder<{className}> builder)"+
"	{" +
"		builder.HasKey(x => x.Id);"+
"		builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();" +
"	}"+
"}";

thing.Dump();