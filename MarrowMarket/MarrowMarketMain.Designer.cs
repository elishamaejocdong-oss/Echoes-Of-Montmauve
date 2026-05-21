namespace Echoes_of_Montmauve.MarrowMarket
{
    partial class MarrowMarketMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MarrowMarketMain));
            MapBtn = new Button();
            pnlChyra = new Panel();
            btnStartGame = new Button();
            lblDialogue = new Label();
            pbChyra = new PictureBox();
            pnlNPC = new Panel();
            lblNPCDialogue = new Label();
            lblNPCName = new Label();
            pnlChyra.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbChyra).BeginInit();
            pnlNPC.SuspendLayout();
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
            // pnlChyra
            // 
            pnlChyra.BackColor = Color.Transparent;
            pnlChyra.BackgroundImage = (Image)resources.GetObject("pnlChyra.BackgroundImage");
            pnlChyra.BackgroundImageLayout = ImageLayout.Center;
            pnlChyra.Controls.Add(btnStartGame);
            pnlChyra.Controls.Add(lblDialogue);
            pnlChyra.Dock = DockStyle.Bottom;
            pnlChyra.Location = new Point(0, 311);
            pnlChyra.Name = "pnlChyra";
            pnlChyra.Size = new Size(1262, 342);
            pnlChyra.TabIndex = 11;
            // 
            // btnStartGame
            // 
            btnStartGame.BackColor = Color.PapayaWhip;
            btnStartGame.FlatAppearance.BorderColor = Color.Gold;
            btnStartGame.FlatAppearance.BorderSize = 2;
            btnStartGame.FlatStyle = FlatStyle.Flat;
            btnStartGame.Font = new Font("Calisto MT", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStartGame.ForeColor = Color.Goldenrod;
            btnStartGame.Location = new Point(893, 263);
            btnStartGame.Name = "btnStartGame";
            btnStartGame.Size = new Size(170, 43);
            btnStartGame.TabIndex = 9;
            btnStartGame.TabStop = false;
            btnStartGame.Text = "Play";
            btnStartGame.UseVisualStyleBackColor = false;
            btnStartGame.Click += btnStartGame_Click;
            // 
            // lblDialogue
            // 
            lblDialogue.Font = new Font("Calisto MT", 12F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblDialogue.ForeColor = Color.Chocolate;
            lblDialogue.Location = new Point(356, 177);
            lblDialogue.MaximumSize = new Size(695, 69);
            lblDialogue.Name = "lblDialogue";
            lblDialogue.Size = new Size(695, 69);
            lblDialogue.TabIndex = 0;
            // 
            // pbChyra
            // 
            pbChyra.BackColor = Color.Transparent;
            pbChyra.BackgroundImage = (Image)resources.GetObject("pbChyra.BackgroundImage");
            pbChyra.BackgroundImageLayout = ImageLayout.Stretch;
            pbChyra.Location = new Point(743, 189);
            pbChyra.Name = "pbChyra";
            pbChyra.Size = new Size(86, 116);
            pbChyra.TabIndex = 12;
            pbChyra.TabStop = false;
            // 
            // pnlNPC
            // 
            pnlNPC.BackColor = Color.Transparent;
            pnlNPC.BackgroundImage = (Image)resources.GetObject("pnlNPC.BackgroundImage");
            pnlNPC.BackgroundImageLayout = ImageLayout.Center;
            pnlNPC.Controls.Add(lblNPCName);
            pnlNPC.Controls.Add(lblNPCDialogue);
            pnlNPC.Dock = DockStyle.Bottom;
            pnlNPC.Location = new Point(0, -31);
            pnlNPC.Name = "pnlNPC";
            pnlNPC.Size = new Size(1262, 342);
            pnlNPC.TabIndex = 13;
            // 
            // lblNPCDialogue
            // 
            lblNPCDialogue.Font = new Font("Calisto MT", 12F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblNPCDialogue.ForeColor = Color.SaddleBrown;
            lblNPCDialogue.Location = new Point(258, 191);
            lblNPCDialogue.MaximumSize = new Size(695, 69);
            lblNPCDialogue.Name = "lblNPCDialogue";
            lblNPCDialogue.Size = new Size(695, 69);
            lblNPCDialogue.TabIndex = 0;
            // 
            // lblNPCName
            // 
            lblNPCName.Font = new Font("Calisto MT", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNPCName.ForeColor = Color.SeaShell;
            lblNPCName.Location = new Point(834, 127);
            lblNPCName.MaximumSize = new Size(217, 42);
            lblNPCName.Name = "lblNPCName";
            lblNPCName.Size = new Size(217, 42);
            lblNPCName.TabIndex = 1;
            lblNPCName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MarrowMarketMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1262, 653);
            Controls.Add(pnlNPC);
            Controls.Add(pbChyra);
            Controls.Add(pnlChyra);
            Controls.Add(MapBtn);
            Name = "MarrowMarketMain";
            StartPosition = FormStartPosition.CenterScreen;
            Load += MarrowMarketMain_Load;
            Paint += MarrowMarketMain_Paint;
            KeyDown += MarrowMarketMain_KeyDown;
            KeyUp += MarrowMarketMain_KeyUp;
            pnlChyra.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbChyra).EndInit();
            pnlNPC.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button MapBtn;
        private Panel pnlChyra;
        private Label lblDialogue;
        private Button btnStartGame;
        private PictureBox pbChyra;
        private Panel pnlNPC;
        private Label lblNPCName;
        private Label lblNPCDialogue;
    }
}