<Query Kind="Program">
  <NuGetReference>CsvHelper</NuGetReference>
  <NuGetReference>Microsoft.ML</NuGetReference>
  <NuGetReference>Microsoft.ML.AutoML</NuGetReference>
  <Namespace>CsvHelper</Namespace>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>Microsoft.ML</Namespace>
  <Namespace>Microsoft.ML.Data</Namespace>
  <Namespace>Microsoft.ML.Trainers</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.ML.AutoML</Namespace>
</Query>

void Main()
{
    var mlContext = new MLContext();
    
    // Create a new AutoML experiment
    var experiment = mlContext.Auto().CreateExperiment();
    //var experiment = new Experiment<RunDetail, MulticlassClassificationMetrics>(
    //    context: mlContext,
    //    columnInference: true,
    //    cancellationToken: new CancellationToken(),
    //    progressHandler: new Progress<RunDetail<MulticlassClassificationMetrics>>()
    //);

    // Load the training data from a file
    IDataView trainingData = mlContext.Data.LoadFromTextFile<ModelInput>(
        path: "data.csv",
        hasHeader: true,
        separatorChar: ','
    );

    // Run the AutoML experiment to train a model
    var model = experiment.Train<ModelInput, ModelOutput>(
        trainingData: trainingData,
        labelColumnName: "Label"
    );

    // Save the trained model to a file
    mlContext.Model.Save(
        model: model,
        inputSchema: trainingData.Schema,
        filePath: "model.zip"
    );

}

// You can define other methods, fields, classes and namespaces here