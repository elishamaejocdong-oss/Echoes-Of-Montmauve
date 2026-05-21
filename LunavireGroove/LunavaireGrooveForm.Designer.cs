namespace Echoes_of_Montmauve
{
    partial class LunavaireGrooveForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LunavaireGrooveForm));
            MapBtn = new Button();
            pbStroemielle = new PictureBox();
            dbStroemielle = new Panel();
            StartGameBtn = new Button();
            lblDialogue = new Label();
            ((System.ComponentModel.ISupportInitialize)pbStroemielle).BeginInit();
            dbStroemielle.SuspendLayout();
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
            MapBtn.Location = new Point(21, 12);
            MapBtn.Name = "MapBtn";
            MapBtn.Size = new Size(82, 85);
            MapBtn.TabIndex = 10;
            MapBtn.TabStop = false;
            MapBtn.UseVisualStyleBackColor = false;
            MapBtn.Click += MapBtn_Click;
            // 
            // pbStroemielle
            // 
            pbStroemielle.BackColor = Color.Transparent;
            pbStroemielle.BackgroundImage = (Image)resources.GetObject("pbStroemielle.BackgroundImage");
            pbStroemielle.BackgroundImageLayout = ImageLayout.Stretch;
            pbStroemielle.Location = new Point(493, 380);
            pbStroemielle.Name = "pbStroemielle";
            pbStroemielle.Size = new Size(86, 116);
            pbStroemielle.TabIndex = 13;
            pbStroemielle.TabStop = false;
            // 
            // dbStroemielle
            // 
            dbStroemielle.BackColor = Color.Transparent;
            dbStroemielle.BackgroundImage = (Image)resources.GetObject("dbStroemielle.BackgroundImage");
            dbStroemielle.BackgroundImageLayout = ImageLayout.Center;
            dbStroemielle.Controls.Add(StartGameBtn);
            dbStroemielle.Controls.Add(lblDialogue);
            dbStroemielle.Dock = DockStyle.Bottom;
            dbStroemielle.Location = new Point(0, 310);
            dbStroemielle.Name = "dbStroemielle";
            dbStroemielle.Size = new Size(1262, 343);
            dbStroemielle.TabIndex = 14;
            // 
            // StartGameBtn
            // 
            StartGameBtn.BackColor = Color.PapayaWhip;
            StartGameBtn.FlatAppearance.BorderColor = Color.Gold;
            StartGameBtn.FlatAppearance.BorderSize = 2;
            StartGameBtn.FlatStyle = FlatStyle.Flat;
            StartGameBtn.Font = new Font("Calisto MT", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            StartGameBtn.ForeColor = Color.Goldenrod;
            StartGameBtn.Location = new Point(840, 260);
            StartGameBtn.Name = "StartGameBtn";
            StartGameBtn.Size = new Size(224, 46);
            StartGameBtn.TabIndex = 7;
            StartGameBtn.Text = "Play";
            StartGameBtn.UseVisualStyleBackColor = false;
            StartGameBtn.Click += StartGameBtn_Click;
            // 
            // lblDialogue
            // 
            lblDialogue.Font = new Font("Calisto MT", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDialogue.Location = new Point(381, 175);
            lblDialogue.Margin = new Padding(0);
            lblDialogue.MaximumSize = new Size(691, 66);
            lblDialogue.Name = "lblDialogue";
            lblDialogue.Size = new Size(663, 66);
            lblDialogue.TabIndex = 0;
            lblDialogue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // LunavaireGrooveForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1262, 653);
            Controls.Add(dbStroemielle);
            Controls.Add(pbStroemielle);
            Controls.Add(MapBtn);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "LunavaireGrooveForm";
            StartPosition = FormStartPosition.CenterScreen;
            Load += LunavaireGrooveForm_Load;
            Paint += LunavaireGrooveForm_Paint;
            KeyUp += LunavaireGrooveForm_KeyUp;
            ((System.ComponentModel.ISupportInitialize)pbStroemielle).EndInit();
            dbStroemielle.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Button MapBtn;
        private PictureBox pbStroemielle;
        private Panel dbStroemielle;
        private Button StartGameBtn;
        private Label lblDialogue;
    }
}