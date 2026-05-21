using Echoes_of_Montmauve.GameLogic;
using System.Drawing;
using System.Windows.Forms;

namespace Echoes_of_Montmauve.Admin
{
    public partial class AdminMenu : Form
    {
        public AdminMenu()
        {
            InitializeComponent();
            BuildAdminButtons();
        }

        private void BuildAdminButtons()
        {
            Label title = new Label
            {
                Text = "ADMIN MENU",
                Font = new Font("Calisto MT", 26f, FontStyle.Bold),
                ForeColor = Color.Goldenrod,
                BackColor = Color.FromArgb(130, Color.Black),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(390, 95),
                Size = new Size(480, 64)
            };
            Controls.Add(title);

            Button btnAnalytics = MakeButton("Player Analytics", new Point(440, 230));
            btnAnalytics.Click += (s, e) =>
            {
                GameAnalytics analytics = new GameAnalytics();
                analytics.Show();
            };
            Controls.Add(btnAnalytics);

            Button btnFeedback = MakeButton("Player Feedback", new Point(440, 320));
            btnFeedback.Click += (s, e) =>
            {
                FeedbackForm feedback = new FeedbackForm();
                feedback.Show();
            };
            Controls.Add(btnFeedback);

            Button btnLogout = MakeButton("Log Out", new Point(440, 410));
            btnLogout.Click += (s, e) =>
            {
                SessionContent.CurrentActivePlayer = null;
                new SignIn().Show();
                Close();
            };
            Controls.Add(btnLogout);
        }

        private Button MakeButton(string text, Point location)
        {
            return new Button
            {
                Text = text,
                Font = new Font("Calisto MT", 16f, FontStyle.Bold),
                ForeColor = Color.Sienna,
                BackColor = Color.PapayaWhip,
                FlatStyle = FlatStyle.Flat,
                Location = location,
                Size = new Size(380, 66),
                FlatAppearance = { BorderColor = Color.SaddleBrown, BorderSize = 2 }
            };
        }
    }
}
