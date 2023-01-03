<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Hosting</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
</Query>

#load ".\SudokuModels"
#load ".\SudokuRuleEngine"


#load ".\SudokuBoard"
#load ".\SudokuGameBoard"


class Program
{
	static void Main(string[] args)
	{
		// Create a new service collection and register the dependencies
		var sudokuGame = new ServiceCollection()
			.AddSudokuDependencies()
			.BuildServiceProvider()
			.GetRequiredService<ISudokuGame>();

		// Run the Sudoku game
		sudokuGame.Run();
	}
}

static class ServiceCollectionExtensions
{
	public static IServiceCollection AddSudokuDependencies(this IServiceCollection services)
	{
		var config = @"{
  ""Size"": 9,
  ""SubRegionSize"": 3,
  ""Board"": [
    [5, 3, 0, 0, 7, 0, 0, 0, 0],
    [6, 0, 0, 1, 9, 5, 0, 0, 0],
    [0, 9, 8, 0, 0, 0, 0, 6, 0],
    [8, 0, 0, 0, 6, 0, 0, 0, 3],
    [4, 0, 0, 8, 0, 3, 0, 0, 1],
    [7, 0, 0, 0, 2, 0, 0, 0, 6],
    [0, 6, 0, 0, 0, 0, 2, 8, 0],
    [0, 0, 0, 4, 1, 9, 0, 0, 5],
    [0, 0, 0, 0, 8, 0, 0, 7, 9]
  ]
}";

		services.AddSingleton<ISudokuBoard, SudokuBoard>();
		services.AddSingleton<SudokuBoardOptions>(JsonSerializer.Deserialize<SudokuBoardOptions>(config));
		services.AddSingleton<ISudokuGame, SudokuGame>();
		services.AddSingleton<ISudokuGame, SudokuGame>();
		
		return services;
	}
}


public interface ISudokuGame
{
	void Run();
}

public class SudokuGame : ISudokuGame
{
	private readonly ISudokuBoard sudokuBoard;

	public SudokuGame(ISudokuBoard sudokuBoard)
	{
		this.sudokuBoard = sudokuBoard;
	}

	public void Run()
	{
		// Print the game board
		sudokuBoard.PrintGameBoard().Dump();
	}

}


public static class ArrayExtensions
{
	public static IEnumerable<TResult> Select<TSource, TResult>(this TSource[,] array, Func<TSource, int, int, TResult> selector)
	{
		for (int i = 0; i < array.GetLength(0); i++)
		{
			for (int j = 0; j < array.GetLength(1); j++)
			{
				yield return selector(array[i, j], i, j);
			}
		}
	}
}

