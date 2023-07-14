<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

public class JaroWinklerDistance
{
    public static double GetSimilarity(string string1, string string2)
    {
        string string1Lower = string1.ToLower();
        string string2Lower = string2.ToLower();
        int matchingChars = GetMatchingChars(string1Lower, string2Lower);
        int transpositions = GetTranspositions(string1Lower, string2Lower);
        double jaroDistance = (matchingChars / (double)string1Lower.Length +
                              matchingChars / (double)string2Lower.Length +
                              (matchingChars - transpositions) / (double)matchingChars) / 3;
        int prefixLength = GetPrefixLength(string1Lower, string2Lower);
        double jaroWinklerDistance = jaroDistance + (prefixLength * 0.1 * (1 - jaroDistance));
        return jaroWinklerDistance;
    }
    
    private static int GetMatchingChars(string string1, string string2)
    {
        int matchingChars = 0;
        for (int i = 0; i < string1.Length; i++)
        {
            if (string2.Contains(string1[i]))
            {
                matchingChars++;
            }
        }
        return matchingChars;
    }

    private static int GetTranspositions(string string1, string string2)
    {
        int transpositions = 0;
        int j = 0;
        for (int i = 0; i < string1.Length; i++)
        {
            if (string1[i] == string2[j])
            {
                j++;
            }
            else
            {
                transpositions++;
            }
        }
        return transpositions / 2;
    }

    private static int GetPrefixLength(string string1, string string2)
    {
        int prefixLength = 0;
        for (int i = 0; i < Math.Min(string1.Length, string2.Length); i++)
        {
            if (string1[i] == string2[i])
            {
                prefixLength++;
            }
            else
            {
                break;
            }
        }
        return prefixLength;
    }
}
