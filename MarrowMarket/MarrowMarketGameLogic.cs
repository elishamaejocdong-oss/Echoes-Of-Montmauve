using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Echoes_of_Montmauve.GameLogic;

namespace Echoes_of_Montmauve.MarrowMarket
{
    internal class MarrowMarketGameLogic
    {
        // ── Selection ─────────────────────────────────────────────────────────
        public List<Button> selectedButtons = new List<Button>();

        // ── Events ────────────────────────────────────────────────────────────
        public event Action VictoryTriggered;
        public event Action DefeatTriggered;

        // ── Mistakes ──────────────────────────────────────────────────────────
        private int mistakes = 0;
        private const int MaxMistakes = 4;
        public int Mistakes => mistakes;

        // ── Scoring (mirrors LeakFixFrenzy) ───────────────────────────────────
        private int score = 0;
        private int combo = 0;
        private int maxCombo = 0;
        public int Score => score;
        public int Combo => combo;
        public int MaxCombo => maxCombo;

        // ── Timer ─────────────────────────────────────────────────────────────
        private int timeLeft = 120;
        public int TimeLeft => timeLeft;
        private System.Windows.Forms.Timer _countdownTimer;

        // Callbacks so MarrowFamilies can refresh its HUD labels
        public event Action<int> TimerTicked;  // passes timeLeft each second
        public event Action TimeExpired;  // fires when timer hits 0

        // ── Category tracking ─────────────────────────────────────────────────
        private int categoriesFound = 0;
        private const int TotalCategories = 3;

        // ── Misc ──────────────────────────────────────────────────────────────
        private Label lblCategoryHeader;
        private DateTime _startTime;
        public DateTime StartTime => _startTime;

        public MarrowMarketGameLogic(Label categoryHeader)
        {
            lblCategoryHeader = categoryHeader;
            _startTime = DateTime.Now;

            _countdownTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _countdownTimer.Tick += CountdownTimer_Tick;
            _countdownTimer.Start();
        }

        // ── Timer ─────────────────────────────────────────────────────────────
        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            TimerTicked?.Invoke(timeLeft);
            if (timeLeft <= 0)
            {
                _countdownTimer.Stop();
                TimeExpired?.Invoke();
            }
        }

        private void StopTimer() { _countdownTimer?.Stop(); }

        // ── Selection ─────────────────────────────────────────────────────────
        public void AddSelection(Button btn)
        {
            if (btn == null || btn.Tag == null) return;
            if (selectedButtons.Contains(btn)) return;

            selectedButtons.Add(btn);
            btn.BackColor = Color.LightBlue;

            if (selectedButtons.Count == 4)
                CheckMatch();
        }

        public void CheckMatch()
        {
            if (selectedButtons.Count != 4) { ResetSelection(); return; }

            string category = selectedButtons[0].Tag?.ToString();
            if (string.IsNullOrEmpty(category)) { ResetSelection(); return; }

            bool allMatch = selectedButtons.All(btn =>
                btn.Tag != null && btn.Tag.ToString() == category);

            if (allMatch)
            {
                categoriesFound++;

                // 50 base pts + 25 per consecutive correct match (combo)
                combo++;
                if (combo > maxCombo) maxCombo = combo;
                int points = 50 + (combo - 1) * 25;
                score += points;

                lblCategoryHeader.Text = $"Category Found: {category.ToUpper()}  +{points}pts";
                lblCategoryHeader.Visible = true;

                foreach (var btn in selectedButtons)
                {
                    btn.Enabled = false;
                    btn.BackColor = Color.Moccasin;
                    btn.Text = "";
                }
                selectedButtons.Clear();
                CheckVictory();
            }
            else
            {
                mistakes++;
                combo = 0;    // wrong guess breaks combo

                if (mistakes >= MaxMistakes)
                {
                    StopTimer();
                    DefeatTriggered?.Invoke();
                }
                else
                {
                    ResetSelection();
                }
            }
        }

        private void ResetSelection()
        {
            foreach (var btn in selectedButtons)
                if (btn != null) btn.BackColor = Color.White;
            selectedButtons.Clear();
        }

        private void CheckVictory()
        {
            if (categoriesFound >= TotalCategories)
            {
                StopTimer();
                VictoryTriggered?.Invoke();
            }
        }

        public void ResetGame()
        {
            mistakes = 0;
            categoriesFound = 0;
            score = 0;
            combo = 0;
            maxCombo = 0;
            timeLeft = 120;
            selectedButtons.Clear();
            _startTime = DateTime.Now;

            lblCategoryHeader.Visible = false;
            lblCategoryHeader.Text = "";

            _countdownTimer.Stop();
            _countdownTimer.Start();
        }

        public void Dispose()
        {
            _countdownTimer?.Stop();
            _countdownTimer?.Dispose();
        }
    }
}