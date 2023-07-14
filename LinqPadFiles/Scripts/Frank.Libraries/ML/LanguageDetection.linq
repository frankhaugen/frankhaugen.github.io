<Query Kind="Program">
  <NuGetReference>Frank.Libraries.ML</NuGetReference>
  <NuGetReference>Microsoft.Data.Analysis</NuGetReference>
  <NuGetReference>Microsoft.ML</NuGetReference>
  <NuGetReference>Microsoft.ML.Probabilistic</NuGetReference>
  <NuGetReference>Microsoft.ML.Tokenizers</NuGetReference>
  <Namespace>Microsoft.ML</Namespace>
  <Namespace>Microsoft.ML.Data</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Frank.Libraries.ML.LanguageDetection</Namespace>
  <Namespace>Frank.Libraries.Enums.Countries</Namespace>
</Query>

public class LanguageModelInput
{
    [ColumnName("Label")]
    public string LanguageCode { get; set; }

    public string Word { get; set; }

    public string Frequency { get; set; }
}
//public class LanguageModelInput
//{
//    [ColumnName("Label")]
//    public string LanguageCode { get; set; }
//
//    [VectorType]
//    public string[] Words { get; set; }
//
//    [VectorType]
//    public float[] Frequency { get; set; }
//}

public class LanguagePrediction
{
    public string LanguageCode { get; set; }
    public float[] Score { get; set; }
}

public class LanguageDetectionBuilder
{
    private readonly MLContext _mlContext;
    private IDataView _data;
    private ITransformer _model;

    public LanguageDetectionBuilder()
    {
        _mlContext = new MLContext();
    }

    public LanguageDetectionBuilder LoadData()
    {
        var languages = Languages.List;

        IEnumerable<LanguageModelInput> languageModelInputs = languages.SelectMany(languageModel =>
        languageModel.Frequency.Select(kvp => new LanguageModelInput
        {
            LanguageCode = languageModel.LanguageCode.ToString(),
            Word = kvp.Key,
            Frequency = ((float)kvp.Value).ToString()
        }));

        _data = _mlContext.Data.LoadFromEnumerable(languageModelInputs);
        //_data = _mlContext.Data.LoadFromEnumerable(languages.Select(item => new LanguageModelInput()
        //{
        //    LanguageCode = item.LanguageCode.ToString(),
        //    Words = item.Frequency.Keys.ToArray(),
        //    Frequency = item.Frequency.Values.Select(value => (float)value).ToArray()
        //    //Frequency = item.Frequency.Values.Select(value => ((float)value).ToString()).ToArray()
        //}));
        return this;
    }

    public LanguageDetectionBuilder BuildPipeline()
    {
        //var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(LanguageModel.Frequency))
        //    .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
        //    .Append(_mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(LanguageModel.LanguageCode)))
        //    .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"))
        //    .Append(_mlContext.Transforms.Concatenate("Features", "Features"))
        //    .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
        //    .Append(_mlContext.Transforms.NormalizeMeanVariance("Features"))
        //    .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
        //    .Append(_mlContext.Transforms.NormalizeMeanVariance("Features"))
        //    .Append(_mlContext.Transforms.Conversion.MapValueToKey("Label"));

        var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label")
                    .Append(_mlContext.Transforms.Concatenate("Features", "Word", "Frequency"))
                    //.Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                    //.Append(_mlContext.Transforms.NormalizeMeanVariance("Features"))
                    //.Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"))
                    
                    ;

        _model = pipeline.Fit(_data);
        return this;
    }

    public LanguageDetectionBuilder Evaluate()
    {
        var predictions = _model.Transform(_data);
        var metrics = _mlContext.MulticlassClassification.Evaluate(predictions);
        Console.WriteLine($"Accuracy: {metrics.MacroAccuracy}");
        return this;
    }

    public LanguageDetectionBuilder Predict(string inputText)
    {
        var testSample = new LanguageModel
        {
            Frequency = new Dictionary<string, int>()
        };

        string[] words = inputText.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var word in words)
        {
            if (testSample.Frequency.ContainsKey(word))
                testSample.Frequency[word]++;
            else
                testSample.Frequency[word] = 1;
        }

        var predictionEngine = _mlContext.Model.CreatePredictionEngine<LanguageModel, LanguagePrediction>(_model);
        var prediction = predictionEngine.Predict(testSample);

        Console.WriteLine($"Predicted language: {prediction.LanguageCode}");

        var predictedLanguages = Enum.GetValues(typeof(LanguageCode))
            .Cast<LanguageCode>()
            .Select((language, index) => new { Language = language, Probability = prediction.Score[index] })
            .OrderByDescending(item => item.Probability);

        foreach (var item in predictedLanguages)
        {
            Console.WriteLine($"{item.Language}: {item.Probability}");
        }

        return this;
    }
}

public class Program
{
    static void Main(string[] args)
    {
        var builder = new LanguageDetectionBuilder()
            .LoadData()
            .BuildPipeline()
            .Evaluate();

        Console.WriteLine("Enter the text to detect its language:");
        string inputText = Console.ReadLine().Trim();

        builder.Predict(inputText);
    }
}
