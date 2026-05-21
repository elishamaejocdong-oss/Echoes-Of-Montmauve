using Echoes_of_Montmauve.GameLogic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Echoes_of_Montmauve.Admin
{
    public partial class FeedbackForm : Form
    {
        private readonly Label lblSummary = new Label();
        private readonly DataGridView dgvFeedback = new DataGridView();

        public FeedbackForm()
        {
            InitializeComponent();
            BuildUI();
            LoadFeedback();
        }

        private void BuildUI()
        {
            Text = "Player Feedback";
            ClientSize = new Size(980, 640);
            BackColor = Color.FromArgb(34, 22, 8);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label
            {
                Text = "PLAYER FEEDBACK",
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

            dgvFeedback.Location = new Point(24, 150);
            dgvFeedback.Size = new Size(932, 400);
            dgvFeedback.AllowUserToAddRows = false;
            dgvFeedback.ReadOnly = true;
            dgvFeedback.RowHeadersVisible = false;
            dgvFeedback.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFeedback.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFeedback.BackgroundColor = Color.FromArgb(55, 38, 18);
            dgvFeedback.BorderStyle = BorderStyle.None;
            StyleGrid(dgvFeedback);
            Controls.Add(dgvFeedback);

            Button btnClose = MakeButton("Close", new Point(390, 570));
            btnClose.Click += (s, e) => Close();
            Controls.Add(btnClose);
        }

        private void LoadFeedback()
        {
            lblSummary.Text = DatabaseManager.GetFeedbackSummary();
            DataTable data = DatabaseManager.GetPlayerFeedback();
            dgvFeedback.DataSource = data;

            if (dgvFeedback.Columns.Count == 0) return;

            dgvFeedback.Columns["Username"].HeaderText = "Player";
            dgvFeedback.Columns["Rating"].HeaderText = "Rating";
            dgvFeedback.Columns["Comment"].HeaderText = "Comment";
            dgvFeedback.Columns["SubmittedOn"].HeaderText = "Submitted";
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
