using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Echoes_of_Montmauve.EidraxisHall;
using Echoes_of_Montmauve.GameLogic;

namespace Echoes_of_Montmauve
{
    public partial class EidraxisWordle : Form
    {
        private EidraxisHallClass _logic;

        private int _score = 0;
        private int _combo = 0;
        private int _maxCombo = 0;
        private int _lives = 6;
        private int _timeLeft = 60;
        private bool _gameOver = false;

        private int currentColumn = 0;
        private int currentRow = 0;
        private DateTime _startTime;

        private Label lblScore;
        private Label lblCombo;
        private Label lblTimer;
        private Label lblLives;
        private Label lblMessage;

        private System.Windows.Forms.Timer _countdownTimer;
        private System.Windows.Forms.Timer _messageTimer;

        private static readonly Color COL_GOLD = Color.Goldenrod;
        private static readonly Color COL_CREAM = Color.PapayaWhip;
        private static readonly Color COL_RED = Color.IndianRed;
        private static readonly Font FONT_HUD = new Font("Calisto MT", 13f, FontStyle.Bold);
        private static readonly Font FONT_TITLE = new Font("Calisto MT", 18f, FontStyle.Bold);

        internal EidraxisWordle(EidraxisHallClass logic)
        {
            InitializeComponent();
            this._logic = logic;

            _logic.StartPuzzle();
            _startTime = DateTime.Now;

            lblRiddle.Text = _logic.CurrentRiddle;

            BuildHUD();
            StartCountdown();

            this.KeyPreview = true;
            this.ActiveControl = null;
        }

        private void BuildHUD()
        {
            int x = 20;

            lblScore = MakeHudLabel("Score: 0", x, 6); x += 200;
            lblCombo = MakeHudLabel("Combo: x1", x, 6); x += 200;
            lblTimer = MakeHudLabel("Time: 60", x, 6); x += 200;
            lblLives = MakeHudLabel("Lives: ♥♥♥♥♥♥", x, 6);

            Controls.Add(lblScore);
            Controls.Add(lblCombo);
            Controls.Add(lblTimer);
            Controls.Add(lblLives);

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
            lblLives.Text = $"Lives: {new string('♥', _lives)}{new string('♡', 6 - _lives)}";
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
                EndGame(won: false, reason: "Time's up! The Miasma has won...");
        }

        private void EidraxisWordle_KeyDown(object sender, KeyEventArgs e)
        {
            if (_gameOver) return;

            if (e.KeyCode == Keys.Back && currentColumn > 0)
            {
                currentColumn--;
                GetLabel(currentRow, currentColumn).Text = "";
            }
            else if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z && currentColumn < 5)
            {
                GetLabel(currentRow, currentColumn).Text = e.KeyCode.ToString();
                currentColumn++;
            }
            else if (e.KeyCode == Keys.Enter && currentColumn == 5)
            {
                SubmitGuess();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && !_gameOver && currentColumn == 5)
            {
                SubmitGuess();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private Label GetLabel(int row, int column)
        {
            return this.Controls.Find($"lbl_{row}_{column}", true)[0] as Label;
        }

        private void SubmitGuess()
        {
            try
            {
                string guess = "";
                for (int i = 0; i < 5; i++)
                    guess += GetLabel(currentRow, i).Text;

                int[] results = _logic.CheckGuess(guess);
                bool isCorrect = true;

                for (int i = 0; i < 5; i++)
                {
                    Label lbl = GetLabel(currentRow, i);
                    if (results[i] == 2)
                    {
                        lbl.BackColor = ColorTranslator.FromHtml("#8A9A5B");   // green
                    }
                    else if (results[i] == 1)
                    {
                        lbl.BackColor = ColorTranslator.FromHtml("#D2B450");   // yellow
                        isCorrect = false;
                    }
                    else
                    {
                        lbl.BackColor = ColorTranslator.FromHtml("#D3D3D3");   // grey
                        isCorrect = false;
                    }
                }

                if (isCorrect)
                {
                    _combo++;
                    if (_combo > _maxCombo) _maxCombo = _combo;

                    int attemptsUsed = currentRow + 1;
                    int basePoints = (7 - attemptsUsed) * 20;
                    int timeBonus = Math.Max(0, _timeLeft);
                    int points = basePoints + timeBonus;
                    _score += points;

                    ShowMessage($"Correct! +{points}", COL_GOLD);
                    UpdateHUD();

                    EndGame(won: true);
                }
                else
                {
                    _combo = 0;
                    _lives = Math.Max(0, _lives - 1);
                    ShowMessage("Wrong guess! -1 Life", COL_RED);
                    UpdateHUD();

                    if (_lives <= 0)
                    {
                        EndGame(won: false, reason: $"No lives left! The word was: {_logic.TargetWord}");
                        return;
                    }

                    if (currentRow < 5)
                    {
                        currentRow++;
                        currentColumn = 0;
                    }
                    else
                    {
                        EndGame(won: false, reason: $"Out of guesses! The word was: {_logic.TargetWord}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eidraxis puzzle error: " + ex.Message, "Puzzle Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                RestartGame();
            }
        }

        private void EndGame(bool won, string reason = "")
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
                    username, "Eidraxis Wordle", "Eidraxis Hall",
                    _score, timeTaken, isVictory: true);

                DatabaseManager.UpdateDistrictPurified(
                username, "Eidraxis Hall", timeTaken);

                // Unlock the clue in the player's notebook
                DatabaseManager.UnlockNotebookItem(username, "Restricted Council Blueprint");

                // Flag for EidraxisMain to trigger the transition dialogue on return
                SessionContent.MinigameClearedForCurrentDistrict = true;

                // Return to the district hub so the transition dialogue can fire
                EidraxisMain eidraxisMain = new EidraxisMain();
                eidraxisMain.Show();
                this.Close();
            }
            else
            {
                // ── Defeat ──────────────────────────────────────────────────
                DatabaseManager.LogGameSession(
                    username, "Eidraxis Wordle", "Eidraxis Hall",
                    _score, timeTaken, isVictory: false);

                using (FailedForm failed = new FailedForm(RestartGame))
                {
                    if (!string.IsNullOrEmpty(reason))
                        MessageBox.Show(reason, "Game Over",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                    failed.ShowDialog();
                }
            }
        }

        private void RestartGame()
        {
            _score = 0;
            _combo = 0;
            _maxCombo = 0;
            _lives = 6;
            _timeLeft = 60;
            _gameOver = false;
            currentRow = 0;
            currentColumn = 0;

            for (int r = 0; r < 6; r++)
                for (int c = 0; c < 5; c++)
                {
                    Label lbl = GetLabel(r, c);
                    lbl.Text = "";
                    lbl.BackColor = Color.Empty;
                }

            UpdateHUD();
            _logic.StartPuzzle();
            lblRiddle.Text = _logic.CurrentRiddle;
            _startTime = DateTime.Now;
            StartCountdown();
        }

        private void btnOracle_Click(object sender, EventArgs e)
        {
            string definition = DatabaseManager.GetWordDescription(_logic.TargetWord);
            MessageBox.Show($"The Oracle reveals:\n\n{definition}");
        }
    }
}
