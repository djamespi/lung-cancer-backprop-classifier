namespace BackProp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnDevToggle = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblStep = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.pnlDev = new System.Windows.Forms.Panel();
            this.txtLR = new System.Windows.Forms.TextBox();
            this.lblLR = new System.Windows.Forms.Label();
            this.btnLoadWeights = new System.Windows.Forms.Button();
            this.btnSaveWeights = new System.Windows.Forms.Button();
            this.btnDevTrain = new System.Windows.Forms.Button();
            this.txtEpochs = new System.Windows.Forms.TextBox();
            this.lblEpochs = new System.Windows.Forms.Label();
            this.txtHNodes = new System.Windows.Forms.TextBox();
            this.lblHNodes = new System.Windows.Forms.Label();
            this.lblDevTitle = new System.Windows.Forms.Label();
            this.lblFooterNote = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.tblResult = new System.Windows.Forms.TableLayoutPanel();
            this.lblResultHeading = new System.Windows.Forms.Label();
            this.lblRiskPercent = new System.Windows.Forms.Label();
            this.prgRisk = new System.Windows.Forms.ProgressBar();
            this.lblRiskLevel = new System.Windows.Forms.Label();
            this.lblResultNote = new System.Windows.Forms.Label();
            this.btnContinue = new System.Windows.Forms.Button();
            this.tblAge = new System.Windows.Forms.TableLayoutPanel();
            this.lblAgeValue = new System.Windows.Forms.Label();
            this.trackAge = new System.Windows.Forms.TrackBar();
            this.btnAgeNext = new System.Windows.Forms.Button();
            this.tblChoice = new System.Windows.Forms.TableLayoutPanel();
            this.btnChoiceA = new System.Windows.Forms.Button();
            this.btnChoiceB = new System.Windows.Forms.Button();
            this.lblHint = new System.Windows.Forms.Label();
            this.lblQuestion = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlDev.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.tblResult.SuspendLayout();
            this.tblAge.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackAge)).BeginInit();
            this.tblChoice.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.pnlHeader.Controls.Add(this.btnDevToggle);
            this.pnlHeader.Controls.Add(this.progressBar);
            this.pnlHeader.Controls.Add(this.btnReset);
            this.pnlHeader.Controls.Add(this.lblStep);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(860, 96);
            this.pnlHeader.TabIndex = 2;
            // 
            // btnDevToggle
            // 
            this.btnDevToggle.BackColor = System.Drawing.Color.Transparent;
            this.btnDevToggle.FlatAppearance.BorderSize = 0;
            this.btnDevToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDevToggle.Font = new System.Drawing.Font("Segoe UI", 6F);
            this.btnDevToggle.Location = new System.Drawing.Point(3, 3);
            this.btnDevToggle.Name = "btnDevToggle";
            this.btnDevToggle.Size = new System.Drawing.Size(20, 20);
            this.btnDevToggle.TabIndex = 4;
            this.btnDevToggle.Text = "⚙";
            this.btnDevToggle.UseVisualStyleBackColor = false;
            this.btnDevToggle.Click += new System.EventHandler(this.btnDevToggle_Click);
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(0, 88);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(860, 8);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 3;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.BackColor = System.Drawing.Color.White;
            this.btnReset.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnReset.Location = new System.Drawing.Point(716, 22);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(120, 40);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblStep
            // 
            this.lblStep.AutoSize = true;
            this.lblStep.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblStep.ForeColor = System.Drawing.Color.DimGray;
            this.lblStep.Location = new System.Drawing.Point(26, 54);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(113, 20);
            this.lblStep.TabIndex = 1;
            this.lblStep.Text = "Question 1 of 15";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(24, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(296, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Lung Cancer Risk Screening";
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.pnlDev);
            this.pnlFooter.Controls.Add(this.lblFooterNote);
            this.pnlFooter.Controls.Add(this.btnBack);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 524);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(860, 76);
            this.pnlFooter.TabIndex = 1;
            // 
            // pnlDev
            // 
            this.pnlDev.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlDev.Controls.Add(this.txtLR);
            this.pnlDev.Controls.Add(this.lblLR);
            this.pnlDev.Controls.Add(this.btnLoadWeights);
            this.pnlDev.Controls.Add(this.btnSaveWeights);
            this.pnlDev.Controls.Add(this.btnDevTrain);
            this.pnlDev.Controls.Add(this.txtEpochs);
            this.pnlDev.Controls.Add(this.lblEpochs);
            this.pnlDev.Controls.Add(this.txtHNodes);
            this.pnlDev.Controls.Add(this.lblHNodes);
            this.pnlDev.Controls.Add(this.lblDevTitle);
            this.pnlDev.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDev.Location = new System.Drawing.Point(170, 0);
            this.pnlDev.Name = "pnlDev";
            this.pnlDev.Size = new System.Drawing.Size(690, 76);
            this.pnlDev.TabIndex = 2;
            // 
            // txtLR
            // 
            this.txtLR.Location = new System.Drawing.Point(286, 32);
            this.txtLR.Name = "txtLR";
            this.txtLR.Size = new System.Drawing.Size(46, 25);
            this.txtLR.TabIndex = 9;
            this.txtLR.Text = "0.15";
            // 
            // lblLR
            // 
            this.lblLR.AutoSize = true;
            this.lblLR.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLR.Location = new System.Drawing.Point(260, 37);
            this.lblLR.Name = "lblLR";
            this.lblLR.Size = new System.Drawing.Size(24, 15);
            this.lblLR.TabIndex = 8;
            this.lblLR.Text = "LR:";
            // 
            // btnLoadWeights
            // 
            this.btnLoadWeights.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnLoadWeights.Location = new System.Drawing.Point(583, 31);
            this.btnLoadWeights.Name = "btnLoadWeights";
            this.btnLoadWeights.Size = new System.Drawing.Size(95, 27);
            this.btnLoadWeights.TabIndex = 7;
            this.btnLoadWeights.Text = "Load Weights";
            this.btnLoadWeights.UseVisualStyleBackColor = true;
            this.btnLoadWeights.Click += new System.EventHandler(this.btnLoadWeights_Click);
            // 
            // btnSaveWeights
            // 
            this.btnSaveWeights.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSaveWeights.Location = new System.Drawing.Point(482, 31);
            this.btnSaveWeights.Name = "btnSaveWeights";
            this.btnSaveWeights.Size = new System.Drawing.Size(95, 27);
            this.btnSaveWeights.TabIndex = 6;
            this.btnSaveWeights.Text = "Save Weights";
            this.btnSaveWeights.UseVisualStyleBackColor = true;
            this.btnSaveWeights.Click += new System.EventHandler(this.btnSaveWeights_Click);
            // 
            // btnDevTrain
            // 
            this.btnDevTrain.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDevTrain.Location = new System.Drawing.Point(344, 31);
            this.btnDevTrain.Name = "btnDevTrain";
            this.btnDevTrain.Size = new System.Drawing.Size(120, 27);
            this.btnDevTrain.TabIndex = 5;
            this.btnDevTrain.Text = "Run Training";
            this.btnDevTrain.UseVisualStyleBackColor = true;
            this.btnDevTrain.Click += new System.EventHandler(this.btnDevTrain_Click);
            // 
            // txtEpochs
            // 
            this.txtEpochs.Location = new System.Drawing.Point(198, 32);
            this.txtEpochs.Name = "txtEpochs";
            this.txtEpochs.Size = new System.Drawing.Size(56, 25);
            this.txtEpochs.TabIndex = 4;
            this.txtEpochs.Text = "1000";
            // 
            // lblEpochs
            // 
            this.lblEpochs.AutoSize = true;
            this.lblEpochs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEpochs.Location = new System.Drawing.Point(146, 37);
            this.lblEpochs.Name = "lblEpochs";
            this.lblEpochs.Size = new System.Drawing.Size(49, 15);
            this.lblEpochs.TabIndex = 3;
            this.lblEpochs.Text = "Epochs:";
            // 
            // txtHNodes
            // 
            this.txtHNodes.Location = new System.Drawing.Point(94, 32);
            this.txtHNodes.Name = "txtHNodes";
            this.txtHNodes.Size = new System.Drawing.Size(46, 25);
            this.txtHNodes.TabIndex = 2;
            this.txtHNodes.Text = "8";
            // 
            // lblHNodes
            // 
            this.lblHNodes.AutoSize = true;
            this.lblHNodes.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHNodes.Location = new System.Drawing.Point(31, 37);
            this.lblHNodes.Name = "lblHNodes";
            this.lblHNodes.Size = new System.Drawing.Size(58, 15);
            this.lblHNodes.TabIndex = 1;
            this.lblHNodes.Text = "H-Nodes:";
            // 
            // lblDevTitle
            // 
            this.lblDevTitle.AutoSize = true;
            this.lblDevTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDevTitle.Location = new System.Drawing.Point(13, 10);
            this.lblDevTitle.Name = "lblDevTitle";
            this.lblDevTitle.Size = new System.Drawing.Size(126, 15);
            this.lblDevTitle.TabIndex = 0;
            this.lblDevTitle.Text = "Developer & Tune Mode";
            // 
            // lblFooterNote
            // 
            this.lblFooterNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFooterNote.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFooterNote.ForeColor = System.Drawing.Color.Gray;
            this.lblFooterNote.Location = new System.Drawing.Point(170, 16);
            this.lblFooterNote.Name = "lblFooterNote";
            this.lblFooterNote.Size = new System.Drawing.Size(666, 44);
            this.lblFooterNote.TabIndex = 1;
            this.lblFooterNote.Text = "Survey-based demo. Not a medical diagnosis.";
            this.lblFooterNote.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.White;
            this.btnBack.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnBack.Location = new System.Drawing.Point(24, 16);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(130, 44);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "\u2190 Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.tblResult);
            this.pnlContent.Controls.Add(this.tblAge);
            this.pnlContent.Controls.Add(this.tblChoice);
            this.pnlContent.Controls.Add(this.lblHint);
            this.pnlContent.Controls.Add(this.lblQuestion);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 96);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(24);
            this.pnlContent.Size = new System.Drawing.Size(860, 428);
            this.pnlContent.TabIndex = 0;
            // 
            // tblResult
            // 
            this.tblResult.ColumnCount = 1;
            this.tblResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblResult.Controls.Add(this.lblResultHeading, 0, 0);
            this.tblResult.Controls.Add(this.lblRiskPercent, 0, 1);
            this.tblResult.Controls.Add(this.prgRisk, 0, 2);
            this.tblResult.Controls.Add(this.lblRiskLevel, 0, 3);
            this.tblResult.Controls.Add(this.lblResultNote, 0, 4);
            this.tblResult.Controls.Add(this.btnContinue, 0, 5);
            this.tblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblResult.Location = new System.Drawing.Point(24, 24);
            this.tblResult.Name = "tblResult";
            this.tblResult.RowCount = 6;
            this.tblResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tblResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26F));
            this.tblResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tblResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tblResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tblResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tblResult.Size = new System.Drawing.Size(812, 380);
            this.tblResult.TabIndex = 4;
            this.tblResult.Visible = false;
            // 
            // lblResultHeading
            // 
            this.lblResultHeading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResultHeading.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lblResultHeading.ForeColor = System.Drawing.Color.DimGray;
            this.lblResultHeading.Name = "lblResultHeading";
            this.lblResultHeading.TabIndex = 0;
            this.lblResultHeading.Text = "Model risk score";
            this.lblResultHeading.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblRiskPercent
            // 
            this.lblRiskPercent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRiskPercent.Font = new System.Drawing.Font("Segoe UI Semibold", 54F, System.Drawing.FontStyle.Bold);
            this.lblRiskPercent.Name = "lblRiskPercent";
            this.lblRiskPercent.TabIndex = 1;
            this.lblRiskPercent.Text = "0%";
            this.lblRiskPercent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // prgRisk
            // 
            this.prgRisk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.prgRisk.Margin = new System.Windows.Forms.Padding(120, 3, 120, 3);
            this.prgRisk.Name = "prgRisk";
            this.prgRisk.Size = new System.Drawing.Size(572, 20);
            this.prgRisk.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgRisk.TabIndex = 2;
            // 
            // lblRiskLevel
            // 
            this.lblRiskLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRiskLevel.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblRiskLevel.Name = "lblRiskLevel";
            this.lblRiskLevel.TabIndex = 3;
            this.lblRiskLevel.Text = "Lower risk indicated";
            this.lblRiskLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResultNote
            // 
            this.lblResultNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResultNote.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblResultNote.ForeColor = System.Drawing.Color.DimGray;
            this.lblResultNote.Margin = new System.Windows.Forms.Padding(60, 0, 60, 0);
            this.lblResultNote.Name = "lblResultNote";
            this.lblResultNote.TabIndex = 4;
            this.lblResultNote.Text = "Note";
            this.lblResultNote.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnContinue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.btnContinue.FlatAppearance.BorderSize = 0;
            this.btnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinue.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.btnContinue.ForeColor = System.Drawing.Color.White;
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(220, 52);
            this.btnContinue.TabIndex = 5;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = false;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // tblAge
            // 
            this.tblAge.ColumnCount = 1;
            this.tblAge.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblAge.Controls.Add(this.lblAgeValue, 0, 0);
            this.tblAge.Controls.Add(this.trackAge, 0, 1);
            this.tblAge.Controls.Add(this.btnAgeNext, 0, 2);
            this.tblAge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblAge.Location = new System.Drawing.Point(24, 24);
            this.tblAge.Name = "tblAge";
            this.tblAge.RowCount = 3;
            this.tblAge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tblAge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tblAge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tblAge.Size = new System.Drawing.Size(812, 380);
            this.tblAge.TabIndex = 3;
            this.tblAge.Visible = false;
            // 
            // lblAgeValue
            // 
            this.lblAgeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAgeValue.Font = new System.Drawing.Font("Segoe UI Semibold", 36F, System.Drawing.FontStyle.Bold);
            this.lblAgeValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.lblAgeValue.Name = "lblAgeValue";
            this.lblAgeValue.TabIndex = 0;
            this.lblAgeValue.Text = "40 years old";
            this.lblAgeValue.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // trackAge
            // 
            this.trackAge.AutoSize = false;
            this.trackAge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackAge.LargeChange = 5;
            this.trackAge.Margin = new System.Windows.Forms.Padding(80, 10, 80, 10);
            this.trackAge.Maximum = 100;
            this.trackAge.Minimum = 18;
            this.trackAge.Name = "trackAge";
            this.trackAge.TabIndex = 1;
            this.trackAge.TickFrequency = 5;
            this.trackAge.Value = 40;
            this.trackAge.ValueChanged += new System.EventHandler(this.trackAge_ValueChanged);
            // 
            // btnAgeNext
            // 
            this.btnAgeNext.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAgeNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.btnAgeNext.FlatAppearance.BorderSize = 0;
            this.btnAgeNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgeNext.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.btnAgeNext.ForeColor = System.Drawing.Color.White;
            this.btnAgeNext.Name = "btnAgeNext";
            this.btnAgeNext.Size = new System.Drawing.Size(220, 52);
            this.btnAgeNext.TabIndex = 2;
            this.btnAgeNext.Text = "Next";
            this.btnAgeNext.UseVisualStyleBackColor = false;
            this.btnAgeNext.Click += new System.EventHandler(this.btnAgeNext_Click);
            // 
            // tblChoice
            // 
            this.tblChoice.ColumnCount = 2;
            this.tblChoice.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblChoice.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblChoice.Controls.Add(this.btnChoiceA, 0, 0);
            this.tblChoice.Controls.Add(this.btnChoiceB, 1, 0);
            this.tblChoice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblChoice.Location = new System.Drawing.Point(24, 24);
            this.tblChoice.Name = "tblChoice";
            this.tblChoice.RowCount = 1;
            this.tblChoice.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblChoice.Size = new System.Drawing.Size(812, 380);
            this.tblChoice.TabIndex = 2;
            // 
            // btnChoiceA
            // 
            this.btnChoiceA.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnChoiceA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(241)))), ((int)(((byte)(245)))));
            this.btnChoiceA.FlatAppearance.BorderSize = 0;
            this.btnChoiceA.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.btnChoiceA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChoiceA.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.btnChoiceA.Name = "btnChoiceA";
            this.btnChoiceA.Size = new System.Drawing.Size(280, 120);
            this.btnChoiceA.TabIndex = 0;
            this.btnChoiceA.Text = "Yes";
            this.btnChoiceA.UseVisualStyleBackColor = false;
            this.btnChoiceA.Click += new System.EventHandler(this.btnChoice_Click);
            // 
            // btnChoiceB
            // 
            this.btnChoiceB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnChoiceB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(241)))), ((int)(((byte)(245)))));
            this.btnChoiceB.FlatAppearance.BorderSize = 0;
            this.btnChoiceB.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.btnChoiceB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChoiceB.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.btnChoiceB.Name = "btnChoiceB";
            this.btnChoiceB.Size = new System.Drawing.Size(280, 120);
            this.btnChoiceB.TabIndex = 1;
            this.btnChoiceB.Text = "No";
            this.btnChoiceB.UseVisualStyleBackColor = false;
            this.btnChoiceB.Click += new System.EventHandler(this.btnChoice_Click);
            // 
            // lblHint
            // 
            this.lblHint.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHint.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblHint.ForeColor = System.Drawing.Color.Gray;
            this.lblHint.Location = new System.Drawing.Point(24, 154);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(812, 36);
            this.lblHint.TabIndex = 1;
            this.lblHint.Text = "Hint";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblQuestion
            // 
            this.lblQuestion.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblQuestion.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.lblQuestion.Location = new System.Drawing.Point(24, 24);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(812, 130);
            this.lblQuestion.TabIndex = 0;
            this.lblQuestion.Text = "Question";
            this.lblQuestion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(860, 600);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.MinimumSize = new System.Drawing.Size(740, 560);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lung Cancer Risk Screening";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.pnlDev.ResumeLayout(false);
            this.pnlDev.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.tblResult.ResumeLayout(false);
            this.tblAge.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackAge)).EndInit();
            this.tblChoice.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblStep;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblFooterNote;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblQuestion;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.TableLayoutPanel tblChoice;
        private System.Windows.Forms.Button btnChoiceA;
        private System.Windows.Forms.Button btnChoiceB;
        private System.Windows.Forms.TableLayoutPanel tblAge;
        private System.Windows.Forms.Label lblAgeValue;
        private System.Windows.Forms.TrackBar trackAge;
        private System.Windows.Forms.Button btnAgeNext;
        private System.Windows.Forms.TableLayoutPanel tblResult;
        private System.Windows.Forms.Label lblResultHeading;
        private System.Windows.Forms.Label lblRiskPercent;
        private System.Windows.Forms.ProgressBar prgRisk;
        private System.Windows.Forms.Label lblRiskLevel;
        private System.Windows.Forms.Label lblResultNote;
        private System.Windows.Forms.Button btnContinue;

        // Developer Panel Components
        private System.Windows.Forms.Button btnDevToggle;
        private System.Windows.Forms.Panel pnlDev;
        private System.Windows.Forms.Label lblDevTitle;
        private System.Windows.Forms.Label lblHNodes;
        private System.Windows.Forms.TextBox txtHNodes;
        private System.Windows.Forms.Label lblEpochs;
        private System.Windows.Forms.TextBox txtEpochs;
        private System.Windows.Forms.Label lblLR;
        private System.Windows.Forms.TextBox txtLR;
        private System.Windows.Forms.Button btnDevTrain;
        private System.Windows.Forms.Button btnSaveWeights;
        private System.Windows.Forms.Button btnLoadWeights;
    }
}