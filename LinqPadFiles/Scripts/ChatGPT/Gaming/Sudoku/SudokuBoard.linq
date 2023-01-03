<Query Kind="Statements">
  <Namespace>System.Text.Json.Serialization</Namespace>
</Query>

#load "SudokuGameBoard"
#load "SudokuModels"


public interface ISudokuBoard
{
	string PrintGameBoard();
	int GetValue(int row, int col);
	void SetValue(int row, int col, int value);
	bool IsValidMove(int row, int col, int value);
}

public class SudokuBoard : ISudokuBoard
{
	// The board is represented as a 2D array of integers
	private readonly IGameBoard<int> board;

	// The size of the board (e.g. a standard Sudoku board is size 9)
	private int size;

	// The size of the sub-regions within the board (e.g. for a standard Sudoku board, the sub-region size is 3)
	private int subRegionSize;

	public SudokuBoard(SudokuBoardOptions options)
	{
		// Initialize the board and size variables
		this.board = new GameBoard<int>(new GameBoardOptions()
		{
			Size = options.Size
		});
		board.Setup(options.Board.ListOfListsToArray());
		this.size = options.Size;
		this.subRegionSize = options.SubRegionSize;
	}

	public string PrintGameBoard()
	{
		return board.PrintGameBoard(subRegionSize);
	}

	public int GetValue(int row, int col)
	{
		// Get the value at the specified position on the board
		return board.GetValue(new(row, col));
	}

	public void SetValue(int row, int col, int value)
	{
		// Set the value at the specified position on the board
		board.SetValue(new(row, col), value);
	}

	public bool IsValidMove(int row, int col, int value)
	{
		// Check if the value is already present in the same row or column
		for (int i = 0; i < size; i++)
		{
			if (board.GetValue(new(i, col)) == value || board.GetValue(new(row, i)) == value)
			{
				return false;
			}
		}

		// Check if the value is already present in the same sub-region
		int startRow = (row / subRegionSize) * subRegionSize;
		int startCol = (col / subRegionSize) * subRegionSize;
		for (int i = startRow; i < startRow + subRegionSize; i++)
		{
			for (int j = startCol; j < startCol + subRegionSize; j++)
			{
				if (board.GetValue(new(i, j)) == value)
				{
					return false;
				}
			}
		}

		// If the value is not present in the same row, column, or sub-region, the move is valid
		return true;
	}
}

public static class EnumerableExtensions
{
	public static int[,] ListOfListsToArray(this List<List<int>> listOfLists)
	{
		// Get the number of rows and columns in the array
		int rows = listOfLists.Count;
		int cols = listOfLists[0].Count;

		// Create a new 2D array with the specified number of rows and columns
		var array = new int[rows, cols];

		// Iterate over the rows and columns of the array
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				// Get the value at the current position in the array
				int value = listOfLists[i][j];

				// Set the value in the 2D array
				array[i, j] = value;
			}
		}

		return array;
	}

	public static List<List<int>> ArrayToListOfLists(this int[,] array)
	{
		// Get the number of rows and columns in the array
		int rows = array.GetLength(0);
		int cols = array.GetLength(1);

		// Create a new list of lists
		var listOfLists = new List<List<int>>();

		// Iterate over the rows and columns of the array
		for (int i = 0; i < rows; i++)
		{
			// Create a new inner list
			var innerList = new List<int>();

			for (int j = 0; j < cols; j++)
			{
				// Get the value at the current position in the array
				int value = array[i, j];

				// Add the value to the inner list
				innerList.Add(value);
			}

			// Add the inner list to the list of lists
			listOfLists.Add(innerList);
		}

		return listOfLists;
	}

}

