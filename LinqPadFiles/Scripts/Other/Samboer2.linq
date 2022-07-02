<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
</Query>

// 

var household = new Houshold("Heimen", new Address("", "", ""), new Vector2(), new List<User>(), new User("", new Person("", "", new Address("", "", ""), 1), new List<Claim>()));




household.ToString().Dump();

public record Houshold(string Name, Address Address, Vector2 Position, List<User> Members, User Head);
public record Chore(int Score);
public record Claim(Guid ChoreId, DateTime DateTime, int Score); // Hva skjer når man endrer en chores score etter man har claimet en chore?
public record User(string Username, Person Person, List<Claim> ClaimedChores);
public record Person(string Name, string Email, Address Address, short Capacity);
public record Address(string Street, string City, string Country);