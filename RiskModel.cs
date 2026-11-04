using System;
using System.IO;
using Backprop;

namespace BackProp
{
    public sealed class RiskModel
    {
        private NeuralNet _net;

        public bool IsLoaded { get; private set; }
        public string StatusMessage { get; private set; } = "";

        public RiskModel()
        {
            LoadWeights(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ModelConfig.WeightsFile));
        }

        public NeuralNet Network => _net;

        public void ReinitializeNetwork(int inputs, int hidden, int output)
        {
            _net = new NeuralNet(inputs, hidden, output);
            IsLoaded = false;
            StatusMessage = "Network reinitialized for training.";
        }

        public void SaveWeights(string path)
        {
            if (!IsLoaded) throw new InvalidOperationException("Train or load a model before saving weights.");
            string temporaryPath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(path)), Path.GetRandomFileName());
            try
            {
                _net.saveWeights(temporaryPath);
                if (File.Exists(path)) File.Replace(temporaryPath, path, null);
                else File.Move(temporaryPath, path);
            }
            finally
            {
                File.Delete(temporaryPath);
            }
        }

        public bool LoadWeights(string path)
        {
            if (!File.Exists(path))
            {
                StatusMessage = "Weights file not found at: " + path;
                return false;
            }

            try
            {
                double[] values = NeuralNet.ReadWeightValues(path);
                int weightsPerHidden = ModelConfig.InputCount + ModelConfig.OutputCount + 1;
                int hiddenWeights = values.Length - ModelConfig.OutputCount;
                int hidden = hiddenWeights / weightsPerHidden;
                if (hidden < 1 || hidden > ModelConfig.MaxHiddenCount || hiddenWeights % weightsPerHidden != 0)
                    throw new InvalidDataException("The weights file has an invalid number of values for this survey model.");

                var candidate = new NeuralNet(ModelConfig.InputCount, hidden, ModelConfig.OutputCount);
                candidate.LoadWeightValues(values);
                _net = candidate;
                IsLoaded = true;
                StatusMessage = "Weights loaded successfully.";
                return true;
            }
            catch (Exception ex) when (ex is IOException || ex is InvalidDataException || ex is UnauthorizedAccessException)
            {
                StatusMessage = "Could not read weights: " + ex.Message;
                return false;
            }
        }

        public double Predict(double[] raw)
        {
            if (raw == null || raw.Length != ModelConfig.InputCount)
                throw new ArgumentException("Expected " + ModelConfig.InputCount + " answers.");

            if (!IsLoaded) return DemoScore(raw);

            double[] x = Normalize(raw);
            for (int k = 0; k < x.Length; k++) _net.setInputs(k, x[k]);

            _net.run();
            return _net.getOutputData(0);
        }

        private static double[] Normalize(double[] raw)
        {
            double[] x = (double[])raw.Clone();
            double scaled = (x[ModelConfig.AgeIndex] - ModelConfig.AgeMin) / (ModelConfig.AgeMax - ModelConfig.AgeMin);
            x[ModelConfig.AgeIndex] = Math.Max(0.0, Math.Min(1.0, scaled));
            return x;
        }

        private static double DemoScore(double[] raw)
        {
            double yes = 0;
            int count = 0;
            for (int k = 0; k < raw.Length; k++)
            {
                if (k == 0 || k == ModelConfig.AgeIndex) continue;
                yes += raw[k];
                count++;
            }
            return yes / count;
        }
    }
}