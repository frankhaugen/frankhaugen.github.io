<Query Kind="Program">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

#load "BenchmarkDotNet"

void Main()
{
	RunBenchmark(true);
}

private List<int> list;
private Bag<int> bag;
private int[] data;

[GlobalSetup]
public void Setup()
{
	data = Enumerable.Range(0, 10000).ToArray();
	list = new List<int>(10000);
	bag = new Bag<int>(10000);
}

[Benchmark]
public void AddToList()
{
	foreach (var item in data)
	{
		list.Add(item);
	}
}

[Benchmark]
public void AddToBag()
{
	foreach (var item in data)
	{
		bag.Add(item);
	}
}

[Benchmark]
public void RemoveFromList()
{
	for (int i = data.Length - 1; i >= 0; i--)
	{
		list.Remove(data[i]);
	}
}

[Benchmark]
public void RemoveFromBag()
{
	for (int i = data.Length - 1; i >= 0; i--)
	{
		bag.Remove(data[i]);
	}
}

// Define other methods and classes here
public class Bag<T>
{
	private T[] items;
	private int count;

	public Bag(int capacity = 16)
	{
		items = new T[capacity];
		count = 0;
	}

	public void Add(T item)
	{
		if (count == items.Length)
		{
			Resize(items.Length + 1);  // Double the size
		}
		items[count++] = item;
	}

	public bool Remove(T item)
	{
		for (int i = 0; i < count; i++)
		{
			if (Equals(items[i], item))
			{
				items[i] = items[--count]; // Move the last item to the removed spot
				items[count] = default; // Clear the last item

				if (count > 0 && count == items.Length / 4)
				{
					Resize(items.Length / 2);
				}
				return true;
			}
		}
		return false;
	}

	private void Resize(double newSize)
	{
		Resize(Convert.ToInt32(newSize));
	}
	
	private void Resize(int newSize)
	{
		T[] newItems = new T[newSize];
		Array.Copy(items, newItems, count);
		items = newItems;
	}
}