<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>


GetChunks().Dump();

IEnumerable<TimeChunk> GetChunks()
{
    var list = new List<TimeChunk>();

    for (int i = 0; i < 100; i++)
    {
        list.Add(new TimeChunk(new TimeOnly(8, 0), new TimeOnly(15, 30), new PayModifier(100, PayModifierType.Normal), new List<TimeChunkDimension>() { new TimeChunkDimension(DimensionType.Project, "SemineConnect", "Pogo", false) }));
    }

    return list;
}


public record YearWeeks(int Year, IEnumerable<Week> Weeks);
public record Week(IEnumerable<Day> Days);
public record Day(WeekDay WeekDay, IEnumerable<TimeChunk> TimeChunks);
public record WeekDay(int Year, int Week, DayOfWeek DayOfWeek);


public enum DimensionType
{
    Generic,
    Project,
    Dimension,
    Customer
}

/// <summary>Modifier type to categorize what a modifier on pay is</summary>
public enum PayModifierType
{
    Normal,
    FlexPlus,
    FlexMinus,
    Furlough,
    UnpaidLeave,
    SickLeave,
    SickDependentLeave,
    ParentalLeave,
    Standby,
    OvetimeRegular,
    OvertimeLow,
    OvertimeMid,
    OvertimeHigh,
    UnpaidSocialLeave
}


public record TimeChunk(TimeOnly StartTime, TimeOnly EndTime);

public record Workday(DateOnly Date, IEnumerable<WorkChunk> WorkChunks);
public record WorkChunk(TimeChunk TimeChunk, PayModifier PayModifier, IEnumerable<TimeChunkDimension> Dimensions);

public record TimeChunkDimension(DimensionType DimensionType, string Name, string Value, bool IsUnique);
public record PayModifier(decimal Percentage, PayModifierType PayModifierType);
