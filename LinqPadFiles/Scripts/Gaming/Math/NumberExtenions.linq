<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
</Query>

void Main()
{
    10.Dump()
    .PowerOf(2).Dump()
    .Divide(2).Dump()
    .Multiply(2).Dump()
    .SquareRoot().Dump()
    ;
}

public static class NumberExtensions
{
    public static T Add<T>(this T number, T value) where T : INumber<T>
    {
        return number + value;
    }

    public static T Subtract<T>(this T number, T value) where T : INumber<T>
    {
        return number - value;
    }

    public static T Multiply<T>(this T number, T value) where T : INumber<T>
    {
        return number * value;
    }

    public static T Divide<T>(this T number, T value) where T : INumber<T>
    {
        return number / value;
    }

    public static T SquareRoot<T>(this T number, TimeSpan? timeout = null) where T : INumber<T>
    {
        return number.PowerOf(T.Half);
//        T guess = number.Divide(T.One + T.One); // Start with a guess of half the value
//        T error = number.Subtract(guess.Multiply(guess)).Abs(); // Calculate the error
//        T tolerance = T.CreateChecked(0.001); // Set a tolerance level
//
//        timeout ??= TimeSpan.FromMilliseconds(100000);
//
//        var stopwatch = Stopwatch.StartNew();
//
//        while (error.GreaterThan(tolerance) && stopwatch.Elapsed < timeout.Value)
//        {
//            guess = guess
//                .Add(number.Subtract(guess.Multiply(guess))
//                .Divide(guess.Multiply(T.One + T.One))); // Improve the guess
//            error = number.Subtract(guess.Multiply(guess)).Abs(); // Calculate the error again
//        }
//
//        return guess; // Return the final guess
//        return guess; // Return the final guess
    }



    public static bool GreaterThan<T>(this T number, T value) where T : INumber<T>
    {
        return number > value;
    }
    public static bool LessThan<T>(this T number, T value) where T : INumber<T>
    {
        return number < value;
    }


    public static T Abs<T>(this T number) where T : INumber<T>
    {
        return T.Abs(number);
    }

    public static T PowerOf<T>(this T number, T value) where T : INumber<T>
    {
        if (value.Equals(T.Zero)) // Anything to the power of 0 is 1
        {
            return T.One;
        }
        else if (value.Equals(T.One)) // Anything to the power of 1 is itself
        {
            return number;
        }
        else if (T.IsNegative(value)) // Negative powers are not supported
        {
            throw new ArgumentException("Negative powers are not supported.");
        }
        else // For positive powers greater than 1, use an iterative algorithm
        {
            T result = number;
            for (T i = T.One; i.LessThan(value); i = i.Add(T.One))
            {
                result = result.Multiply(number);
            }
            return result;
        }
    }
}