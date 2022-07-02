<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

var incrementSize = 7;
var incrementCount = 26;
var startDay = 2;
var startDate = DateOnly.FromDayNumber(startDay);
var currentDate = DateOnly.FromDayNumber(startDay);

for (int i = 0; i < incrementCount; i++)
{
	Console.WriteLine(currentDate.ToString("d/ MMM"));
	currentDate = currentDate.AddDays(incrementSize);
}