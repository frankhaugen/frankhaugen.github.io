<Query Kind="Statements">
  <NuGetReference>PublicHoliday</NuGetReference>
  <Namespace>PublicHoliday</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

var owners = GetOwners();
var mondays = GetDatesOfDaysOfWeek(2023, DayOfWeek.Monday);
var mondaysOrFirstWorkday = IncrementAnyToFirstWorkday(mondays);


List<HomeNumber> homeNumbers = new List<HomeNumber>
{
    new HomeNumber("H0202"),
    new HomeNumber("H0101"),
    new HomeNumber("H0102"),
    new HomeNumber("U0101"),
    new HomeNumber("H0302"),
    new HomeNumber("H0201"),
    new HomeNumber("H0301"),
};
homeNumbers.Dump();
homeNumbers.Sort(new HomeNumberComparer());
homeNumbers.Dump();

owners.Select(x => new HomeNumber(x.ApartmentNumber)).Dump();
owners.Select(x => new HomeNumber(x.ApartmentNumber).ToString()).Dump();


CreateBuilding(CreateApartmentOwners(owners)).Dump();


mondaysOrFirstWorkday
    .Select(x => DateFormatter.FormatDate(x))
    .Dump();

owners
    .Select(x => new { x.FirstAndMiddleName, x.LastName, AppartmentNumber = x.ApartmentNumber , Section = Enum.Parse<SectionLetter>(x.Address.Split(" ").Last())})
    .GroupBy(x => x.Section)
    .OrderBy(x => x.Key)
    
.Dump();

owners.Dump();

Building CreateBuilding(IEnumerable<ApartmentOwner> owners)
{
    var building = new Building();
    
    var sectionGroups = owners.GroupBy(x => x.Section);
    foreach (var sectionGroup in sectionGroups)
    {
        var section = new Section()
        {
            Letter = sectionGroup.Key
        };
        var apartmentGroups = sectionGroup.GroupBy(x => new{x.Floor, x.Side}).Dump(); 
        foreach (var apartmentGroup in apartmentGroups)
        {
            //var apartment = new Apartment();
            
            
        }
    }
    
    
    return new();
}

IEnumerable<ApartmentOwner> CreateApartmentOwners(IEnumerable<Owner> owners)
{
    return owners.Select(x => new ApartmentOwner(x.FirstAndMiddleName, x.LastName, x.ApartmentNumber, x.Address));    
}

Building CreateBuildingOld(IEnumerable<Owner> owners)
{
    var apatmentsAndNames = owners.Select(x => new { x.LastName, x.FirstAndMiddleName, x.ApartmentNumber, Section = Enum.Parse<SectionLetter>(x.Address.Split(" ").Last()) });
    var sections = apatmentsAndNames.GroupBy(x => x.Section).Select(x => new Section(){Letter = x.Key});

    foreach (var section in sections)
    {
        //var sectionGroup = 
    }
    
    return new Building()
    {
       Sections = sections.ToList()
    };
}


IEnumerable<DateOnly> IncrementAnyToFirstWorkday(IEnumerable<DateOnly> dates)
{
    var output = new List<DateOnly>();
    foreach (var date in dates)
    {
        var holidays = new NorwayPublicHoliday();
        if (!holidays.IsWorkingDay(date.ToDateTime(TimeOnly.MinValue)))
        {
            output.Add(IncrementToFirstWorkday(date));
        }
        else
        {
            output.Add(date);
        }
    }
    return output;
}

DateOnly IncrementToFirstWorkday(DateOnly date)
{
    var holidays = new NorwayPublicHoliday();

    while (holidays.IsPublicHoliday(date.ToDateTime(TimeOnly.MinValue)))
    {
        date = date.AddDays(1);
    }

    return date;
}

IEnumerable<DateOnly> GetDatesOfDaysOfWeek(int year, DayOfWeek dayOfWeek)
{
    List<DateOnly> dates = new List<DateOnly>();

    // Start at the first day of the year
    DateTime date = new DateTime(year, 1, 1);

    // Loop through the days of the year
    while (date.Year == year)
    {
        // If the day of the week matches the specified day of the week, add it to the list
        if (date.DayOfWeek == dayOfWeek)
        {
            dates.Add(new DateOnly(date.Year, date.Month, date.Day));
        }
        date = date.AddDays(1);
    }
    return dates;
}

IEnumerable<Owner> GetOwners()
{
    var owners = new List<Owner>();
    foreach (string line in GetOwnerStringList())
    {
        string[] fields = line.Split('\t');

        var owner = new Owner
        {
            ApartmentNumber = fields[0],
            DeveloperApartmentNumber = fields[1],
            Address = fields[2],
            PostalCode = fields[3],
            Role = fields[4],
            FirstAndMiddleName = fields[5],
            LastName = fields[6]
        };

        owners.Add(owner);
    }
    return owners;
}

IEnumerable<string> GetOwnerStringList() => """
""".Split("\n");

public struct HomeNumber
{
    public HomeNumber(string input)
    {
        Type = input.Substring(0,1);
        Floor = int.Parse(input.Substring(1,2));
        Door = int.Parse(input.Substring(3,2));
    }
    
    public string Type { get; }
    public int Floor { get; }
    public int Door { get; }

    public override string ToString()
    {
        return Type + Floor.ToString("00") + Door.ToString("00");
    }
}
public class HomeNumberComparer : IComparer<HomeNumber>
{
    public int Compare(HomeNumber x, HomeNumber y)
    {
        if (x.Type == y.Type)
        {
            if (x.Floor == y.Floor)
            {
                return x.Door.CompareTo(y.Door);
            }
            else
            {
                return x.Floor.CompareTo(y.Floor);
            }
        }
        else if (x.Type == "U")
        {
            return -1;
        }
        else if (y.Type == "U")
        {
            return 1;
        }
        else if (x.Type == "H")
        {
            return -1;
        }
        else if (y.Type == "H")
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}

public static class DateFormatter
{
    public static string FormatDate(DateOnly date)
    {
        string day = date.Day.ToString();
        string month = date.ToString("MMM", CultureInfo.CurrentCulture).ToLower();
        return $"{day}. {month}";
    }
}

public class ApartmentComparer : IComparer<Apartment>
{
    public int Compare(Apartment x, Apartment y)
    {
        if (x.Floor == Floor.Basement && y.Floor != Floor.Basement)
        {
            return -1;
        }
        else if (x.Floor != Floor.Basement && y.Floor == Floor.Basement)
        {
            return 1;
        }
        else if (x.Floor == Floor.Basement && y.Floor == Floor.Basement)
        {
            return x.Number.CompareTo(y.Number);
        }
        else
        {
            return x.Floor.CompareTo(y.Floor);
        }
    }
}


public class Building
{
    public List<Section> Sections { get; set; }
}

public class Section
{
    public SectionLetter Letter { get; set; }
    public List<Apartment> Apartments { get; set; }
}

public enum SectionLetter
{
    A,
    B,
    C,
    D,
    E,
    F
}

public enum Floor
{
    Basement,
    First,
    Second,
    Third
}

public enum Side
{
    Left = 1,
    Right = 2
}

public class Owner
{
    public string ApartmentNumber { get; set; }
    public string DeveloperApartmentNumber { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string Role { get; set; }
    public string FirstAndMiddleName { get; set; }
    public string LastName { get; set; }
}

public class ApartmentOwner
{
    public ApartmentOwner(string firstName, string lastName, string apartmentNumber, string address)
    { 
        FirstName = firstName;
        LastName = lastName;
        if (apartmentNumber.Substring(0, 1) == "U")
            Floor = Floor.Basement;
        else
            Floor = (Floor)int.Parse(apartmentNumber.Substring(2, 1));
        Side = (Side)int.Parse(apartmentNumber.Substring(4));
        Section = Enum.Parse<SectionLetter>(address.Split(" ").Last());
    }
    
    public Floor Floor { get; set; }
    public SectionLetter Section { get; set; }
    public Side Side { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class OwnerName
{
    public string FirstName { get; set; }
    public string lastName { get; set; }
}

public class Apartment
{
    public List<OwnerName> Owners { get; set; }
    public int Number { get; set; }
    public Side Side { get; set; }
    public Floor Floor { get; set; }

    public Apartment(string apartmentNumber)
    {
        // Extract the floor information from the apartment number
        string floorString = apartmentNumber.Substring(0, 1);

        // Parse the floor information
        if (floorString == "U")
        {
            Floor = Floor.Basement;
        }
        else
        {
            Floor = (Floor)int.Parse(apartmentNumber.Substring(2, 1));
        }

        // Parse the apartment number
        Number = int.Parse(apartmentNumber.Substring(4));
        
        Side = (Side)int.Parse(apartmentNumber.Substring(4));
    }

    /// <summary>Returns the legal "Bolignummer" for the apartment</summary>
    public override string ToString()
    {
        string floorString = (Floor == Floor.Basement) ? "U" : ((int)Floor).ToString("D2");
        return $"{floorString}{Number:D2}";
    }
}