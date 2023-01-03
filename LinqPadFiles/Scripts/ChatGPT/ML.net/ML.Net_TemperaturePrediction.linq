<Query Kind="Program">
  <NuGetReference>CsvHelper</NuGetReference>
  <NuGetReference>Microsoft.ML</NuGetReference>
  <NuGetReference>Microsoft.ML.AutoML</NuGetReference>
  <NuGetReference>Microsoft.ML.FastTree</NuGetReference>
  <Namespace>CsvHelper</Namespace>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>Microsoft.ML</Namespace>
  <Namespace>Microsoft.ML.Data</Namespace>
  <Namespace>Microsoft.ML.Runtime</Namespace>
  <Namespace>Microsoft.ML.Trainers</Namespace>
  <Namespace>Microsoft.ML.Trainers.FastTree</Namespace>
  <Namespace>Microsoft.ML.Transforms</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.ML.AutoML</Namespace>
  <RuntimeVersion>7.0</RuntimeVersion>
</Query>

#load "DataAndML\ML.NetExtensions\MLContextExtensions"


static void Main(string[] args)
{
    var mlClass = new MyMachineLearning("C:/Users/frank/Downloads/archive/city_temperature.csv");

    mlClass.AutoTrainModel<TemperatureDataRaw, TemperatureData>();
    //mlClass.TrainFastTree();
}

public class MyMachineLearning
{
    private readonly MLContext _mlContext = new();
    private readonly IEnumerable<TemperatureDataRaw> _rawData;
    private readonly string _csvPath;

    public MyMachineLearning(string path)
    {
        _mlContext.AddLogging();
        _csvPath = path;
        _rawData = TemperatureDataReader.Read(_csvPath);
    }

    public ExperimentResult<MulticlassClassificationMetrics> AutoTrainModel<TInput, TOutput>()
         where TInput : class
         where TOutput : class, new()
    {
        var splitData = _mlContext.SplitTrainTest(GetData());
        
        // Use AutoML to find the best model
        var settings =  new MulticlassExperimentSettings()
        {
            OptimizingMetric = MulticlassClassificationMetric.TopKAccuracy,
            MaxExperimentTimeInSeconds = 3600
        };

        var pipeline = _mlContext
                .AddOneHotEncoding("Region", "RegionEncoded")
                .AddOneHotEncoding(_mlContext, "Country", "CountryEncoded")
                .AddOneHotEncoding(_mlContext, "City", "CityEncoded")
                .Append(_mlContext.Transforms.Concatenate("Features",
                    "RegionEncoded",
                    "CountryEncoded",
                    "CityEncoded",
                    "Month", "Day", "Year"));
                    
        var experiment = _mlContext
                .Auto()
                .CreateMulticlassClassificationExperiment(settings);

        
        var experimentResult = experiment.Execute(splitData.Test);
        
        var bestRun = experimentResult.BestRun;
        bestRun.Dump();
        // Return the trained model
        return experimentResult;
    }

    public void TrainFastTree()
    {
        var data = GetData();
        var splitData = _mlContext.SplitTrainTest(data);
        
        var pipeline =
                _mlContext
                .AddOneHotEncoding("Region", "RegionEncoded")
                .AddOneHotEncoding(_mlContext, "Country", "CountryEncoded")
                .AddOneHotEncoding(_mlContext, "City", "CityEncoded")
                .Append(_mlContext.Transforms.Concatenate("Features",
                    "RegionEncoded",
                    "CountryEncoded",
                    "CityEncoded",
                    "Month", "Day", "Year"))
                .Append(_mlContext.Regression.Trainers.FastTree("AvgTemperature"));

        var model = pipeline.Fit(splitData.Train);

        var predictions = model.Transform(splitData.Test);

        var metrics = _mlContext.Regression.Evaluate(predictions, "AvgTemperature", "Score");
        metrics.Dump();

        var predictionEngine = _mlContext.Model.CreatePredictionEngine<TemperatureDataRaw, TemperatureData>(model);

        var prediction = predictionEngine.Predict(new TemperatureDataRaw
        {
            Year = DateTime.Parse("2022-12-11").Year,//.ToString(),
            Month = DateTime.Parse("2022-12-11").Month,//.ToString(),
            Day = DateTime.Parse("2022-12-11").Day,//.ToString(),
            City = "Oslo"
        });
        
        Console.WriteLine($"Predicted temperature: {prediction.Temperature:F2} degrees");

    }


    public IDataView GetData()
    {
        if (!_rawData!.Any())
        {
            Debugger.Break();
        }

        IEnumerable<TemperatureDataRaw> filteredData = _rawData
            .Where(x => !string.IsNullOrWhiteSpace(x.Region))
            .Where(x => !string.IsNullOrWhiteSpace(x.Country))
            .Where(x => !string.IsNullOrWhiteSpace(x.City))
            .Where(x => x.City == "Oslo")
            ;

        var data = _mlContext.Data.LoadFromEnumerable<TemperatureDataRaw>(filteredData);

        return data;
    }
}
public static class DataViewExtensions
{

    public static EstimatorChain<OneHotEncodingTransformer> AddOneHotEncoding(this OneHotEncodingEstimator estimator, MLContext mlContext, string inputColumn, string outputColumn)
    {
        var oneHotEncodingEstimator = estimator.Append(mlContext.Transforms.Categorical.OneHotEncoding(
            outputColumnName: outputColumn,
            inputColumnName: inputColumn
        ));
        return oneHotEncodingEstimator;
    }

    public static EstimatorChain<OneHotEncodingTransformer> AddOneHotEncoding(this EstimatorChain<OneHotEncodingTransformer> estimatorChain, MLContext mlContext, string inputColumn, string outputColumn)
    {
        var oneHotEncodingEstimator = estimatorChain.Append(mlContext.Transforms.Categorical.OneHotEncoding(
            outputColumnName: outputColumn,
            inputColumnName: inputColumn
        ));
        return oneHotEncodingEstimator;
    }
}

// Define the data schema
public record TemperatureData
{
    public DateTime Date { get; set; }
    public float Temperature { get; set; }
}

// Define the data schema
public static class TemperatureDataReader
{
    public static IEnumerable<TemperatureDataRaw> Read(string path)
    {
        var csvconfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = true
        };

        using (var csvreader = new StreamReader(path))
        using (var csv = new CsvReader(csvreader, csvconfig))
        {
            csv.Context.RegisterClassMap<TemperatureDataRawClassMap>();
            while (csv.Read())
            {
                yield return csv.GetRecord<TemperatureDataRaw>();
            }
        }
    }
}

public class TemperatureDataRaw
{
    public string Region { get; set; }
    public string Country { get; set; }
    //public string State { get; set; }
    public string City { get; set; }
    public float Month { get; set; }
    public float Day { get; set; }
    public float Year { get; set; }
    public float AvgTemperature { get; set; }
}

public class TemperatureDataRawClassMap : ClassMap<TemperatureDataRaw>
{
    public TemperatureDataRawClassMap()
    {
        Map(m => m.Region).Name("Region");
        Map(m => m.Country).Name("Country");
        //Map(m => m.State).Name("State");
        Map(m => m.City).Name("City");
        Map(m => m.Month).Name("Month");
        Map(m => m.Day).Name("Day");
        Map(m => m.Year).Name("Year");
        Map(m => m.AvgTemperature).Name("AvgTemperature");
    }
}