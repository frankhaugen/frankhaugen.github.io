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
                new TrainingData() { Label = true, Feature1 = 0.5f, Feature2 = 0.1f },
                new TrainingData() { Label = false, Feature1 = 0.3f, Feature2 = 0.9f },
                new TrainingData() { Label = true, Feature1 = 0.8f, Feature2 = 0.3f },
                new TrainingData() { Label = false, Feature1 = 0.2f, Feature2 = 0.7f },
                new TrainingData() { Label = true, Feature1 = 0.6f, Feature2 = 0.5f },
                new TrainingData() { Label = false, Feature1 = 0.1f, Feature2 = 0.2f }
    };

    // Load the training data into an IDataView
    MLContext mlContext = new MLContext();
    IDataView dataView = mlContext.Data.LoadFromEnumerable(trainingData);

    // Define the pipeline
    var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label")
     .Append(mlContext.Transforms.Concatenate("Features", "Feature1", "Feature2"))
        .Append(mlContext.MulticlassClassification.Trainers.NaiveBayes("Label", "Features"))
        .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

    // Train the model
    ITransformer model = pipeline.Fit(dataView);

    // Create a test data instance and set the feature values
    TrainingData testData = new TrainingData() { Feature1 = 0.7f, Feature2 = 0.8f };

    // Make a prediction
    var predictionEngine = mlContext.Model.CreatePredictionEngine<TrainingData, Prediction>(model);
    var prediction = predictionEngine.Predict(testData);

    // Print the prediction
    Console.WriteLine("Prediction: {0}", prediction.PredictedLabel);
}

public class TrainingData
{
    public bool Label { get; set; }
    public float Feature1 { get; set; }
    public float Feature2 { get; set; }
}

public class Prediction
{
    public bool PredictedLabel { get; set; }
}