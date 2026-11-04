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
            int hidden, epochs;
            double rate;
            if (!int.TryParse(txtHNodes.Text, out hidden) || hidden < 1 || hidden > ModelConfig.MaxHiddenCount ||
                !int.TryParse(txtEpochs.Text, out epochs) || epochs < 1 || epochs > ModelConfig.MaxEpochs ||
                !double.TryParse(txtLR.Text, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out rate) ||
                !(rate > 0 && rate <= 1))
            {
                MessageBox.Show($"Enter 1–{ModelConfig.MaxHiddenCount} hidden nodes, 1–{ModelConfig.MaxEpochs} epochs, and a learning rate greater than 0 and at most 1 (e.g. 0.10).",
                    "Invalid Training Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadReport report;
                Dataset data = DataPreprocessor.Load(ModelTraining.DatasetPath, out report);
                Dataset train, test;
                DataPreprocessor.StratifiedSplit(data, 0.2, 42, out train, out test);
                MinMaxScaler scaler;
                var network = ModelTraining.Train(train, hidden, epochs, rate, out scaler,
                    (epoch, mse) =>
                    {
                        if (epoch == 1 || epoch % 100 == 0 || epoch == epochs)
                            Console.WriteLine($"[Epoch {epoch,4} / {epochs}] Loss (MSE): {mse:F6}");
                    });
                ModelTraining.CheckDeploymentScaling(scaler);
                Metrics metrics = ModelTraining.Evaluate(network, test, scaler);
                _model.UseTrainedNetwork(network);
                if (_showingResult) ShowResult();

                string results = $"Training complete. The new model is ready for predictions.\n\n" +
                    $"Test accuracy: {metrics.Accuracy:P1}\nPrecision: {metrics.Precision:P1} | Recall: {metrics.Recall:P1}\n\n" +
                    $"{report}\n\nClick Save Weights to keep this model for a future session.";
                MessageBox.Show(results, "Training Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during training:\n" + ex.Message, "Training Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSaveWeights_Click(object sender, EventArgs e)
        {
            if (!_model.IsLoaded)
            {
                MessageBox.Show("Train or load a model before saving weights.");
                return;
            }

            using (var dialog = new SaveFileDialog { Filter = "Text Files|*.txt" })
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                try
                {
                    _model.SaveWeights(dialog.FileName);
                    MessageBox.Show("Weights saved successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Could Not Save Weights", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLoadWeights_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog { Filter = "Text Files|*.txt" })
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                bool loaded = _model.LoadWeights(dialog.FileName);
                if (loaded && _showingResult) ShowResult();
                MessageBox.Show(_model.StatusMessage, "Load Weights", MessageBoxButtons.OK,
                    loaded ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
        }
    }
}
