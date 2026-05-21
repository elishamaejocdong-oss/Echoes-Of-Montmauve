namespace Echoes_of_Montmauve.VeloryxSpire
{
    partial class VeloryxMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VeloryxMain));
            MapBtn = new Button();
            pbXerion = new PictureBox();
            pnlXerion = new Panel();
            btnStartGame = new Button();
            lblDialogue = new Label();
            ((System.ComponentModel.ISupportInitialize)pbXerion).BeginInit();
            pnlXerion.SuspendLayout();
            SuspendLayout();
            // 
            // MapBtn
            // 
            MapBtn.BackColor = Color.Transparent;
            MapBtn.BackgroundImage = (Image)resources.GetObject("MapBtn.BackgroundImage");
            MapBtn.BackgroundImageLayout = ImageLayout.Stretch;
            MapBtn.FlatAppearance.BorderColor = Color.SaddleBrown;
            MapBtn.FlatAppearance.BorderSize = 0;
            MapBtn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            MapBtn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            MapBtn.FlatStyle = FlatStyle.Flat;
            MapBtn.Font = new Font("Calisto MT", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            MapBtn.ForeColor = Color.Transparent;
            MapBtn.Location = new Point(12, 6);
            MapBtn.Name = "MapBtn";
            MapBtn.Size = new Size(82, 85);
            MapBtn.TabIndex = 9;
            MapBtn.TabStop = false;
            MapBtn.UseVisualStyleBackColor = false;
            MapBtn.Click += MapBtn_Click;
            // 
            // pbXerion
            // 
            pbXerion.BackColor = Color.Transparent;
            pbXerion.BackgroundImage = (Image)resources.GetObject("pbXerion.BackgroundImage");
            pbXerion.BackgroundImageLayout = ImageLayout.Stretch;
            pbXerion.Location = new Point(801, 322);
            pbXerion.Name = "pbXerion";
            pbXerion.Size = new Size(86, 117);
            pbXerion.TabIndex = 13;
            pbXerion.TabStop = false;
            // 
            // pnlXerion
            // 
            pnlXerion.BackColor = Color.Transparent;
            pnlXerion.BackgroundImage = (Image)resources.GetObject("pnlXerion.BackgroundImage");
            pnlXerion.BackgroundImageLayout = ImageLayout.Center;
            pnlXerion.Controls.Add(btnStartGame);
            pnlXerion.Controls.Add(lblDialogue);
            pnlXerion.Dock = DockStyle.Bottom;
            pnlXerion.Location = new Point(0, 315);
            pnlXerion.Name = "pnlXerion";
            pnlXerion.Size = new Size(1262, 338);
            pnlXerion.TabIndex = 14;
            // 
            // btnStartGame
            // 
            btnStartGame.BackColor = Color.PapayaWhip;
            btnStartGame.FlatAppearance.BorderColor = Color.Gold;
            btnStartGame.FlatAppearance.BorderSize = 2;
            btnStartGame.FlatStyle = FlatStyle.Flat;
            btnStartGame.Font = new Font("Calisto MT", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStartGame.ForeColor = Color.Goldenrod;
            btnStartGame.Location = new Point(888, 262);
            btnStartGame.Name = "btnStartGame";
            btnStartGame.Size = new Size(181, 44);
            btnStartGame.TabIndex = 8;
            btnStartGame.TabStop = false;
            btnStartGame.Text = "Start Game";
            btnStartGame.UseVisualStyleBackColor = false;
            btnStartGame.Click += btnStartGame_Click;
            // 
            // lblDialogue
            // 
            lblDialogue.Font = new Font("Calisto MT", 12F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblDialogue.Location = new Point(300, 188);
            lblDialogue.Name = "lblDialogue";
            lblDialogue.Size = new Size(694, 102);
            lblDialogue.TabIndex = 0;
            // 
            // VeloryxMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1262, 653);
            Controls.Add(pnlXerion);
            Controls.Add(MapBtn);
            Controls.Add(pbXerion);
            Name = "VeloryxMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "VeloryxMain";
            Load += VeloryxMain_Load;
            Paint += VeloryxMain_Paint;
            KeyUp += VeloryxMain_KeyUp;
            ((System.ComponentModel.ISupportInitialize)pbXerion).EndInit();
            pnlXerion.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button MapBtn;
        private PictureBox pbXerion;
        private Panel pnlXerion;
        private Button btnStartGame;
        private Label lblDialogue;
    }
}