<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>


    var testCases = new List<(string expression, double expectedAverage)>()
    {
        ("2d5 + 1d20 + 4 - 2", 10.5),
        ("1d6 + 1d6 + 1d6", 10.5),
        ("1d20 + 5", 15.5),
        ("2d10 - 1d4", 13.5),
        ("1d100", 50.5),
        ("2d6 + 1d8 - 3", 7.5),
        ("1d20 + 1d20 + 1d20 + 1d20 + 1d20", 52.5),
        ("1d4 + 1d6 + 1d8 + 1d10 + 1d12 + 1d20", 32.5),
        ("1d100 - 1d100", 0),
        ("1d20 + 1d20 + 1d20 + 1d20 + 1d20 - 100", -47.5)
    };

    var numRolls = 1000000;

EvaluateTestCases(testCases, numRolls);

static void EvaluateTestCases(List<(string expression, double expectedAverage)> testCases, int numRolls)
{
    var results = testCases.AsParallel().Select(testCase =>
    {
        var random = Random.Shared;
        var expression = testCase.expression;
        var total = 0;

        for (var i = 0; i < numRolls; i++)
        {
            total = DnDDiceRollEvaluator.EvaluateExpression(expression, random);
        }

        return total;
    }).ToList();

    for (var i = 0; i < testCases.Count; i++)
    {
        var testCase = testCases[i];
        var expectedAverage = testCase.expectedAverage;
        var average = results.Skip(i * numRolls).Take(numRolls).Average();

        Console.WriteLine($"Expression: {testCase.expression}");
        Console.WriteLine($"Expected average: {expectedAverage}");
        Console.WriteLine($"Actual average: {average}");
        Console.WriteLine($"Difference: {Math.Abs(expectedAverage - average)}");
        Console.WriteLine();
    }
}

public static class DnDDiceRollEvaluator
{
    public static int EvaluateExpression(string expression)
    {
        var regex = new Regex(@"(\d+)d(\d+)");
        var matches = regex.Matches(expression);

        var total = 0;

        foreach (Match match in matches)
        {
            var count = int.Parse(match.Groups[1].Value);
            var sides = int.Parse(match.Groups[2].Value);

            for (var i = 0; i < count; i++)
            {
                total += Random.Shared.Next(1, sides + 1);
            }

            expression = expression.Replace(match.Value, total.ToString());
        }

        return (int)new DataTable().Compute(expression, null);
    }
}