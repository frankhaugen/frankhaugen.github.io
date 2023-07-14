<Query Kind="Statements" />

#load ".\SudokuConstants"
#load ".\SudokuModels"



public interface IGameBoard<T>
{
	T GetValue(Position position);
	void SetValue(Position position, T value);
	T[,] GetBoard();
}

public class GameBoard<T> : IGameBoard<T>
{
	private readonly T[,] board;

	public GameBoard(GameBoardOptions options)
	{
		// Create a new 2D array with the size specified in the options object
		board = new T[options.Size, options.Size];
	}

	public T GetValue(Position position)
	{
		// Get the value at the specified position on the board
		return board[position.Row, position.Col];
	}

	public void SetValue(Position position, T value)
	{
		// Set the value at the specified position on the board
		board[position.Row, position.Col] = value;
	}

	public T[,] GetBoard()
	{
		// Return a copy of the board array
		return (T[,])board.Clone();
	}
}

public static class GameBoardExtensions
{
	public static int GetSize<T>(this IGameBoard<T> board)
	{
		// Get the board array from the IGameBoard<T> object
		T[,] boardArray = board.GetBoard();

		// Return the length of the first dimension of the board array
		return boardArray.GetLength(0);
	}

	/// <summary>
	/// Prints the game board of an <see cref="IGameBoard{T}"/> to the console.
	/// </summary>
	/// <param name="board">The game board to be printed.</param>
	/// <param name="subRegionSize">The size of the sub-regions within the board. Default value is 0 (no sub-regions).</param>
	/// <param name="verticalLine">The character to be used as the vertical line. Default value is '|'.</param>
	/// <param name="horizontalLine">The character to be used as the horizontal line. Default value is '-'.</param>
	public static string PrintGameBoard<T>(this IGameBoard<T> board, int subRegionSize = 0, char horizontalLine = '-', char verticalLine = '|')
	{
		StringBuilder sb = new StringBuilder();

		for (int i = 0; i < board.GetSize(); i++)
		{
			sb.Append(horizontalLine).Append(horizontalLine).Append(horizontalLine);
		}
		sb.AppendLine();

		for (int i = 0; i < board.GetSize(); i++)
		{
			sb.Append(verticalLine);

			for (int j = 0; j < board.GetSize(); j++)
			{
				T value = board.GetValue(new Position() { Row = i, Col = j });
				sb.AppendFormat(" {0} ", value == null ? " " : value.ToString());
				if (subRegionSize > 0 && (j + 1) % subRegionSize == 0)
				{
					sb.Append(verticalLine);
				}
			}
			sb.AppendLine();

			if (subRegionSize > 0 && (i + 1) % subRegionSize == 0)
			{
				for (int j = 0; j < board.GetSize(); j++)
				{
					sb.Append(horizontalLine).Append(horizontalLine).Append(horizontalLine);
				}
				sb.AppendLine();
			}
		}
		return sb.ToString();
	}

	public static void Setup<T>(this IGameBoard<T> board, T[,] values)
	{
		// Ensure that the values array has the same size as the board
		if (values.GetLength(0) != board.GetBoard().GetLength(0) ||
			values.GetLength(1) != board.GetBoard().GetLength(1))
		{
			throw new ArgumentException("The values array has a different size than the board.");
		}

		// Iterate over the rows and columns of the values array
		for (int row = 0; row < values.GetLength(0); row++)
		{
			for (int col = 0; col < values.GetLength(1); col++)
			{
				// Set the value on the board
				board.SetValue(new Position { Row = row, Col = col }, values[row, col]);
			}
		}
	}
}
