namespace Echoes_of_Montmauve
{
    partial class SignUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignUp));
            tableLayoutPanel1 = new TableLayoutPanel();
            SignUpPnl = new Panel();
            label6 = new Label();
            BdaydateTimePicker = new DateTimePicker();
            GenderCombBox = new ComboBox();
            label5 = new Label();
            label4 = new Label();
            AgetxtBox = new TextBox();
            UsernameTxtBox = new TextBox();
            SignUp2Btn = new Button();
            label2 = new Label();
            label3 = new Label();
            label1 = new Label();
            PasswordTxtBox = new TextBox();
            tableLayoutPanel1.SuspendLayout();
            SignUpPnl.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.184845F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90.815155F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105F));
            tableLayoutPanel1.Controls.Add(SignUpPnl, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.6559715F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 87.34403F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 87F));
            tableLayoutPanel1.Size = new Size(1095, 649);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // SignUpPnl
            // 
            SignUpPnl.BackColor = Color.PeachPuff;
            SignUpPnl.Controls.Add(label6);
            SignUpPnl.Controls.Add(BdaydateTimePicker);
            SignUpPnl.Controls.Add(GenderCombBox);
            SignUpPnl.Controls.Add(label5);
            SignUpPnl.Controls.Add(label4);
            SignUpPnl.Controls.Add(AgetxtBox);
            SignUpPnl.Controls.Add(UsernameTxtBox);
            SignUpPnl.Controls.Add(SignUp2Btn);
            SignUpPnl.Controls.Add(label2);
            SignUpPnl.Controls.Add(label3);
            SignUpPnl.Controls.Add(label1);
            SignUpPnl.Controls.Add(PasswordTxtBox);
            SignUpPnl.Dock = DockStyle.Fill;
            SignUpPnl.Location = new Point(93, 75);
            SignUpPnl.Margin = new Padding(3, 4, 3, 4);
            SignUpPnl.Name = "SignUpPnl";
            SignUpPnl.Size = new Size(893, 482);
            SignUpPnl.TabIndex = 0;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Calisto MT", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(292, 297);
            label6.Name = "label6";
            label6.Size = new Size(71, 22);
            label6.TabIndex = 11;
            label6.Text = "Gender";
            // 
            // BdaydateTimePicker
            // 
            BdaydateTimePicker.CalendarMonthBackground = Color.IndianRed;
            BdaydateTimePicker.CalendarTitleBackColor = Color.LightSalmon;
            BdaydateTimePicker.CalendarTrailingForeColor = Color.AntiqueWhite;
            BdaydateTimePicker.Font = new Font("Calisto MT", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            BdaydateTimePicker.Location = new Point(483, 232);
            BdaydateTimePicker.Name = "BdaydateTimePicker";
            BdaydateTimePicker.Size = new Size(291, 25);
            BdaydateTimePicker.TabIndex = 10;
            // 
            // GenderCombBox
            // 
            GenderCombBox.DropDownStyle = ComboBoxStyle.DropDownList;
            GenderCombBox.FormattingEnabled = true;
            GenderCombBox.Items.AddRange(new object[] { "Male", "Female", "Non-Binary" });
            GenderCombBox.Location = new Point(323, 322);
            GenderCombBox.Name = "GenderCombBox";
            GenderCombBox.Size = new Size(268, 28);
            GenderCombBox.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Calisto MT", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(483, 206);
            label5.Name = "label5";
            label5.Size = new Size(89, 22);
            label5.TabIndex = 8;
            label5.Text = "Birthdate:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Calisto MT", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(483, 98);
            label4.Name = "label4";
            label4.Size = new Size(48, 22);
            label4.TabIndex = 7;
            label4.Text = "Age:";
            // 
            // AgetxtBox
            // 
            AgetxtBox.BackColor = Color.Linen;
            AgetxtBox.BorderStyle = BorderStyle.None;
            AgetxtBox.Font = new Font("Calisto MT", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            AgetxtBox.Location = new Point(483, 131);
            AgetxtBox.Margin = new Padding(3, 4, 3, 4);
            AgetxtBox.Name = "AgetxtBox";
            AgetxtBox.Size = new Size(291, 27);
            AgetxtBox.TabIndex = 6;
            // 
            // UsernameTxtBox
            // 
            UsernameTxtBox.BackColor = Color.Linen;
            UsernameTxtBox.BorderStyle = BorderStyle.None;
            UsernameTxtBox.Font = new Font("Calisto MT", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            UsernameTxtBox.Location = new Point(84, 131);
            UsernameTxtBox.Margin = new Padding(3, 4, 3, 4);
            UsernameTxtBox.Name = "UsernameTxtBox";
            UsernameTxtBox.Size = new Size(301, 27);
            UsernameTxtBox.TabIndex = 1;
            // 
            // SignUp2Btn
            // 
            SignUp2Btn.BackColor = Color.Bisque;
            SignUp2Btn.FlatAppearance.BorderColor = Color.Bisque;
            SignUp2Btn.FlatAppearance.MouseOverBackColor = Color.NavajoWhite;
            SignUp2Btn.FlatStyle = FlatStyle.Flat;
            SignUp2Btn.Font = new Font("Calisto MT", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            SignUp2Btn.ForeColor = Color.Sienna;
            SignUp2Btn.Location = new Point(229, 395);
            SignUp2Btn.Margin = new Padding(3, 4, 3, 4);
            SignUp2Btn.Name = "SignUp2Btn";
            SignUp2Btn.Size = new Size(437, 52);
            SignUp2Btn.TabIndex = 5;
            SignUp2Btn.Text = "Sign Up";
            SignUp2Btn.UseVisualStyleBackColor = false;
            SignUp2Btn.Click += SignUp2Btn_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Calisto MT", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(84, 98);
            label2.Name = "label2";
            label2.Size = new Size(102, 22);
            label2.TabIndex = 3;
            label2.Text = "Username: ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Calisto MT", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(84, 206);
            label3.Name = "label3";
            label3.Size = new Size(96, 22);
            label3.TabIndex = 4;
            label3.Text = "Password: ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calisto MT", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.SaddleBrown;
            label1.Location = new Point(352, 9);
            label1.Name = "label1";
            label1.Size = new Size(190, 53);
            label1.TabIndex = 0;
            label1.Text = "Sign Up";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PasswordTxtBox
            // 
            PasswordTxtBox.BackColor = Color.Linen;
            PasswordTxtBox.BorderStyle = BorderStyle.None;
            PasswordTxtBox.Font = new Font("Calisto MT", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            PasswordTxtBox.Location = new Point(84, 232);
            PasswordTxtBox.Margin = new Padding(3, 4, 3, 4);
            PasswordTxtBox.Name = "PasswordTxtBox";
            PasswordTxtBox.Size = new Size(301, 27);
            PasswordTxtBox.TabIndex = 2;
            // 
            // SignUp
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1095, 649);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new Padding(3, 4, 3, 4);
            Name = "SignUp";
            StartPosition = FormStartPosition.CenterScreen;
            KeyDown += SignUp_KeyDown;
            tableLayoutPanel1.ResumeLayout(false);
            SignUpPnl.ResumeLayout(false);
            SignUpPnl.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Panel SignUpPnl;
        private Label label1;
        private TextBox PasswordTxtBox;
        private Label label3;
        private Label label2;
        private Button SignUp2Btn;
        private TextBox UsernameTxtBox;
        private DateTimePicker BdaydateTimePicker;
        private ComboBox GenderCombBox;
        private Label label5;
        private Label label4;
        private TextBox AgetxtBox;
        private Label label6;
    }
}