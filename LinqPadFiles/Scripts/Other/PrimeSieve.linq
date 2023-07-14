<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

GetPrimesUpTo(100_000_000).Dump();

static List<int> GetPrimesUpTo(int limit)
{
    // Create a boolean array "prime[0..n]" and
    // initialize all entries it as true.
    // A value in prime[i] will finally be false
    // if i is Not a prime, else true.
    bool[] prime = new bool[limit + 1];
    for (int i = 0; i < prime.Length; i++)
        prime[i] = true;

    for (int p = 2; p * p <= limit; p++)
    {
        // If prime[p] is not changed, then it is
        // a prime
        if (prime[p] == true)
        {
            // Update all multiples of p
            for (int i = p * 2; i <= limit; i += p)
                prime[i] = false;
        }
    }

    // Create a list of prime numbers
    List<int> primes = new List<int>();
    for (int i = 2; i <= limit; i++)
    {
        if (prime[i] == true)
            primes.Add(i);
    }

    // Return the list of prime numbers
    return primes;
}
