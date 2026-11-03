using System;
using System.IO;
using Backprop;

namespace BackProp.HyperparameterTuning
{
    public static class AutoTuner
    {
        public static void RunGridSearch()
        {
            int[] hiddenNodesMatrix = { 4, 8, 15 };
            double[] learningRatesMatrix = { 0.05, 0.1, 0.2 };
            int[] epochsMatrix = { 500, 1000 };

            string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "survey lung cancer.csv");
            if (!File.Exists(dataPath))
            {
                Console.WriteLine($"[Error] CSV not found at {dataPath}");
                return;
            }

            // Ensure the directory exists in the build output folder
            string outDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HyperparameterTuning");
            Directory.CreateDirectory(outDir);
            string exportPath = Path.Combine(outDir, "tuning_results.csv");

            Console.WriteLine("\n[Auto-Tune] Starting Hyperparameter Grid Search...");
            
            LoadReport report;
            Dataset data = DataPreprocessor.Load(dataPath, out report);
            Dataset train, test;
            DataPreprocessor.StratifiedSplit(data, 0.2, 42, out train, out test);
            train = DataPreprocessor.OversampleMinority(train, 7);

            MinMaxScaler scaler = new MinMaxScaler();
            scaler.Fit(train.X);
            double[][] trX = scaler.Transform(train.X);
            double[][] teX = scaler.Transform(test.X);

            Random rng = new Random(1);

            using (StreamWriter sw = new StreamWriter(exportPath))
            {
                sw.WriteLine("LearningRate,HiddenNodes,Epochs,Accuracy,Precision,Recall,FinalMSE");

                foreach (double lr in learningRatesMatrix)
                {
                    foreach (int hn in hiddenNodesMatrix)
                    {
                        foreach (int ep in epochsMatrix)
                        {
                            Console.WriteLine($"Testing LR={lr}, HNodes={hn}, Epochs={ep}...");
                            
                            NeuralNet net = new NeuralNet(ModelConfig.InputCount, hn, ModelConfig.OutputCount);
                            net.LRPIN = lr;
                            net.LRPOUT = lr;

                            double finalMse = 0.0;
                            
                            for (int epoch = 1; epoch <= ep; epoch++)
                            {
                                double totalLoss = 0.0;
                                int[] order = DataPreprocessor.ShuffledIndices(trX.Length, rng);
                                foreach (int i in order)
                                {
                                    for (int f = 0; f < trX[i].Length; f++) net.setInputs(f, trX[i][f]);

                                    double expected = train.Y[i][0];
                                    net.setDesiredOutput(0, expected);
                                    net.learn();

                                    double actual = net.getOutputData(0);
                                    totalLoss += Math.Pow(expected - actual, 2);
                                }
                                if (epoch == ep) finalMse = totalLoss / trX.Length;
                            }

                            int tp = 0, tn = 0, fp = 0, fn = 0;
                            for (int i = 0; i < teX.Length; i++)
                            {
                                for (int f = 0; f < teX[i].Length; f++) net.setInputs(f, teX[i][f]);

                                net.run();
                                bool pred = net.getOutputData(0) >= ModelConfig.DecisionThreshold;
                                bool actual = test.Y[i][0] >= ModelConfig.DecisionThreshold;

                                if (pred && actual) tp++;
                                else if (!pred && !actual) tn++;
                                else if (pred && !actual) fp++;
                                else fn++;
                            }

                            double acc = (double)(tp + tn) / teX.Length;
                            double prec = tp + fp == 0 ? 0 : (double)tp / (tp + fp);
                            double rec = tp + fn == 0 ? 0 : (double)tp / (tp + fn);

                            sw.WriteLine($"{lr},{hn},{ep},{acc},{prec},{rec},{finalMse}");
                        }
                    }
                }
            }
            Console.WriteLine($"\n[Auto-Tune] Complete! Results saved to:\n{exportPath}");
        }
    }
}