<Query Kind="Statements" />

#load ".\SudokuGameBoard"
#load ".\SudokuModels"


public interface IRulesSet<T>
{
	bool Validate(int row, int col, T value);
	string GetValidationFailureMessage(Position position, T value);
}
public interface IRule<T>
{
	bool Validate(int row, int col, T value, IGameBoard<T> gameBoard);
}
public interface IRulesEngine<T>
{
	ValidationResult Validate(Position position, T value);
}