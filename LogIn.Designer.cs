namespace Echoes_of_Montmauve
{
    partial class LogIn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogIn));
            tableLayoutPanel1 = new TableLayoutPanel();
            LogInPnl = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            label1 = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            PasswordTxtBox = new TextBox();
            label3 = new Label();
            label2 = new Label();
            UsernameTxtBox = new TextBox();
            LogInBtn = new Button();
            ForgotPasswordBtn = new Button();
            tableLayoutPanel1.SuspendLayout();
            LogInPnl.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.184845F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90.815155F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tableLayoutPanel1.Controls.Add(LogInPnl, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.9263916F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 87.07361F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 91F));
            tableLayoutPanel1.Size = new Size(1095, 649);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // LogInPnl
            // 
            LogInPnl.BackColor = Color.PeachPuff;
            LogInPnl.Controls.Add(tableLayoutPanel2);
            LogInPnl.Controls.Add(tableLayoutPanel3);
            LogInPnl.Dock = DockStyle.Fill;
            LogInPnl.Location = new Point(93, 76);
            LogInPnl.Margin = new Padding(3, 4, 3, 4);
            LogInPnl.Name = "LogInPnl";
            LogInPnl.Size = new Size(888, 477);
            LogInPnl.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.Transparent;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Top;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(888, 93);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Calisto MT", 27.75F, FontStyle.Bold);
            label1.ForeColor = Color.SaddleBrown;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(882, 93);
            label1.TabIndex = 0;
            label1.Text = "Sign In";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.Transparent;
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.6012878F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66.39871F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 227F));
            tableLayoutPanel3.Controls.Add(PasswordTxtBox, 1, 3);
            tableLayoutPanel3.Controls.Add(label3, 1, 2);
            tableLayoutPanel3.Controls.Add(label2, 1, 0);
            tableLayoutPanel3.Controls.Add(UsernameTxtBox, 1, 1);
            tableLayoutPanel3.Controls.Add(LogInBtn, 1, 5);
            tableLayoutPanel3.Controls.Add(ForgotPasswordBtn, 1, 6);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 7;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 74.40476F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 25.5952377F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
            tableLayoutPanel3.Size = new Size(888, 477);
            tableLayoutPanel3.TabIndex = 5;
            // 
            // PasswordTxtBox
            // 
            PasswordTxtBox.BackColor = Color.Linen;
            PasswordTxtBox.BorderStyle = BorderStyle.None;
            PasswordTxtBox.Dock = DockStyle.Fill;
            PasswordTxtBox.Font = new Font("Calisto MT", 16.2F);
            PasswordTxtBox.Location = new Point(225, 254);
            PasswordTxtBox.Margin = new Padding(3, 4, 3, 4);
            PasswordTxtBox.Name = "PasswordTxtBox";
            PasswordTxtBox.Size = new Size(432, 32);
            PasswordTxtBox.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Dock = DockStyle.Bottom;
            label3.Font = new Font("Calisto MT", 11.25F);
            label3.Location = new Point(225, 228);
            label3.Name = "label3";
            label3.Size = new Size(432, 22);
            label3.TabIndex = 4;
            label3.Text = "Password: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Dock = DockStyle.Bottom;
            label2.Font = new Font("Calisto MT", 11.25F);
            label2.Location = new Point(225, 143);
            label2.Name = "label2";
            label2.Size = new Size(432, 22);
            label2.TabIndex = 3;
            label2.Text = "Username: ";
            // 
            // UsernameTxtBox
            // 
            UsernameTxtBox.BackColor = Color.Linen;
            UsernameTxtBox.BorderStyle = BorderStyle.None;
            UsernameTxtBox.Dock = DockStyle.Fill;
            UsernameTxtBox.Font = new Font("Calisto MT", 16.2F);
            UsernameTxtBox.Location = new Point(225, 169);
            UsernameTxtBox.Margin = new Padding(3, 4, 3, 4);
            UsernameTxtBox.Name = "UsernameTxtBox";
            UsernameTxtBox.Size = new Size(432, 32);
            UsernameTxtBox.TabIndex = 1;
            // 
            // LogInBtn
            // 
            LogInBtn.BackColor = Color.Bisque;
            LogInBtn.Dock = DockStyle.Fill;
            LogInBtn.FlatAppearance.BorderColor = Color.Bisque;
            LogInBtn.FlatAppearance.MouseOverBackColor = Color.NavajoWhite;
            LogInBtn.FlatStyle = FlatStyle.Flat;
            LogInBtn.Font = new Font("Calisto MT", 18F, FontStyle.Bold);
            LogInBtn.ForeColor = Color.Sienna;
            LogInBtn.Location = new Point(225, 344);
            LogInBtn.Margin = new Padding(3, 4, 3, 4);
            LogInBtn.Name = "LogInBtn";
            LogInBtn.Size = new Size(432, 52);
            LogInBtn.TabIndex = 5;
            LogInBtn.Text = "Sign In";
            LogInBtn.UseVisualStyleBackColor = false;
            LogInBtn.Click += LogInBtn_Click;
            // 
            // ForgotPasswordBtn
            // 
            ForgotPasswordBtn.BackColor = Color.Transparent;
            ForgotPasswordBtn.Dock = DockStyle.Top;
            ForgotPasswordBtn.FlatAppearance.BorderSize = 0;
            ForgotPasswordBtn.FlatAppearance.MouseOverBackColor = Color.NavajoWhite;
            ForgotPasswordBtn.FlatStyle = FlatStyle.Flat;
            ForgotPasswordBtn.Font = new Font("Calisto MT", 11F, FontStyle.Bold);
            ForgotPasswordBtn.ForeColor = Color.Sienna;
            ForgotPasswordBtn.Location = new Point(225, 404);
            ForgotPasswordBtn.Margin = new Padding(3, 4, 3, 4);
            ForgotPasswordBtn.Name = "ForgotPasswordBtn";
            ForgotPasswordBtn.Size = new Size(432, 36);
            ForgotPasswordBtn.TabIndex = 6;
            ForgotPasswordBtn.Text = "Forgot Password?";
            ForgotPasswordBtn.UseVisualStyleBackColor = false;
            ForgotPasswordBtn.Click += ForgotPasswordBtn_Click;
            // 
            // LogIn
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1095, 649);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "LogIn";
            StartPosition = FormStartPosition.CenterScreen;
            tableLayoutPanel1.ResumeLayout(false);
            LogInPnl.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Panel LogInPnl;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label1;
        private TextBox UsernameTxtBox;
        private Label label3;
        private Label label2;
        private TextBox PasswordTxtBox;
        private TableLayoutPanel tableLayoutPanel3;
        private Button LogInBtn;
        private Button ForgotPasswordBtn;
    }
}
