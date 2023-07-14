<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>


var document = new CsvDocument();

// Populate document

//document.GetValue<decimal>(12, 15);



public class CsvCell
{
    public string? Value { get; set; }
    public TypeCode Type { get; set; }
}

public class CsvCell<T> : CsvCell
{
    public T GetValue()
    {
        return default;
    }
}

public class CsvRow : List<CsvCell>
{
}

public class CsvDocument : List<CsvRow>
{
    private readonly CsvOptions _options;

    public CsvDocument()
    {
    }

    public CsvDocument(CsvOptions options)
    {
        _options = options;
    }
}

public class CsvOptions
{

    public readonly static CsvOptions Default = new();

    public string Delimiter { get; set; } = ";";
    public string QuoteStart { get; set; } = "\"";
    public string QuoteEnd { get; set; } = "\"";
    public string NewLine { get; set; } = Environment.NewLine;
    public bool HasHeaderRow { get; set; } = true;
    public bool IgnoreEmptyLines { get; set; } = true;
}

public static class CsvParser
{
    public static CsvRow ParseRow(string source, CsvOptions options = null)
    {
        options ??= CsvOptions.Default;

        var row = new CsvRow();
        // QuoteStart and QuoteEnd is not respected so a delimiter or line-break in a quote will mess things up
        var cellValues = source.Split(options.Delimiter);
        foreach (var cellValue in cellValues)
        {
            var cell = new CsvCell();
            cell.Value = cellValue;
            row.Add(cell);
        }
        return row;
    }
    
    public static CsvDocument ParseDocument(string source, CsvOptions options = null)
    {
        options ??= CsvOptions.Default;

        var document = new CsvDocument(options);
        // QuoteStart and QuoteEnd is not respected so a delimiter or line-break in a quote will mess things up
        var rowValues = source.Split(options.NewLine);
        // Header is not handled
        foreach (var rowValue in rowValues)
        {
            if (options.IgnoreEmptyLines && string.IsNullOrWhiteSpace(rowValue))
                continue;

            var row = CsvParser.ParseRow(rowValue, options);
            document.Add(row);
        }
        return document;
    }
}
