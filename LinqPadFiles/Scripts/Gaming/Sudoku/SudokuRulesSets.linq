<Query Kind="Statements" />

#load ".\SudokuRulesSet"
#load ".\SudokuRules"
#load ".\SudokuGameBoard"

public class StandardSudokuRuleSet<T> : RuleSet<T>
{
	public StandardSudokuRuleSet(IGameBoard<T> board) : base(board)
	{
		AddRule(new NoDuplicatesInRowRule<T>());
		AddRule(new NoDuplicatesInColumnRule<T>());
		AddRule(new NoDuplicatesInSubRegionRule<T>());
	}
}