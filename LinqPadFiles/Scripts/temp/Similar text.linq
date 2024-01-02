<Query Kind="Program" />

void Main()
{
	// Set the desired similarity quotient
	double similarityQuotient = 0.7;

	// Read in the dataset
	string[] lines = { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz", "bbc" };

	Dictionary<string, List<string>> groups = GroupLines(lines, similarityQuotient);

	PrintGroups(groups);
}

void PrintGroups(Dictionary<string, List<string>> groups)
{
	// Iterate through the groups
	foreach (var group in groups)
	{
		// Create a string representation of the group
		string groupString = "Group " + group.Key + ": " + String.Join(", ", group.Value);

		// Print the group string
		Console.WriteLine(groupString);
	}
}

string FindMatchingGroup(string line, Dictionary<string, List<string>> groups, double similarityQuotient)
{
    // Iterate through the groups
    foreach (var group in groups)
    {
        // Iterate through the strings in the group
        foreach (string s in group.Value)
        {
            // Calculate the Levenshtein similarity between the line and the string in the group
            double similarity = LevenshteinSimilarity(line, s);

            // Check if the similarity is greater than or equal to the similarity quotient
            if (similarity >= similarityQuotient)
            {
				// Return the group key
				return group.Key;
			}
		}
	}

	// If no matching group was found, return null
	return null;
}

void AddToGroup(string line, string groupKey, Dictionary<string, List<string>> groups)
{
	// Add the line to the group
	groups[groupKey].Add(line);
}

Dictionary<string, List<string>> GroupLines(string[] lines, double similarityQuotient)
{
	// Initialize the groups dictionary
	Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>();

	// Iterate through the lines
	foreach (string line in lines)
	{
		// Try to find a matching group for the current line
		string matchingGroup = FindMatchingGroup(line, groups, similarityQuotient);

		// If a matching group was found, add the line to the group
		if (matchingGroup != null)
		{
			AddToGroup(line, matchingGroup, groups);
		}
		// If no matching group was found, create a new group for the line
		else
		{
			groups[line] = new List<string> { line };
		}
	}

	return groups;
}

double LevenshteinSimilarity(string s, string t)
{
	// Initialize the distance matrix
	int[,] distance = new int[s.Length + 1, t.Length + 1];
	for (int i = 0; i <= s.Length; i++) distance[i, 0] = i;
	for (int j = 0; j <= t.Length; j++) distance[0, j] = j;

	// Iterate through the rows and columns of the distance matrix
	for (int i = 1; i <= s.Length; i++)
	{
		for (int j = 1; j <= t.Length; j++)
		{
			// Calculate the cost of a substitution
			int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;

			// Update the distance matrix
			distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
		}
	}

	// Calculate the maximum length of the two strings
	int maxLength = Math.Max(s.Length, t.Length);

	// Return the similarity quotient (1.0 - distance / maxLength)
	return 1.0 - (double)distance[s.Length, t.Length] / maxLength;
}
