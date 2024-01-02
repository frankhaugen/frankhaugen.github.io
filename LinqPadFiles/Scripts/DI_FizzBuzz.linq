<Query Kind="Statements">
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
</Query>


var builder = Host.CreateApplicationBuilder();

builder.Services.AddSingleton<IGame, Game>();
builder.Services.AddTransient<IRule, TriMultiplicativeRule>();
builder.Services.AddTransient<IRule, PentaMultiplicativeRule>();

var app = builder.Build();

var game = app.Services.GetRequiredService<IGame>();

game.Play(new Range(Index.FromStart(1), Index.FromStart(100)));

public class TriMultiplicativeRule : RuleBase
{
	public override bool Run(int input)
	{
		return input % 3 == 0;
	}

	public override string GetValue()
	{
		return "Fizz";
	}
}

public class PentaMultiplicativeRule : RuleBase
{
	public override bool Run(int input)
	{
		return input % 5 == 0;
	}

	public override string GetValue()
	{
		return "Buzz";
	}
}

public class Game : IGame
{
	private readonly IEnumerable<IRule> _rules;

	public Game(IEnumerable<IRule> rules)
	{
		_rules = rules;
	}
	
	public void Play(Range range)
	{
		for (int i = range.Start.Value; i <= range.End.Value; i++)
		{
			var output = string.Empty;
			foreach (var rule in _rules)
			{
				output += rule.Run(i) ? rule.GetValue() : string.Empty;
			}

			if (output == string.Empty)
				output = i.ToString();

			Console.WriteLine(output);
		}
		
	}
}

public abstract class RuleBase : IRule
{
	public virtual string GetValue()
	{
		return string.Empty;
	}

	public virtual bool Run(int input)
	{
		return false;
	}
}

public interface IRule
{
	bool Run(int input);

	string GetValue();
}

public interface IGame
{
	void Play(Range range);
}