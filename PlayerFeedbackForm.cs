using Echoes_of_Montmauve.GameLogic;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Echoes_of_Montmauve
{
    public class PlayerFeedbackForm : Form
    {
        private readonly NumericUpDown nudRating = new NumericUpDown();
        private readonly TextBox txtComment = new TextBox();

        public PlayerFeedbackForm()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            Text = "Rate the Game";
            ClientSize = new Size(620, 470);
            BackColor = Color.FromArgb(34, 22, 8);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label
            {
                Text = "PLAYER FEEDBACK",
                Font = new Font("Calisto MT", 18f, FontStyle.Bold),
                ForeColor = Color.Goldenrod,
                BackColor = Color.Transparent,
                Location = new Point(0, 24),
                Size = new Size(ClientSize.Width, 36),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(title);

            Label ratingLabel = MakeLabel("Rating (1-5)", 80);
            Controls.Add(ratingLabel);

            nudRating.Location = new Point(80, 112);
            nudRating.Size = new Size(120, 30);
            nudRating.Minimum = 1;
            nudRating.Maximum = 5;
            nudRating.Value = 5;
            nudRating.Font = new Font("Calisto MT", 12f);
            Controls.Add(nudRating);

            Label commentLabel = MakeLabel("Comment", 162);
            Controls.Add(commentLabel);

            txtComment.Location = new Point(80, 194);
            txtComment.Size = new Size(460, 170);
            txtComment.Multiline = true;
            txtComment.ScrollBars = ScrollBars.Vertical;
            txtComment.Font = new Font("Calisto MT", 12f);
            txtComment.BackColor = Color.Linen;
            Controls.Add(txtComment);

            Button btnSubmit = MakeButton("Submit", new Point(130, 390));
            btnSubmit.Click += BtnSubmit_Click;
            Controls.Add(btnSubmit);

            Button btnCancel = MakeButton("Cancel", new Point(320, 390));
            btnCancel.Click += (s, e) => ReturnToMenu();
            Controls.Add(btnCancel);
        }

        private Label MakeLabel(string text, int top)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Calisto MT", 11f),
                ForeColor = Color.PapayaWhip,
                BackColor = Color.Transparent,
                Location = new Point(80, top),
                Size = new Size(460, 24)
            };
        }

        private Button MakeButton(string text, Point location)
        {
            return new Button
            {
                Text = text,
                Font = new Font("Calisto MT", 12f, FontStyle.Bold),
                ForeColor = Color.Goldenrod,
                BackColor = Color.FromArgb(55, 38, 18),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(170, 46),
                Location = location,
                FlatAppearance = { BorderColor = Color.Goldenrod, BorderSize = 2 }
            };
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            string username = SessionContent.CurrentActivePlayer?.Username ?? "Unknown";
            string comment = txtComment.Text.Trim();

            if (DatabaseManager.SubmitPlayerFeedback(username, (int)nudRating.Value, comment))
            {
                MessageBox.Show("Thank you for rating Echoes of Montmauve.", "Feedback Saved");
                ReturnToMenu();
            }
        }

        private void ReturnToMenu()
        {
            MainMenu menu = new MainMenu();
            menu.Show();
            Close();
        }
    }
}
