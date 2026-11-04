using System;
using System.IO;
using Backprop;

namespace BackProp
{
    internal static class ModelTraining
    {
        public static string DatasetPath => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Data", "survey lung cancer.csv");

        public static NeuralNet Train(Dataset data, int hidden, int epochs, double rate,
            out MinMaxScaler scaler, Action<int, double> reportLoss = null)
        {
            scaler = new MinMaxScaler();
            scaler.Fit(data.X);
            Dataset train = DataPreprocessor.OversampleMinority(data, 7);
            double[][] inputs = scaler.Transform(train.X);
            var net = new NeuralNet(ModelConfig.InputCount, hidden, ModelConfig.OutputCount, new Random(17))
            {
                LRPIN = rate,
                LRPOUT = rate
            };
            var shuffle = new Random(1);

            for (int epoch = 1; epoch <= epochs; epoch++)
            {
                double totalLoss = 0;
                foreach (int row in DataPreprocessor.ShuffledIndices(train.Count, shuffle))
                {
                    SetInputs(net, inputs[row]);
                    double target = train.Y[row][0];
                    net.setDesiredOutput(0, target);
                    net.learn();
                    double error = target - net.getOutputData(0);
                    totalLoss += error * error;
                }
                reportLoss?.Invoke(epoch, totalLoss / train.Count);
            }
            return net;
        }

        public static Metrics Evaluate(NeuralNet net, Dataset data, MinMaxScaler scaler)
        {
            var metrics = new Metrics();
            double[][] inputs = scaler.Transform(data.X);
            for (int i = 0; i < data.Count; i++)
            {
                SetInputs(net, inputs[i]);
                net.run();
                bool positive = net.getOutputData(0) >= ModelConfig.DecisionThreshold;
                bool actual = data.Y[i][0] == 1;
                if (positive && actual) metrics.TruePositive++;
                else if (positive) metrics.FalsePositive++;
                else if (actual) metrics.FalseNegative++;
                else metrics.TrueNegative++;
            }
            return metrics;
        }

        public static void CheckDeploymentScaling(MinMaxScaler scaler)
        {
            // Saved survey weights use the age range in ModelConfig, including legacy files.
            if (scaler.Min[ModelConfig.AgeIndex] != ModelConfig.AgeMin ||
                scaler.Max[ModelConfig.AgeIndex] != ModelConfig.AgeMax)
                throw new InvalidDataException("The training age range differs from the questionnaire's age scaling.");
        }

        private static void SetInputs(NeuralNet net, double[] inputs)
        {
            for (int i = 0; i < inputs.Length; i++) net.setInputs(i, inputs[i]);
        }
    }

    internal sealed class Metrics
    {
        public int TruePositive, TrueNegative, FalsePositive, FalseNegative;

        public double Accuracy => Ratio(TruePositive + TrueNegative,
            TruePositive + TrueNegative + FalsePositive + FalseNegative);
        public double Precision => Ratio(TruePositive, TruePositive + FalsePositive);
        public double Recall => Ratio(TruePositive, TruePositive + FalseNegative);
        public double Specificity => Ratio(TrueNegative, TrueNegative + FalsePositive);
        public double BalancedAccuracy => (Recall + Specificity) / 2;

        private static double Ratio(int numerator, int denominator) =>
            denominator == 0 ? 0 : (double)numerator / denominator;
    }
}
