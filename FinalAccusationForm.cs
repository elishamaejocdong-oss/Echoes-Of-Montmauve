using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Echoes_of_Montmauve.GameLogic;

namespace Echoes_of_Montmauve
{
    public partial class FinalAccusationForm : Form
    {
        private bool internalTimeoutFlag = false;
        private Label lblSceneTitle;
        private Label lblSceneSubtitle;
        private Label lblEvidenceTitle;
        private Label lblEvidenceText;
        private Label lblCulpritPrompt;
        private Label lblEndingBody;
        private Panel pnlEvidenceCard;
        private Panel pnlVerdictCard;
        private Color verdictAccent = Color.Goldenrod;

        public FinalAccusationForm(bool isTimeout)
        {
            InitializeComponent();
            this.internalTimeoutFlag = isTimeout;
            ConfigureAesthetic();
            BuildSelectionScreen();
            BuildEndingScreen();
        }

        private void FinalAccusationForm_Load(object sender, EventArgs e)
        {
            if (internalTimeoutFlag)
            {
                TriggerEndingSequence("FAILURE ENDING", "The 72-hour sequence expired. The containment fields failed entirely, leaving Montmauve exposed to permanent toxic atmospheric collapse.");
                pnlSelectionElements.Enabled = false;
                return;
            }

            cmbCulprits.Items.Add("Moriarty");
            cmbCulprits.Items.Add("Archmage Xerion");
            cmbCulprits.Items.Add("The Royal Council");
            cmbCulprits.SelectedIndex = 0;
        }

        private void btnSubmmitAccusation_Click(object sender, EventArgs e)
        {
            string chosenCulprit = cmbCulprits.SelectedItem?.ToString() ?? "";

            if (chosenCulprit == "The Royal Council")
            {
                TriggerEndingSequence("TRUE ENDING", "Your evidence holds under the eyes of the city. Ledger marks, diverted repairs, and burned supply manifests all point to the same hand: the Royal Council. The chamber breaks into uproar as the lower districts stand together, and Montmauve finally turns its judgment toward the architects of the Miasma.");
            }
            else
            {
                TriggerEndingSequence("WRONG ENDING", $"You name {chosenCulprit}, and the chamber accepts the easier story. Yet the evidence slips out of alignment, one clue at a time. The true conspirators vanish behind ceremony and sealed doors while the Royal Council continues to steer Montmauve into the Miasma.");
            }
        }

        private void TriggerEndingSequence(string title, string storyText)
        {
            if (SessionContent.CurrentActivePlayer != null)
                DatabaseManager.RecordEndingUnlocked(SessionContent.CurrentActivePlayer.Username, title);

            verdictAccent = title == "TRUE ENDING"
                ? Color.FromArgb(214, 175, 72)
                : title == "FAILURE ENDING"
                    ? Color.FromArgb(143, 76, 111)
                    : Color.FromArgb(166, 66, 58);

            lblEndingTitle.Text = title;
            lblEndingTitle.ForeColor = verdictAccent;
            lblEndingBody.Text = storyText;
            txtEndingNarrative.Text = storyText;
            pnlSelectionElements.Visible = false;
            pnlEndingScreen.Visible = true;
            pnlEndingScreen.BringToFront();
            pnlVerdictCard.Invalidate();
            Invalidate();
        }

        private void btnExitGame_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ConfigureAesthetic()
        {
            DoubleBuffered = true;
            Text = "Final Accusation";
            BackColor = Color.FromArgb(29, 19, 34);
            Font = new Font("Calisto MT", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);

            pnlSelectionElements.BackColor = Color.Transparent;
            pnlSelectionElements.Location = new Point(102, 60);
            pnlSelectionElements.Size = new Size(1058, 535);

            pnlEndingScreen.BackColor = Color.Transparent;

            StyleButton(btnSubmmitAccusation);
            StyleButton(btnExitGame);
            UIHelper.AddButtonScaleEffect(btnSubmmitAccusation);
            UIHelper.AddButtonScaleEffect(btnExitGame);
        }

        private void BuildSelectionScreen()
        {
            pnlSelectionElements.Controls.Clear();

            lblSceneTitle = CreateLabel("THE FINAL ACCUSATION", 26F, FontStyle.Bold, Color.FromArgb(242, 211, 143), ContentAlignment.MiddleCenter);
            lblSceneTitle.SetBounds(0, 0, pnlSelectionElements.Width, 52);

            lblSceneSubtitle = CreateLabel("The city waits beneath a violet hush. Choose the power behind the Miasma.", 12.5F, FontStyle.Italic, Color.FromArgb(226, 207, 230), ContentAlignment.MiddleCenter);
            lblSceneSubtitle.SetBounds(110, 54, 840, 34);

            pnlEvidenceCard = new Panel
            {
                BackColor = Color.Transparent,
                Location = new Point(96, 118),
                Size = new Size(866, 228)
            };
            pnlEvidenceCard.Paint += ParchmentPanel_Paint;

            lblEvidenceTitle = CreateLabel("Evidence Before The Council", 16F, FontStyle.Bold, Color.FromArgb(100, 50, 37), ContentAlignment.MiddleLeft);
            lblEvidenceTitle.SetBounds(40, 28, 780, 34);

            lblEvidenceText = CreateLabel(
                "Delayed supply manifests. Rerouted repairs. Burned allocation orders. Every district points upward, past the scapegoats and into the sealed chambers of authority.",
                12.5F, FontStyle.Italic, Color.FromArgb(58, 38, 34), ContentAlignment.TopLeft);
            lblEvidenceText.SetBounds(40, 74, 780, 88);

            lblCulpritPrompt = CreateLabel("Name the culprit", 13F, FontStyle.Bold, Color.FromArgb(92, 42, 35), ContentAlignment.MiddleLeft);
            lblCulpritPrompt.SetBounds(40, 168, 240, 32);

            cmbCulprits.Location = new Point(282, 168);
            cmbCulprits.Size = new Size(390, 34);
            cmbCulprits.BackColor = Color.FromArgb(255, 240, 211);
            cmbCulprits.ForeColor = Color.Maroon;
            cmbCulprits.Font = new Font("Calisto MT", 13.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmbCulprits.FlatStyle = FlatStyle.Flat;

            btnSubmmitAccusation.Location = new Point(692, 164);
            btnSubmmitAccusation.Size = new Size(132, 46);
            btnSubmmitAccusation.Text = "Judge";

            pnlEvidenceCard.Controls.Add(lblEvidenceTitle);
            pnlEvidenceCard.Controls.Add(lblEvidenceText);
            pnlEvidenceCard.Controls.Add(lblCulpritPrompt);
            pnlEvidenceCard.Controls.Add(cmbCulprits);
            pnlEvidenceCard.Controls.Add(btnSubmmitAccusation);

            pnlSelectionElements.Controls.Add(lblSceneTitle);
            pnlSelectionElements.Controls.Add(lblSceneSubtitle);
            pnlSelectionElements.Controls.Add(pnlEvidenceCard);
        }

        private void BuildEndingScreen()
        {
            pnlEndingScreen.Controls.Clear();

            pnlVerdictCard = new Panel
            {
                BackColor = Color.Transparent,
                Location = new Point(142, 92),
                Size = new Size(978, 456)
            };
            pnlVerdictCard.Paint += VerdictPanel_Paint;

            lblEndingTitle.AutoSize = false;
            lblEndingTitle.Font = new Font("Calisto MT", 28F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEndingTitle.ForeColor = Color.Goldenrod;
            lblEndingTitle.Location = new Point(54, 52);
            lblEndingTitle.Size = new Size(870, 62);
            lblEndingTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblEndingTitle.BackColor = Color.Transparent;

            lblEndingBody = CreateLabel("", 15F, FontStyle.Italic, Color.FromArgb(248, 232, 196), ContentAlignment.TopCenter);
            lblEndingBody.SetBounds(104, 146, 770, 182);

            txtEndingNarrative.Visible = false;

            btnExitGame.Location = new Point(354, 354);
            btnExitGame.Size = new Size(270, 54);
            btnExitGame.Text = "Close Case";

            pnlVerdictCard.Controls.Add(lblEndingTitle);
            pnlVerdictCard.Controls.Add(lblEndingBody);
            pnlVerdictCard.Controls.Add(btnExitGame);
            pnlEndingScreen.Controls.Add(pnlVerdictCard);
        }

        private Label CreateLabel(string text, float size, FontStyle style, Color color, ContentAlignment align)
        {
            return new Label
            {
                Text = text,
                AutoSize = false,
                BackColor = Color.Transparent,
                Font = new Font("Calisto MT", size, style, GraphicsUnit.Point, 0),
                ForeColor = color,
                TextAlign = align
            };
        }

        private void StyleButton(Button button)
        {
            button.BackColor = Color.FromArgb(255, 229, 184);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(117, 66, 38);
            button.FlatAppearance.BorderSize = 2;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 215, 157);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(218, 164, 102);
            button.Font = new Font("Calisto MT", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button.ForeColor = Color.Maroon;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(ClientRectangle,
                Color.FromArgb(20, 14, 30),
                Color.FromArgb(68, 38, 66),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }

            DrawMiasma(e.Graphics);
        }

        private void DrawMiasma(Graphics graphics)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen goldPen = new Pen(Color.FromArgb(70, 214, 175, 72), 2F))
            using (Pen mistPen = new Pen(Color.FromArgb(58, 188, 147, 190), 5F))
            {
                graphics.DrawLine(goldPen, 86, 74, Width - 86, 74);
                graphics.DrawLine(goldPen, 86, Height - 74, Width - 86, Height - 74);

                graphics.DrawBezier(mistPen, -80, 190, 280, 90, 500, 312, Width + 80, 160);
                graphics.DrawBezier(mistPen, -60, 482, 280, 594, 724, 332, Width + 90, 520);
            }
        }

        private void ParchmentPanel_Paint(object sender, PaintEventArgs e)
        {
            DrawPanel(e.Graphics, pnlEvidenceCard.ClientRectangle,
                Color.FromArgb(230, 255, 239, 198),
                Color.FromArgb(170, 104, 63, 38));
        }

        private void VerdictPanel_Paint(object sender, PaintEventArgs e)
        {
            DrawPanel(e.Graphics, pnlVerdictCard.ClientRectangle,
                Color.FromArgb(224, 39, 24, 45),
                Color.FromArgb(190, verdictAccent.R, verdictAccent.G, verdictAccent.B));
        }

        private void DrawPanel(Graphics graphics, Rectangle bounds, Color fill, Color border)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 5, bounds.Height - 5);
            using (GraphicsPath path = RoundedRect(rect, 8))
            using (SolidBrush fillBrush = new SolidBrush(fill))
            using (Pen borderPen = new Pen(border, 3F))
            {
                graphics.FillPath(fillBrush, path);
                graphics.DrawPath(borderPen, path);
            }
        }

        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
