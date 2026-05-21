using Echoes_of_Montmauve.GameLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Echoes_of_Montmauve.MarrowMarket
{
    public partial class MarrowMarketMain : Form
    {
        // ── Dialogue state ──────────────────────────────────────────────
        private bool dialogueActive = false;
        private string[] activeLines;
        private int currentLineIndex = 0;

        // Chyra intro (used before minigame)
        private string[] chyraIntroLines =
        {
            "Traveler, look at this chaos! The Maisma hasn't just clouded our vision; it has severed the invisible threads that bind our lands together. A city cannot survive as an island. It breathes through the fields of the farmers and the workshops of the artisan",
            "The Urban-Rural Link is broken. Look at these crates! The logistics of the realm are in shambles because the Maisma has 'de-categorized' our resources. The city planners in the spire can no longer find the materials they need from the peri-urban zones, and the rural haevests are rotting because they have no home",
            "If we cannot group these resources back into their proper 'Families', the economic heart of this district will stop beating. We need a mind that seas the connections where others sees only clutter.",
            "Are you up for the challenge?"
        };

        // Chyra transition (used after minigame is cleared)
        private string[] chyraTransitionLines =
        {
            "The shortages didn't begin here…",
            "The merchants said the orders came directly from the upper districts.",
            "If you seek answers, head to the Archmage Tower.",
            "The scholars there guard the city's oldest records."
        };

        // Roaming NPC lines
        private string[] npc1Lines =
        {
            "The vital pipeline between the rural farming belt and our local stalls is failing.",
            "Without proper infrastructure for an 'Urban-Rural Link', the entire district will starve!"
        };

        private string[] npc2Lines =
        {
            "I noticed something strange near the supply wagon...",
            "The distribution manifest was signed with a royal seal, but half of it was burned on purpose!"
        };

        // Phase 2 urgent Chyra line
        private string[] chyraPhase2Lines =
        {
            "Urgency demands action, Scholar! Check your Notes panel and bring your evidence to Cinder Pipes immediately!"
        };

        // ── Sprite / player ─────────────────────────────────────────────
        private Image playact;
        private Point playPos = new Point(580, 552);

        // ── NPC state guards ────────────────────────────────────────────
        private bool hasSpokenToChyra = false;
        private bool hasSpokenToNPC1 = false;
        private bool hasSpokenToNPC2 = false;
        private bool chyraFinished = false;

        // ── Trigger zones ───────────────────────────────────────────────
        private Rectangle chyraTriggerZone = new Rectangle(743, 189, 80, 80);
        private Rectangle npc1TriggerZone = new Rectangle(304, 336, 80, 80);
        private Rectangle npc2TriggerZone = new Rectangle(532, 132, 80, 80);

        // ── Frame-change callback ────────────────────────────────────────
        private void OnFrameChanged(object sender, EventArgs e) => this.Invalidate();

        private Rectangle GetNPCHitBox()
            => new Rectangle(pbChyra.Location.X, pbChyra.Location.Y, pbChyra.Width, pbChyra.Height);

        // ── Constructor ──────────────────────────────────────────────────
        public MarrowMarketMain()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            sprite.Load();

            UIHelper.AddButtonScaleEffect(btnStartGame);
            UIHelper.AddButtonScaleEffect(MapBtn);
            this.KeyPreview = true;
        }

        // ── Load ─────────────────────────────────────────────────────────
        private void MarrowMarketMain_Load(object sender, EventArgs e)
        {
            sprite.register(OnFrameChanged);
            playact = sprite.frontidle;

            pnlChyra.Visible = false;
            pnlNPC.Visible = false;
            btnStartGame.Visible = false;

            // If the minigame was just cleared, jump straight to transition dialogue
            if (SessionContent.MinigameClearedForCurrentDistrict)
            {
                hasSpokenToChyra = true; // prevent re-triggering intro
                TriggerDialogue(chyraTransitionLines, "Chyra Veyne", isChyra: true, isTransition: true);
            }
        }

        // ── Input ─────────────────────────────────────────────────────────
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && dialogueActive)
            {
                AdvanceDialogue();
                return true;
            }

            if (!dialogueActive)
            {
                int speed = 12;
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
                playact = nextSprite; // always face direction even if blocked

                CheckNPCProximity();

                this.Text = $"X: {playPos.X}, Y: {playPos.Y}";
                this.Invalidate();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MarrowMarketMain_KeyUp(object sender, KeyEventArgs e)
        {
            playact = sprite.frontidle;
            this.Invalidate();
        }

        private void MarrowMarketMain_KeyDown(object sender, KeyEventArgs e)
        {
            this.Invalidate();
        }

        // ── NPC proximity check ───────────────────────────────────────────
        private void CheckNPCProximity()
        {
            if (dialogueActive) return;

            Rectangle playerHitbox = new Rectangle(playPos.X, playPos.Y, 100, 100);
            string user = SessionContent.CurrentActivePlayer?.Username;

            // ── Phase 2: override — only Chyra gives urgent message ───────
            if (SessionContent.CurrentPhase == SessionContent.GamePhase.Phase2_Madman)
            {
                if (!hasSpokenToChyra && playerHitbox.IntersectsWith(chyraTriggerZone))
                {
                    hasSpokenToChyra = true;
                    TriggerDialogue(chyraPhase2Lines, "Chyra Veyne", isChyra: true);
                }
                return;
            }

            // ── Chyra (main NPC) ──────────────────────────────────────────
            if (!hasSpokenToChyra && playerHitbox.IntersectsWith(chyraTriggerZone))
            {
                hasSpokenToChyra = true;

                if (!SessionContent.MinigameClearedForCurrentDistrict)
                    TriggerDialogue(chyraIntroLines, "Chyra Veyne", isChyra: true);
                else
                    TriggerDialogue(chyraTransitionLines, "Chyra Veyne", isChyra: true, isTransition: true);

                return;
            }

            // ── NPC 1 (SDG / Market Overseer) ────────────────────────────
            if (!hasSpokenToNPC1 && playerHitbox.IntersectsWith(npc1TriggerZone))
            {
                hasSpokenToNPC1 = true;
                TriggerDialogue(npc1Lines, "Market Overseer");
                return;
            }

            // ── NPC 2 (Hint / Scout) ──────────────────────────────────────
            if (!hasSpokenToNPC2 && playerHitbox.IntersectsWith(npc2TriggerZone))
            {
                hasSpokenToNPC2 = true;
                TriggerDialogue(npc2Lines, "Scout");
                return;
            }
        }

        // ── Dialogue helpers ──────────────────────────────────────────────
        private bool _isTransitionDialogue = false;
        private bool _isPhase2Dialogue = false;

        private void TriggerDialogue(string[] lines, string speakerName,
                                     bool isChyra = false, bool isTransition = false, bool isPhase2 = false)
        {
            dialogueActive = true;
            currentLineIndex = 0;
            activeLines = lines;
            playact = sprite.frontidle;
            btnStartGame.Visible = false;
            this.ActiveControl = null;
            _isTransitionDialogue = isTransition;
            _isPhase2Dialogue = isPhase2;

            if (isChyra)
            {
                pnlChyra.Visible = true;
                pnlNPC.Visible = false;
                UIHelper.StartTypewriter(lblDialogue, activeLines[0]);
            }
            else
            {
                pnlNPC.Visible = true;
                pnlChyra.Visible = false;
                lblNPCName.Text = speakerName;
                UIHelper.StartTypewriter(lblNPCDialogue, activeLines[0]);
            }
        }

        private void AdvanceDialogue()
        {
            // ── chyraFinished sentinel: one extra Enter to dismiss ─────────
            if (chyraFinished)
            {
                chyraFinished = false;
                dialogueActive = false;
                this.ActiveControl = null;
                btnStartGame.Visible = true;
                btnStartGame.TabStop = false;
                return;
            }

            currentLineIndex++;

            if (currentLineIndex < activeLines.Length)
            {
                if (pnlChyra.Visible)
                    UIHelper.StartTypewriter(lblDialogue, activeLines[currentLineIndex]);
                else
                    UIHelper.StartTypewriter(lblNPCDialogue, activeLines[currentLineIndex]);
            }
            else
            {
                // Reached the end of current dialogue set
                if (pnlChyra.Visible)
                {
                    if (_isTransitionDialogue)
                    {
                        // Post-minigame Chyra transition finished → unlock district
                        dialogueActive = false;
                        pnlChyra.Visible = false;
                        this.ActiveControl = null;

                        SessionContent.MinigameClearedForCurrentDistrict = false;
                        SessionContent.PurifyDistrict("Marrow Market");

                        MessageBox.Show("'Delayed Supply Manifest' added to notebook!",
                            "Clue Discovered", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MessageBox.Show(
                            $"{SessionContent.GetNextDistrict("Marrow Market")} has been unlocked!",
                            "District Unlocked");

                        ReturnToWorldMap();
                    }
                    else
                    {
                        if (_isPhase2Dialogue)
                        {
                            _isPhase2Dialogue = false;
                            dialogueActive = false;
                            pnlChyra.Visible = false;
                            this.ActiveControl = null;
                            return;
                        }

                        // Intro dialogue — show "The choice is yours…" then await one more Enter
                        chyraFinished = true;
                        UIHelper.StartTypewriter(lblDialogue, "The choice is yours...");
                    }
                }
                else
                {
                    dialogueActive = false;
                    pnlNPC.Visible = false;
                    this.ActiveControl = null;
                }
            }
        }

        // ── Paint ─────────────────────────────────────────────────────────
        protected override void OnPaintBackground(PaintEventArgs e) { /* suppress default */ }

        private void MarrowMarketMain_Paint(object sender, PaintEventArgs e)
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

        // ── Button handlers ───────────────────────────────────────────────
        private void btnStartGame_Click(object sender, EventArgs e)
        {
            MarrowFamilies marrowFamilies = new MarrowFamilies();
            marrowFamilies.Show();
            this.Close();
        }

        private void MapBtn_Click(object sender, EventArgs e) => ReturnToWorldMap();

        private void btnBackToMap_Click(object sender, EventArgs e) => ReturnToWorldMap();

        private void ReturnToWorldMap()
        {
            Map worldMap = new Map();
            worldMap.Show();
            this.Close();
        }
    }
}
