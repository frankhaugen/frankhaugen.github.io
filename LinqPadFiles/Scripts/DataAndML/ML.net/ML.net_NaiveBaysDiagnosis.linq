<Query Kind="Program">
  <NuGetReference>Microsoft.ML</NuGetReference>
  <Namespace>Microsoft.ML</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

static void Main(string[] args)
{
    // Set up the training data
    TrainingData[] trainingData = new TrainingData[]
    {
                new TrainingData() { Label = "Flu", Fever = true, Cough = true, SoreThroat = true },
                new TrainingData() { Label = "Flu", Fever = true, Cough = true, SoreThroat = false },
                new TrainingData() { Label = "Allergy", Fever = false, Cough = true, SoreThroat = false },
                new TrainingData() { Label = "Allergy", Fever = false, Cough = true, SoreThroat = true },
                new TrainingData() { Label = "Cold", Fever = true, Cough = false, SoreThroat = true },
                new TrainingData() { Label = "Cold", Fever = false, Cough = false, SoreThroat = true }
    };

    // Load the training data into an IDataView
    MLContext mlContext = new MLContext();
    IDataView dataView = mlContext.Data.LoadFromEnumerable(trainingData);

    mlContext.Log += (object? sender, LoggingEventArgs e) => e.Dump();

    // Define the pipeline
    var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label")
        .Append(mlContext.Transforms.Categorical.OneHotEncoding("Fever"))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding("Cough"))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding("SoreThroat"))
        .Append(mlContext.Transforms.Concatenate("Features", "Fever", "Cough", "SoreThroat"))
        .Append(mlContext.MulticlassClassification.Trainers.NaiveBayes("Label", "Features"))
        .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"))
        ;


    // Train the model
    ITransformer model = pipeline.Fit(dataView);

    // Create a test data instance and set the symptom values
    TrainingData testData = new TrainingData() { Fever = true, Cough = true, SoreThroat = false };

    // Make a prediction
    var predictionEngine = mlContext.Model.CreatePredictionEngine<TrainingData, Result>(model);
    var prediction = predictionEngine.Predict(testData);

    // Print the prediction and probability scores
    Console.WriteLine("Prediction: {0}", prediction.PredictedLabel);
    Console.WriteLine("Probability scores:");
    foreach (var classProbability in prediction.Probability)
    {
        Console.WriteLine("{0}: {1:0.##}", classProbability.Key, classProbability.Value);
    }
}


public class TrainingData
{
    public string Label { get; set; }
    public bool Fever { get; set; }
    public bool Cough { get; set; }
    public bool SoreThroat { get; set; }
}

public class Result
{
    public string PredictedLabel { get; set; }
    public IDictionary<string, float> Probability { get; set; }
    public bool Prediction { get; set; }
    public float Score { get; set; }
}

