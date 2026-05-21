namespace Echoes_of_Montmauve
{
    partial class FinalAccusationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlSelectionElements = new Panel();
            btnSubmmitAccusation = new Button();
            cmbCulprits = new ComboBox();
            pnlEndingScreen = new Panel();
            btnExitGame = new Button();
            txtEndingNarrative = new TextBox();
            lblEndingTitle = new Label();
            pnlSelectionElements.SuspendLayout();
            pnlEndingScreen.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSelectionElements
            // 
            pnlSelectionElements.Controls.Add(btnSubmmitAccusation);
            pnlSelectionElements.Controls.Add(cmbCulprits);
            pnlSelectionElements.Location = new Point(161, 104);
            pnlSelectionElements.Name = "pnlSelectionElements";
            pnlSelectionElements.Size = new Size(942, 427);
            pnlSelectionElements.TabIndex = 0;
            // 
            // btnSubmmitAccusation
            // 
            btnSubmmitAccusation.BackColor = Color.PeachPuff;
            btnSubmmitAccusation.BackgroundImageLayout = ImageLayout.Stretch;
            btnSubmmitAccusation.FlatAppearance.BorderColor = Color.SaddleBrown;
            btnSubmmitAccusation.FlatAppearance.BorderSize = 3;
            btnSubmmitAccusation.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSubmmitAccusation.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnSubmmitAccusation.FlatStyle = FlatStyle.Flat;
            btnSubmmitAccusation.Font = new Font("Calisto MT", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSubmmitAccusation.ForeColor = Color.DarkRed;
            btnSubmmitAccusation.Location = new Point(273, 293);
            btnSubmmitAccusation.Name = "btnSubmmitAccusation";
            btnSubmmitAccusation.Size = new Size(389, 54);
            btnSubmmitAccusation.TabIndex = 8;
            btnSubmmitAccusation.Text = "Deliver Final Judgement";
            btnSubmmitAccusation.UseVisualStyleBackColor = false;
            btnSubmmitAccusation.Click += btnSubmmitAccusation_Click;
            // 
            // cmbCulprits
            // 
            cmbCulprits.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCulprits.Font = new Font("Calisto MT", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmbCulprits.ForeColor = Color.DarkRed;
            cmbCulprits.FormattingEnabled = true;
            cmbCulprits.Location = new Point(203, 147);
            cmbCulprits.Name = "cmbCulprits";
            cmbCulprits.Size = new Size(525, 34);
            cmbCulprits.TabIndex = 0;
            // 
            // pnlEndingScreen
            // 
            pnlEndingScreen.Controls.Add(btnExitGame);
            pnlEndingScreen.Controls.Add(txtEndingNarrative);
            pnlEndingScreen.Controls.Add(lblEndingTitle);
            pnlEndingScreen.Dock = DockStyle.Fill;
            pnlEndingScreen.Location = new Point(0, 0);
            pnlEndingScreen.Name = "pnlEndingScreen";
            pnlEndingScreen.Size = new Size(1262, 653);
            pnlEndingScreen.TabIndex = 1;
            pnlEndingScreen.Visible = false;
            // 
            // btnExitGame
            // 
            btnExitGame.BackColor = Color.PeachPuff;
            btnExitGame.BackgroundImageLayout = ImageLayout.Stretch;
            btnExitGame.FlatAppearance.BorderColor = Color.SaddleBrown;
            btnExitGame.FlatAppearance.BorderSize = 3;
            btnExitGame.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnExitGame.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnExitGame.FlatStyle = FlatStyle.Flat;
            btnExitGame.Font = new Font("Calisto MT", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExitGame.ForeColor = Color.DarkRed;
            btnExitGame.Location = new Point(512, 574);
            btnExitGame.Name = "btnExitGame";
            btnExitGame.Size = new Size(270, 54);
            btnExitGame.TabIndex = 8;
            btnExitGame.Text = "Close Case (Exit)";
            btnExitGame.UseVisualStyleBackColor = false;
            btnExitGame.Click += btnExitGame_Click;
            // 
            // txtEndingNarrative
            // 
            txtEndingNarrative.Location = new Point(113, 304);
            txtEndingNarrative.Multiline = true;
            txtEndingNarrative.Name = "txtEndingNarrative";
            txtEndingNarrative.ReadOnly = true;
            txtEndingNarrative.ScrollBars = ScrollBars.Vertical;
            txtEndingNarrative.Size = new Size(1022, 244);
            txtEndingNarrative.TabIndex = 1;
            // 
            // lblEndingTitle
            // 
            lblEndingTitle.AutoSize = true;
            lblEndingTitle.Font = new Font("Calisto MT", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEndingTitle.ForeColor = Color.SaddleBrown;
            lblEndingTitle.Location = new Point(548, 154);
            lblEndingTitle.Name = "lblEndingTitle";
            lblEndingTitle.Size = new Size(0, 46);
            lblEndingTitle.TabIndex = 0;
            // 
            // FinalAccusationForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1262, 653);
            Controls.Add(pnlEndingScreen);
            Controls.Add(pnlSelectionElements);
            Name = "FinalAccusationForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FinalAccusationForm";
            Load += FinalAccusationForm_Load;
            pnlSelectionElements.ResumeLayout(false);
            pnlEndingScreen.ResumeLayout(false);
            pnlEndingScreen.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlSelectionElements;
        private ComboBox cmbCulprits;
        private Button btnSubmmitAccusation;
        private Panel pnlEndingScreen;
        private TextBox txtEndingNarrative;
        private Label lblEndingTitle;
        private Button btnExitGame;
    }
}