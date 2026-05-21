using Echoes_of_Montmauve.GameLogic;
using Echoes_of_Montmauve.SharedUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace Echoes_of_Montmauve.MarrowMarket
{
    public partial class MarrowFamilies : Form
    {
        private MarrowMarketGameLogic _gameLogic;
        private List<Button> tileButtons;

        public MarrowFamilies()
        {
            InitializeComponent();
            _gameLogic = new MarrowMarketGameLogic(this.lblCategoryHeader);

            tileButtons = new List<Button>
            {
                btnTile1, btnTile2, btnTile3, btnTile4,
                btnTile5, btnTile6, btnTile7, btnTile8,
                btnTile9, btnTile10, btnTile11, btnTile12
            };

            _gameLogic.VictoryTriggered += OnVictory;
            _gameLogic.DefeatTriggered += HandleDefeat;
            _gameLogic.TimerTicked += OnTimerTick;
            _gameLogic.TimeExpired += OnTimeExpired;

            UIHelper.AddButtonScaleEffect(btnOracle);

            UpdateHUD();
        }

        // ── Puzzle loading ────────────────────────────────────────────────────
        private void LoadPuzzle()
        {
            List<DatabaseManager.MarrowWordTile> puzzleData = DatabaseManager.GetMarrowFamiliesPuzzle(1);
            var shuffledData = puzzleData.OrderBy(x => new Random().Next()).ToList();

            for (int i = 0; i < shuffledData.Count && i < tileButtons.Count; i++)
            {
                Button btn = tileButtons[i];
                btn.Text = shuffledData[i].WordText;
                btn.Tag = shuffledData[i].WordCategory;
                btn.Enabled = true;
                btn.ForeColor = Color.DarkGoldenrod;
                btn.Visible = true;
                btn.Click -= btnTile_Click;
                btn.Click += btnTile_Click;
            }
        }

        // ── HUD ───────────────────────────────────────────────────────────────
        private void UpdateHUD()
        {
            lblScore.Text = $"Score: {_gameLogic.Score}";
            lblCombo.Text = _gameLogic.Combo > 1 ? $"Combo: x{_gameLogic.Combo}!" : "Combo: x1";
            lblMistakes.Text = $"Mistakes: {_gameLogic.Mistakes}/4";
            lblTimer.Text = $"Time: {_gameLogic.TimeLeft}s";
        }

        private void OnTimerTick(int timeLeft)
        {
            lblTimer.Text = $"Time: {timeLeft}s";
            lblTimer.ForeColor = timeLeft <= 30 ? Color.IndianRed : Color.DarkGoldenrod;
        }

        private void OnTimeExpired() => HandleDefeat();

        // ── Tile click ────────────────────────────────────────────────────────
        private void btnTile_Click(object sender, EventArgs e)
        {
            _gameLogic.AddSelection((Button)sender);
            UpdateHUD();
        }

        // ── Victory ───────────────────────────────────────────────────────────
        private void OnVictory()
        {
            int timeTaken = Math.Min(
                (int)(DateTime.Now - _gameLogic.StartTime).TotalSeconds, 3600);

            string username = SessionContent.CurrentActivePlayer.Username;

            // Log the session for analytics
            DatabaseManager.LogGameSession(
                username, "Marrow Families", "Marrow Market",
                _gameLogic.Score, timeTaken, true);

            // Purify district and reduce Miasma — empty string skips artifact insert
            DatabaseManager.UpdateDistrictPurified(
                username, "Marrow Market", timeTaken);

            // Unlock the clue in the player's notebook
            DatabaseManager.UnlockNotebookItem(username, "Delayed Supply Manifest");

            // Flag for MarrowMarketMain to trigger the transition dialogue on return
            SessionContent.MinigameClearedForCurrentDistrict = true;

            // Return to the market hub so the transition dialogue can fire
            MarrowMarketMain marketMain = new MarrowMarketMain();
            marketMain.Show();
            this.Close();
        }

        // ── Defeat ────────────────────────────────────────────────────────────
        private void HandleDefeat()
        {
            int timeTaken = Math.Min(
                (int)(DateTime.Now - _gameLogic.StartTime).TotalSeconds, 3600);

            string username = SessionContent.CurrentActivePlayer.Username;

            DatabaseManager.LogGameSession(
                username, "Marrow Families", "Marrow Market",
                _gameLogic.Score, timeTaken, false);

            using (FailedForm failed = new FailedForm(RestartGame))
            {
                failed.ShowDialog();
            }
        }

        // ── Restart ───────────────────────────────────────────────────────────
        private void RestartGame()
        {
            _gameLogic.ResetGame();

            foreach (Button btn in tileButtons)
            {
                btn.BackColor = Color.White;
                btn.Enabled = true;
                btn.ForeColor = Color.DarkGoldenrod;
            }

            LoadPuzzle();
            UpdateHUD();

            tblOverAll.Invalidate();
            tblOverAll.Update();
        }

        // ── Form events ───────────────────────────────────────────────────────
        private void MarrowFamilies_Load(object sender, EventArgs e) => LoadPuzzle();

        private void btnReturn_Click(object sender, EventArgs e)
        {
            _gameLogic.Dispose();
            MarrowMarketMain marketMain = new MarrowMarketMain();
            marketMain.Show();
            this.Close();
        }

        private void btnOracle_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Match words belonging in the same category");
        }
    }
}
