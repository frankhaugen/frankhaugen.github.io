<Query Kind="Statements">
  <NuGetReference>CsvHelper</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>CsvHelper</Namespace>
</Query>





public interface ICsvFileReader
{
    
}

public class CsvFileReader : ICsvFileReader
{
    public static IEnumerable<T> ReadCsvFile<T, TMap>(FileInfo file) where TMap : ClassMap<T>
    {
        var csvconfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = true
        };

        using (var csvreader = new StreamReader(file.OpenRead()))
        using (var csv = new CsvReader(csvreader, csvconfig))
        {
            csv.Context.RegisterClassMap<TMap>();
            while (csv.Read())
            {
                yield return csv.GetRecord<T>();
            }
        }
    }
}