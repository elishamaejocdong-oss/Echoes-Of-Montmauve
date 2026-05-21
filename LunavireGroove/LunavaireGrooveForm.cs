using Echoes_of_Montmauve.GameLogic;
using Echoes_of_Montmauve.LunavireGroove;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Echoes_of_Montmauve
{
    public partial class LunavaireGrooveForm : Form
    {
        private bool dialogueActive = false;
        private bool stroemielleFinished = false;
        private bool hasSpokenToStroemielle = false;
        private bool _isTransitionDialogue = false;
        private bool _isPhase2Dialogue = false;
        private string[] activeLines;
        private int currentLineIndex = 0;

        // ── Stroemielle intro dialogue (before minigame) ──────────────────────
        private string[] dialogueLines =
        {
            "Traveler, hold your breath. Lunavaire Groove was once the lungs of Montmauve—the ancient canopy breathing life into the city and sheltering the spirits of the old world. But the Miasma has settled in the hollows, turning the sweet oxygen into a suffocating shroud.",
            "The Miasma has twisted the ley lines and tangled the ancient roots, blocking the natural flow of the Groove's essence. You must step into the emerald fog and realign the sacred segments until the spiritual circuit is restored.",
            "If you can restore the roots, the forest will exhale a pulse of pure energy to dispel the Miasma and heal the district. But be warned: if you fail, the corruption will rot the heart of the Groove, and the forest will collapse into a stagnant, toxic marsh."
        };

        // ── Stroemielle transition dialogue (after minigame is cleared) ────────
        private string[] stroemielleTransitionLines =
        {
            "The Groove breathes again… but the source of this corruption did not grow here.",
            "The spores of the Miasma were carried on the wind from the upper districts.",
            "If you seek the root of this blight, follow the river north toward Veloryx Spire.",
            "The archivists there guard the oldest maps of Montmauve."
        };

        private string[] stroemiellePhase2Lines =
        {
            "The Groove has given all the guidance it can.",
            "Carry your evidence to the final confrontation before the city's last breath is gone."
        };

        private Image playact;
        private Point playPos = new Point(700, 552);

        private void OnFrameChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private Rectangle GetNPCHitBox()
        {
            return new Rectangle(pbStroemielle.Location.X, pbStroemielle.Location.Y,
                                 pbStroemielle.Width, pbStroemielle.Height);
        }

        public LunavaireGrooveForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            sprite.Load();
            UIHelper.AddButtonScaleEffect(StartGameBtn);
            UIHelper.AddButtonScaleEffect(MapBtn);
            this.KeyPreview = true;
        }

        private void LunavaireGrooveForm_Load(object sender, EventArgs e)
        {
            sprite.register(OnFrameChanged);
            playact = sprite.frontidle;

            dbStroemielle.Visible = false;
            StartGameBtn.Visible = false;

            // If the minigame was just cleared, jump straight to transition dialogue
            if (SessionContent.MinigameClearedForCurrentDistrict)
            {
                hasSpokenToStroemielle = true; // prevent intro re-triggering
                TriggerDialogue(stroemielleTransitionLines, isTransition: true);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && dialogueActive)
            {
                AdvanceDialogue();
                return true;
            }

            int speed = 12;

            if (!dialogueActive)
            {
                int nextX = playPos.X;
                int nextY = playPos.Y;
                Image nextSprite = playact;

                switch (keyData)
                {
                    case Keys.Left: nextX -= speed; nextSprite = sprite.walkleft; break;
                    case Keys.Right: nextX += speed; nextSprite = sprite.walkright; break;
                    case Keys.Up: nextY -= speed; nextSprite = sprite.walkup; break;
                    case Keys.Down: nextY += speed; nextSprite = sprite.walkdown; break;
                    default: return base.ProcessCmdKey(ref msg, keyData);
                }

                Rectangle nextHitBox = new Rectangle(nextX, nextY, 100, 100);
                Rectangle npcBlock = Rectangle.Inflate(GetNPCHitBox(), -20, -20);

                if (!nextHitBox.IntersectsWith(npcBlock))
                {
                    playPos.X = nextX;
                    playPos.Y = nextY;
                }
                playact = nextSprite;

                CheckNPCProximity();

                this.Text = $"X: {playPos.X} Y: {playPos.Y}";
                this.Invalidate();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LunavaireGrooveForm_KeyUp(object sender, KeyEventArgs e)
        {
            playact = sprite.frontidle;
            this.Invalidate();
        }

        private void CheckNPCProximity()
        {
            if (dialogueActive || hasSpokenToStroemielle) return;

            Rectangle playerHitbox = new Rectangle(playPos.X, playPos.Y, 100, 100);
            Rectangle triggerZone = new Rectangle(493, 380, 80, 80);

            if (playerHitbox.IntersectsWith(triggerZone))
            {
                hasSpokenToStroemielle = true;

                if (SessionContent.CurrentPhase == SessionContent.GamePhase.Phase2_Madman)
                    TriggerDialogue(stroemiellePhase2Lines, isTransition: false);
                else if (!SessionContent.MinigameClearedForCurrentDistrict)
                    TriggerDialogue(dialogueLines, isTransition: false);
                else
                    TriggerDialogue(stroemielleTransitionLines, isTransition: true);
            }
        }

        private void TriggerDialogue(string[] lines, bool isTransition, bool isPhase2 = false)
        {
            dialogueActive = true;
            currentLineIndex = 0;
            activeLines = lines;
            playact = sprite.frontidle;
            StartGameBtn.Visible = false;
            this.ActiveControl = null;
            _isTransitionDialogue = isTransition;
            _isPhase2Dialogue = isPhase2;

            dbStroemielle.Visible = true;
            UIHelper.StartTypewriter(lblDialogue, activeLines[0]);
        }

        private void AdvanceDialogue()
        {
            if (stroemielleFinished)
            {
                stroemielleFinished = false;
                dialogueActive = false;
                this.ActiveControl = null;
                StartGameBtn.Visible = true;
                StartGameBtn.TabStop = false;
                StartGameBtn.Focus();
                return;
            }

            currentLineIndex++;

            if (currentLineIndex < activeLines.Length)
            {
                UIHelper.StartTypewriter(lblDialogue, activeLines[currentLineIndex]);
            }
            else
            {
                if (_isTransitionDialogue)
                {
                    // Post-minigame transition finished → purify district
                    dialogueActive = false;
                    dbStroemielle.Visible = false;
                    this.ActiveControl = null;

                    SessionContent.MinigameClearedForCurrentDistrict = false;
                    SessionContent.PurifyDistrict("Lunavaire Groove");

                    MessageBox.Show("'Corrupted Root Sample' added to notebook!",
                        "Clue Discovered", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(
                        $"{SessionContent.GetNextDistrict("Lunavaire Groove")} has been unlocked!",
                        "District Unlocked");

                    ReturnToWorldMap();
                }
                else
                {
                    if (_isPhase2Dialogue)
                    {
                        _isPhase2Dialogue = false;
                        dialogueActive = false;
                        dbStroemielle.Visible = false;
                        this.ActiveControl = null;
                        return;
                    }

                    // Intro dialogue — sentinel to await one more Enter
                    stroemielleFinished = true;
                    UIHelper.StartTypewriter(lblDialogue, "The choice is yours...");
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e) { }

        private void LunavaireGrooveForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            if (this.BackgroundImage != null)
                g.DrawImage(this.BackgroundImage, this.ClientRectangle);

            if (playact != null)
            {
                ImageAnimator.UpdateFrames(playact);
                g.DrawImage(playact, playPos.X, playPos.Y, 100, 100);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            sprite.unregister(OnFrameChanged);
        }

        private void MapBtn_Click(object sender, EventArgs e) => ReturnToWorldMap();

        private void ReturnToWorldMap()
        {
            Map map = new Map();
            map.Show();
            this.Close();
        }

        private void StartGameBtn_Click(object sender, EventArgs e)
        {
            LunavaireGroove logic = new LunavaireGroove();
            InfectionGame game = new InfectionGame(logic);
            game.Show();
            this.Close();
        }
    }
}
