<Query Kind="Statements" />

#load ".\SudokuModels"
#load ".\SudokuRulesInterfaces"
#load ".\SudokuGameBoard"



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
public class RuleFailureMessageAttribute : Attribute
{
	public string Message { get; }

	public RuleFailureMessageAttribute(string message)
	{
		Message = message;
	}
}

public static class RuleExtensions
{
	public static string GetRuleFailureMessage<T>(this IRule<T> rule)
	{
		var attribute = rule.GetType().GetCustomAttribute<RuleFailureMessageAttribute>();
		return attribute?.Message ?? string.Empty;
	}
}
