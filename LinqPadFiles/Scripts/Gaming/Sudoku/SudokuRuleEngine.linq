<Query Kind="Statements" />

#load "SudokuModels"
#load "SudokuRulesInterfaces"


public class RulesEngine<T>
{
	private readonly IRulesSet<T> _rulesSet;

	public RulesEngine(IRulesSet<T> rulesSet)
	{
		_rulesSet = rulesSet;
	}

	public ValidationResult Validate(Position position, T value)
	{
		var isValid = _rulesSet.Validate(position.Row, position.Col, value);
		if (isValid)
		{
			return new ValidationResult { IsValid = true };
		}

		return new ValidationResult
		{
			IsValid = false,
			ValidationFailureMessage = _rulesSet.GetValidationFailureMessage(position, value)
		};
	}
}
