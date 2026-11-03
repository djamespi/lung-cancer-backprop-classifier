using System;

namespace BackProp
{
    /// <summary>
    /// Single place for every number the UI and the trained model must agree on.
    /// Sync these with whoever does the data prep / training BEFORE the demo.
    /// </summary>
    public static class ModelConfig
    {
        // ---- Network shape (must match the network that produced the weights file) ----
        public const int InputCount = 15;   // 15 survey features (see ScreeningQuestions)
        public const int HiddenCount = 8;   // TODO: set to the hidden size chosen after tuning
        public const int OutputCount = 1;   // 1 output neuron = risk score between 0 and 1

        // ---- Saved weights (written by NeuralNet.saveWeights after training) ----
        // Put the file in the project, set "Copy to Output Directory" = "Copy if newer".
        public const string WeightsFile = "lung_cancer_weights.txt";

        // ---- Feature encoding (must match the training CSV after cleaning) ----
        public const double MaleValue = 0.0;     // GENDER M -> 0
        public const double FemaleValue = 1.0;   // GENDER F -> 1
        // Yes/No questions: Yes = 1, No = 0 (dataset's 2/1 becomes 1/0)

        // ---- Age scaling (use the TRAINING-set min and max from the data prep) ----
        public const int AgeIndex = 1;           // position of AGE in the feature vector
        public const double AgeMin = 21;         // TODO: confirm with groupmate's scaler
        public const double AgeMax = 87;         // TODO: confirm with groupmate's scaler

        // ---- Decision threshold used on the result screen ----
        public const double DecisionThreshold = 0.5;   // TODO: use the threshold chosen in evaluation
    }
}
