<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>



var text = "My name is Frank";

var uniqueCharacters = text.Distinct();
var characterDictionary = uniqueCharacters.ToDictionary(x => x, x => 0);

foreach(var character in characterDictionary)
{
	characterDictionary[character.Key] = text.Where(x => x.Equals(character.Key)).Count();
}

characterDictionary.Dump();
