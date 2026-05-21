using Echoes_of_Montmauve.GameLogic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Echoes_of_Montmauve.Admin
{
    public partial class GameAnalytics : Form
    {
        private readonly Label lblSummary = new Label();
        private readonly DataGridView dgvAnalytics = new DataGridView();
        private readonly DataGridView dgvEndings = new DataGridView();

        public GameAnalytics()
        {
            InitializeComponent();
            BuildUI();
            LoadAnalytics();
        }

        private void BuildUI()
        {
            Text = "Player Analytics";
            ClientSize = new Size(980, 640);
            BackColor = Color.FromArgb(34, 22, 8);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label
            {
                Text = "PLAYER ANALYTICS",
                Font = new Font("Calisto MT", 20f, FontStyle.Bold),
                ForeColor = Color.Goldenrod,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 20),
                Size = new Size(ClientSize.Width, 40)
            };
            Controls.Add(title);

            lblSummary.Font = new Font("Calisto MT", 11f, FontStyle.Bold);
            lblSummary.ForeColor = Color.PapayaWhip;
            lblSummary.BackColor = Color.FromArgb(55, 38, 18);
            lblSummary.Location = new Point(24, 76);
            lblSummary.Size = new Size(932, 54);
            lblSummary.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(lblSummary);

            dgvAnalytics.Location = new Point(24, 150);
            dgvAnalytics.Size = new Size(932, 260);
            dgvAnalytics.AllowUserToAddRows = false;
            dgvAnalytics.ReadOnly = true;
            dgvAnalytics.RowHeadersVisible = false;
            dgvAnalytics.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAnalytics.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAnalytics.BackgroundColor = Color.FromArgb(55, 38, 18);
            dgvAnalytics.BorderStyle = BorderStyle.None;
            StyleGrid(dgvAnalytics);
            Controls.Add(dgvAnalytics);

            Label storyTitle = new Label
            {
                Text = "STORY ENDINGS",
                Font = new Font("Calisto MT", 13f, FontStyle.Bold | FontStyle.Italic),
                ForeColor = Color.Goldenrod,
                BackColor = Color.Transparent,
                Location = new Point(24, 424),
                Size = new Size(240, 28)
            };
            Controls.Add(storyTitle);

            dgvEndings.Location = new Point(24, 456);
            dgvEndings.Size = new Size(932, 94);
            dgvEndings.AllowUserToAddRows = false;
            dgvEndings.ReadOnly = true;
            dgvEndings.RowHeadersVisible = false;
            dgvEndings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEndings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEndings.BackgroundColor = Color.FromArgb(55, 38, 18);
            dgvEndings.BorderStyle = BorderStyle.None;
            StyleGrid(dgvEndings);
            Controls.Add(dgvEndings);

            Button btnClose = MakeButton("Close", new Point(390, 570));
            btnClose.Click += (s, e) => Close();
            Controls.Add(btnClose);
        }

        private void LoadAnalytics()
        {
            lblSummary.Text = DatabaseManager.GetAdminAnalyticsSummary();
            DataTable data = DatabaseManager.GetAdminGameAnalytics();
            dgvAnalytics.DataSource = data;

            if (dgvAnalytics.Columns.Count == 0) return;

            dgvAnalytics.Columns["GameName"].HeaderText = "Game";
            dgvAnalytics.Columns["TotalPlays"].HeaderText = "Total Plays";
            dgvAnalytics.Columns["Wins"].HeaderText = "Wins";
            dgvAnalytics.Columns["Losses"].HeaderText = "Losses";
            dgvAnalytics.Columns["BestScore"].HeaderText = "Best Score";
            dgvAnalytics.Columns["AverageScore"].HeaderText = "Average Score";
            dgvAnalytics.Columns["FastestWin"].HeaderText = "Fastest Win";
            dgvAnalytics.Columns["WinRate"].HeaderText = "Win Rate";

            DataTable endingData = DatabaseManager.GetAdminEndingAnalytics();
            dgvEndings.DataSource = endingData;

            if (dgvEndings.Columns.Count == 0) return;

            dgvEndings.Columns["EndingUnlocked"].HeaderText = "Ending Unlocked";
            dgvEndings.Columns["Players"].HeaderText = "Players";
        }

        private void StyleGrid(DataGridView grid)
        {
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Calisto MT", 10f, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Goldenrod;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(42, 28, 8);
            grid.DefaultCellStyle.Font = new Font("Calisto MT", 9f);
            grid.DefaultCellStyle.ForeColor = Color.PapayaWhip;
            grid.DefaultCellStyle.BackColor = Color.FromArgb(55, 38, 18);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 70, 20);
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(45, 32, 12);
            grid.AlternatingRowsDefaultCellStyle.ForeColor = Color.PapayaWhip;
        }

        private Button MakeButton(string text, Point location)
        {
            return new Button
            {
                Text = text,
                Font = new Font("Calisto MT", 13f, FontStyle.Bold),
                ForeColor = Color.Goldenrod,
                BackColor = Color.FromArgb(55, 38, 18),
                FlatStyle = FlatStyle.Flat,
                Location = location,
                Size = new Size(200, 48),
                FlatAppearance = { BorderColor = Color.Goldenrod, BorderSize = 2 }
            };
        }
    }
}
