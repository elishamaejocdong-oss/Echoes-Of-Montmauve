using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Echoes_of_Montmauve.GameLogic;
using Echoes_of_Montmauve.SharedUI;

namespace Echoes_of_Montmauve.LunavireGroove
{
    public partial class InfectionGame : Form
    {
        private LunavaireGroove gameLogic;
        private Button[,] gridButtons = new Button[7, 7];

        private int _score = 0;
        private int _combo = 0;
        private int _maxCombo = 0;
        private int _lives = 3;
        private int _timeLeft = 60;
        private bool _gameOver = false;

        private Label lblScore;
        private Label lblCombo;
        private Label lblTimer;
        private Label lblLives;
        private Label lblMessage;

        private System.Windows.Forms.Timer _countdownTimer;
        private System.Windows.Forms.Timer _messageTimer;
        private DateTime _startTime;

        private static readonly Color COL_GOLD = Color.Goldenrod;
        private static readonly Color COL_RED = Color.IndianRed;
        private static readonly Font FONT_HUD = new Font("Calisto MT", 13f, FontStyle.Bold);
        private static readonly Font FONT_TITLE = new Font("Calisto MT", 18f, FontStyle.Bold);

        internal InfectionGame(LunavaireGroove gameLogic)
        {
            InitializeComponent();
            this.gameLogic = gameLogic;

            gameLogic.StartPuzzle();
            _startTime = DateTime.Now;

            SetUpGrid();
            BuildHUD();
            StartCountdown();
            UIHelper.AddButtonScaleEffect(btnOracle);
        }

        private void BuildHUD()
        {
            int x = 20;

            lblScore = MakeHudLabel("Score: 0", x, 6); x += 200;
            lblCombo = MakeHudLabel("Combo: x1", x, 6); x += 200;
            lblTimer = MakeHudLabel("Time: 60", x, 6); x += 200;
            lblLives = MakeHudLabel("Lives: ♥♥♥", x, 6);

            Controls.Add(lblScore);
            Controls.Add(lblCombo);
            Controls.Add(lblTimer);
            Controls.Add(lblLives);

            lblMessage = new TransparentLabel
            {
                AutoSize = false,
                Size = new Size(340, 40),
                Location = new Point(ClientSize.Width / 2 - 170, ClientSize.Height / 2 - 20),
                Font = FONT_TITLE,
                ForeColor = COL_GOLD,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };
            Controls.Add(lblMessage);
            lblMessage.BringToFront();

            _messageTimer = new System.Windows.Forms.Timer { Interval = 900 };
            _messageTimer.Tick += (s, e) => { lblMessage.Visible = false; _messageTimer.Stop(); };

            UpdateHUD();
        }

        private Label MakeHudLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = FONT_HUD,
                ForeColor = COL_GOLD,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(x, y)
            };
        }

        private void UpdateHUD()
        {
            lblScore.Text = $"Score: {_score}";
            lblCombo.Text = _combo > 1 ? $"Combo: x{_combo}!" : "Combo: x1";
            lblLives.Text = $"Lives: {new string('♥', _lives)}{new string('♡', 3 - _lives)}";
            lblSeeds.Text = $"Seeds: {gameLogic.seedsRemaining}";
        }

        private void ShowMessage(string text, Color col)
        {
            lblMessage.Text = text;
            lblMessage.ForeColor = col;
            lblMessage.Visible = true;
            lblMessage.BringToFront();
            _messageTimer.Stop();
            _messageTimer.Start();
        }

        private void StartCountdown()
        {
            _countdownTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _countdownTimer.Tick += CountdownTimer_Tick;
            _countdownTimer.Start();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (_gameOver) return;

            _timeLeft--;
            lblTimer.Text = $"Time: {_timeLeft}";

            if (_timeLeft <= 0)
                EndGame(won: false);
        }

        private void SetUpGrid()
        {
            foreach (Control c in GetAllControls(this))
            {
                if (c is Button btn && btn.Tag != null && btn.Name.StartsWith("btn"))
                {
                    string[] coords = btn.Tag.ToString().Split(',');
                    int r = int.Parse(coords[0].Trim());
                    int col = int.Parse(coords[1].Trim());

                    gridButtons[r, col] = btn;
                    btn.Click -= Tile_Click;
                    btn.Click += Tile_Click;
                    btn.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }

            for (int r = 0; r < 7; r++)
                for (int c = 0; c < 7; c++)
                    if (gridButtons[r, c] == null)
                        throw new Exception($"Grid button missing at [{r},{c}]");

            RefreshGrid();
        }

        private void Tile_Click(object sender, EventArgs e)
        {
            if (_gameOver) return;

            Button clickedButton = (Button)sender;
            string[] coords = clickedButton.Tag.ToString().Split(',');
            int r = int.Parse(coords[0].Trim());
            int c = int.Parse(coords[1].Trim());

            bool hasWon = gameLogic.PlantSeed(r, c);
            RefreshGrid();

            if (hasWon)
            {
                _combo++;
                if (_combo > _maxCombo) _maxCombo = _combo;

                int points = 15 + (_combo > 1 ? (_combo - 1) * 5 : 0);
                _score += points;

                string msg = _combo >= 5 ? $"MEGA COMBO x{_combo}! +{points}"
                           : _combo >= 3 ? $"Combo x{_combo}! +{points}"
                           : $"+{points}";
                ShowMessage(msg, _combo >= 3 ? COL_GOLD : Color.PapayaWhip);

                UpdateHUD();
                EndGame(won: true);
            }
            else if (gameLogic.CheckLossConditions())
            {
                EndGame(won: false);
            }
            else
            {
                _combo++;
                if (_combo > _maxCombo) _maxCombo = _combo;

                int points = 10 + (_combo > 1 ? (_combo - 1) * 5 : 0);
                _score += points;

                ShowMessage($"+{points}", COL_GOLD);
                UpdateHUD();
            }
        }

        private void RefreshGrid()
        {
            for (int r = 0; r < 7; r++)
                for (int c = 0; c < 7; c++)
                    gridButtons[r, c].BackgroundImage = gameLogic.IsMaismic(r, c)
                        ? Properties.Resources.Thorn_Purple
                        : Properties.Resources.Flower_Green;
        }

        private void EndGame(bool won)
        {
            if (_gameOver) return;
            _gameOver = true;
            _countdownTimer.Stop();

            int timeTaken = Math.Min(gameLogic.GetTimeTaken(), 3600);
            string username = SessionContent.CurrentActivePlayer.Username;

            if (won)
            {
                // ── Victory: mirror MarrowFamilies pattern ──────────────────
                DatabaseManager.LogGameSession(
                    username, "Lunavaire Infection", "Lunavaire Groove",
                    _score, timeTaken, isVictory: true);

                // Unlock the clue in the player's notebook
                DatabaseManager.UnlockNotebookItem(username, "Corrupted Root Sample");

                DatabaseManager.UpdateDistrictPurified(
                username, "Lunavaire Groove", timeTaken);

                // Flag for LunavaireGrooveForm to trigger the transition dialogue on return
                SessionContent.MinigameClearedForCurrentDistrict = true;

                // Return to the district hub so the transition dialogue can fire
                LunavaireGrooveForm grooveMain = new LunavaireGrooveForm();
                grooveMain.Show();
                this.Close();
            }
            else
            {
                // ── Defeat ──────────────────────────────────────────────────
                DatabaseManager.LogGameSession(
                    username, "Lunavaire Infection", "Lunavaire Groove",
                    _score, timeTaken, isVictory: false);

                using (FailedForm failed = new FailedForm(RestartGame))
                {
                    failed.ShowDialog();
                }
            }
        }

        private void RestartGame()
        {
            _score = 0;
            _combo = 0;
            _maxCombo = 0;
            _lives = 3;
            _timeLeft = 60;
            _gameOver = false;

            gameLogic.StartPuzzle();
            _startTime = DateTime.Now;

            RefreshGrid();
            UpdateHUD();
            StartCountdown();
        }

        private IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                yield return control;
                foreach (Control child in GetAllControls(control))
                    yield return child;
            }
        }

        private void btnOracle_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Plant the seeds in infected areas!");
        }
    }
}
