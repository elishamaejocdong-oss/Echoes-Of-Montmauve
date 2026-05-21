using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Echoes_of_Montmauve.GameLogic;
using Echoes_of_Montmauve.NewFolder;

namespace Echoes_of_Montmauve.VeloryxSpire
{
    public partial class VeloryxMain : Form
    {
        private bool dialogueActive = false;
        private bool xerionFinished = false;
        private bool hasSpokenToXerion = false;
        private bool _isTransitionDialogue = false;
        private bool _isPhase2Dialogue = false;
        private string[] activeLines;
        private int currentLineIndex = 0;

        // ── Xerion intro dialogue (before minigame) ───────────────────────────
        private string[] dialogueLines =
        {
            "The Miasma is a thief of history, Traveler. It doesn't just destroy; it makes us forget. The artifacts of our heritage—the very proof of our community's journey—are being erased from our minds.",
            "I caught a glimpse of them just moments ago, but the fog has grown thick. If we cannot remember where our history is kept, we lose the foundation of our future. A city without a past is a city without a soul.",
            "I will use a surge of my remaining light to clear the fog for a mere heartbeat. Look closely! Memorize the resting place of every relic.",
            "Once the light fades and the Miasma returns, I will ask you for a specific artifact. You must trust your memory and point to the exact spot where it lies. Fail too many times, and the Miasma will consume the relics forever."
        };

        // ── Xerion transition dialogue (after minigame is cleared) ───────────
        private string[] xerionTransitionLines =
        {
            "You have done it. The relics are remembered… and the Miasma has retreated from this tower.",
            "But the corruption has roots deeper than memory, Traveler.",
            "I have seen its tendrils stretch toward Lunavaire Groove — the canopy district to the south.",
            "The forest spirits there may know who planted this blight. Go swiftly."
        };

        private string[] xerionPhase2Lines =
        {
            "There is no time left for memory trials, Traveler.",
            "Review the clues you have gathered and bring your accusation to the final confrontation."
        };

        private Image playact;
        private Point playPos = new Point(580, 552);

        private void OnFrameChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private Rectangle GetNPCHitBox()
        {
            return new Rectangle(pbXerion.Location.X, pbXerion.Location.Y,
                                 pbXerion.Width, pbXerion.Height);
        }

        public VeloryxMain()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            sprite.Load();
            UIHelper.AddButtonScaleEffect(btnStartGame);
            UIHelper.AddButtonScaleEffect(MapBtn);
            this.KeyPreview = true;
        }

        private void VeloryxMain_Load(object sender, EventArgs e)
        {
            sprite.register(OnFrameChanged);
            playact = sprite.frontidle;

            pnlXerion.Visible = false;
            btnStartGame.Visible = false;

            // If the minigame was just cleared, jump straight to transition dialogue
            if (SessionContent.MinigameClearedForCurrentDistrict)
            {
                hasSpokenToXerion = true; // prevent intro re-triggering
                TriggerDialogue(xerionTransitionLines, isTransition: true);
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

                Rectangle nextHitbox = new Rectangle(nextX, nextY, 100, 100);
                Rectangle npcBlock = Rectangle.Inflate(GetNPCHitBox(), -20, -20);

                if (!nextHitbox.IntersectsWith(npcBlock))
                {
                    playPos.X = nextX;
                    playPos.Y = nextY;
                }
                playact = nextSprite;

                CheckNPCProximity();
                this.Text = $"X: {playPos.X}, Y: {playPos.Y}";
                this.Invalidate();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void VeloryxMain_KeyUp(object sender, KeyEventArgs e)
        {
            playact = sprite.frontidle;
            this.Invalidate();
        }

        private void CheckNPCProximity()
        {
            if (dialogueActive || hasSpokenToXerion) return;

            Rectangle playerHitbox = new Rectangle(playPos.X, playPos.Y, 100, 100);
            Rectangle triggerZone = new Rectangle(801, 322, 80, 80);

            if (playerHitbox.IntersectsWith(triggerZone))
            {
                hasSpokenToXerion = true;

                if (SessionContent.CurrentPhase == SessionContent.GamePhase.Phase2_Madman)
                    TriggerDialogue(xerionPhase2Lines, isTransition: false);
                else if (!SessionContent.MinigameClearedForCurrentDistrict)
                    TriggerDialogue(dialogueLines, isTransition: false);
                else
                    TriggerDialogue(xerionTransitionLines, isTransition: true);
            }
        }

        private void TriggerDialogue(string[] lines, bool isTransition, bool isPhase2 = false)
        {
            dialogueActive = true;
            currentLineIndex = 0;
            activeLines = lines;
            playact = sprite.frontidle;
            btnStartGame.Visible = false;
            this.ActiveControl = null;
            _isTransitionDialogue = isTransition;
            _isPhase2Dialogue = isPhase2;

            pnlXerion.Visible = true;
            UIHelper.StartTypewriter(lblDialogue, activeLines[0]);
        }

        private void AdvanceDialogue()
        {
            if (xerionFinished)
            {
                xerionFinished = false;
                dialogueActive = false;
                this.ActiveControl = null;
                btnStartGame.Visible = true;
                btnStartGame.TabStop = false;
                btnStartGame.Focus();
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
                    pnlXerion.Visible = false;
                    this.ActiveControl = null;

                    SessionContent.MinigameClearedForCurrentDistrict = false;
                    SessionContent.PurifyDistrict("Veloryx Spire");

                    MessageBox.Show("'Erased Historical Record' added to notebook!",
                        "Clue Discovered", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(
                        $"{SessionContent.GetNextDistrict("Veloryx Spire")} has been unlocked!",
                        "District Unlocked");

                    ReturnToWorldMap();
                }
                else
                {
                    if (_isPhase2Dialogue)
                    {
                        _isPhase2Dialogue = false;
                        dialogueActive = false;
                        pnlXerion.Visible = false;
                        this.ActiveControl = null;
                        return;
                    }

                    // Intro dialogue — sentinel to await one more Enter
                    xerionFinished = true;
                    UIHelper.StartTypewriter(lblDialogue, "The choice is yours...");
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e) { }

        private void VeloryxMain_Paint(object sender, PaintEventArgs e)
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

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            VeloryxMatch match = new VeloryxMatch(new VeloryxSpireLogic());
            match.Show();
            this.Close();
        }
    }
}
