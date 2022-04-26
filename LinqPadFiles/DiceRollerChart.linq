<Query Kind="Statements">
  <Reference Relative="..\..\Frank.Libraries\src\Frank.Libraries.Gaming\bin\Debug\net6.0\Frank.Libraries.Gaming.dll">C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.Gaming\bin\Debug\net6.0\Frank.Libraries.Gaming.dll</Reference>
  <NuGetReference>Humanizer</NuGetReference>
  <NuGetReference>HumanTimeParser.Core</NuGetReference>
  <NuGetReference>NumbersToWordsConverter</NuGetReference>
  <Namespace>Frank.Libraries.Gaming.Starfinder</Namespace>
  <Namespace>Frank.Libraries.Gaming.Starfinder.Characters</Namespace>
  <Namespace>Frank.Libraries.Gaming.Starfinder.Characters.Models.Enums</Namespace>
  <Namespace>Frank.Libraries.Gaming.Starfinder.Combat.Ground</Namespace>
  <Namespace>Frank.Libraries.Gaming.Starfinder.Combat.Space</Namespace>
  <Namespace>Frank.Libraries.Gaming.Starfinder.Extensions</Namespace>
  <Namespace>Frank.Libraries.Gaming.Starfinder.Space</Namespace>
  <Namespace>Frank.Libraries.Gaming.Starfinder.Space.Constants</Namespace>
  <Namespace>Frank.Libraries.Gaming.Starfinder.Space.Models</Namespace>
  <Namespace>Frank.Libraries.Gaming.Starfinder.Space.Models.Enums</Namespace>
  <Namespace>Frank.Libraries.Gaming.Starfinder.Utilities</Namespace>
  <Namespace>Humanizer</Namespace>
  <Namespace>Humanizer.Bytes</Namespace>
  <Namespace>Humanizer.Configuration</Namespace>
  <Namespace>Humanizer.DateTimeHumanizeStrategy</Namespace>
  <Namespace>Humanizer.Inflections</Namespace>
  <Namespace>Humanizer.Localisation</Namespace>
  <Namespace>Humanizer.Localisation.CollectionFormatters</Namespace>
  <Namespace>Humanizer.Localisation.DateToOrdinalWords</Namespace>
  <Namespace>Humanizer.Localisation.Formatters</Namespace>
  <Namespace>Humanizer.Localisation.NumberToWords</Namespace>
  <Namespace>Humanizer.Localisation.Ordinalizers</Namespace>
  <Namespace>Humanizer.Localisation.TimeToClockNotation</Namespace>
  <Namespace>HumanTimeParser.Core.Culture</Namespace>
  <Namespace>HumanTimeParser.Core.Extensions</Namespace>
  <Namespace>HumanTimeParser.Core.Parsing</Namespace>
  <Namespace>HumanTimeParser.Core.Parsing.Default</Namespace>
  <Namespace>HumanTimeParser.Core.Parsing.State</Namespace>
  <Namespace>HumanTimeParser.Core.Sectioning</Namespace>
  <Namespace>HumanTimeParser.Core.TimeConstructs</Namespace>
  <Namespace>HumanTimeParser.Core.Tokenization</Namespace>
  <Namespace>HumanTimeParser.Core.Tokenization.Tokens</Namespace>
  <Namespace>NumbersToWords</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Windows.Forms.DataVisualization.Charting</Namespace>
</Query>

	public record Struct Ability(Frank.Libraries.Gaming.Starfinder.Characters.Models.Enums.AbilityName AbilityName);
	
	
	public enum AbilityNameCV
	{
		Strength = 0,
		Dexterity = 1,
		Constitution = 2,
		Intelligence = 3,
		Wisdom = 4,
		Charisma = 5
	}