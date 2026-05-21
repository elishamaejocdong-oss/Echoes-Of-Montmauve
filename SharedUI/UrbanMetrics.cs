using Echoes_of_Montmauve.GameLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Echoes_of_Montmauve.SharedUI
{
    public partial class UrbanMetrics : Form
    {
        private ProgressBar pgRestoration;
        private DataGridView dgvAnalytics;
        private DataGridView dgvSessions;
        private Label lblEndingUnlocked;

        // ── Theme (mirrors LeakFixFrenzy) ─────────────────────────────────
        private static readonly Color COL_BG_DARK = Color.FromArgb(34, 22, 8);
        private static readonly Color COL_BG_PANEL = Color.FromArgb(55, 38, 18);
        private static readonly Color COL_GOLD = Color.Goldenrod;
        private static readonly Color COL_CREAM = Color.PapayaWhip;
        private static readonly Font FONT_HUD = new Font("Calisto MT", 13f, FontStyle.Bold);
        private static readonly Font FONT_TITLE = new Font("Calisto MT", 18f, FontStyle.Bold);
        private static readonly Font FONT_BTN = new Font("Calisto MT", 12f, FontStyle.Bold);
        private static readonly Font FONT_GRID_HDR = new Font("Calisto MT", 10f, FontStyle.Bold);
        private static readonly Font FONT_GRID_CELL = new Font("Calisto MT", 9f);

        public UrbanMetrics()
        {
            InitializeComponent();
            BuildUI();
            LoadPlayerStats();
        }

        private void BuildUI()
        {
            // ── Form ───────────────────────────────────────────────────────
            this.Text = "Urban Metrics — Echoes of Montmauve";
            this.ClientSize = new Size(900, 640);
            this.BackColor = COL_BG_DARK;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // ── Title label ────────────────────────────────────────────────
            var lblTitle = new Label
            {
                Text = "URBAN METRICS",
                Font = FONT_TITLE,
                ForeColor = COL_GOLD,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(0, 18)
            };
            this.Controls.Add(lblTitle);
            lblTitle.Left = (this.ClientSize.Width - lblTitle.PreferredWidth) / 2;

            // ── Divider ────────────────────────────────────────────────────
            var divider = new Panel
            {
                BackColor = COL_GOLD,
                Size = new Size(860, 1),
                Location = new Point(20, 52)
            };
            this.Controls.Add(divider);

            // ── lblProgress ────────────────────────────────────────────────
            lblProgress = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Font = FONT_HUD,
                ForeColor = COL_GOLD,
                Location = new Point(20, 66),
                Text = "0% Urban Restoration"
            };
            this.Controls.Add(lblProgress);

            // ── lblMiasma ──────────────────────────────────────────────────
            lblMiasma = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Font = FONT_HUD,
                ForeColor = COL_CREAM,
                Location = new Point(20, 92),
                Text = "Miasma Level: 0"
            };
            this.Controls.Add(lblMiasma);

            lblEndingUnlocked = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Font = FONT_HUD,
                ForeColor = COL_CREAM,
                Location = new Point(520, 92),
                Text = "Ending Unlocked: Not yet unlocked"
            };
            this.Controls.Add(lblEndingUnlocked);

            // ── pgRestoration ──────────────────────────────────────────────
            pgRestoration = new ProgressBar
            {
                Location = new Point(20, 122),
                Size = new Size(860, 18),
                Style = ProgressBarStyle.Continuous,
                ForeColor = Color.ForestGreen,
                BackColor = COL_BG_PANEL
            };
            this.Controls.Add(pgRestoration);

            // ── Section label: Analytics ───────────────────────────────────
            this.Controls.Add(MakeSectionLabel("Game Analytics", 20, 152));

            // ── dgvAnalytics ───────────────────────────────────────────────
            dgvAnalytics = new DataGridView
            {
                Location = new Point(20, 174),
                Size = new Size(860, 180),
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                BackgroundColor = COL_BG_PANEL,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(80, 60, 20),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true
            };
            StyleGrid(dgvAnalytics);
            this.Controls.Add(dgvAnalytics);

            // ── Close button ───────────────────────────────────────────────
            var btnClose = MakeButton("Close", new Point(350, 556));
            btnClose.Click += btnCollect_Click;
            this.Controls.Add(btnClose);

            UIHelper.AddButtonScaleEffect(btnClose);
        }

        // ── Helpers ────────────────────────────────────────────────────────
        private Label MakeSectionLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Calisto MT", 11f, FontStyle.Bold | FontStyle.Italic),
                ForeColor = COL_GOLD,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(x, y)
            };
        }

        private Button MakeButton(string text, Point loc)
        {
            return new Button
            {
                Text = text,
                Font = FONT_BTN,
                ForeColor = COL_GOLD,
                BackColor = COL_BG_PANEL,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 50),
                Location = loc,
                FlatAppearance = { BorderColor = COL_GOLD, BorderSize = 2 }
            };
        }

        private void StyleGrid(DataGridView dgv)
        {
            // Column headers
            dgv.ColumnHeadersDefaultCellStyle.Font = FONT_GRID_HDR;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = COL_GOLD;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(42, 28, 8);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(42, 28, 8);
            dgv.EnableHeadersVisualStyles = false;

            // Rows
            dgv.DefaultCellStyle.Font = FONT_GRID_CELL;
            dgv.DefaultCellStyle.ForeColor = COL_CREAM;
            dgv.DefaultCellStyle.BackColor = COL_BG_PANEL;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 70, 20);
            dgv.DefaultCellStyle.SelectionForeColor = COL_GOLD;

            // Alternating rows
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(45, 32, 12);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = COL_CREAM;

            dgv.ColumnHeadersHeight = 30;
            dgv.RowTemplate.Height = 26;
            dgv.ScrollBars = ScrollBars.Vertical;
        }

        // ── Data ───────────────────────────────────────────────────────────
        public void LoadPlayerStats()
        {
            string user = SessionContent.CurrentActivePlayer.Username;

            int totalDistricts = 5;
            int purifiedCount = DatabaseManager.GetPurifiedCount(user);
            double progressPct = (double)purifiedCount / totalDistricts * 100;
            int miasma = DatabaseManager.GetMiasmaLevel(user);

            lblProgress.Text = $"{progressPct:0}% Urban Restoration";
            lblMiasma.Text = $"Miasma Level: {miasma}";
            lblEndingUnlocked.Text = $"Ending Unlocked: {DatabaseManager.GetPlayerEndingUnlocked(user)}";
            pgRestoration.Value = Math.Min((int)progressPct, 100);

            DataTable dt = DatabaseManager.GetPlayerAnalytics(user);
            dgvAnalytics.DataSource = dt;

            if (dgvAnalytics.Columns.Count > 0)
            {
                dgvAnalytics.Columns["GameName"].HeaderText = "Game";
                dgvAnalytics.Columns["TotalPlays"].HeaderText = "Played";
                dgvAnalytics.Columns["Wins"].HeaderText = "Wins";
                dgvAnalytics.Columns["Losses"].HeaderText = "Losses";
                dgvAnalytics.Columns["BestScore"].HeaderText = "Best Score";
                dgvAnalytics.Columns["BestTime"].HeaderText = "Best Time (s)";
                dgvAnalytics.Columns["WinRate"].HeaderText = "Win Rate";
            }

        }

        private void btnCollect_Click(object sender, EventArgs e)
        {
            MainMenu menu = new MainMenu();
            menu.Show();
            this.Close();
        }
    }
}
