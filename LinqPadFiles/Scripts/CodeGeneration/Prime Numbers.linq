<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

const int MAX = 1_000_000_000;
var stopwatch = Stopwatch.StartNew();

// Create an array of boolean values indicating whether a number is prime.
// Start by assuming all numbers are prime by setting them to true.
bool[] primes = new bool[MAX + 1];
for (int i = 0; i < primes.Length; i++)
{
    primes[i] = true;
}

// Loop through a portion of the array (up to the square root of MAX). If
// it's a prime, ensure all multiples of it are set to false, as they
// clearly cannot be prime.
for (int i = 2; i < Math.Sqrt(MAX) + 1; i++)
{
    if (primes[i - 1])
    {
        for (int j = (int)Math.Pow(i, 2); j <= MAX; j += i)
        {
            primes[j - 1] = false;
        }
    }
}

// Output the results
int count = 0;
for (int i = 2; i < primes.Length; i++)
{
    if (primes[i - 1])
    {
        //Console.WriteLine(i);
        count++;
    }
}
stopwatch.Stop();

Console.WriteLine($"{stopwatch.Elapsed.ToString("s")}\tThere are {count} primes up to {MAX}");
  