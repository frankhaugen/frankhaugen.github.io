<Query Kind="Statements">
  <NuGetReference>System.Device.Location.Portable</NuGetReference>
  <Namespace>System.Device.Location</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
</Query>

var address = new Address("Hovedveien 1", "Oslo");

var person1 = new Person("Frank Haugen", "frank.haugen@gmail.com", address);
var coordinates1 = new System.Device.Location.GeoCoordinate(58.147117, 7.951453);
var interests1 = new List<Interest>(){
	new Interest("1", "Movies", 0),
	new Interest("2", "Long walks on the bitch", 1),
	new Interest("3", "Fencing", 2),
	new Interest("4", "Cosplay", 3),
	new Interest("5", "Archery", 4)
};

var person2 = new Person("Sarah Dragedræberdottir", "hhbgiyhbi@gmail.com", address);
var coordinates2 = new System.Device.Location.GeoCoordinate(58.146781, 7.956843);
var interests2 = new List<Interest>(){
	new Interest("1", "Movies", 0),
	new Interest("6", "War", 1),
	new Interest("7", "Crossfit", 2),
	new Interest("4", "Cosplay", 3),
	new Interest("5", "Archery", 4)
};

var user1 = new User("Wolfie", person1, interests1);
var userCoordinates1 = new UserCoordinates(user1, coordinates1);

var user2 = new User("Sage", person2, interests2);
var userCoordinates2 = new UserCoordinates(user2, coordinates2);

// Distance in meters
var distance = userCoordinates1.Coordinates.GetDistanceTo(userCoordinates2.Coordinates);

if (userCoordinates1.Coordinates.GetDistanceTo(userCoordinates2.Coordinates) < 500)
{
	int matchCount;
	
	matchCount = user2.Interests.Select(x => x.Id).Where(x => user1.Interests.Select(y => y.Id).Contains(x)).Count();
	
	if (matchCount >= 3)
	{
		Console.WriteLine($"match found with {matchCount} matching interests with a distance of {Math.Round(distance, 1)}m");
	}
}

public record UserCoordinates(User User, GeoCoordinate Coordinates);
public record UserLocation(User User, Location Location);
public record User(string Username, Person Person, List<Interest> Interests);
public record Person(string Name, string Email, Address Address);
public record Address(string Street, string City);
public record Interest(string Id, string Name, uint Order);
public record Location(Vector2 Coordinates, string Country, string City);