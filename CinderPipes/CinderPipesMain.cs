using Echoes_of_Montmauve.EidraxisHall;
using Echoes_of_Montmauve.GameLogic;
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
    public partial class CinderPipesMain : Form
    {
        private bool dialogueActive = false;
        private bool corvenFinished = false;
        private bool hasSpokenToCorven = false;
        private bool hasSpokenToMadman = false;      // NEW: guards Madman trigger
        private bool _isTransitionDialogue = false;   // NEW: mirrors MarrowMarketMain pattern
        private bool _isPhase2Dialogue = false;
        private string[] activeLines;

        private int currentLineIndex = 0;
        private Button MapBtn;
        private Panel pnlCorven;
        private Button btnStartGame;
        private Label lblDialogue;
        private PictureBox pbCorven;

        // ── Corven intro dialogue (before minigame) ──────────────────────────
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

        // ── Corven transition dialogue (after minigame is cleared) ────────────
        private string[] corvenTransitionLines =
        {
            "The pressure is stable again… but this ledger proves the damage was ordered.",
            "Someone rerouted repairs away from the lower districts and left these pipes to fail.",
            "Keep the ledger close, Scholar. It is the final thread tying the city's collapse together."
        };

        // ── Madman encounter (after all districts are purified) ───────────────
        private string[] madmanTransitionLines =
        {
            "You're already too late.",
            "So… you fixed the pipes. How quaint.",
            "Did you think that would stop anything? The rot runs deeper than iron and rust.",
            "The Accord was broken long before your arrival, Scholar.",
            "Seventy-two hours remain before Montmauve's last protections fail."
        };

        private Image playact;
        private PictureBox pbMadman;
        private Panel pnlMadman;
        private Label lblMadmanDialogue;
        private Point playPos = new Point(580, 552);

        // ── Trigger zones ────────────────────────────────────────────────────
        // Corven zone is defined by pbCorven's position (192, 171) — kept as-is
        // Madman trigger zone — adjust X/Y to match pbMadman.Location (607, 200)
        private Rectangle madmanTriggerZone = new Rectangle(607, 200, 80, 80);

        private void OnFrameChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private Rectangle GetNPCHitBox()
        {
            return new Rectangle(pbCorven.Location.X, pbCorven.Location.Y,
                                 pbCorven.Width, pbCorven.Height);
        }

        public CinderPipesMain()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            sprite.Load();
            UIHelper.AddButtonScaleEffect(btnStartGame);
            UIHelper.AddButtonScaleEffect(MapBtn);

            this.KeyPreview = true;
        }

        private void CinderPipesMain_Load(object sender, EventArgs e)
        {
            sprite.register(OnFrameChanged);
            playact = sprite.frontidle;

            pnlCorven.Visible = false;
            pnlMadman.Visible = false;
            btnStartGame.Visible = false;
            pbMadman.Visible = SessionContent.CurrentPhase == SessionContent.GamePhase.Phase1_Containment;

            // ── If minigame was just cleared, jump straight to Corven's transition ──
            if (SessionContent.MinigameClearedForCurrentDistrict)
            {
                hasSpokenToCorven = true;    // prevent Corven intro re-triggering
                TriggerDialogue(corvenTransitionLines, isCorven: true, isTransition: true);
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

                this.Text = $"X: {playPos.X}, Y: {playPos.Y}"; // remove after finding coords
                this.Invalidate();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CinderPipesMain_KeyUp(object sender, KeyEventArgs e)
        {
            playact = sprite.frontidle;
            this.Invalidate();
        }

        // ── NPC proximity check ───────────────────────────────────────────────
        private void CheckNPCProximity()
        {
            if (dialogueActive) return;

            Rectangle playerHitbox = new Rectangle(playPos.X, playPos.Y, 100, 100);
            Rectangle corvenTriggerZone = new Rectangle(192, 171, 80, 80);

            // ── Phase 2: only Corven can be triggered on revisits ─────────────
            if (SessionContent.CurrentPhase == SessionContent.GamePhase.Phase2_Madman
                && !hasSpokenToCorven
                && playerHitbox.IntersectsWith(corvenTriggerZone))
            {
                hasSpokenToCorven = true;
                TriggerDialogue(new[]
                {
                    "The clock is moving now, Scholar.",
                    "Seal the pipes one last time. If the ledger holds, the final accusation can begin."
                }, isCorven: true, isTransition: false);
                return;
            }

            // ── Corven intro (only before minigame is cleared) ────────────────
            if (!hasSpokenToCorven && !SessionContent.MinigameClearedForCurrentDistrict
                && playerHitbox.IntersectsWith(corvenTriggerZone))
            {
                hasSpokenToCorven = true;
                TriggerDialogue(dialogueLines, isCorven: true, isTransition: false);
                return;
            }

            // ── Madman encounter starts automatically after final purification ─
            if (!hasSpokenToMadman
                && SessionContent.AreAllDistrictsPurified()
                && SessionContent.CurrentPhase == SessionContent.GamePhase.Phase1_Containment)
            {
                hasSpokenToMadman = true;
                TriggerDialogue(madmanTransitionLines, isCorven: false, isTransition: false);
                return;
            }
        }

        // ── Dialogue helpers ──────────────────────────────────────────────────
        private void TriggerDialogue(string[] lines, bool isCorven, bool isTransition, bool isPhase2 = false)
        {
            dialogueActive = true;
            currentLineIndex = 0;
            activeLines = lines;
            playact = sprite.frontidle;
            btnStartGame.Visible = false;
            this.ActiveControl = null;
            _isTransitionDialogue = isTransition;
            _isPhase2Dialogue = isPhase2;

            if (isCorven)
            {
                pnlCorven.Visible = true;
                pnlMadman.Visible = false;
                UIHelper.StartTypewriter(lblDialogue, activeLines[0]);
            }
            else
            {
                pnlMadman.Visible = true;
                pnlCorven.Visible = false;
                UIHelper.StartTypewriter(lblMadmanDialogue, activeLines[0]);
            }
        }

        private void AdvanceDialogue()
        {
            // ── Corven "The choice is yours…" sentinel ────────────────────────
            if (corvenFinished)
            {
                corvenFinished = false;
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
                if (pnlCorven.Visible)
                    UIHelper.StartTypewriter(lblDialogue, activeLines[currentLineIndex]);
                else
                    UIHelper.StartTypewriter(lblMadmanDialogue, activeLines[currentLineIndex]);
            }
            else
            {
                // ── End of dialogue set ───────────────────────────────────────
                if (_isTransitionDialogue)
                {
                    // Post-minigame Corven transition finished → purify district.
                    dialogueActive = false;
                    pnlCorven.Visible = false;
                    this.ActiveControl = null;

                    SessionContent.MinigameClearedForCurrentDistrict = false;
                    SessionContent.PurifyDistrict("Cinder Pipes");

                    MessageBox.Show("'Nexus Directive' added to notebook!",
                        "Clue Discovered", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (SessionContent.CurrentPhase == SessionContent.GamePhase.Phase2_Madman)
                    {
                        SessionContent.AdvanceToPhase3();
                        FinalAccusationForm finalAccForm = new FinalAccusationForm(isTimeout: false);
                        finalAccForm.Show();
                        this.Close();
                        return;
                    }

                    MessageBox.Show(
                        $"{SessionContent.GetNextDistrict("Cinder Pipes")} has been unlocked!",
                        "District Unlocked", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    TriggerDialogue(madmanTransitionLines, isCorven: false, isTransition: false);
                }
                else
                {
                    // Corven intro — show prompt then await one more Enter
                    if (pnlCorven.Visible)
                    {
                        if (_isPhase2Dialogue)
                        {
                            _isPhase2Dialogue = false;
                            dialogueActive = false;
                            pnlCorven.Visible = false;
                            this.ActiveControl = null;
                            return;
                        }

                        corvenFinished = true;
                        UIHelper.StartTypewriter(lblDialogue, "The choice is yours...");
                    }
                    else
                    {
                        dialogueActive = false;
                        pnlMadman.Visible = false;
                        this.ActiveControl = null;

                        if (SessionContent.AreAllDistrictsPurified()
                            && SessionContent.CurrentPhase == SessionContent.GamePhase.Phase1_Containment)
                        {
                            SessionContent.StartPhase2Loop();
                            ReturnToWorldMap();
                        }
                    }
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e) { }

        private void CinderPipesMain_Paint(object sender, PaintEventArgs e)
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

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(CinderPipesMain));
            MapBtn = new Button();
            pnlCorven = new Panel();
            btnStartGame = new Button();
            lblDialogue = new Label();
            pbCorven = new PictureBox();
            pbMadman = new PictureBox();
            pnlMadman = new Panel();
            lblMadmanDialogue = new Label();
            pnlCorven.SuspendLayout();
            ((ISupportInitialize)pbCorven).BeginInit();
            ((ISupportInitialize)pbMadman).BeginInit();
            pnlMadman.SuspendLayout();
            SuspendLayout();
            // 
            // MapBtn
            // 
            MapBtn.BackColor = Color.Transparent;
            MapBtn.BackgroundImage = (Image)resources.GetObject("MapBtn.BackgroundImage");
            MapBtn.BackgroundImageLayout = ImageLayout.Stretch;
            MapBtn.FlatAppearance.BorderColor = Color.SaddleBrown;
            MapBtn.FlatAppearance.BorderSize = 0;
            MapBtn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            MapBtn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            MapBtn.FlatStyle = FlatStyle.Flat;
            MapBtn.Font = new Font("Calisto MT", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            MapBtn.ForeColor = Color.Transparent;
            MapBtn.Location = new Point(12, 6);
            MapBtn.Name = "MapBtn";
            MapBtn.Size = new Size(82, 85);
            MapBtn.TabIndex = 9;
            MapBtn.TabStop = false;
            MapBtn.UseVisualStyleBackColor = false;
            MapBtn.Click += MapBtn_Click;
            // 
            // pnlCorven
            // 
            pnlCorven.BackColor = Color.Transparent;
            pnlCorven.BackgroundImage = (Image)resources.GetObject("pnlCorven.BackgroundImage");
            pnlCorven.BackgroundImageLayout = ImageLayout.Center;
            pnlCorven.Controls.Add(btnStartGame);
            pnlCorven.Controls.Add(lblDialogue);
            pnlCorven.Dock = DockStyle.Bottom;
            pnlCorven.Location = new Point(0, 308);
            pnlCorven.Name = "pnlCorven";
            pnlCorven.Size = new Size(1262, 345);
            pnlCorven.TabIndex = 8;
            // 
            // btnStartGame
            // 
            btnStartGame.BackColor = Color.PapayaWhip;
            btnStartGame.FlatAppearance.BorderColor = Color.Gold;
            btnStartGame.FlatAppearance.BorderSize = 2;
            btnStartGame.FlatStyle = FlatStyle.Flat;
            btnStartGame.Font = new Font("Calisto MT", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStartGame.ForeColor = Color.Goldenrod;
            btnStartGame.Location = new Point(881, 265);
            btnStartGame.Name = "btnStartGame";
            btnStartGame.Size = new Size(181, 44);
            btnStartGame.TabIndex = 8;
            btnStartGame.TabStop = false;
            btnStartGame.Text = "Start Game";
            btnStartGame.UseVisualStyleBackColor = false;
            btnStartGame.Click += btnStartGame_Click;
            // 
            // lblDialogue
            // 
            lblDialogue.Font = new Font("Calisto MT", 12F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblDialogue.Location = new Point(350, 204);
            lblDialogue.Name = "lblDialogue";
            lblDialogue.Size = new Size(712, 58);
            lblDialogue.TabIndex = 0;
            // 
            // pbCorven
            // 
            pbCorven.BackColor = Color.Transparent;
            pbCorven.BackgroundImage = (Image)resources.GetObject("pbCorven.BackgroundImage");
            pbCorven.BackgroundImageLayout = ImageLayout.Stretch;
            pbCorven.Location = new Point(192, 171);
            pbCorven.Name = "pbCorven";
            pbCorven.Size = new Size(73, 102);
            pbCorven.TabIndex = 14;
            pbCorven.TabStop = false;
            // 
            // pbMadman
            // 
            pbMadman.BackColor = Color.Transparent;
            pbMadman.BackgroundImage = (Image)resources.GetObject("pbMadman.BackgroundImage");
            pbMadman.BackgroundImageLayout = ImageLayout.Stretch;
            pbMadman.Location = new Point(607, 200);
            pbMadman.Name = "pbMadman";
            pbMadman.Size = new Size(73, 102);
            pbMadman.TabIndex = 15;
            pbMadman.TabStop = false;
            // 
            // pnlMadman
            // 
            pnlMadman.BackColor = Color.Transparent;
            pnlMadman.BackgroundImage = (Image)resources.GetObject("pnlMadman.BackgroundImage");
            pnlMadman.BackgroundImageLayout = ImageLayout.Center;
            pnlMadman.Controls.Add(lblMadmanDialogue);
            pnlMadman.Dock = DockStyle.Bottom;
            pnlMadman.Location = new Point(0, -37);
            pnlMadman.Name = "pnlMadman";
            pnlMadman.Size = new Size(1262, 345);
            pnlMadman.TabIndex = 16;
            pnlMadman.Visible = false;
            // 
            // lblMadmanDialogue
            // 
            lblMadmanDialogue.Font = new Font("Calisto MT", 12F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblMadmanDialogue.Location = new Point(350, 226);
            lblMadmanDialogue.Name = "lblMadmanDialogue";
            lblMadmanDialogue.Size = new Size(712, 58);
            lblMadmanDialogue.TabIndex = 0;
            // 
            // CinderPipesMain
            // 
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1262, 653);
            Controls.Add(pnlMadman);
            Controls.Add(pbMadman);
            Controls.Add(pbCorven);
            Controls.Add(MapBtn);
            Controls.Add(pnlCorven);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "CinderPipesMain";
            StartPosition = FormStartPosition.CenterScreen;
            Load += CinderPipesMain_Load;
            Paint += CinderPipesMain_Paint;
            KeyUp += CinderPipesMain_KeyUp;
            pnlCorven.ResumeLayout(false);
            ((ISupportInitialize)pbCorven).EndInit();
            ((ISupportInitialize)pbMadman).EndInit();
            pnlMadman.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void MapBtn_Click(object sender, EventArgs e)
        {
            ReturnToWorldMap();
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            LeakFixFrenzy fixFrenzy = new LeakFixFrenzy();
            fixFrenzy.Show();
            this.Close();
        }

        private void ReturnToWorldMap()
        {
            Map map = new Map();
            map.Show();
            this.Close();
        }
    }
}
