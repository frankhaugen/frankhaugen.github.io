<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
</Query>


static void Main(string[] args)
{
    // The Cron expression to interpret
    string cronExpression = "0 0 12 * * ?";

    // Extract the individual parts of the Cron expression
    string[] cronParts = Regex.Split(cronExpression, @"\s+");

    // Parse the minute value
    int minute = int.Parse(cronParts[0]);
    Console.WriteLine($"Minute: {minute}");

    // Parse the hour value
    int hour = int.Parse(cronParts[1]);
    Console.WriteLine($"Hour: {hour}");

    // Parse the day of the month value
    int dayOfMonth = int.Parse(cronParts[2]);
    Console.WriteLine($"Day of month: {dayOfMonth}");

    // Parse the month value
    int month = int.Parse(cronParts[3]);
    Console.WriteLine($"Month: {month}");

    // Parse the day of the week value
    int dayOfWeek = int.Parse(cronParts[4]);
    Console.WriteLine($"Day of week: {dayOfWeek}");

    // Parse the year value
    int year = int.Parse(cronParts[5]);
    Console.WriteLine($"Year: {year}");

    // Parse the time zone value
    string timeZone = cronParts[6];
    Console.WriteLine($"Time zone: {timeZone}");
}