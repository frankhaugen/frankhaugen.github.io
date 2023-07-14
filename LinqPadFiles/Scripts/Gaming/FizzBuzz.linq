<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

var rules = new FizzBuzzRules();

rules.RunRules(new UintRange(30, 1)).Dump();
rules.RunRules(new UintRange(1, 30)).Dump();

public static class SomeExtensions
{
    public static IEnumerable<string> RunRules(this IEnumerable<int> source, List<Rule> rules) => source.Select(x => rules.RunRules((uint)x));
    public static string RunRules(this List<Rule> rules, uint value)
    {
        var results = rules.Select(x => x.RunRule(value)).Where(x => !string.IsNullOrWhiteSpace(x));
        if (!results.Any()) return value.ToString();
        return string.Join("", results);
    }
    public static string RunRule(this Rule rule, uint value) => value % rule.Divisor == 0 ? rule.Text : string.Empty;
    public static bool IsAschending(this UintRange range) => range.Start <= range.End;
    public static IEnumerable<int> AsEnumerable(this UintRange range) => range.IsAschending() ? Enumerable.Range((int)range.Start, (int)range.End) : Enumerable.Range((int)range.End, (int)range.Start).Reverse();
    public static IEnumerable<T> ReverseIf<T>(this IEnumerable<T> source, bool value) => value ? source.Reverse() : source;
}

public readonly record struct UintRange(uint Start, uint End);
public readonly record struct Rule(uint Divisor, string Text);

public interface IRules
{
    IRules AddRule(Rule rule);
    string RunRules(uint value);
    List<string> RunRules(UintRange range);
}

public class FizzBuzzRules : RulesBase
{
    public FizzBuzzRules()
    {
        base.AddRule(new Rule(3, "Fizz"));
        base.AddRule(new Rule(5, "Buzz"));
    }
}

public abstract class RulesBase : IRules
{
    private readonly List<Rule> _rules = new();

    public IRules AddRule(Rule rule)
    {
        _rules.Add(rule);
        return this;
    }

    public string RunRules(uint value) => _rules.RunRules(value);
    public List<string> RunRules(UintRange range) => range.AsEnumerable().RunRules(_rules).ToList();
}