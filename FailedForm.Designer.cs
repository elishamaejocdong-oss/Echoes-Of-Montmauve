namespace Echoes_of_Montmauve
{
    partial class FailedForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FailedForm));
            btnTryAgain = new Button();
            btnExit = new Button();
            lblDescription = new Label();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnTryAgain
            // 
            btnTryAgain.BackColor = Color.AntiqueWhite;
            btnTryAgain.Font = new Font("Calisto MT", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTryAgain.ForeColor = Color.DarkGoldenrod;
            btnTryAgain.Location = new Point(298, 471);
            btnTryAgain.Name = "btnTryAgain";
            btnTryAgain.Size = new Size(239, 61);
            btnTryAgain.TabIndex = 4;
            btnTryAgain.Text = "Try Again";
            btnTryAgain.UseVisualStyleBackColor = false;
            btnTryAgain.Click += btnTryAgain_Click;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.AntiqueWhite;
            btnExit.Font = new Font("Calisto MT", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExit.ForeColor = Color.DarkGoldenrod;
            btnExit.Location = new Point(589, 471);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(239, 61);
            btnExit.TabIndex = 5;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // lblDescription
            // 
            lblDescription.BackColor = Color.Transparent;
            lblDescription.Font = new Font("Calisto MT", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDescription.ForeColor = Color.Sienna;
            lblDescription.Location = new Point(298, 320);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(530, 92);
            lblDescription.TabIndex = 6;
            lblDescription.Text = "Don't let the echoes fade just yet. Give it another go!";
            lblDescription.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(480, 158);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(132, 138);
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // FailedForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1077, 693);
            Controls.Add(pictureBox1);
            Controls.Add(lblDescription);
            Controls.Add(btnExit);
            Controls.Add(btnTryAgain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FailedForm";
            StartPosition = FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnTryAgain;
        private Button btnExit;
        private Label lblDescription;
        private PictureBox pictureBox1;
    }
}