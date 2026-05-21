using Echoes_of_Montmauve.GameLogic;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Echoes_of_Montmauve
{
    public class ForgotPasswordForm : Form
    {
        private readonly TextBox txtUsername = new TextBox();
        private readonly DateTimePicker dtpBirthdate = new DateTimePicker();
        private readonly TextBox txtNewPassword = new TextBox();
        private readonly TextBox txtConfirmPassword = new TextBox();

        public ForgotPasswordForm()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            Text = "Forgot Password";
            ClientSize = new Size(520, 430);
            BackColor = Color.FromArgb(34, 22, 8);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            Label title = MakeLabel("RESET PASSWORD", 18, FontStyle.Bold, Color.Goldenrod);
            title.Location = new Point(0, 24);
            title.Size = new Size(ClientSize.Width, 36);
            title.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(title);

            AddField("Username", txtUsername, 76);
            AddDateField("Birthdate", dtpBirthdate, 146);
            AddField("New Password", txtNewPassword, 216);
            AddField("Confirm Password", txtConfirmPassword, 286);

            txtNewPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;

            Button btnReset = MakeButton("Reset Password", new Point(80, 356));
            btnReset.Click += BtnReset_Click;
            Controls.Add(btnReset);

            Button btnCancel = MakeButton("Cancel", new Point(270, 356));
            btnCancel.Click += (s, e) => Close();
            Controls.Add(btnCancel);
        }

        private void AddField(string label, TextBox box, int top)
        {
            Label lbl = MakeLabel(label, 11, FontStyle.Regular, Color.PapayaWhip);
            lbl.Location = new Point(80, top);
            lbl.Size = new Size(360, 22);
            Controls.Add(lbl);

            box.Location = new Point(80, top + 26);
            box.Size = new Size(360, 30);
            box.Font = new Font("Calisto MT", 12f);
            box.BackColor = Color.Linen;
            box.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(box);
        }

        private void AddDateField(string label, DateTimePicker picker, int top)
        {
            Label lbl = MakeLabel(label, 11, FontStyle.Regular, Color.PapayaWhip);
            lbl.Location = new Point(80, top);
            lbl.Size = new Size(360, 22);
            Controls.Add(lbl);

            picker.Location = new Point(80, top + 26);
            picker.Size = new Size(360, 30);
            picker.Font = new Font("Calisto MT", 12f);
            picker.Format = DateTimePickerFormat.Short;
            Controls.Add(picker);
        }

        private Label MakeLabel(string text, float size, FontStyle style, Color color)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Calisto MT", size, style),
                ForeColor = color,
                BackColor = Color.Transparent
            };
        }

        private Button MakeButton(string text, Point location)
        {
            return new Button
            {
                Text = text,
                Font = new Font("Calisto MT", 11f, FontStyle.Bold),
                ForeColor = Color.Goldenrod,
                BackColor = Color.FromArgb(55, 38, 18),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(170, 44),
                Location = location,
                FlatAppearance = { BorderColor = Color.Goldenrod, BorderSize = 2 }
            };
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Please enter your username and new password.", "Input Error");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("The new passwords do not match.", "Input Error");
                return;
            }

            if (!IsPasswordStrong(newPassword))
            {
                MessageBox.Show("Password must be at least 8 characters long and contain at least one uppercase letter and one number.", "Input Error");
                return;
            }

            if (DatabaseManager.ResetPassword(username, dtpBirthdate.Value.Date, newPassword))
            {
                MessageBox.Show("Password updated. You can now sign in with the new password.", "Password Reset");
                Close();
            }
            else
            {
                MessageBox.Show("Reset failed. Check that the username and birthdate match your account.", "Password Reset");
            }
        }

        private bool IsPasswordStrong(string password)
        {
            if (password.Length < 8) return false;

            bool hasUpper = false;
            bool hasNumber = false;
            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                if (char.IsDigit(c)) hasNumber = true;
            }
            return hasUpper && hasNumber;
        }
    }
}
