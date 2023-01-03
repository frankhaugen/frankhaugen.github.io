<Query Kind="Statements">
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
  <Namespace>Microsoft.ML.Trainers.LightGbm</Namespace>
  <RuntimeVersion>7.0</RuntimeVersion>
</Query>















public static class MLContextExtensions
{
    /// <summary>
    /// Retrains model using the pipeline generated as part of the training process. For more information on how to load data, see aka.ms/loaddata.
    /// </summary>
    /// <param name="mlContext"></param>
    /// <param name="trainData"></param>
    /// <returns></returns>
    public static ITransformer RetrainSomeSpecificPipeline(this MLContext mlContext, IDataView trainData)
    {
        var pipeline = mlContext.BuildTemperaturePredictionPipeline();
        var model = pipeline.Fit(trainData);

        return model;
    }

    /// <summary>
    /// build the pipeline that is used from model builder. Use this function to retrain model.
    /// </summary>
    /// <param name="mlContext"></param>
    /// <returns></returns>
    public static IEstimator<ITransformer> BuildTemperaturePredictionPipeline(this MLContext mlContext)
    {
        // Data process configuration with pipeline data transformations
        var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(new[] { new InputOutputColumnPair(@"Region", @"Region"), new InputOutputColumnPair(@"Country", @"Country"), new InputOutputColumnPair(@"City", @"City") }, outputKind: OneHotEncodingEstimator.OutputKind.Indicator)
                                .Append(mlContext.Transforms.ReplaceMissingValues(new[] { new InputOutputColumnPair(@"Month", @"Month"), new InputOutputColumnPair(@"Day", @"Day"), new InputOutputColumnPair(@"Year", @"Year") }))
                                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: @"State", outputColumnName: @"State"))
                                .Append(mlContext.Transforms.Concatenate(@"Features", new[] { @"Region", @"Country", @"City", @"Month", @"Day", @"Year", @"State" }))
                                .Append(mlContext.Regression.Trainers.LightGbm(new LightGbmRegressionTrainer.Options() { NumberOfLeaves = 4, NumberOfIterations = 4, MinimumExampleCountPerLeaf = 20, LearningRate = 1, LabelColumnName = @"AvgTemperature", FeatureColumnName = @"Features", ExampleWeightColumnName = null, Booster = new GradientBooster.Options() { SubsampleFraction = 1, FeatureFraction = 1, L1Regularization = 2E-10, L2Regularization = 1 }, MaximumBinCountPerFeature = 255 }));

        return pipeline;
    }

    public static SplitData SplitTrainTest(this MLContext mlContext, IDataView data, float testFraction = 0.2f)
    {
        var splits = mlContext.Data.TrainTestSplit(data, testFraction: testFraction);
        var trainingData = splits.TrainSet;
        var testData = splits.TestSet;
        return new(trainingData, testData);
    }

    public static void AddLogging(this MLContext mlContext)
    {
        mlContext.Log += (sender, e) =>
        {
            e.RawMessage.Dump();
        };
    }
    public static OneHotEncodingEstimator AddOneHotEncoding(this MLContext mlContext, string inputColumn, string outputColumn)
    {
        var oneHotEncodingEstimator = mlContext.Transforms.Categorical.OneHotEncoding(
            outputColumnName: outputColumn,
            inputColumnName: inputColumn
        );
        return oneHotEncodingEstimator;
    }
}


public record SplitData(IDataView Train, IDataView Test);








