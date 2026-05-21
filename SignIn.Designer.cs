namespace Echoes_of_Montmauve
{
    partial class SignIn
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignIn));
            SignInBtn = new Button();
            SignUpBtn = new Button();
            ExitBtn = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            pictureBox1 = new PictureBox();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // SignInBtn
            // 
            SignInBtn.BackColor = Color.Bisque;
            SignInBtn.Dock = DockStyle.Fill;
            SignInBtn.FlatAppearance.BorderColor = Color.Bisque;
            SignInBtn.FlatAppearance.BorderSize = 0;
            SignInBtn.FlatAppearance.MouseOverBackColor = Color.NavajoWhite;
            SignInBtn.FlatStyle = FlatStyle.Flat;
            SignInBtn.Font = new Font("Calisto MT", 18F, FontStyle.Bold);
            SignInBtn.ForeColor = Color.Sienna;
            SignInBtn.Location = new Point(28, 309);
            SignInBtn.Margin = new Padding(0);
            SignInBtn.Name = "SignInBtn";
            SignInBtn.Size = new Size(404, 73);
            SignInBtn.TabIndex = 0;
            SignInBtn.Text = "Sign In";
            SignInBtn.UseVisualStyleBackColor = false;
            SignInBtn.Click += SignInBtn_Click;
            // 
            // SignUpBtn
            // 
            SignUpBtn.BackColor = Color.Bisque;
            SignUpBtn.Dock = DockStyle.Fill;
            SignUpBtn.FlatAppearance.BorderColor = Color.Bisque;
            SignUpBtn.FlatAppearance.BorderSize = 0;
            SignUpBtn.FlatAppearance.MouseOverBackColor = Color.NavajoWhite;
            SignUpBtn.FlatStyle = FlatStyle.Flat;
            SignUpBtn.Font = new Font("Calisto MT", 18F, FontStyle.Bold);
            SignUpBtn.ForeColor = Color.Sienna;
            SignUpBtn.Location = new Point(28, 404);
            SignUpBtn.Margin = new Padding(0);
            SignUpBtn.Name = "SignUpBtn";
            SignUpBtn.Size = new Size(404, 88);
            SignUpBtn.TabIndex = 1;
            SignUpBtn.Text = "Sign Up";
            SignUpBtn.UseVisualStyleBackColor = false;
            SignUpBtn.Click += SignUpBtn_Click;
            // 
            // ExitBtn
            // 
            ExitBtn.BackColor = Color.Bisque;
            ExitBtn.Dock = DockStyle.Fill;
            ExitBtn.FlatAppearance.BorderColor = Color.Bisque;
            ExitBtn.FlatAppearance.BorderSize = 0;
            ExitBtn.FlatAppearance.MouseOverBackColor = Color.NavajoWhite;
            ExitBtn.FlatStyle = FlatStyle.Flat;
            ExitBtn.Font = new Font("Calisto MT", 18F, FontStyle.Bold);
            ExitBtn.ForeColor = Color.Sienna;
            ExitBtn.Location = new Point(28, 514);
            ExitBtn.Margin = new Padding(0);
            ExitBtn.Name = "ExitBtn";
            ExitBtn.Size = new Size(404, 76);
            ExitBtn.TabIndex = 2;
            ExitBtn.Text = "Exit";
            ExitBtn.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 6.526994F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 93.47301F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 829F));
            tableLayoutPanel1.Controls.Add(ExitBtn, 1, 6);
            tableLayoutPanel1.Controls.Add(SignInBtn, 1, 2);
            tableLayoutPanel1.Controls.Add(SignUpBtn, 1, 4);
            tableLayoutPanel1.Controls.Add(pictureBox1, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 8;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 82.775116F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 17.22488F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 73F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            tableLayoutPanel1.Size = new Size(1262, 653);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(31, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(398, 250);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // SignIn
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.The_Echos_of_Montmauve_Urban_Illusion__974_x_494_px_;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1262, 653);
            Controls.Add(tableLayoutPanel1);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "SignIn";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Echoes of Montmauve: Urban Illusion";
            Load += SignIn_Load;
            Shown += SignIn_Shown;
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button SignInBtn;
        private Button SignUpBtn;
        private Button ExitBtn;
        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox pictureBox1;
    }
}
