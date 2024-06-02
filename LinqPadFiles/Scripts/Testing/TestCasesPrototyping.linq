<Query Kind="Statements">
  <NuGetReference>CompareNETObjects</NuGetReference>
  <NuGetReference>DeepEqual</NuGetReference>
  <NuGetReference>DiffSharp.Core</NuGetReference>
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <NuGetReference>ObjectsComparer</NuGetReference>
  <Namespace>DiffSharp</Namespace>
  <Namespace>DiffSharp.Backends</Namespace>
  <Namespace>DiffSharp.Data</Namespace>
  <Namespace>DiffSharp.Distributions</Namespace>
  <Namespace>DiffSharp.Model</Namespace>
  <Namespace>DiffSharp.Optim</Namespace>
  <Namespace>DiffSharp.Util</Namespace>
  <Namespace>KellermanSoftware.CompareNetObjects</Namespace>
  <Namespace>KellermanSoftware.CompareNetObjects.IgnoreOrderTypes</Namespace>
  <Namespace>KellermanSoftware.CompareNetObjects.Reports</Namespace>
  <Namespace>KellermanSoftware.CompareNetObjects.TypeComparers</Namespace>
  <Namespace>ObjectsComparer</Namespace>
  <Namespace>ObjectsComparer.Exceptions</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

var car1 = new Car()
{
	Model = "Ford",
	SerialNumber = 1,
	Purchased = new(2020, 1, 1),
	Seller = new Business
	{
		Name = "Cars Inc,",
		Number = "123456"
	},
	OwnerHistory = new()
	{
		new Person() {
			 Age = 99,
			 Name = "Rudolf"
		}
	},
};

var car2 = new Car()
{
	Model = "Ford",
	SerialNumber = 1,
	Purchased = new(2020, 1, 1),
	Owner = new Person{
		Age = 22,
		Name = "Bob"
	},
	OwnerHistory = new()
	{
		new Person() {
			 Age = 29,
			 Name = "Lisa"
		}
	},
	Seller = new Business{
		Name = "Cars Inc,",
		Number = "12345"
	}
};

var comparer = new ObjectsComparer.Comparer<Car>();
var diff = comparer.CalculateDifferences(car1, car2);
diff.Dump();


class TestCase<T1, T2>
{
	Func<T1, T2> _act;
	IEnumerable<string> _arrange;
	IEnumerable<string> _assert;
	
	void Arrange(params string[] arrange)
	{
		_arrange = arrange;
	}

	void Act(Func<T1, T2> act)
	{
		_act = act;
	}

	void Assert(params string[] assert)
	{
		_assert = assert;
	}
	
	void Run()
	{
		// Throws Xunit Exception if GetAssertionResults().Any() is true
	}
	
	List<AssertionResult> GetAssertionResults()
	{
		
	}
}


class Car
{
	public int SerialNumber { get; set; }
	
	public string Model { get; set; }	
	
	public DateOnly Purchased { get; set; }

	public Person? Owner { get; set; }
	
	public List<Person> OwnerHistory { get; set; }
	
	public Business Seller { get; set; }
}

public class Person
{
	public string Name { get; set; }
	public int Age { get; set; }
}

public class Business
{
	public string Name { get; set; }
	public string Number { get; set; }
}

public class Differ
{
	public static List<Difference> GetDifferences<T>(T expected, T actual, string parentPath = "")
	{
		List<Difference> differences = new List<Difference>();
		if (expected == null && actual == null)
			return differences;

		if ((expected == null && actual != null) || (expected != null && actual == null))
		{
			differences.Add(new Difference(parentPath, expected, actual));
			return differences;
		}

		Type type = typeof(T);
		foreach (PropertyInfo propertyInfo in type.GetProperties())
		{
			object expectedValue = propertyInfo.GetValue(expected);
			object actualValue = propertyInfo.GetValue(actual);
			string currentPath = string.IsNullOrEmpty(parentPath) ? propertyInfo.Name : $"{parentPath}.{propertyInfo.Name}";

			if (AreValuesDifferent(expectedValue, actualValue))
			{
				if (propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string))
				{
					differences.AddRange(GetDifferences(expectedValue, actualValue, currentPath));
				}
				else
				{
					differences.Add(new Difference(currentPath, expectedValue, actualValue));
				}
			}
		}
		return differences;
	}

	private static bool AreValuesDifferent(object expected, object actual)
	{
		if (expected == null && actual == null)
			return false;
		if (expected == null || actual == null)
			return true;
		return !expected.Equals(actual);
	}
}

public class Difference
{
	public string PropertyPath { get; }
	public object ExpectedValue { get; }
	public object ActualValue { get; }

	public Difference(string propertyPath, object expectedValue, object actualValue)
	{
		PropertyPath = propertyPath;
		ExpectedValue = expectedValue;
		ActualValue = actualValue;
	}

	public override string ToString()
	{
		return $"{PropertyPath}: Expected <{ExpectedValue}>, Actual <{ActualValue}>";
	}
}
