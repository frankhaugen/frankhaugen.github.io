<Query Kind="Statements" />

#load ".\SudokuRulesInterfaces"
#load ".\SudokuGameBoard"
#load ".\SudokuModels"



[RuleFailureMessage("The value is already present in the same row.")]
public class NoDuplicatesInRowRule<T> : IRule<T>
{
	public bool Validate(int row, int col, T value, IGameBoard<T> gameBoard)
	{
		for (int i = 0; i < gameBoard.GetSize(); i++)
		{
			if (i != col && gameBoard.GetValue(new(row, i)).Equals(value))
			{
				return false;
			}
		}

		return true;
	}
}

public class NoDuplicatesInColumnRule<T> : IRule<T>
{
	public bool Validate(int row, int col, T value, IGameBoard<T> gameBoard)
	{
		for (int i = 0; i < gameBoard.GetSize(); i++)
		{
			if (i != row && gameBoard.GetValue(new(i, col)).Equals(value))
			{
				return false;
			}
		}

		return true;
	}
}



public class NoDuplicatesInSubRegionRule<T> : IRule<T>
{
	public bool Validate(int row, int col, T value, IGameBoard<T> gameBoard)
	{
		// Calculate the start and end rows and columns of the sub-region
		int startRow = row - row % (int)Math.Sqrt(gameBoard.GetSize());
		int endRow = startRow + (int)Math.Sqrt(gameBoard.GetSize()) - 1;
		int startCol = col - col % (int)Math.Sqrt(gameBoard.GetSize());
		int endCol = startCol + (int)Math.Sqrt(gameBoard.GetSize()) - 1;
		
		// Check if the value is already present in the sub-region
		for (int i = startRow; i <= endRow; i++)
		{
			for (int j = startCol; j <= endCol; j++)
			{
				if (gameBoard.GetValue(new(i, j)).Equals(value))
				{
					return false;
				}
			}
		}

		return true;
	}
}
