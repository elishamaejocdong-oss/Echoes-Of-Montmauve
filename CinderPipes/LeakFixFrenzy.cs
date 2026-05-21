using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Echoes_of_Montmauve.GameLogic;
using Echoes_of_Montmauve.SharedUI;

namespace Echoes_of_Montmauve
{
    public class LeakFixFrenzy : Form
    {
        private int score = 0;
        private int combo = 0;
        private int maxCombo = 0;
        private int lives = 3;
        private int timeLeft = 60;
        private int difficultyLevel = 1;
        private bool gameRunning = false;
        private bool gameOver = false;
        private DateTime _startTime;

        private Label lblScore;
        private Label lblCombo;
        private Label lblTimer;
        private Label lblLives;
        private Label lblLevel;
        private Label lblMessage;
        private Button btnStart;
        private Button btnBack;
        private Panel gamePanel;

        private System.Windows.Forms.Timer countdownTimer;
        private System.Windows.Forms.Timer spawnTimer;
        private System.Windows.Forms.Timer messageTimer;

        private List<LeakButton> activeLeaks = new List<LeakButton>();
        private Random rng = new Random();

        private const int FORM_W = 1112;
        private const int FORM_H = 685;
        private const int HUD_H = 80;
        private const int PANEL_PAD = 20;

        private static readonly Color COL_BG_DARK = Color.FromArgb(34, 22, 8);
        private static readonly Color COL_BG_PANEL = Color.FromArgb(55, 38, 18);
        private static readonly Color COL_GOLD = Color.Goldenrod;
        private static readonly Color COL_CREAM = Color.PapayaWhip;
        private static readonly Color COL_RED = Color.IndianRed;
        private static readonly Color COL_LEAK_SMALL = Color.FromArgb(60, 140, 200);
        private static readonly Color COL_LEAK_BIG = Color.FromArgb(30, 90, 160);
        private static readonly Color COL_FAKE = Color.FromArgb(170, 90, 30);
        private static readonly Font FONT_HUD = new Font("Calisto MT", 13f, FontStyle.Bold);
        private static readonly Font FONT_TITLE = new Font("Calisto MT", 18f, FontStyle.Bold);
        private static readonly Font FONT_BTN = new Font("Calisto MT", 12f, FontStyle.Bold);
        private static readonly Font FONT_LEAK = new Font("Calisto MT", 10f, FontStyle.Bold);

        public LeakFixFrenzy()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            Text = "Leak Fix Frenzy — Echoes of Montmauve";
            ClientSize = new Size(FORM_W, FORM_H);
            BackColor = COL_BG_DARK;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            KeyPreview = true;

            int col = 20;
            lblScore = MakeHudLabel("Score: 0", col, 20); col += 200;
            lblCombo = MakeHudLabel("Combo: x1", col, 20); col += 200;
            lblTimer = MakeHudLabel("Time: 60", col, 20); col += 200;
            lblLives = MakeHudLabel("Lives: ♥♥♥", col, 20); col += 230;
            lblLevel = MakeHudLabel("Level: 1", col, 20);

            lblMessage = new Label
            {
                AutoSize = false,
                Size = new Size(300, 40),
                Location = new Point(FORM_W / 2 - 150, FORM_H / 2 - 20),
                Font = FONT_TITLE,
                ForeColor = COL_GOLD,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };
            Controls.Add(lblMessage);

            gamePanel = new Panel
            {
                Location = new Point(PANEL_PAD, HUD_H),
                Size = new Size(FORM_W - PANEL_PAD * 2, FORM_H - HUD_H - PANEL_PAD),
                BackColor = Color.FromArgb(42, 30, 12),
                BorderStyle = BorderStyle.FixedSingle
            };
            gamePanel.Paint += GamePanel_Paint;
            Controls.Add(gamePanel);

            btnStart = MakeButton("Start Game", new Point(FORM_W / 2 - 100, FORM_H / 2 - 25));
            btnStart.Click += BtnStart_Click;
            Controls.Add(btnStart);

            btnBack = MakeButton("← Back", new Point(20, FORM_H - 55));
            btnBack.Size = new Size(110, 36);
            btnBack.Click += BtnBack_Click;
            Controls.Add(btnBack);

            countdownTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            countdownTimer.Tick += CountdownTimer_Tick;

            spawnTimer = new System.Windows.Forms.Timer { Interval = GetSpawnInterval() };
            spawnTimer.Tick += SpawnTimer_Tick;

            messageTimer = new System.Windows.Forms.Timer { Interval = 800, Enabled = false };
            messageTimer.Tick += (s, e) => { lblMessage.Visible = false; messageTimer.Stop(); };

            Controls.Add(lblScore);
            Controls.Add(lblCombo);
            Controls.Add(lblTimer);
            Controls.Add(lblLives);
            Controls.Add(lblLevel);

            UIHelper.AddButtonScaleEffect(btnStart);
            UIHelper.AddButtonScaleEffect(btnBack);

            DrawIdleScreen();
        }

        private Label MakeHudLabel(string text, int x, int y)
        {
            var l = new Label
            {
                Text = text,
                Font = FONT_HUD,
                ForeColor = COL_GOLD,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(x, y)
            };
            return l;
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

        private void DrawIdleScreen()
        {
            gamePanel.Invalidate();
        }

        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw a simple stylised district grid
            using (var gridPen = new Pen(Color.FromArgb(60, 200, 160, 80), 1f))
            {
                int rows = 6, cols = 8;
                int cw = gamePanel.Width / cols;
                int rh = gamePanel.Height / rows;
                for (int r = 0; r <= rows; r++)
                    g.DrawLine(gridPen, 0, r * rh, gamePanel.Width, r * rh);
                for (int c = 0; c <= cols; c++)
                    g.DrawLine(gridPen, c * cw, 0, c * cw, gamePanel.Height);
            }

            if (!gameRunning && !gameOver)
            {
                using (var titleFont = new Font("Calisto MT", 26f, FontStyle.Bold))
                using (var sb = new SolidBrush(Color.FromArgb(200, COL_GOLD)))
                {
                    string title = "LEAK FIX FRENZY";
                    SizeF ts = g.MeasureString(title, titleFont);
                    g.DrawString(title, titleFont, sb,
                        (gamePanel.Width - ts.Width) / 2f,
                        gamePanel.Height / 2f - 100);
                }
                using (var descFont = new Font("Calisto MT", 12f, FontStyle.Italic))
                using (var sb = new SolidBrush(Color.FromArgb(180, COL_CREAM)))
                {
                    string[] lines = {
                        "Leaks are bursting across the district's water grid!",
                        "Tap them before the Miasma floods the Commons.",
                        "",
                        "BLUE = normal leak  |  LARGE BLUE = 2 hits  |  ORANGE = FAKE — ignore it!",
                        "Fix leaks quickly for combo bonuses."
                    };
                    float y = gamePanel.Height / 2f - 40;
                    foreach (var line in lines)
                    {
                        SizeF ls = g.MeasureString(line, descFont);
                        g.DrawString(line, descFont, sb, (gamePanel.Width - ls.Width) / 2f, y);
                        y += 26;
                    }
                }
            }

            if (gameOver)
            {
                using (var ovFont = new Font("Calisto MT", 28f, FontStyle.Bold))
                using (var sb = new SolidBrush(Color.FromArgb(220, COL_RED)))
                {
                    string msg = score >= 100 ? "District Saved!" : "The Miasma Won...";
                    SizeF ms = g.MeasureString(msg, ovFont);
                    g.DrawString(msg, ovFont, sb, (gamePanel.Width - ms.Width) / 2f, gamePanel.Height / 2f - 80);
                }
                using (var sf = new Font("Calisto MT", 14f, FontStyle.Bold))
                using (var sb = new SolidBrush(COL_GOLD))
                {
                    string stats = $"Final Score: {score}    Best Combo: x{maxCombo}";
                    SizeF ss = g.MeasureString(stats, sf);
                    g.DrawString(stats, sf, sb, (gamePanel.Width - ss.Width) / 2f, gamePanel.Height / 2f - 30);
                }
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (gameOver)
                ResetGame();

            StartGame();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            StopAll();
            CinderPipesMain intro = new CinderPipesMain();
            intro.Show();
            this.Close();
        }

        private void StartGame()
        {
            gameRunning = true;
            gameOver = false;
            _startTime = DateTime.Now;
            btnStart.Visible = false;
            gamePanel.Invalidate();

            spawnTimer.Interval = GetSpawnInterval();
            countdownTimer.Start();
            spawnTimer.Start();
        }

        private void ResetGame()
        {
            score = 0; combo = 0; maxCombo = 0; lives = 3; timeLeft = 60; difficultyLevel = 1;
            ClearLeaks();
            UpdateHUD();
        }

        private void StopAll()
        {
            countdownTimer.Stop();
            spawnTimer.Stop();
            ClearLeaks();
        }

        private void ClearLeaks()
        {
            foreach (var l in activeLeaks)
            {
                gamePanel.Controls.Remove(l);
                l.Dispose();
            }
            activeLeaks.Clear();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            lblTimer.Text = $"Time: {timeLeft}";

            difficultyLevel = 1 + (60 - timeLeft) / 15;
            lblLevel.Text = $"Level: {difficultyLevel}";
            spawnTimer.Interval = GetSpawnInterval();

            var expired = new List<LeakButton>();
            foreach (var leak in activeLeaks)
            {
                leak.Age++;
                if (leak.Age >= leak.Lifetime)
                    expired.Add(leak);
            }
            foreach (var leak in expired)
                MissLeak(leak);

            if (timeLeft <= 0)
                EndGame();
        }

        private void SpawnTimer_Tick(object sender, EventArgs e)
        {
            SpawnLeak();
        }

        private void SpawnLeak()
        {
            if (!gameRunning) return;
            if (activeLeaks.Count >= 8) return;

            int roll = rng.Next(100);
            LeakType type;
            if (roll < 15) type = LeakType.Fake;
            else if (roll < 35) type = LeakType.Big;
            else type = LeakType.Small;

            int margin = 30;
            int bSize = type == LeakType.Big ? 80 : 55;
            int x = rng.Next(margin, gamePanel.Width - bSize - margin);
            int y = rng.Next(margin, gamePanel.Height - bSize - margin);

            var leak = new LeakButton(type, bSize, FONT_LEAK)
            {
                Location = new Point(x, y)
            };
            leak.Click += Leak_Click;

            activeLeaks.Add(leak);
            gamePanel.Controls.Add(leak);
            leak.BringToFront();
        }

        private void Leak_Click(object sender, EventArgs e)
        {
            if (!(sender is LeakButton leak)) return;

            if (leak.IsFake)
            {
                combo = 0;
                LoseLife();
                ShowMessage("FAKE! -1 Life", COL_RED);
                RemoveLeak(leak);
                return;
            }

            leak.HitsLeft--;
            if (leak.HitsLeft <= 0)
            {
                combo++;
                if (combo > maxCombo) maxCombo = combo;

                int points = 10 + (combo > 1 ? (combo - 1) * 5 : 0);
                score += points;

                string msg = combo >= 5 ? $"MEGA COMBO x{combo}! +{points}"
                           : combo >= 3 ? $"Combo x{combo}! +{points}"
                           : $"+{points}";
                ShowMessage(msg, combo >= 3 ? COL_GOLD : COL_CREAM);
                RemoveLeak(leak);
            }
            else
            {
                leak.Size = new Size(leak.Width - 10, leak.Height - 10);
                leak.Invalidate();
            }

            UpdateHUD();
        }

        private void MissLeak(LeakButton leak)
        {
            if (!leak.IsFake)
            {
                combo = 0;
                LoseLife();
                ShowMessage("Leak missed! -1 Life", COL_RED);
            }
            RemoveLeak(leak);
            UpdateHUD();
        }

        private void RemoveLeak(LeakButton leak)
        {
            activeLeaks.Remove(leak);
            gamePanel.Controls.Remove(leak);
            leak.Dispose();
        }

        private void LoseLife()
        {
            lives = Math.Max(0, lives - 1);
            if (lives <= 0)
                EndGame();
        }

        private void EndGame()
        {
            StopAll();
            gameRunning = false;
            gameOver = true;

            int timeTaken = Math.Min(
                (int)(DateTime.Now - _startTime).TotalSeconds, 3600);

            string username = SessionContent.CurrentActivePlayer.Username;

            if (lives > 0)
            {
                // ── Victory: mirror MarrowFamilies pattern ──────────────────
                // Log session
                DatabaseManager.LogGameSession(
                    username, "Leak Fix Frenzy", "Cinder Pipes",
                    score, timeTaken, isVictory: true);

                DatabaseManager.UpdateDistrictPurified(
                    username, "Cinder Pipes", timeTaken);

                // Unlock the clue in the player's notebook
                DatabaseManager.UnlockNotebookItem(username, "Nexus Directive");

                // Flag for CinderPipesMain to trigger the Madman transition dialogue
                SessionContent.MinigameClearedForCurrentDistrict = true;

                // Return to the district hub so the Madman dialogue can fire
                CinderPipesMain cinderMain = new CinderPipesMain();
                cinderMain.Show();
                this.Close();
            }
            else
            {
                // ── Defeat ──────────────────────────────────────────────────
                DatabaseManager.LogGameSession(
                    username, "Leak Fix Frenzy", "Cinder Pipes",
                    score, timeTaken, isVictory: false);

                using (FailedForm failed = new FailedForm(RestartGame))
                {
                    failed.ShowDialog();
                }
            }
        }

        private void RestartGame()
        {
            ResetGame();
            StartGame();
        }

        private void UpdateHUD()
        {
            lblScore.Text = $"Score: {score}";
            lblCombo.Text = combo > 1 ? $"Combo: x{combo}!" : "Combo: x1";
            lblLives.Text = $"Lives: {new string('♥', lives)}{new string('♡', 3 - lives)}";
        }

        private void ShowMessage(string text, Color col)
        {
            lblMessage.Text = text;
            lblMessage.ForeColor = col;
            lblMessage.Visible = true;
            lblMessage.BringToFront();
            messageTimer.Stop();
            messageTimer.Start();
        }

        private int GetSpawnInterval()
        {
            return Math.Max(700, 2000 - difficultyLevel * 300);
        }

        private enum LeakType { Small, Big, Fake }

        private class LeakButton : Button
        {
            private bool _isFake;
            private int _hitsLeft;
            private int _age;
            private int _lifetime;

            [System.ComponentModel.DesignerSerializationVisibility(
                System.ComponentModel.DesignerSerializationVisibility.Hidden)]
            public bool IsFake { get { return _isFake; } }

            [System.ComponentModel.DesignerSerializationVisibility(
                System.ComponentModel.DesignerSerializationVisibility.Hidden)]
            public int HitsLeft { get { return _hitsLeft; } set { _hitsLeft = value; } }

            [System.ComponentModel.DesignerSerializationVisibility(
                System.ComponentModel.DesignerSerializationVisibility.Hidden)]
            public int Age { get { return _age; } set { _age = value; } }

            [System.ComponentModel.DesignerSerializationVisibility(
                System.ComponentModel.DesignerSerializationVisibility.Hidden)]
            public int Lifetime { get { return _lifetime; } }

            private LeakType _type;
            private bool _animState = false;
            private System.Windows.Forms.Timer _pulseTimer;

            public LeakButton(LeakType type, int size, Font font)
            {
                _type = type;
                _isFake = (type == LeakType.Fake);
                _hitsLeft = (type == LeakType.Big) ? 2 : 1;
                _age = 0;

                if (type == LeakType.Big)
                    _lifetime = 5;
                else if (type == LeakType.Fake)
                    _lifetime = 3;
                else
                    _lifetime = 4;

                Size = new Size(size, size);
                FlatStyle = FlatStyle.Flat;
                FlatAppearance.BorderSize = 0;
                BackColor = Color.Transparent;
                Font = font;
                Text = "";
                Cursor = Cursors.Hand;

                _pulseTimer = new System.Windows.Forms.Timer { Interval = 400 };
                _pulseTimer.Tick += (s, e) => { _animState = !_animState; Invalidate(); };
                _pulseTimer.Start();
            }

            protected override void OnPaint(PaintEventArgs pevent)
            {
                var g = pevent.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                int pad = 4;
                var rect = new Rectangle(pad, pad, Width - pad * 2 - 1, Height - pad * 2 - 1);
                Color fill = _type switch
                {
                    LeakType.Big => (_animState ? Color.FromArgb(50, 120, 200) : Color.FromArgb(30, 90, 160)),
                    LeakType.Fake => (_animState ? Color.FromArgb(200, 110, 40) : Color.FromArgb(150, 75, 20)),
                    _ => (_animState ? Color.FromArgb(80, 160, 220) : Color.FromArgb(50, 130, 190))
                };

                using (var glowPen = new Pen(Color.FromArgb(_animState ? 200 : 80, fill), 3f))
                    g.DrawEllipse(glowPen, rect);

                using (var brush = new SolidBrush(fill))
                    g.FillEllipse(brush, rect);

                string icon = _type == LeakType.Fake ? "⚠" : "💧";
                using (var iconFont = new Font("Segoe UI Symbol", Width > 65 ? 18f : 14f))
                using (var sb = new SolidBrush(Color.White))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(icon, iconFont, sb, new RectangleF(0, 0, Width, Height), sf);
                }

                if (_type == LeakType.Big && HitsLeft > 0)
                {
                    using (var hf = new Font("Calisto MT", 9f, FontStyle.Bold))
                    using (var sb = new SolidBrush(Color.White))
                        g.DrawString($"{HitsLeft}", hf, sb, Width - 18, Height - 18);
                }
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing) _pulseTimer?.Dispose();
                base.Dispose(disposing);
            }
        }
    }
}
