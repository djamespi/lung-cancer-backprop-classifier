using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BackProp
{
    /// <summary>
    /// One-question-at-a-time screening wizard with a hidden Developer Panel.
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
            double[] features = new double[_questions.Count];
            for (int i = 0; i < features.Length; i++) features[i] = _answers[i].GetValueOrDefault();

            double score;
            try { score = _model.Predict(features); }
            catch (Exception ex)
            {
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
        private void btnReset_Click(object sender, EventArgs e) => ResetScreening();
        private void btnContinue_Click(object sender, EventArgs e) => ResetScreening();

        // =====================================================================
        //  Developer & Training Panel
        // =====================================================================

        private void btnDevToggle_Click(object sender, EventArgs e)
        {
            pnlDev.Visible = !pnlDev.Visible;
        }

        private void btnDevTrain_Click(object sender, EventArgs e)
        {
            int hNodes = int.Parse(txtHNodes.Text);
            int epochs = int.Parse(txtEpochs.Text);
            double learningRate = double.Parse(txtLR.Text); // Note: Requires modification of const in BackNeural 1.cs

            // Re-initialize network in the model with new hidden nodes
            _model.ReinitializeNetwork(ModelConfig.InputCount, hNodes, ModelConfig.OutputCount);

            // TODO for Groupmate: Insert CSV parsing and training loop here using _model.Network
            MessageBox.Show($"Network re-initialized with {hNodes} hidden nodes.\n\nGroupmate: Insert CSV parsing and a loop of {epochs} epochs here.\nModify BackNeural 1.cs to apply Learning Rate: {learningRate}.", "Training Framework Ready");
        }

        private void btnSaveWeights_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "Text Files|*.txt" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    _model.SaveWeights(sfd.FileName);
                    MessageBox.Show("Weights saved successfully.");
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
                    MessageBox.Show("Weights loaded successfully.");
                }
            }
        }
    }
}