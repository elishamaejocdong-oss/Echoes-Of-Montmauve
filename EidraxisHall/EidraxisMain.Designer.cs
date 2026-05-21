namespace Echoes_of_Montmauve
{
    partial class EidraxisMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EidraxisMain));
            pnlNoctyra = new Panel();
            btnStartGame = new Button();
            lblDialogue = new Label();
            MapBtn = new Button();
            pbNoctyra = new PictureBox();
            pnlNoctyra.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbNoctyra).BeginInit();
            SuspendLayout();
            // 
            // pnlNoctyra
            // 
            pnlNoctyra.BackColor = Color.Transparent;
            pnlNoctyra.BackgroundImage = (Image)resources.GetObject("pnlNoctyra.BackgroundImage");
            pnlNoctyra.BackgroundImageLayout = ImageLayout.Center;
            pnlNoctyra.Controls.Add(btnStartGame);
            pnlNoctyra.Controls.Add(lblDialogue);
            pnlNoctyra.Dock = DockStyle.Bottom;
            pnlNoctyra.Location = new Point(0, 326);
            pnlNoctyra.Name = "pnlNoctyra";
            pnlNoctyra.Size = new Size(1262, 327);
            pnlNoctyra.TabIndex = 0;
            // 
            // btnStartGame
            // 
            btnStartGame.BackColor = Color.PapayaWhip;
            btnStartGame.FlatAppearance.BorderColor = Color.Gold;
            btnStartGame.FlatAppearance.BorderSize = 2;
            btnStartGame.FlatStyle = FlatStyle.Flat;
            btnStartGame.Font = new Font("Calisto MT", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStartGame.ForeColor = Color.Goldenrod;
            btnStartGame.Location = new Point(879, 261);
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
            lblDialogue.ForeColor = Color.MidnightBlue;
            lblDialogue.Location = new Point(348, 188);
            lblDialogue.Name = "lblDialogue";
            lblDialogue.Size = new Size(712, 58);
            lblDialogue.TabIndex = 0;
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
            MapBtn.Location = new Point(12, 12);
            MapBtn.Name = "MapBtn";
            MapBtn.Size = new Size(82, 85);
            MapBtn.TabIndex = 7;
            MapBtn.TabStop = false;
            MapBtn.UseVisualStyleBackColor = false;
            MapBtn.Click += MapBtn_Click;
            // 
            // pbNoctyra
            // 
            pbNoctyra.BackColor = Color.Transparent;
            pbNoctyra.BackgroundImage = (Image)resources.GetObject("pbNoctyra.BackgroundImage");
            pbNoctyra.BackgroundImageLayout = ImageLayout.Stretch;
            pbNoctyra.Location = new Point(432, 168);
            pbNoctyra.Name = "pbNoctyra";
            pbNoctyra.Size = new Size(86, 116);
            pbNoctyra.TabIndex = 13;
            pbNoctyra.TabStop = false;
            // 
            // EidraxisMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1262, 653);
            Controls.Add(pbNoctyra);
            Controls.Add(MapBtn);
            Controls.Add(pnlNoctyra);
            Name = "EidraxisMain";
            StartPosition = FormStartPosition.CenterScreen;
            Load += EidraxisMain_Load;
            Paint += EidraxisMain_Paint;
            KeyUp += EidraxisMain_KeyUp;
            pnlNoctyra.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbNoctyra).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlNoctyra;
        private Label lblDialogue;
        private Button btnStartGame;
        private Button MapBtn;
        private PictureBox pbNoctyra;
    }
}