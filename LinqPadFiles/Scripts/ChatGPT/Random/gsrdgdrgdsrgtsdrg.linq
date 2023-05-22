<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
    var kid = string.Empty;
    
    
}

string GetRandomString(int length = 24)
{
    string result = "";

    for (int i = 0; i < length; i++)
    {
        result += RandomNumberGenerator.GetInt32(9).ToString();
    }
    
    return result;
}

public static class Helper
{
    public static bool IsNumeric(this string str)
    {
        double value;
        bool success = Double.TryParse(str, out value);
        return success;
    }

    public static int CharToInt(this char x)
    {
        return Int32.Parse(x.ToString());
    }

    public static IEnumerable<char> ConvertToCharArray(this string str)
    {
        return str.ToCharArray();
    }

    public static IEnumerable<char> RevertArray(this IEnumerable<char> arr)
    {
        Array.Reverse(arr.ToArray());
        return arr;
    }

    public static IEnumerable<int> ConvertToNumArray(this IEnumerable<char> charArray)
    {
        return charArray.Select(x => CharToInt(x)).ToArray();
    }

    public static IEnumerable<int> DoubleEverySecondNumber(this IEnumerable<int> arr)
    {
        return arr.Select((x, i) =>
        {
            return i % 2 == 0 ? x * 2 : x;
        });
    }

    public static int SplitNumber(this int x)
    {
        return x > 9 ? 1 + (x - 10) : x;
    }

    public static IEnumerable<int> SplitNumbersOver10(this IEnumerable<int> arr)
    {
        return arr.Select(x => SplitNumber(x));
    }
}

public class Mod11
{
    public static int GetControlNumber(string modbase)
    {
        if (string.IsNullOrWhiteSpace(modbase))
            throw new Exception("modbase was null or empty");
        if (Helper.IsNumeric(modbase) == false)
            throw new Exception("modbase is not numeric");

        int[] weightNumbers = { 2, 3, 4, 5, 6, 7 };

        Func<int, int, int> modnum = (num, weightNumber) => num * weightNumber;

        int sum = modbase
                    .ConvertToCharArray()
                    .RevertArray()
                    .ConvertToNumArray()
                    .Select((x, i) => modnum(x, weightNumbers[i % 6]))
                    .Sum();

        int controlnumber = 11 - (sum % 11);
        return controlnumber;
    }

    public static string Calculate(string modbase)
    {
        int controlnumber = GetControlNumber(modbase);

        switch (controlnumber)
        {
            case 11:
                return modbase + "0";
            default:
                return modbase + controlnumber.ToString();
        }
    }
}

public class Mod10
{
    private int GetControlNumber(string modbase)
    {
        if (String.IsNullOrWhiteSpace(modbase))
            throw new ArgumentException("modbase was null or empty");
        if (!modbase.IsNumeric())
            throw new ArgumentException("modbase is not numeric");
        var modnum = modbase.ConvertToCharArray()
            .RevertArray()
            .ConvertToNumArray()
            .DoubleEverySecondNumber()
            .SplitNumbersOver10()
            .Sum();
        var controlnumber = 10 - (modnum % 10);
        return controlnumber;
    }
    
    public string Calculate(string modbase)
    {
        var controlnumber = GetControlNumber(modbase);
        return controlnumber == 10 ? modbase + "0" : modbase + controlnumber.ToString();
    }
}
