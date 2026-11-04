using System;
using System.Globalization;
using System.IO;
using Backprop;

namespace BackProp.HyperparameterTuning
{
    internal static class AutoTuner
    {
        public static void RunGridSearch(string outputDirectory)
        {
            Directory.CreateDirectory(outputDirectory);
            LoadReport report;
            Dataset data = DataPreprocessor.Load(ModelTraining.DatasetPath, out report);
            Dataset development, test, tuningTrain, validation;
            DataPreprocessor.StratifiedSplit(data, 0.2, 42, out development, out test);
            DataPreprocessor.StratifiedSplit(development, 0.2, 43, out tuningTrain, out validation);

            int bestHidden = 0, bestEpochs = 0;
            double bestRate = 0, bestScore = -1, bestLoss = double.PositiveInfinity;
            using (var csv = new StreamWriter(Path.Combine(outputDirectory, "tuning_results.csv")))
            {
                csv.WriteLine("LearningRate,HiddenNodes,Epochs,ValidationAccuracy,Precision,Recall,Specificity,BalancedAccuracy,FinalTrainingMSE");
                foreach (double rate in new[] { 0.05, 0.10, 0.20 })
                foreach (int hidden in new[] { 4, 8, 15 })
                foreach (int epochs in new[] { 500, 1000 })
                {
                    MinMaxScaler scaler;
                    double loss = 0;
                    NeuralNet net = ModelTraining.Train(tuningTrain, hidden, epochs, rate, out scaler,
                        (epoch, mse) => loss = mse);
                    Metrics metrics = ModelTraining.Evaluate(net, validation, scaler);
                    csv.WriteLine(Invariant($"{rate},{hidden},{epochs},{metrics.Accuracy:F6},{metrics.Precision:F6},{metrics.Recall:F6},{metrics.Specificity:F6},{metrics.BalancedAccuracy:F6},{loss:F6}"));
                    if (metrics.BalancedAccuracy > bestScore ||
                        (metrics.BalancedAccuracy == bestScore && loss < bestLoss))
                    {
                        bestScore = metrics.BalancedAccuracy;
                        bestLoss = loss;
                        bestHidden = hidden;
                        bestEpochs = epochs;
                        bestRate = rate;
                    }
                }
            }

            // Refit the selected model on all development rows, then evaluate the held-out test rows.
            string weightsPath = Path.Combine(outputDirectory, ModelConfig.WeightsFile);
            using (var log = new StreamWriter(Path.Combine(outputDirectory, "training_log.txt")))
            {
                log.WriteLine(report);
                log.WriteLine($"Tuning train: {tuningTrain.Count} | Validation: {validation.Count} | Final train: {development.Count} | Test: {test.Count}");
                log.WriteLine(Invariant($"Selected {bestHidden} hidden nodes, {bestEpochs} epochs, learning rate {bestRate:F2}. Validation balanced accuracy: {bestScore:P2}"));
                MinMaxScaler scaler;
                NeuralNet final = ModelTraining.Train(development, bestHidden, bestEpochs, bestRate, out scaler,
                    (epoch, mse) =>
                    {
                        if (epoch == 1 || epoch % 100 == 0 || epoch == bestEpochs)
                            log.WriteLine(Invariant($"Epoch {epoch,4} / {bestEpochs}: MSE {mse:F6}"));
                    });
                ModelTraining.CheckDeploymentScaling(scaler);
                final.saveWeights(weightsPath);
                var reloaded = new NeuralNet(ModelConfig.InputCount, bestHidden, ModelConfig.OutputCount);
                reloaded.loadWeights(weightsPath);
                Metrics result = ModelTraining.Evaluate(reloaded, test, scaler);
                log.WriteLine($"Test confusion matrix: TP={result.TruePositive}, TN={result.TrueNegative}, FP={result.FalsePositive}, FN={result.FalseNegative}");
                log.WriteLine(Invariant($"Test accuracy: {result.Accuracy:P2} | Precision: {result.Precision:P2} | Recall: {result.Recall:P2} | Specificity: {result.Specificity:P2} | Balanced accuracy: {result.BalancedAccuracy:P2}"));
            }
        }

        private static string Invariant(FormattableString value) => value.ToString(CultureInfo.InvariantCulture);
    }
}
