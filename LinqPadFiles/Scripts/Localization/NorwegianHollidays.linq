<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>


public static class NorwegianHolidays
{
    public static bool IsHoliday(DateOnly date) => IsHoliday(date.ToDateTime(TimeOnly.MinValue));
    public static bool IsHoliday(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Sunday)
        {
            return true;
        }

        int year = date.Year;
        return date == GetNewYearsDay(year) ||
               date == GetMaundyThursday(year) ||
               date == GetGoodFriday(year) ||
               date == GetEasterSunday(year) ||
               date == GetEasterMonday(year) ||
               date == GetLaborDay(year) ||
               date == GetConstitutionDay(year) ||
               date == GetAscensionDay(year) ||
               date == GetWhitMonday(year) ||
               date == GetChristmasDay(year) ||
               date == GetBoxingDay(year);
    }

    public static DateTime GetNewYearsDay(int year) => new DateTime(year, 1, 1);
    public static DateTime GetMaundyThursday(int year) => GetEasterSunday(year).AddDays(-3);
    public static DateTime GetGoodFriday(int year) => GetEasterSunday(year).AddDays(-2);
    public static DateTime GetEasterSunday(int year)
    {
        int month = EasterCalculator.GetEasterMonth(year);
        int day = EasterCalculator.GetEasterDay(year);
        return new DateTime(year, month, day);
    }
    public static DateTime GetEasterMonday(int year) => GetEasterSunday(year).AddDays(1);
    public static DateTime GetLaborDay(int year) => new DateTime(year, 5, 1);
    public static DateTime GetConstitutionDay(int year) => new DateTime(year, 5, 17);
    public static DateTime GetAscensionDay(int year) => GetEasterSunday(year).AddDays(39);
    public static DateTime GetWhitSunday(int year) => GetEasterSunday(year).AddDays(49);
    public static DateTime GetWhitMonday(int year) => GetWhitSunday(year).AddDays(1);
    public static DateTime GetChristmasDay(int year) => new DateTime(year, 12, 25);
    public static DateTime GetBoxingDay(int year) => new DateTime(year, 12, 26);
}


public static class EasterCalculator
{
    private static int GetA(int year) => year % 19;
    private static int GetB(int year) => year / 100;
    private static int GetC(int year) => year % 100;
    private static int GetD(int year) => GetB(year) / 4;
    private static int GetE(int year) => GetB(year) % 4;
    private static int GetF(int year) => (GetB(year) + 8) / 25;
    private static int GetG(int year) => (GetB(year) - GetF(year) + 1) / 3;
    private static int GetH(int year) => (19 * GetA(year) + GetB(year) - GetD(year) - GetG(year) + 15) % 30;
    private static int GetI(int year) => GetC(year) / 4;
    private static int GetK(int year) => GetC(year) % 4;
    private static int GetL(int year) => (32 + 2 * GetE(year) + 2 * GetI(year) - GetH(year) - GetK(year)) % 7;
    private static int GetM(int year) => (GetA(year) + 11 * GetH(year) + 22 * GetL(year)) / 451;
    public static int GetEasterMonth(int year) => (GetH(year) + GetL(year) - 7 * GetM(year) + 114) / 31;
    public static int GetEasterDay(int year) => ((GetH(year) + GetL(year) - 7 * GetM(year) + 114) % 31) + 1;

}

public enum Holiday
{
    NewYearsDay,
    MaundyThursday,
    GoodFriday,
    EasterSunday,
    EasterMonday,
    LaborDay,
    ConstitutionDay,
    AscensionDay,
    WhitSunday,
    WhitMonday,
    ChristmasDay,
    BoxingDay,
    Sunday
}