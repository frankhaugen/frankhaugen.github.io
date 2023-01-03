<Query Kind="Statements">
  <Namespace>System.Text.Json.Serialization</Namespace>
</Query>





public struct ValidationResult
{
	public bool IsValid { get; set; }
	public string ValidationFailureMessage { get; set; }
}

public class SudokuBoardOptions
{
	[JsonPropertyName("Size")]
	public int Size { get; set; }

	[JsonPropertyName("SubRegionSize")]
	public int SubRegionSize { get; set; }

	[JsonPropertyName("Board")]
	public List<List<int>> Board { get; set; }
}

public class GameBoardOptions
{
	public int Size { get; set; }
}

public struct Position
{
	public int Row { get; set; }
	public int Col { get; set; }

	public Position(int row, int col)
	{
		Row = row;
		Col = col;
	}
}