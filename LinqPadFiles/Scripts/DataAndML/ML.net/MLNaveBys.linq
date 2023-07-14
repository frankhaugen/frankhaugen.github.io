<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
</Query>


static void Main(string[] args)
{
    // Set up the training data
    string[][] trainingData = new string[][]
    {
                // Positive examples
                new string[] { "sun", "hot", "high", "weak" },
                new string[] { "sun", "hot", "high", "strong" },
                new string[] { "overcast", "hot", "high", "weak" },
                new string[] { "rain", "mild", "high", "weak" },
                new string[] { "rain", "cool", "normal", "weak" },
                new string[] { "rain", "cool", "normal", "strong" },
                new string[] { "overcast", "cool", "normal", "strong" },

                // Negative examples
                new string[] { "sun", "mild", "high", "weak" },
                new string[] { "sun", "cool", "normal", "weak" },
                new string[] { "rain", "mild", "normal", "weak" },
                new string[] { "sun", "mild", "normal", "strong" },
                new string[] { "overcast", "mild", "high", "strong" },
                new string[] { "overcast", "hot", "normal", "weak" },
                new string[] { "rain", "mild", "high", "strong" }
    };

    string[] labels = new string[] { "positive", "negative" };

    // Set up the test data
    string[][] testData = new string[][]
    {
                new string[] { "overcast", "hot", "high", "weak" },
                new string[] { "rain", "cool", "normal", "strong" }
    };

    // Create a new naive Bayes classifier
    NaiveBayesClassifier classifier = new NaiveBayesClassifier(trainingData, labels);

    // Test the classifier on the test data
    for (int i = 0; i < testData.Length; i++)
    {
        string label = classifier.Classify(testData[i]);
        Console.WriteLine("Example {0} was classified as {1}", i + 1, label);
    }
}


class NaiveBayesClassifier
{
    // The number of classes
    private int numClasses;

    // The number of features
    private int numFeatures;

    // The log of the class priors
    private double[] logPriors;

    // The log of the class likelihoods
    private double[][] logLikelihoods;

    // the labels
    private string[] labels;

    public NaiveBayesClassifier(string[][] trainingData, string[] labelsInput)
    {
        labels = labelsInput;

        // Set the number of classes and features
        numClasses = labels.Distinct().Count();
        numFeatures = trainingData[0].Length;

        // Initialize the log priors and log likelihoods
        logPriors = new double[numClasses];
        logLikelihoods = new double[numClasses][];
        for (int i = 0; i < numClasses; i++)
        {
            logLikelihoods[i] = new double[numFeatures];
        }

        // Calculate the log priors and log likelihoods
        for (int i = 0; i < trainingData.Length; i++)
        {
            string label = labels[i];
            int classIndex = Array.IndexOf(labels, label);

            logPriors[classIndex]++;

            for (int j = 0; j < numFeatures; j++)
            {
                logLikelihoods[classIndex][j]++;
            }
        }
        // Normalize the log priors and log likelihoods
        for (int i = 0; i < numClasses; i++)
        {
            logPriors[i] = Math.Log(logPriors[i] / trainingData.Length);

            for (int j = 0; j < numFeatures; j++)
            {
                logLikelihoods[i][j] = Math.Log(logLikelihoods[i][j] / logPriors[i]);
            }
        }
    }

    public string Classify(string[] features)
    {
        // Initialize the scores for each class
        double[] scores = new double[numClasses];
        for (int i = 0; i < numClasses; i++)
        {
            scores[i] = logPriors[i];
        }

        // Add the log likelihoods for each feature
        for (int i = 0; i < features.Length; i++)
        {
            for (int j = 0; j < numClasses; j++)
            {
                int index = Array.IndexOf(logLikelihoods[j], features[i]);
                if (index >= 0)
                {
                    scores[j] += logLikelihoods[j][index];
                }
            }
        }

        // Find the class with the highest score
        int maxIndex = -1;
        double maxScore = double.NegativeInfinity;
        for (int i = 0; i < numClasses; i++)
        {
            if (scores[i] > maxScore)
            {
                maxIndex = i;
                maxScore = scores[i];
            }
        }

        // Return the class with the highest score
        return labels[maxIndex];
    }
}