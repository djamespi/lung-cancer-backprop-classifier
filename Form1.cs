using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Backprop;

namespace BackProp
{
    /// <summary>
    /// One-question-at-a-time screening wizard with a hidden Developer Panel.
    /// Added Console logging for background process tracking and startup weight status.
    /// </summary>
    public partial class Form1 : Form
    {
        // ----- look & feel -----
        private static readonly Color AccentColor = Color.FromArgb(0, 120, 212);
        private static readonly Color NeutralColor = Color.FromArgb(238, 241, 245);
        private static readonly Color TextColor = Color.FromArgb(32, 32, 32);
        private const int DefaultAge = 40;
        private const int ClickGuardMs = 300;

        // ----- state -----
        private readonly IList<Question> _questions = ScreeningQuestions.All;
        private readonly double?[] _answers;
        private RiskModel _model = new RiskModel();
        private int _index;
        private bool _showingResult;
        private DateTime _lastAnswerAt = DateTime.MinValue;

        public Form1()
        {
            InitializeComponent();

            if (_questions.Count != ModelConfig.InputCount)
                throw new InvalidOperationException("Number of questions must equal ModelConfig.InputCount.");

            _answers = new double?[_questions.Count];
            progressBar.Maximum = _questions.Count;

            ResetScreening();
            pnlDev.Visible = false; // Hide dev panel by default

            Console.WriteLine("[System] Form1 initialized and ready.");

            // --- Startup Weight Status Indicator ---
            if (_model.IsLoaded)
            {
                Console.WriteLine($"[Model Status] SUCCESS: Default weights '{ModelConfig.WeightsFile}' loaded successfully on startup.");
            }
            else
            {
                Console.WriteLine($"[Model Status] WARNING: {_model.StatusMessage} App is running in Demo Mode.");
            }
            Console.WriteLine("====================================");
        }

        // =====================================================================
        //  Navigation & UI
        // =====================================================================

        private void ResetScreening()
        {
            Array.Clear(_answers, 0, _answers.Length);
            _index = 0;
            ShowQuestion();
        }

        private void ShowQuestion()
        {
            _showingResult = false;
            Question q = _questions[_index];

            tblResult.Visible = false;
            lblQuestion.Visible = true;
            lblHint.Visible = true;

            lblQuestion.Text = q.Text;
            lblHint.Text = q.Hint;
            lblStep.Text = "Question " + (_index + 1) + " of " + _questions.Count;
            progressBar.Value = _index;
            btnBack.Enabled = _index > 0;

            if (q.Type == QuestionType.Age)
            {
                tblChoice.Visible = false;
                tblAge.Visible = true;
                trackAge.Value = _answers[_index].HasValue ? (int)_answers[_index].Value : DefaultAge;
                lblAgeValue.Text = trackAge.Value + " years old";
            }
            else
            {
                tblAge.Visible = false;
                tblChoice.Visible = true;
                if (q.Type == QuestionType.Gender)
                {
                    SetChoice(btnChoiceA, "Male", ModelConfig.MaleValue);
                    SetChoice(btnChoiceB, "Female", ModelConfig.FemaleValue);
                }
                else
                {
                    SetChoice(btnChoiceA, "Yes", 1.0);
                    SetChoice(btnChoiceB, "No", 0.0);
                }
            }
        }

        private void SetChoice(Button button, string caption, double value)
        {
            button.Text = caption;
            button.Tag = value;
            bool selected = _answers[_index].HasValue && _answers[_index].Value == value;
            button.BackColor = selected ? AccentColor : NeutralColor;
            button.ForeColor = selected ? Color.White : TextColor;
        }

        private void Answer(double value)
        {
            if ((DateTime.UtcNow - _lastAnswerAt).TotalMilliseconds < ClickGuardMs) return;
            _lastAnswerAt = DateTime.UtcNow;
            _answers[_index] = value;

            Console.WriteLine($"[Survey] Answered Q{_index + 1}: {value}");

            if (_index < _questions.Count - 1)
            {
                _index++;
                ShowQuestion();
            }
            else
            {
                ShowResult();
            }
        }

        private void ShowResult()
        {
            _showingResult = true;
            Console.WriteLine("[Survey] Survey completed. Calculating prediction...");

            double[] features = new double[_questions.Count];
            for (int i = 0; i < features.Length; i++) features[i] = _answers[i].GetValueOrDefault();

            double score;
            try
            {
                score = _model.Predict(features);
                Console.WriteLine($"[Survey] Prediction calculated. Raw score: {score:F4}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Prediction failed: {ex.Message}");
                MessageBox.Show("Error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetScreening();
                return;
            }

            bool high = score >= ModelConfig.DecisionThreshold;
            lblRiskPercent.Text = (score * 100).ToString("0") + "%";
            prgRisk.Value = Math.Max(0, Math.Min(100, (int)Math.Round(score * 100)));
            lblRiskLevel.Text = high ? "Higher risk indicated" : "Lower risk indicated";
            lblRiskLevel.ForeColor = high ? Color.Firebrick : Color.SeaGreen;

            string note = "Survey-based prediction demo.";
            if (!_model.IsLoaded) note += "\n\nDEMO MODE: " + _model.StatusMessage;
            lblResultNote.Text = note;

            lblQuestion.Visible = false;
            lblHint.Visible = false;
            tblChoice.Visible = false;
            tblAge.Visible = false;
            tblResult.Visible = true;
            lblStep.Text = "Your result";
            progressBar.Value = progressBar.Maximum;
            btnBack.Enabled = true;
        }

        private void btnChoice_Click(object sender, EventArgs e) => Answer((double)((Button)sender).Tag);
        private void trackAge_ValueChanged(object sender, EventArgs e) => lblAgeValue.Text = trackAge.Value + " years old";
        private void btnAgeNext_Click(object sender, EventArgs e) => Answer(trackAge.Value);
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (_showingResult || _index > 0)
            {
                if (!_showingResult) _index--;
                ShowQuestion();
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[System] Survey reset by user.");
            ResetScreening();
        }
        private void btnContinue_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[System] Continuing to new survey.");
            ResetScreening();
        }

        // =====================================================================
        //  Developer & Training Panel
        // =====================================================================

        private void btnDevToggle_Click(object sender, EventArgs e)
        {
            pnlDev.Visible = !pnlDev.Visible;
            Console.WriteLine($"[System] Developer panel visibility toggled to {pnlDev.Visible}");
        }

        private void btnDevTrain_Click(object sender, EventArgs e)
        {
            Console.WriteLine("\n====================================");
            Console.WriteLine("[Training] Initialization started...");

            int hNodes = int.Parse(txtHNodes.Text);
            int epochs = int.Parse(txtEpochs.Text);
            double learningRate = double.Parse(txtLR.Text);

            // 1. Re-initialize network in the model with new hidden nodes
            _model.ReinitializeNetwork(ModelConfig.InputCount, hNodes, ModelConfig.OutputCount);

            // Set the new public learning rates
            _model.Network.LRPIN = learningRate;
            _model.Network.LRPOUT = learningRate;

            Console.WriteLine($"[Training] Network structured with {ModelConfig.InputCount} In, {hNodes} Hidden, {ModelConfig.OutputCount} Out.");
            Console.WriteLine($"[Training] Hyperparameters - LR: {learningRate}, Epochs: {epochs}");

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "survey lung cancer.csv");
            if (!File.Exists(path))
            {
                Console.WriteLine($"[Training Error] CSV not found at {path}");
                MessageBox.Show($"Could not find the dataset at:\n{path}\n\nPlease ensure the CSV's 'Copy to Output Directory' is set to 'Copy if newer'.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // 2. Load Data using groupmate's preprocessor
                Console.WriteLine("[Training] Parsing CSV data...");
                LoadReport report;
                Dataset data = DataPreprocessor.Load(path, out report);
                Console.WriteLine($"[Training] Dataset loaded. Final valid rows: {report.FinalRows}");

                // 3. Stratified Split
                Console.WriteLine("[Training] Performing stratified 80/20 train-test split...");
                Dataset train, test;
                DataPreprocessor.StratifiedSplit(data, 0.2, 42, out train, out test);

                // 4. Oversample minority class to balance training data
                Console.WriteLine("[Training] Oversampling minority class to balance dataset...");
                train = DataPreprocessor.OversampleMinority(train, 7);

                // 5. MinMax scale (fit on train only)
                Console.WriteLine("[Training] Applying MinMaxScaler to normalize features...");
                MinMaxScaler scaler = new MinMaxScaler();
                scaler.Fit(train.X);
                double[][] trX = scaler.Transform(train.X);
                double[][] teX = scaler.Transform(test.X);

                // 6. Training Loop with MSE Loss Logging
                Console.WriteLine("[Training] Entering epoch loop. Tracking Mean Squared Error (MSE)...");
                Random rng = new Random(1);

                for (int epoch = 1; epoch <= epochs; epoch++)
                {
                    double epochSquaredError = 0.0;
                    int[] order = DataPreprocessor.ShuffledIndices(trX.Length, rng);

                    foreach (int i in order)
                    {
                        for (int f = 0; f < trX[i].Length; f++)
                            _model.Network.setInputs(f, trX[i][f]);

                        double target = train.Y[i][0];
                        _model.Network.setDesiredOutput(0, target);
                        _model.Network.learn();

                        double output = _model.Network.getOutputData(0); // Note spelling in your library
                        double error = target - output;
                        epochSquaredError += error * error;
                    }

                    double epochMse = epochSquaredError / trX.Length;

                    if (epoch == 1 || epoch % 100 == 0 || epoch == epochs)
                    {
                        Console.WriteLine($"[Epoch {epoch,4} / {epochs}] Loss (MSE): {epochMse:F6}");
                    }
                }

                // 7. Evaluation on Test Set
                Console.WriteLine("[Training] Running test set evaluation...");
                int tp = 0, tn = 0, fp = 0, fn = 0;
                for (int i = 0; i < teX.Length; i++)
                {
                    for (int f = 0; f < teX[i].Length; f++)
                        _model.Network.setInputs(f, teX[i][f]);

                    _model.Network.run();
                    double predVal = _model.Network.getOutputData(0);

                    bool pred = predVal >= ModelConfig.DecisionThreshold;
                    bool actual = test.Y[i][0] >= ModelConfig.DecisionThreshold;

                    if (pred && actual) tp++;
                    else if (!pred && !actual) tn++;
                    else if (pred && !actual) fp++;
                    else fn++;
                }

                double acc = (double)(tp + tn) / teX.Length;
                double prec = tp + fp == 0 ? 0 : (double)tp / (tp + fp);
                double rec = tp + fn == 0 ? 0 : (double)tp / (tp + fn);

                Cursor.Current = Cursors.Default;

                Console.WriteLine("[Training] Sequence Complete!");
                Console.WriteLine($"[Training Metrics] Accuracy: {acc:P1} | Precision: {prec:P1} | Recall: {rec:P1}");
                Console.WriteLine("====================================\n");

                string results = $"Training Complete!\n\n" +
                                 $"Epochs: {epochs} | LR: {learningRate}\n" +
                                 $"Test Accuracy: {acc:P1}\n" +
                                 $"Precision: {prec:P1} | Recall: {rec:P1}\n\n" +
                                 $"--- Data Prep Report ---\n{report}\n\n" +
                                 $"Age Min: {scaler.Min[ModelConfig.AgeIndex]}, Age Max: {scaler.Max[ModelConfig.AgeIndex]}\n\n" +
                                 "Make sure to click 'Save Weights' to keep this tuning!";

                MessageBox.Show(results, "Training Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                Console.WriteLine($"[Training Error] {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show("Error during training:\n" + ex.Message, "Training Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveWeights_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "Text Files|*.txt" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    _model.SaveWeights(sfd.FileName);
                    Console.WriteLine($"[System] Weights successfully saved to {sfd.FileName}");
                    MessageBox.Show("Weights saved successfully.");
                }
                else
                {
                    Console.WriteLine("[System] Save weights operation cancelled.");
                }
            }
        }

        private void btnLoadWeights_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Text Files|*.txt" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _model.LoadWeights(ofd.FileName);
                    Console.WriteLine($"[System] Weights successfully loaded from {ofd.FileName}");
                    MessageBox.Show("Weights loaded successfully.");
                }
                else
                {
                    Console.WriteLine("[System] Load weights operation cancelled.");
                }
            }
        }
    }
}