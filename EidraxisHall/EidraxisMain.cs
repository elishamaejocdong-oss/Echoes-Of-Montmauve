using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Echoes_of_Montmauve.GameLogic;
using Echoes_of_Montmauve.EidraxisHall;

namespace Echoes_of_Montmauve
{
    public partial class EidraxisMain : Form
    {
        private bool dialogueActive = false;
        private bool noctyraFinished = false;
        private bool hasSpokenToNoctyra = false;
        private bool _isTransitionDialogue = false;
        private bool _isPhase2Dialogue = false;
        private string[] activeLines;
        private int currentLineIndex = 0;

        // ── Noctyra intro dialogue (before minigame) ──────────────────────────
        private string[] dialogueLines =
        {
            "Stay your step, Traveler! The Miasma has seeped into the very foundations of our collective memory.",
            " It has unraveled the 'Accord of the Commons'—the very essence of how we build our world together.",
            "Look around you. The letters of our laws and the plans for our cities have been scattered like leaves in a gale.",
            " Without clear communication, there is no Participative Planning. The people's voices are being muffled by this magical smog..",
            "To clear the air, you must reconstruct the Key Words of sustainable living.",
            "I shall provide the riddle; you must find the word. But beware—the Miasma fights back. It will only tell you if your chosen letters are near the truth.",
            "Are you ready to restore order?"
        };

        // ── Noctyra transition dialogue (after minigame is cleared) ──────────
        private string[] noctyraTransitionLines =
        {
            "The words are restored… the Accord breathes once more.",
            "But the one who shattered it left a trail of deliberate gaps in the records.",
            "A scholar of Cinder Pipes may hold the final thread.",
            "Seek them out before the Miasma seals the passage for good."
        };

        private string[] noctyraPhase2Lines =
        {
            "The restored words point to one final judgment.",
            "Open your Notes, weigh the clues, and make the accusation before the deadline."
        };

        private Image playact;
        private Point playPos = new Point(580, 552);

        private void OnFrameChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private Rectangle GetNPCHitBox()
        {
            return new Rectangle(pbNoctyra.Location.X, pbNoctyra.Location.Y,
                                 pbNoctyra.Width, pbNoctyra.Height);
        }

        public EidraxisMain()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            sprite.Load();
            UIHelper.AddButtonScaleEffect(btnStartGame);
            UIHelper.AddButtonScaleEffect(MapBtn);
            this.KeyPreview = true;
        }

        private void EidraxisMain_Load(object sender, EventArgs e)
        {
            sprite.register(OnFrameChanged);
            playact = sprite.frontidle;

            pnlNoctyra.Visible = false;
            btnStartGame.Visible = false;

            // If the minigame was just cleared, jump straight to transition dialogue
            if (SessionContent.MinigameClearedForCurrentDistrict)
            {
                hasSpokenToNoctyra = true; // prevent intro re-triggering
                TriggerDialogue(noctyraTransitionLines, isTransition: true);
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

                this.Text = $"X: {playPos.X}, Y: {playPos.Y}";
                this.Invalidate();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void EidraxisMain_KeyUp(object sender, KeyEventArgs e)
        {
            playact = sprite.frontidle;
            this.Invalidate();
        }

        private void CheckNPCProximity()
        {
            if (dialogueActive || hasSpokenToNoctyra) return;

            Rectangle playerHitBox = new Rectangle(playPos.X, playPos.Y, 100, 100);
            Rectangle triggerZone = new Rectangle(432, 168, 80, 80);

            if (playerHitBox.IntersectsWith(triggerZone))
            {
                hasSpokenToNoctyra = true;

                if (SessionContent.CurrentPhase == SessionContent.GamePhase.Phase2_Madman)
                    TriggerDialogue(noctyraPhase2Lines, isTransition: false);
                else if (!SessionContent.MinigameClearedForCurrentDistrict)
                    TriggerDialogue(dialogueLines, isTransition: false);
                else
                    TriggerDialogue(noctyraTransitionLines, isTransition: true);
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

            pnlNoctyra.Visible = true;
            UIHelper.StartTypewriter(lblDialogue, activeLines[0]);
        }

        private void AdvanceDialogue()
        {
            if (noctyraFinished)
            {
                noctyraFinished = false;
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
                    pnlNoctyra.Visible = false;
                    this.ActiveControl = null;

                    SessionContent.MinigameClearedForCurrentDistrict = false;
                    SessionContent.PurifyDistrict("Eidraxis Hall");

                    MessageBox.Show("'Restricted Council Blueprint' added to notebook!",
                        "Clue Discovered", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(
                        $"{SessionContent.GetNextDistrict("Eidraxis Hall")} has been unlocked!",
                        "District Unlocked");

                    ReturnToWorldMap();
                }
                else
                {
                    if (_isPhase2Dialogue)
                    {
                        _isPhase2Dialogue = false;
                        dialogueActive = false;
                        pnlNoctyra.Visible = false;
                        this.ActiveControl = null;
                        return;
                    }

                    // Intro dialogue — sentinel to await one more Enter
                    noctyraFinished = true;
                    UIHelper.StartTypewriter(lblDialogue, "The choice is yours...");
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e) { }

        private void EidraxisMain_Paint(object sender, PaintEventArgs e)
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

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            EidraxisWordle wordleGame = new EidraxisWordle(new EidraxisHallClass());
            wordleGame.Show();
            this.Close();
        }

        private void MapBtn_Click(object sender, EventArgs e) => ReturnToWorldMap();

        private void ReturnToWorldMap()
        {
            Map map = new Map();
            map.Show();
            this.Close();
        }
    }
}
