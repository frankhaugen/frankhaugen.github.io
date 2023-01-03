<Query Kind="Statements">
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
</Query>










var json = """
{
"Board": [
[
{"type": "Pawn", "color": "White"},
{"type": "Knight", "color": "White"},
{"type": "Bishop", "color": "White"},
{"type": "Rook", "color": "White"},
{"type": "Queen", "color": "White"},
{"type": "King", "color": "White"},
{"type": "Bishop", "color": "White"},
{"type": "Knight", "color": "White"},
{"type": "Pawn", "color": "White"}
],
[
{"type": "Pawn", "color": "White"},
{"type": "Pawn", "color": "White"},
{"type": "Pawn", "color": "White"},
{"type": "Pawn", "color": "White"},
{"type": "Pawn", "color": "White"},
{"type": "Pawn", "color": "White"},
{"type": "Pawn", "color": "White"},
{"type": "Pawn", "color": "White"},
{"type": "Pawn", "color": "White"}
],
[null, null, null, null, null, null, null, null, null],
[null, null, null, null, null, null, null, null, null],
[null, null, null, null, null, null, null, null, null],
[null, null, null, null, null, null, null, null, null],
[
{"type": "Pawn", "color": "Black"},
{"type": "Pawn", "color": "Black"},
{"type": "Pawn", "color": "Black"},
{"type": "Pawn", "color": "Black"},
{"type": "Pawn", "color": "Black"},
{"type": "Pawn", "color": "Black"},
{"type": "Pawn", "color": "Black"},
{"type": "Pawn", "color": "Black"},
{"type": "Pawn", "color": "Black"}
],
[
{"type": "Pawn", "color": "Black"},
{"type": "Knight", "color": "Black"},
{"type": "Bishop", "color": "Black"},
{"type": "Rook", "color": "Black"},
{"type": "Queen", "color": "Black"},
{"type": "King", "color": "Black"},
{"type": "Bishop", "color": "Black"},
{"type": "Knight", "color": "Black"},
{"type": "Pawn", "color": "Black"}
]
]
}
""";

JsonHelper.ReadBoardFromJson<ChessPiece>(json).Dump(); ;


public static class JsonHelper
{
	public static void SaveBoardToJson<T>(GameBoard<T> board, string fileName)
	{
		var json = JsonSerializer.Serialize(board);
		File.WriteAllText(fileName, json);
	}

	public static IGameBoard<T> ReadBoardFromJson<T>(string json)
	{
		var options = new JsonSerializerOptions();
		options.IncludeFields = true;
		options.Converters.Add(new Generic2DArrayJsonConverter<T>());
		return JsonSerializer.Deserialize<GameBoard<T>>(json, options);
	}
	public static IGameBoard<T> ReadBoardFromJson<T>(FileInfo file)
	{
		return JsonSerializer.Deserialize<IGameBoard<T>>(File.ReadAllText(file.FullName));
	}
}

public interface IRule<T>
{
	bool Validate(int row, int col, T value, IGameBoard<T> gameBoard);
	string GetRuleFailureMessage();
}
public interface IGameBoard<T>
{
	T GetValue(Position position);
	void SetValue(Position position, T value);
	T[,] GetBoard();
}
public interface IRulesSet<T>
{
	bool Validate(int row, int col, T value);
	string GetValidationFailureMessage(Position position, T value);
}
public class GameBoardConverter<T> : JsonConverter<GameBoard<T>>
{
	public override GameBoard<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (typeToConvert == typeof(T[,]))
		{
			reader.GetString();
		}

		// Deserialize the json into a 2D array of type T


		//var boardArray = JsonSerializer.Deserialize<T[,]>(ref reader, options);
		// Create a new GameBoard object with the size of the 2D array
		//var gameBoard = new GameBoard<T>(new GameBoardOptions() {
		//Size = boardArray.GetLength(0)
		//});

		// Set the values of the board array in the GameBoard object
		//for (int row = 0; row < boardArray.GetLength(0); row++)
		//{
		//	for (int col = 0; col < boardArray.GetLength(1); col++)
		//	{
		//		gameBoard.SetValue(new Position(row, col), boardArray[row, col]);
		//	}
		//}

		//return gameBoard;
		return null;
	}

	public override void Write(Utf8JsonWriter writer, GameBoard<T> value, JsonSerializerOptions options)
	{
		// Serialize the board array in the GameBoard object
		JsonSerializer.Serialize(writer, value.Board, options);
	}
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

public enum ChessPieceType
{
	Pawn,
	Knight,
	Bishop,
	Rook,
	Queen,
	King
}

public class ChessPiece
{
	public ChessPieceType Type { get; set; }
	public ChessColor Color { get; set; }
}

public enum ChessColor
{
	White,
	Black
}

public static class ChessColorExtensions
{
	public static int GetForwardDirection(this ChessColor color)
	{
		return color == ChessColor.White ? -1 : 1;
	}
}

public abstract class RuleSet<T> : IRulesSet<T>
{
	protected readonly IGameBoard<T> _board;
	protected readonly List<IRule<T>> _rules = new();

	protected RuleSet(IGameBoard<T> board)
	{
		_board = board;
	}

	public virtual bool Validate(int row, int col, T value)
	{
		// Validate the value against all rules in the list
		foreach (var rule in _rules)
		{
			if (!rule.Validate(row, col, value, _board))
			{
				return false;
			}
		}

		return true;
	}

	public virtual string GetValidationFailureMessage(Position position, T value)
	{
		foreach (var rule in _rules)
		{
			if (!rule.Validate(position.Row, position.Col, value, _board))
			{
				return rule.GetRuleFailureMessage();
			}
		}

		return string.Empty;
	}

	protected void AddRule(IRule<T> rule)
	{
		// Add the rule to the list, but only if it doesn't already exist in the list
		if (!_rules.Contains(rule))
		{
			_rules.Add(rule);
		}
	}
}


public class KingRuleSet : RuleSet<ChessPiece>
{
	public KingRuleSet(IGameBoard<ChessPiece> board) : base(board)
	{
		// Add the rule for King movement to the list
		AddRule(new KingMovementRule());
	}
}

public class KingMovementRule : IRule<ChessPiece>
{
	public bool Validate(int row, int col, ChessPiece value, IGameBoard<ChessPiece> gameBoard)
	{
		// The king can only move one space at a time
		if (Math.Abs(row) > 1 || Math.Abs(col) > 1)
		{
			return false;
		}
		return true;
	}

	public string GetRuleFailureMessage()
	{
		return "The king can only move one space at a time.";
	}
}

public class KnightRuleSet : RuleSet<ChessPiece>
{
	public KnightRuleSet(IGameBoard<ChessPiece> board) : base(board)
	{
		// Add the rule for Knight movement to the list
		AddRule(new KnightMovementRule());
	}
}

public class KnightMovementRule : IRule<ChessPiece>
{
	public bool Validate(int row, int col, ChessPiece value, IGameBoard<ChessPiece> gameBoard)
	{
		// The knight can move two spaces in one direction and one space in the other direction
		if (Math.Abs(row) == 2 && Math.Abs(col) == 1 || Math.Abs(row) == 1 && Math.Abs(col) == 2)
		{
			return true;
		}
		return false;
	}

	public string GetRuleFailureMessage()
	{
		return "The knight can only move two spaces in one direction and one space in the other direction.";
	}
}

public class RookRuleSet : RuleSet<ChessPiece>
{
	public RookRuleSet(IGameBoard<ChessPiece> board) : base(board)
	{
		// Add the rules for Rook movement to the list
		AddRule(new RookMovementRule());
		AddRule(new RookPathClearRule());
	}
}

public class RookMovementRule : IRule<ChessPiece>
{
	public bool Validate(int row, int col, ChessPiece value, IGameBoard<ChessPiece> gameBoard)
	{
		// The rook can only move horizontally or vertically
		if (row == 0 || col == 0)
		{
			return true;
		}
		return false;
	}

	public string GetRuleFailureMessage()
	{
		return "The rook can only move horizontally or vertically.";
	}
}
public class RookPathClearRule : IRule<ChessPiece>
{
	public bool Validate(int row, int col, ChessPiece value, IGameBoard<ChessPiece> gameBoard)
	{
		// Check if the path is clear for the rook to move to the new position
		if (row == 0)
		{
			int startCol = Math.Min(col, 4);
			int endCol = Math.Max(col, 4);
			for (int i = startCol + 1; i < endCol; i++)
			{
				if (gameBoard.GetValue(new Position(4, i)) != null)
				{
					return false;
				}
			}
		}
		else if (col == 0)
		{
			int startRow = Math.Min(row, 4);
			int endRow = Math.Max(row, 4);
			for (int i = startRow + 1; i < endRow; i++)
			{
				if (gameBoard.GetValue(new Position(i, 4)) != null)
				{
					return false;
				}
			}
		}
		return true;
	}

	public string GetRuleFailureMessage()
	{
		return "The path must be clear for the rook to move to the new position.";
	}
}

public class QueenRuleSet : RuleSet<ChessPiece>
{
	public QueenRuleSet(IGameBoard<ChessPiece> board) : base(board)
	{
		// Add the rules for Queen movement to the list
		AddRule(new QueenMovementRule());
		AddRule(new QueenPathClearRule());
	}
}

public class QueenMovementRule : IRule<ChessPiece>
{
	public bool Validate(int row, int col, ChessPiece value, IGameBoard<ChessPiece> gameBoard)
	{
		// The queen can move diagonally, horizontally, or vertically
		if (row == col || row == 0 || col == 0)
		{
			return true;
		}
		return false;
	}

	public string GetRuleFailureMessage()
	{
		return "The queen can only move diagonally, horizontally, or vertically.";
	}
}
public class QueenPathClearRule : IRule<ChessPiece>
{
	public bool Validate(int row, int col, ChessPiece value, IGameBoard<ChessPiece> gameBoard)
	{
		// Check if the path is clear for the queen to move to the new position
		if (row == col)
		{
			int startRow = Math.Min(row, 4);
			int endRow = Math.Max(row, 4);
			for (int i = startRow + 1; i < endRow; i++)
			{
				if (gameBoard.GetValue(new Position(i, i)) != null)
				{
					return false;
				}
			}
		}
		else if (row == 0)
		{
			int startCol = Math.Min(col, 4);
			int endCol = Math.Max(col, 4);
			for (int i = startCol + 1; i < endCol; i++)
			{
				if (gameBoard.GetValue(new Position(4, i)) != null)
				{
					return false;
				}
			}
		}
		else if (col == 0)
		{
			int startRow = Math.Min(row, 4);
			int endRow = Math.Max(row, 4);
			for (int i = startRow + 1; i < endRow; i++)
			{
				if (gameBoard.GetValue(new Position(i, 4)) != null)
				{
					return false;
				}
			}
		}
		return true;
	}

	public string GetRuleFailureMessage()
	{
		return "The path must be clear for the queen";
	}
}
public class PawnRuleSet : RuleSet<ChessPiece>
{
	public PawnRuleSet(IGameBoard<ChessPiece> board) : base(board)
	{
		// Add the rules for pawn movement to the list
		AddRule(new PawnMovementRule());
		AddRule(new PawnStartingPositionRule());
	}
}

public class PawnMovementRule : IRule<ChessPiece>
{
	public bool Validate(int row, int col, ChessPiece value, IGameBoard<ChessPiece> gameBoard)
	{
		// The pawn can only move one space forward or capture an enemy piece diagonally
		if (row == 1 && col == 0)
		{
			return true;
		}
		else if (row == 1 && Math.Abs(col) == 1)
		{
			// Check if there is an enemy piece at the diagonal position
			var enemyColor = value.Color == ChessColor.White ? ChessColor.Black : ChessColor.White;
			var enemyPiece = gameBoard.GetValue(new Position(row + value.Color.GetForwardDirection(), col + value.Color.GetForwardDirection()));
			if (enemyPiece != null && enemyPiece.Color == enemyColor)
			{
				return true;
			}
		}
		return false;
	}

	public string GetRuleFailureMessage()
	{
		return "The pawn can only move one space forward or capture an enemy piece diagonally.";
	}
}

public class PawnStartingPositionRule : IRule<ChessPiece>
{
	public bool Validate(int row, int col, ChessPiece value, IGameBoard<ChessPiece> gameBoard)
	{
		// The pawn can only move two spaces forward from its starting position
		if (value.Color == ChessColor.White && row == 3 && col == 0)
		{
			return true;
		}
		else if (value.Color == ChessColor.Black && row == 4 && col == 0)
		{
			return true;
		}
		return false;
	}

	public string GetRuleFailureMessage()
	{
		return "The pawn can only move two spaces forward from its starting position.";
	}
}





public class GameBoardOptions
{
	public int Size { get; set; }
}


public class Generic2DArrayJsonConverter<T> : JsonConverter<T[,]>
{
	public override T[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.StartArray)
		{
			throw new JsonException();
		}
		var array = new List<List<T>>();
		while (reader.Read())
		{
			if (reader.TokenType == JsonTokenType.EndArray)
			{
				break;
			}

			if (reader.TokenType != JsonTokenType.StartArray)
			{
				throw new JsonException();
			}

			var innerList = new List<T>();
			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndArray)
				{
					break;
				}

				innerList.Add(JsonSerializer.Deserialize<T>(ref reader, options));
			}

			array.Add(innerList);
		}

		return ConvertListToArray(array);
	}

	public override void Write(Utf8JsonWriter writer, T[,] value, JsonSerializerOptions options)
	{
		writer.WriteStartArray();
		for (int i = 0; i < value.GetLength(0); i++)
		{
			writer.WriteStartArray();
			for (int j = 0; j < value.GetLength(1); j++)
			{
				JsonSerializer.Serialize(writer, value[i, j], options);
			}
			writer.WriteEndArray();
		}
		writer.WriteEndArray();
	}

	private T[,] ConvertListToArray(List<List<T>> list)
	{
		int rows = list.Count;
		int cols = list[0].Count;

		var array = new T[rows, cols];
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				array[i, j] = list[i][j];
			}
		}

		return array;
	}
}


public class GameBoard<T> : IGameBoard<T>
{
	public T[,] Board { get; init; }

	public GameBoard(GameBoardOptions options)
	{
		Board = new T[options.Size, options.Size];
	}

	public T GetValue(Position position)
	{
		return Board[position.Row, position.Col];
	}

	public void SetValue(Position position, T value)
	{
		Board[position.Row, position.Col] = value;
	}

	public T[,] GetBoard()
	{
		return (T[,])Board.Clone();
	}
}
