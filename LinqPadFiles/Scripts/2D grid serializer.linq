<Query Kind="Statements">
  <NuGetReference Prerelease="true">Frank.Collections</NuGetReference>
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>Frank.Collections.Internals</Namespace>
  <Namespace>Frank.Collections.Multidimensional</Namespace>
  <Namespace>Frank.Collections.Observables</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Frank.Collections.Serialization</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>










var array = new Array2D<Piece>(10, 10);

array[0, 0] = new Piece { Title = "Piece 1", Description = "Description 1", Value = 1 };
array[0, 5] = new Piece { Title = "Piece 2", Description = "Description 2", Value = 2 };
array[7, 2] = new Piece { Title = "Piece 3", Description = "Description 3", Value = 3 };
array[9, 9] = new Piece { Title = "Piece 4", Description = "Description 4", Value = 4 };

var result = Array2DSerializer.Serialize(array);
Console.WriteLine(result);




public record Piece
{
	public string Title { get; init; }
	public string Description { get; init; }
	public int Value { get; init; }
}