<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

public interface ITimeProvider
{
    /// <summary>
    /// Gets the current date and time in UTC
    /// </summary>
    /// <returns></returns>
    DateTime GetDateTime();

    /// <summary>
    /// Gets the current time zone
    /// </summary>
    /// <returns></returns>
    TimeZoneInfo GetTimeZone();
}

public static class TimeProviderExtensions
{
    public static DateOnly GetDate(this ITimeProvider provider)
        => DateOnly.FromDateTime(provider.GetDateTime());

    public static TimeOnly GetTime(this ITimeProvider provider)
        => TimeOnly.FromDateTime(provider.GetDateTime());

    public static DateTime GetLocalDateTime(this ITimeProvider provider)
        => TimeZoneInfo.ConvertTimeFromUtc(provider.GetDateTime(), provider.GetTimeZone());
        
    public static DateTimeOffset GetLocalDateTimeOffset(this ITimeProvider provider)
        => new(provider.GetDateTime(), provider.GetTimeZone().GetUtcOffset(provider.GetDateTime()));
}

public class SystemTimeProvider : ITimeProvider
{
    public DateTime GetDateTime() => DateTime.UtcNow;
    public TimeZoneInfo GetTimeZone() => TimeZoneInfo.Local;
}