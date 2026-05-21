namespace Echoes_of_Montmauve
{
    partial class NotebookForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotebookForm));
            tabNotebookContainer = new TabControl();
            tabSDG = new TabPage();
            lbSDGNotes = new ListBox();
            tabGame = new TabPage();
            lbGameNotes = new ListBox();
            tabClues = new TabPage();
            lbClues = new ListBox();
            lblDescriptionView = new TextBox();
            pbClueIllustration = new PictureBox();
            btnCloseNotebook = new Button();
            label1 = new Label();
            tabNotebookContainer.SuspendLayout();
            tabSDG.SuspendLayout();
            tabGame.SuspendLayout();
            tabClues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbClueIllustration).BeginInit();
            SuspendLayout();
            // 
            // tabNotebookContainer
            // 
            tabNotebookContainer.Controls.Add(tabSDG);
            tabNotebookContainer.Controls.Add(tabGame);
            tabNotebookContainer.Controls.Add(tabClues);
            tabNotebookContainer.Dock = DockStyle.Left;
            tabNotebookContainer.Location = new Point(0, 0);
            tabNotebookContainer.Name = "tabNotebookContainer";
            tabNotebookContainer.SelectedIndex = 0;
            tabNotebookContainer.Size = new Size(459, 653);
            tabNotebookContainer.TabIndex = 0;
            tabNotebookContainer.SelectedIndexChanged += tabNotebookContainer_SelectedIndexChanged;
            // 
            // tabSDG
            // 
            tabSDG.BackColor = Color.PapayaWhip;
            tabSDG.Controls.Add(lbSDGNotes);
            tabSDG.Location = new Point(4, 29);
            tabSDG.Name = "tabSDG";
            tabSDG.Padding = new Padding(3);
            tabSDG.Size = new Size(451, 620);
            tabSDG.TabIndex = 0;
            tabSDG.Text = "SDG 11 Terms";
            // 
            // lbSDGNotes
            // 
            lbSDGNotes.BackColor = Color.PapayaWhip;
            lbSDGNotes.Dock = DockStyle.Fill;
            lbSDGNotes.FormattingEnabled = true;
            lbSDGNotes.Location = new Point(3, 3);
            lbSDGNotes.Name = "lbSDGNotes";
            lbSDGNotes.Size = new Size(445, 614);
            lbSDGNotes.TabIndex = 0;
            lbSDGNotes.SelectedIndexChanged += ListBox_SelectedIndexChanged;
            // 
            // tabGame
            // 
            tabGame.Controls.Add(lbGameNotes);
            tabGame.Location = new Point(4, 29);
            tabGame.Name = "tabGame";
            tabGame.Padding = new Padding(3);
            tabGame.Size = new Size(451, 620);
            tabGame.TabIndex = 1;
            tabGame.Text = "Game Lore";
            tabGame.UseVisualStyleBackColor = true;
            // 
            // lbGameNotes
            // 
            lbGameNotes.Dock = DockStyle.Fill;
            lbGameNotes.FormattingEnabled = true;
            lbGameNotes.Location = new Point(3, 3);
            lbGameNotes.Name = "lbGameNotes";
            lbGameNotes.Size = new Size(445, 614);
            lbGameNotes.TabIndex = 0;
            lbGameNotes.SelectedIndexChanged += ListBox_SelectedIndexChanged;
            // 
            // tabClues
            // 
            tabClues.BackColor = Color.Transparent;
            tabClues.Controls.Add(lbClues);
            tabClues.Font = new Font("Calisto MT", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tabClues.Location = new Point(4, 29);
            tabClues.Name = "tabClues";
            tabClues.Padding = new Padding(3);
            tabClues.Size = new Size(451, 620);
            tabClues.TabIndex = 2;
            tabClues.Text = "Gathered Clues";
            // 
            // lbClues
            // 
            lbClues.Dock = DockStyle.Fill;
            lbClues.FormattingEnabled = true;
            lbClues.Location = new Point(3, 3);
            lbClues.Name = "lbClues";
            lbClues.Size = new Size(445, 614);
            lbClues.TabIndex = 0;
            lbClues.SelectedIndexChanged += ListBox_SelectedIndexChanged;
            // 
            // lblDescriptionView
            // 
            lblDescriptionView.Location = new Point(507, 281);
            lblDescriptionView.Multiline = true;
            lblDescriptionView.Name = "lblDescriptionView";
            lblDescriptionView.ReadOnly = true;
            lblDescriptionView.ScrollBars = ScrollBars.Vertical;
            lblDescriptionView.Size = new Size(705, 245);
            lblDescriptionView.TabIndex = 1;
            // 
            // pbClueIllustration
            // 
            pbClueIllustration.BackgroundImageLayout = ImageLayout.Stretch;
            pbClueIllustration.Location = new Point(782, 116);
            pbClueIllustration.Name = "pbClueIllustration";
            pbClueIllustration.Size = new Size(125, 113);
            pbClueIllustration.TabIndex = 2;
            pbClueIllustration.TabStop = false;
            // 
            // btnCloseNotebook
            // 
            btnCloseNotebook.BackColor = Color.PeachPuff;
            btnCloseNotebook.BackgroundImageLayout = ImageLayout.Stretch;
            btnCloseNotebook.FlatAppearance.BorderColor = Color.SaddleBrown;
            btnCloseNotebook.FlatAppearance.BorderSize = 3;
            btnCloseNotebook.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnCloseNotebook.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnCloseNotebook.FlatStyle = FlatStyle.Flat;
            btnCloseNotebook.Font = new Font("Calisto MT", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCloseNotebook.ForeColor = Color.DarkRed;
            btnCloseNotebook.Location = new Point(957, 573);
            btnCloseNotebook.Name = "btnCloseNotebook";
            btnCloseNotebook.Size = new Size(270, 54);
            btnCloseNotebook.TabIndex = 7;
            btnCloseNotebook.Text = "Close Notebook";
            btnCloseNotebook.UseVisualStyleBackColor = false;
            btnCloseNotebook.Click += btnCloseNotebook_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calisto MT", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.SaddleBrown;
            label1.Location = new Point(630, 32);
            label1.Name = "label1";
            label1.Size = new Size(441, 41);
            label1.TabIndex = 8;
            label1.Text = "Urban Scholar's Notebook";
            // 
            // NotebookForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Wheat;
            ClientSize = new Size(1262, 653);
            Controls.Add(label1);
            Controls.Add(btnCloseNotebook);
            Controls.Add(pbClueIllustration);
            Controls.Add(lblDescriptionView);
            Controls.Add(tabNotebookContainer);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "NotebookForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NotebookForm";
            Load += NotebookForm_Load;
            tabNotebookContainer.ResumeLayout(false);
            tabSDG.ResumeLayout(false);
            tabGame.ResumeLayout(false);
            tabClues.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbClueIllustration).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabNotebookContainer;
        private TabPage tabSDG;
        private TabPage tabGame;
        private TabPage tabClues;
        private ListBox lbSDGNotes;
        private ListBox lbGameNotes;
        private ListBox lbClues;
        private TextBox lblDescriptionView;
        private PictureBox pbClueIllustration;
        private Button btnCloseNotebook;
        private Label label1;
    }
}