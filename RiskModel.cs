using System;
using System.IO;
using System.Linq;
using Backprop;

namespace BackProp
{
    public sealed class RiskModel
    {
        private NeuralNet _net;

        public bool IsLoaded { get; private set; }
        public string StatusMessage { get; private set; } = "";

        // Give the groupmate direct access to the network for their training loop
        public NeuralNet Network => _net;

        public RiskModel()
        {
            _net = new NeuralNet(ModelConfig.InputCount, ModelConfig.HiddenCount, ModelConfig.OutputCount);
            TryLoadWeights(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ModelConfig.WeightsFile));
        }

        public void ReinitializeNetwork(int inputs, int hidden, int output)
        {
            _net = new NeuralNet(inputs, hidden, output);
            IsLoaded = false;
            StatusMessage = "Network reinitialized for training.";
        }

        public void SaveWeights(string path)
        {
            _net.saveWeights(path);
        }

        public void LoadWeights(string path)
        {
            TryLoadWeights(path);
        }

        private void TryLoadWeights(string path)
        {
            if (!File.Exists(path))
            {
                StatusMessage = "Weights file not found at: " + path;
                return;
            }

            try
            {
                _net.loadWeights(path);
                IsLoaded = true;
                StatusMessage = "Weights loaded successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = "Could not read weights: " + ex.Message;
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