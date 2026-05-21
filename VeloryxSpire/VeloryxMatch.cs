using Echoes_of_Montmauve.GameLogic;
using Echoes_of_Montmauve.NewFolder;
using Echoes_of_Montmauve.SharedUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Echoes_of_Montmauve.VeloryxSpire
{
    public partial class VeloryxMatch : Form
    {
        private VeloryxSpireLogic _logic;
        private bool _isProcessing = false;

        private int _score = 0;
        private int _combo = 0;
        private int _maxCombo = 0;
        private int _lives = 10;
        private int _timeLeft = 60;
        private bool _gameOver = false;
        private DateTime _startTime;

        private Label lblScore;
        private Label lblCombo;
        private Label lblLives;
        private Label lblMessage;

        private System.Windows.Forms.Timer _countdownTimer;
        private System.Windows.Forms.Timer _messageTimer;

        private static readonly Color COL_GOLD = Color.Goldenrod;
        private static readonly Color COL_RED = Color.IndianRed;
        private static readonly Font FONT_HUD = new Font("Calisto MT", 13f, FontStyle.Bold);
        private static readonly Font FONT_TITLE = new Font("Calisto MT", 18f, FontStyle.Bold);

        internal VeloryxMatch(VeloryxSpireLogic logic)
        {
            InitializeComponent();
            _logic = logic;
            _startTime = DateTime.Now;

            SetupBoard();
            BuildHUD();
            StartCountdown();
        }

        private void BuildHUD()
        {
            int x = 20;

            lblScore = MakeHudLabel("Score: 0", x, 6); x += 200;
            lblCombo = MakeHudLabel("Combo: x1", x, 6); x += 200;
            lblLives = MakeHudLabel("Lives: ♥♥♥♥♥♥♥♥♥♥", x + 200, 6);

            Controls.Add(lblScore);
            Controls.Add(lblCombo);
            Controls.Add(lblLives);

            lblTimer.Font = FONT_HUD;
            lblTimer.ForeColor = COL_GOLD;
            lblTimer.BackColor = Color.Transparent;
            lblTimer.BorderStyle = BorderStyle.None;
            lblTimer.Text = "Time: 60";
            lblTimer.Location = new Point(x, 6);
            lblTimer.AutoSize = true;
            lblTimer.BringToFront();

            lblMessage = new Label
            {
                AutoSize = false,
                Size = new Size(340, 40),
                Location = new Point(ClientSize.Width / 2 - 170, ClientSize.Height / 2 - 20),
                Font = FONT_TITLE,
                ForeColor = COL_GOLD,
                BackColor = Color.Transparent,
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
            lblTimer.Text = $"Time: {_timeLeft}";
            lblLives.Text = $"Lives: {new string('♥', _lives)}{new string('♡', 10 - _lives)}";
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

            if (_timeLeft <= 10)
                lblTimer.ForeColor = COL_RED;

            if (_timeLeft <= 0)
                EndGame(won: false);
        }

        private void SetupBoard()
        {
            // Reset logic state for replays
            _logic.MatchesFound = 0;
            _logic.SelectedCards.Clear();

            List<Image> images = new List<Image>
            {
                Properties.Resources.Crystal1, Properties.Resources.Crystal2,
                Properties.Resources.Crystal3, Properties.Resources.Crystal4,
                Properties.Resources.Crystal5, Properties.Resources.Crystal6,
            };

            List<Image> pairs = new List<Image>(images);
            pairs.AddRange(images);

            Random rng = new Random();
            pairs = pairs.OrderBy(x => rng.Next()).ToList();

            for (int i = 0; i < pairs.Count; i++)
            {
                Control[] found = this.Controls.Find($"btnCard{i + 1}", true);
                if (found.Length > 0 && found[0] is Button btn)
                {
                    btn.BackgroundImage = Properties.Resources.HiddenPart;
                    btn.Tag = pairs[i];
                    btn.BackColor = Color.MediumPurple;
                    btn.BackgroundImageLayout = ImageLayout.Stretch;
                    btn.Enabled = true;

                    btn.Click -= Card_Click;
                    btn.Click += Card_Click;
                    btn.BringToFront();
                }
            }
        }

        private async void Card_Click(object sender, EventArgs e)
        {
            if (_isProcessing || _gameOver) return;

            Button clicked = (Button)sender;
            if (!clicked.Enabled || _logic.SelectedCards.Contains(clicked)) return;

            clicked.BackgroundImage = (Image)clicked.Tag;
            _logic.SelectedCards.Add(clicked);

            if (_logic.SelectedCards.Count == 2)
            {
                _isProcessing = true;
                await Task.Delay(800);

                if (_logic.CheckMatch())
                {
                    _combo++;
                    if (_combo > _maxCombo) _maxCombo = _combo;

                    int points = 20 + (_combo > 1 ? (_combo - 1) * 10 : 0);
                    _score += points;

                    string msg = _combo >= 5 ? $"MEGA COMBO x{_combo}! +{points}"
                               : _combo >= 3 ? $"Combo x{_combo}! +{points}"
                               : $"Match! +{points}";
                    ShowMessage(msg, _combo >= 3 ? COL_GOLD : Color.PapayaWhip);

                    UpdateHUD();

                    if (_logic.IsGameWon())
                    {
                        EndGame(won: true);
                        _isProcessing = false;
                        return;
                    }
                }
                else
                {
                    _logic.ResetCards();
                    _combo = 0;
                    _lives = Math.Max(0, _lives - 1);
                    ShowMessage("No match! -1 Life", COL_RED);
                    UpdateHUD();

                    if (_lives <= 0)
                    {
                        EndGame(won: false);
                        _isProcessing = false;
                        return;
                    }
                }

                _isProcessing = false;
            }
        }

        private void EndGame(bool won)
        {
            if (_gameOver) return;
            _gameOver = true;
            _countdownTimer.Stop();

            int timeTaken = Math.Min((int)(DateTime.Now - _startTime).TotalSeconds, 3600);
            string username = SessionContent.CurrentActivePlayer.Username;

            if (won)
            {
                // ── Victory: mirror MarrowFamilies pattern ──────────────────
                DatabaseManager.LogGameSession(
                    username, "Veloryx Match", "Veloryx Spire",
                    _score, timeTaken, isVictory: true);

                DatabaseManager.UpdateDistrictPurified(
                username, "Veloryx Spire", timeTaken);

                // Unlock the clue in the player's notebook
                DatabaseManager.UnlockNotebookItem(username, "Erased Historical Record");

                // Flag for VeloryxMain to trigger the transition dialogue on return
                SessionContent.MinigameClearedForCurrentDistrict = true;

                // Return to the district hub so the transition dialogue can fire
                VeloryxMain veloryxMain = new VeloryxMain();
                veloryxMain.Show();
                this.Close();
            }
            else
            {
                // ── Defeat ──────────────────────────────────────────────────
                DatabaseManager.LogGameSession(
                    username, "Veloryx Match", "Veloryx Spire",
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
            _lives = 10;
            _timeLeft = 60;
            _gameOver = false;

            lblTimer.ForeColor = COL_GOLD;

            SetupBoard();
            UpdateHUD();
            _startTime = DateTime.Now;
            StartCountdown();
        }
    }
}
