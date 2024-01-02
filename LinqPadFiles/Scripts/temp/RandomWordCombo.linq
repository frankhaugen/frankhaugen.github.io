<Query Kind="Statements" />

// List of possible adjectives
var adjectives = new List<string>
			{
				"happy",
				"sad",
				"angry",
				"excited",
				"tired",
				"lazy",
				"energetic",
				"nervous",
				"anxious",
				"relaxed"
			};

// List of possible nouns
var nouns = new List<string>
			{
				"cat",
				"dog",
				"bird",
				"fish",
				"lizard",
				"snake",
				"turtle",
				"hamster",
				"gerbil",
				"rat"
			};

// Generate a random index for the adjective and noun lists
var random = new Random();
int adjectiveIndex = random.Next(adjectives.Count);
int nounIndex = random.Next(nouns.Count);

// Get the randomly-selected adjective and noun
string adjective = adjectives[adjectiveIndex];
string noun = nouns[nounIndex];

// Read the suffix from the user
string suffix = "dramatic";

// Prepend the randomly-generated adjective and noun to the suffix
string result = adjective + "-" + noun + "-" + suffix;

// Print the result
Console.WriteLine(result);