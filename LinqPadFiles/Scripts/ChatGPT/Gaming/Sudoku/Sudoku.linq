<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Hosting</NuGetReference>
</Query>


static void Main(string[] args)
{
	// Create a new Sudoku board
	var board = new SudokuBoard(9, 3);

	// Fill a certain number of cells randomly
	Random random = new Random();
	for (int i = 0; i < 50; i++)
	{
		int row = random.Next(9);
		int col = random.Next(9);
		int value = random.Next(1, 10);
		if (board.IsValidMove(row, col, value))
		{
			board.SetValue(row, col, value);
		}
	}

	// Remove a certain number of cells to create the puzzle
	List<int> cells = Enumerable.Range(0, 81).ToList();
	cells = cells.OrderBy(x => random.Next()).ToList();
	for (int i = 0; i < 30; i++)
	{
		int cell = cells[i];
		int row = cell / 9;
		int col = cell % 9;
		board.SetValue(row, col, 0);
	}

	board.PrintGameBoard();
}

class SudokuBoard
{
	// The board is represented as a 2D array of integers
	private int[,] board;

	// The size of the board (e.g. a standard Sudoku board is size 9)
	private int size;

	// The size of the sub-regions within the board (e.g. for a standard Sudoku board, the sub-region size is 3)
	private int subRegionSize;

	public SudokuBoard(int size, int subRegionSize)
	{
		// Initialize the board and size variables
		this.board = new int[size, size];
		this.size = size;
		this.subRegionSize = subRegionSize;
	}


	public void PrintGameBoard()
	{
		var gameBoard = this.IntArrayToCharArray();
		// Print the top row of underscores
		for (int i = 0; i < gameBoard.GetLength(0); i++)
		{
			Console.Write("---");
		}
		Console.WriteLine();

		// Print the rows of the game board
		for (int i = 0; i < gameBoard.GetLength(0); i++)
		{
			// Print the leftmost column of pipes
			Console.Write("|");

			// Print the cells of the row
			for (int j = 0; j < gameBoard.GetLength(1); j++)
			{
				Console.Write($" {gameBoard[i, j]} ");

				// Print a vertical pipe after every sub-region (e.g. every 3 cells in a standard Tic-Tac-Toe board)
				if ((j + 1) % subRegionSize == 0)
				{
					Console.Write("|");
				}
			}
			Console.WriteLine();

			// Print a row of underscores after every row of cells
			if ((i + 1) % subRegionSize == 0)
			{
				for (int j = 0; j < gameBoard.GetLength(0); j++)
				{
					Console.Write("---");
				}
				Console.WriteLine();
			}
		}
	}


	public int GetValue(int row, int col)
	{
		// Get the value at the specified position on the board
		return board[row, col];
	}

	public void SetValue(int row, int col, int value)
	{
		// Set the value at the specified position on the board
		board[row, col] = value;
	}

	public bool IsValidMove(int row, int col, int value)
	{
		// Check if the value is already in the same row
		for (int i = 0; i < size; i++)
		{
			if (board[i, col] == value)
			{
				return false;
			}
		}

		// Check if the value is already in the same sub-region
		int subRegionRowStart = (row / subRegionSize) * subRegionSize;
		int subRegionColStart = (col / subRegionSize) * subRegionSize;
		for (int i = subRegionRowStart; i < subRegionRowStart + subRegionSize; i++)
		{
			for (int j = subRegionColStart; j < subRegionColStart + subRegionSize; j++)
			{
				if (board[i, j] == value)
				{
					return false;
				}
			}
		}

		// If the value is not present in the same row, column, or sub-region, the move is valid
		return true;
	}
	char[,] IntArrayToCharArray()
	{
		// Create a new 2D array of characters with the same dimensions as the input array
		var charArray = new char[board.GetLength(0), board.GetLength(1)];

		// Iterate over the cells of the input array
		for (int i = 0; i < board.GetLength(0); i++)
		{
			for (int j = 0; j < board.GetLength(1); j++)
			{
				// Convert the integer to a character and assign it to the corresponding cell in the output array
				charArray[i, j] = (char)(board[i, j] + '0');
			}
		}

		return charArray;
	}
}
