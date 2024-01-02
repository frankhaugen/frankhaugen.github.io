<Query Kind="Statements" />




public class BattleshipGame
{
	private Grid _playerGrid;
	private Grid _opponentGrid;

	public BattleshipGame(int gridSize)
	{
		_playerGrid = new Grid(gridSize);
		_opponentGrid = new Grid(gridSize);
	}

	public void Play()
	{
		Console.WriteLine("Welcome to Battleship!");

		// Place ships on the player's grid
		PlaceShipsOnGrid(_playerGrid);

		// Place ships on the opponent's grid (randomly for simplicity)
		RandomlyPlaceShipsOnGrid(_opponentGrid);

		bool gameOver = false;
		while (!gameOver)
		{
			Console.Clear();
			Console.WriteLine("Player's Grid:");
			_playerGrid.Display();

			Console.WriteLine("\nOpponent's Grid:");
			_opponentGrid.Display();

			Console.Write("\nEnter a target (e.g. A1): ");
			string target = Console.ReadLine().ToUpper();

			if (IsValidTarget(target))
			{
				int row = target[0] - 'A';
				int col = int.Parse(target.Substring(1)) - 1;

				if (_opponentGrid.IsShipPlaced(row, col))
				{
					Console.WriteLine("Hit!");
					_opponentGrid.PlaceShip(row, col, 'X');
				}
				else
				{
					Console.WriteLine("Miss!");
					_opponentGrid.PlaceShip(row, col, 'O');
				}

				if (IsGameOver(_opponentGrid))
				{
					Console.WriteLine("Congratulations! You won the game!");
					gameOver = true;
				}
			}
			else
			{
				Console.WriteLine("Invalid target. Please try again.");
			}

			Console.WriteLine("\nPress any key to continue...");
			Console.ReadKey();
		}
	}

	private void PlaceShipsOnGrid(Grid grid)
	{
		Console.WriteLine("Place your ships on the grid.");

		for (int i = 0; i < 5; i++)
		{
			Console.Write($"Enter the coordinates for ship {i + 1} (e.g. A1): ");
			string coordinates = Console.ReadLine().ToUpper();

			if (IsValidTarget(coordinates))
			{
				int row = coordinates[0] - 'A';
				int col = int.Parse(coordinates.Substring(1)) - 1;

				if (grid.IsShipPlaced(row, col))
				{
					Console.WriteLine("A ship is already placed on that cell. Please try again.");
					i--;
				}
				else
				{
					grid.PlaceShip(row, col, 'S');
				}
			}
			else
			{
				Console.WriteLine("Invalid coordinates. Please try again.");
				i--;
			}
		}
	}

	private void RandomlyPlaceShipsOnGrid(Grid grid)
	{
		Random random = new Random();

		for (int i = 0; i < 5; i++)
		{
			int row = random.Next(grid.Size);
			int col = random.Next(grid.Size);

			if (grid.IsShipPlaced(row, col))
			{
				i--;
			}
			else
			{
				grid.PlaceShip(row, col, 'S');
			}
		}
	}

	private bool IsValidTarget(string target)
	{
		if (target.Length < 2)
		{
			return false;
		}

		char rowChar = target[0];
		int col;

		if (!char.IsLetter(rowChar) || !int.TryParse(target.Substring(1), out col))
		{
			return false;
		}

		int row = rowChar - 'A';
		col--;

		return row >= 0 && row < _opponentGrid.Size && col >= 0 && col < _opponentGrid.Size;
	}

	private bool IsGameOver(Grid grid)
	{
		for (int row = 0; row < grid.Size; row++)
		{
			for (int col = 0; col < grid.Size; col++)
			{
				if (grid.IsShipPlaced(row, col))
				{
					return false;
				}
			}
		}

		return true;
	}
}
public class Grid
{
	private readonly char[,] _cells;

	public Grid(int size)
	{
		Size = size;
		_cells = new char[size, size];
		InitializeCells();
	}
	
	public int Size {get;}

	private void InitializeCells()
	{
		for (int row = 0; row < Size; row++)
		{
			for (int col = 0; col < Size; col++)
			{
				_cells[row, col] = ' ';
			}
		}
	}

	public void Display()
	{
		Console.WriteLine("  " + string.Join(" ", Enumerable.Range(1, Size).Select(i => (char)('A' + i - 1))));
		for (int row = 0; row < Size; row++)
		{
			Console.Write((char)('A' + row) + " ");
			for (int col = 0; col < Size; col++)
			{
				Console.Write(_cells[row, col] + " ");
			}
			Console.WriteLine();
		}
	}

	public void PlaceShip(int row, int col)
	{
		_cells[row, col] = 'S';
	}

	public bool IsShipPlaced(int row, int col)
	{
		return _cells[row, col] == 'S';
	}

	internal void PlaceShip(int row, int col, char v)
	{
		throw new NotImplementedException();
	}
}