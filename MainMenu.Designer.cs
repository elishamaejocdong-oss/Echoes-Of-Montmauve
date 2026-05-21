namespace Echoes_of_Montmauve
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            MontmauveMapBtn = new Button();
            pictureBox1 = new PictureBox();
            lblMaismaLevel = new Label();
            miasmaProgressBar = new MiasmaProgressBar();
            btnAchievement = new Button();
            btnOracle = new Button();
            button1 = new Button();
            btnNotes = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // MontmauveMapBtn
            // 
            MontmauveMapBtn.BackColor = Color.PapayaWhip;
            MontmauveMapBtn.FlatAppearance.BorderColor = Color.SaddleBrown;
            MontmauveMapBtn.FlatAppearance.BorderSize = 2;
            MontmauveMapBtn.FlatStyle = FlatStyle.Flat;
            MontmauveMapBtn.Font = new Font("Calisto MT", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            MontmauveMapBtn.ForeColor = Color.Maroon;
            MontmauveMapBtn.Location = new Point(895, 552);
            MontmauveMapBtn.Name = "MontmauveMapBtn";
            MontmauveMapBtn.Size = new Size(321, 66);
            MontmauveMapBtn.TabIndex = 3;
            MontmauveMapBtn.Text = "Montmauve Map";
            MontmauveMapBtn.UseVisualStyleBackColor = false;
            MontmauveMapBtn.Click += MontmauveMapBtn_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(1, 34);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(63, 71);
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // lblMaismaLevel
            // 
            lblMaismaLevel.BackColor = Color.Thistle;
            lblMaismaLevel.FlatStyle = FlatStyle.Flat;
            lblMaismaLevel.Font = new Font("Calisto MT", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMaismaLevel.ForeColor = Color.DarkGoldenrod;
            lblMaismaLevel.Location = new Point(60, 53);
            lblMaismaLevel.MaximumSize = new Size(226, 20);
            lblMaismaLevel.Name = "lblMaismaLevel";
            lblMaismaLevel.Size = new Size(226, 20);
            lblMaismaLevel.TabIndex = 9;
            // 
            // miasmaProgressBar
            // 
            miasmaProgressBar.ForeColor = SystemColors.Control;
            miasmaProgressBar.Location = new Point(60, 76);
            miasmaProgressBar.Name = "miasmaProgressBar";
            miasmaProgressBar.Size = new Size(226, 29);
            miasmaProgressBar.TabIndex = 11;
            miasmaProgressBar.Value = 100;
            // 
            // btnAchievement
            // 
            btnAchievement.BackColor = Color.Transparent;
            btnAchievement.BackgroundImage = (Image)resources.GetObject("btnAchievement.BackgroundImage");
            btnAchievement.BackgroundImageLayout = ImageLayout.Stretch;
            btnAchievement.FlatAppearance.BorderSize = 0;
            btnAchievement.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnAchievement.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnAchievement.FlatStyle = FlatStyle.Flat;
            btnAchievement.Location = new Point(1135, 271);
            btnAchievement.Name = "btnAchievement";
            btnAchievement.Size = new Size(81, 85);
            btnAchievement.TabIndex = 12;
            btnAchievement.UseVisualStyleBackColor = false;
            btnAchievement.Click += btnAchievement_Click;
            // 
            // btnOracle
            // 
            btnOracle.BackColor = Color.Transparent;
            btnOracle.BackgroundImage = (Image)resources.GetObject("btnOracle.BackgroundImage");
            btnOracle.BackgroundImageLayout = ImageLayout.Stretch;
            btnOracle.FlatAppearance.BorderSize = 0;
            btnOracle.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnOracle.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnOracle.FlatStyle = FlatStyle.Flat;
            btnOracle.Location = new Point(1135, 362);
            btnOracle.Name = "btnOracle";
            btnOracle.Size = new Size(81, 85);
            btnOracle.TabIndex = 13;
            btnOracle.UseVisualStyleBackColor = false;
            btnOracle.Click += btnOracle_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(1135, 76);
            button1.Name = "button1";
            button1.Size = new Size(81, 85);
            button1.TabIndex = 14;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // btnNotes
            // 
            btnNotes.BackColor = Color.Transparent;
            btnNotes.BackgroundImage = (Image)resources.GetObject("btnNotes.BackgroundImage");
            btnNotes.BackgroundImageLayout = ImageLayout.Stretch;
            btnNotes.FlatAppearance.BorderSize = 0;
            btnNotes.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnNotes.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnNotes.FlatStyle = FlatStyle.Flat;
            btnNotes.Location = new Point(1135, 167);
            btnNotes.Name = "btnNotes";
            btnNotes.Size = new Size(81, 85);
            btnNotes.TabIndex = 16;
            btnNotes.UseVisualStyleBackColor = false;
            btnNotes.Click += btnNotes_Click;
            // 
            // MainMenu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1262, 653);
            Controls.Add(btnNotes);
            Controls.Add(button1);
            Controls.Add(btnOracle);
            Controls.Add(btnAchievement);
            Controls.Add(miasmaProgressBar);
            Controls.Add(lblMaismaLevel);
            Controls.Add(pictureBox1);
            Controls.Add(MontmauveMapBtn);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainMenu";
            StartPosition = FormStartPosition.CenterScreen;
            Activated += MainMenu_Activated;
            Load += MainMenu_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Button MontmauveMapBtn;
        private Label lblMaismaLevel;
        private MiasmaProgressBar miasmaProgressBar;
        private Button btnAchievement;
        private Button btnOracle;
        private Button button1;
        private Button btnNotes;
    }
}