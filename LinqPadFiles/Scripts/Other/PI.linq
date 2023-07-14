<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
</Query>


CalculatePi(100).Dump();

static BigInteger CalculatePi(int precision)
{
    // Initialize variables
    BigInteger a = 1;
    BigInteger b = 3;
    BigInteger c = 3;
    BigInteger d = 1;
    BigInteger e = 0;
    BigInteger f = 0;
    BigInteger g = 0;
    BigInteger h = 0;

    // Iterate over the desired precision
    for (int i = 0; i < precision; i++)
    {
        // Calculate the next digit of pi
        e = d;
        d = c;
        c = b;
        b = a;
        a = (h * b) + (10 * e);
        d = (d * b) + (10 * f);
        g = (BigInteger.DivRem(d, a, out f));
        h = g;
    }

    // Return the result
    return (4 * d) / b;
}