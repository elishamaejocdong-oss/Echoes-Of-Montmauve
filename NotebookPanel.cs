using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NotebookPanel
{
    // ══════════════════════════════════════════════════════════════════════
    //  DATA MODELS
    // ══════════════════════════════════════════════════════════════════════
    public class TermEntry
    {
        public string Word       { get; set; }
        public string Definition { get; set; }
        public bool   Locked     { get; set; }
    }

    public class ClueEntry
    {
        public string Title       { get; set; }
        public Image  Thumbnail   { get; set; }
        public string Information { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════
    //  GRAPHICS EXTENSION HELPERS
    // ══════════════════════════════════════════════════════════════════════
    internal static class GraphicsExtensions
    {
        public static void FillRoundedRect(this Graphics g, Brush brush, Rectangle rect, int radius)
        {
            using var path = RoundedRectPath(rect, radius);
            g.FillPath(brush, path);
        }
        public static void DrawRoundedRect(this Graphics g, Pen pen, Rectangle rect, int radius)
        {
            using var path = RoundedRectPath(rect, radius);
            g.DrawPath(pen, path);
        }
        private static GraphicsPath RoundedRectPath(Rectangle rect, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(rect.X,         rect.Y,          d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y,          d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d,   0, 90);
            path.AddArc(rect.X,         rect.Bottom - d, d, d,  90, 90);
            path.CloseFigure();
            return path;
        }
    }

    // ══════════════════════════════════════════════════════════════════════
    //  MAIN USER CONTROL
    // ══════════════════════════════════════════════════════════════════════
    /// <summary>
    /// A reusable notebook-style panel control.
    ///
    /// USAGE (in any WinForms Form):
    ///
    ///   var notebook = new NotebookPanelControl();
    ///   notebook.Dock = DockStyle.Fill;
    ///   this.Controls.Add(notebook);
    ///
    ///   // Unlock a term after an NPC interaction:
    ///   notebook.AddTerm("Montmauve", "A violet realm that exists between heartbeats...");
    ///
    ///   // Add a locked (unrevealed) placeholder:
    ///   notebook.AddTerm("The Warden", "???", locked: true);
    ///   // Later, unlock it:
    ///   notebook.UnlockTerm("The Warden", "Guardian of the threshold, bound by an oath of silence.");
    ///
    ///   // Add a clue after a mini-game:
    ///   notebook.AddClue("Torn Letter", Image.FromFile("clue_letter.png"), "Discovered in the attic.\nSeems to reference a meeting at midnight.");
    ///
    /// </summary>
    public class NotebookPanelControl : UserControl
    {
        // ── Palette ────────────────────────────────────────────────────────
        private static readonly Color C_PageLeft    = Color.FromArgb(238, 228, 218);
        private static readonly Color C_PageRight   = Color.FromArgb(250, 244, 236);
        private static readonly Color C_Cover       = Color.FromArgb(245, 238, 228);
        private static readonly Color C_Spine       = Color.FromArgb(180, 165, 190);
        private static readonly Color C_Ring        = Color.FromArgb(160, 148, 172);
        private static readonly Color C_Border      = Color.FromArgb(178, 158, 190);
        private static readonly Color C_TabActive   = Color.FromArgb(210, 190, 220);
        private static readonly Color C_TabInactive = Color.FromArgb(190, 172, 200);
        private static readonly Color C_TabText     = Color.FromArgb(80,  60,  90);
        private static readonly Color C_WordNormal  = Color.FromArgb(100, 80, 110);
        private static readonly Color C_WordHover   = Color.FromArgb(160, 80, 120);
        private static readonly Color C_WordSel     = Color.FromArgb(130, 50,  90);
        private static readonly Color C_DefText     = Color.FromArgb(70,  55,  75);
        private static readonly Color C_Locked      = Color.FromArgb(190, 180, 195);
        private static readonly Color C_Header      = Color.FromArgb(140, 110, 150);
        private static readonly Color C_LineRule    = Color.FromArgb(25,  180, 158, 190);

        private static readonly Color[] SideTabColors =
        {
            Color.FromArgb(200, 220, 130,  80),
            Color.FromArgb(200,  80, 170, 160),
            Color.FromArgb(200, 220, 190,  70),
            Color.FromArgb(200, 210, 130, 150),
            Color.FromArgb(200, 220, 130,  80),
            Color.FromArgb(200,  80, 170, 160),
            Color.FromArgb(200, 220, 190,  70),
            Color.FromArgb(200, 210, 130, 150),
        };

        // ── Layout constants ───────────────────────────────────────────────
        private const int SPINE_W      = 44;
        private const int RING_COUNT   = 12;
        private const int TAB_H        = 36;
        private const int SIDE_TAB_W   = 18;
        private const int CORNER_R     = 18;

        // ── Sub-controls ───────────────────────────────────────────────────
        private Panel           _tabBar;
        private Button          _btnFiles, _btnClues;
        private Panel           _body, _leftPage, _spine, _rightPage;

        // Montmauve Files
        private FlowLayoutPanel _wordList;
        private Panel           _defPanel;
        private Label           _defTitle, _defBody;

        // Clues
        private FlowLayoutPanel _clueList;
        private Panel           _clueDetail;
        private PictureBox      _cluePic;
        private Label           _clueInfo;

        private enum Tab { Files, Clues }
        private Tab _activeTab;

        private readonly List<TermEntry> _terms = new List<TermEntry>();
        private readonly List<ClueEntry> _clues = new List<ClueEntry>();

        // ══════════════════════════════════════════════════════════════════
        public NotebookPanelControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint  |
                     ControlStyles.UserPaint             |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            Size      = new Size(820, 540);
            BackColor = Color.Transparent;

            BuildControls();
            SwitchTab(Tab.Files);
        }

        // ══════════════════════════════════════════════════════════════════
        //  PUBLIC API
        // ══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Add a term. Set locked=true to show it as "???" until unlocked.
        /// </summary>
        public void AddTerm(string word, string definition, bool locked = false)
        {
            var e = new TermEntry { Word = word, Definition = definition, Locked = locked };
            _terms.Add(e);
            AttachWordButton(e);
        }

        /// <summary>
        /// Unlock a locked term and reveal its definition.
        /// </summary>
        public void UnlockTerm(string word, string definition = null)
        {
            foreach (var t in _terms)
            {
                if (t.Word.Equals(word, StringComparison.OrdinalIgnoreCase))
                {
                    t.Locked = false;
                    if (definition != null) t.Definition = definition;
                    RebuildWordList();
                    break;
                }
            }
        }

        /// <summary>
        /// Add a clue card. thumbnail can be null if no image is available yet.
        /// </summary>
        public void AddClue(string title, Image thumbnail, string information)
        {
            var e = new ClueEntry { Title = title, Thumbnail = thumbnail, Information = information };
            _clues.Add(e);
            AttachClueCard(e);
        }

        // ══════════════════════════════════════════════════════════════════
        //  CONTROL BUILDER
        // ══════════════════════════════════════════════════════════════════
        private void BuildControls()
        {
            // ── Tab bar ────────────────────────────────────────────────────
            _tabBar = new Panel { Dock = DockStyle.Top, Height = TAB_H, BackColor = Color.Transparent };

            _btnFiles = CreateTabBtn("✦  Montmauve Files");
            _btnClues = CreateTabBtn("🔍  Clues");
            _btnFiles.Location = new Point(18, 2);
            _btnClues.Location = new Point(_btnFiles.Right + 4, 2);
            _btnFiles.Click += (s, e) => SwitchTab(Tab.Files);
            _btnClues.Click += (s, e) => SwitchTab(Tab.Clues);
            _tabBar.Controls.AddRange(new Control[] { _btnFiles, _btnClues });

            // ── Body ───────────────────────────────────────────────────────
            _body = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            _body.Paint  += Body_Paint;
            _body.Resize += (s, e) => LayoutPages();

            _leftPage  = new Panel { BackColor = Color.Transparent, Padding = new Padding(10, 10, 4, 10) };
            _spine     = new Panel { BackColor = Color.Transparent };
            _rightPage = new Panel { BackColor = Color.Transparent, Padding = new Padding(4, 10, 14, 10) };

            _body.Controls.AddRange(new Control[] { _leftPage, _spine, _rightPage });

            Controls.Add(_body);
            Controls.Add(_tabBar);

            // ── Montmauve: word list ───────────────────────────────────────
            _wordList = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,
                WrapContents = false, AutoScroll = true, BackColor = Color.Transparent
            };

            // ── Montmauve: definition panel ───────────────────────────────
            _defPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Padding = new Padding(10, 12, 10, 10) };
            _defTitle = new Label
            {
                Dock = DockStyle.Top, Height = 38,
                Font = new Font("Georgia", 14f, FontStyle.Bold | FontStyle.Italic),
                ForeColor = C_WordSel, Text = "", TextAlign = ContentAlignment.MiddleLeft
            };
            _defBody = new Label
            {
                Dock = DockStyle.Fill, AutoSize = false,
                Font = new Font("Georgia", 10.5f), ForeColor = C_DefText,
                Text = "Select a word from the left panel to view its definition.",
                TextAlign = ContentAlignment.TopLeft
            };
            _defPanel.Controls.Add(_defBody);
            _defPanel.Controls.Add(_defTitle);

            // ── Clues: list ────────────────────────────────────────────────
            _clueList = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,
                WrapContents = false, AutoScroll = true, BackColor = Color.Transparent
            };

            // ── Clues: detail ──────────────────────────────────────────────
            _clueDetail = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Padding = new Padding(10) };
            _cluePic = new PictureBox
            {
                Dock = DockStyle.Top, Height = 180, SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(220, 210, 200), BorderStyle = BorderStyle.None
            };
            _clueInfo = new Label
            {
                Dock = DockStyle.Fill, AutoSize = false,
                Font = new Font("Georgia", 10f), ForeColor = C_DefText,
                Text = "Select a clue to view its details.", TextAlign = ContentAlignment.TopLeft,
                Padding = new Padding(0, 10, 0, 0)
            };
            _clueDetail.Controls.Add(_clueInfo);
            _clueDetail.Controls.Add(_cluePic);

            LayoutPages();
        }

        private void LayoutPages()
        {
            int w = _body.ClientSize.Width;
            int h = _body.ClientSize.Height;
            int lw = (w - SPINE_W) / 2;
            int rw = w - SPINE_W - lw;
            _leftPage.SetBounds(0,          0, lw,       h);
            _spine.SetBounds(lw,            0, SPINE_W,  h);
            _rightPage.SetBounds(lw + SPINE_W, 0, rw,    h);
        }

        // ══════════════════════════════════════════════════════════════════
        //  TAB SWITCHING
        // ══════════════════════════════════════════════════════════════════
        private void SwitchTab(Tab tab)
        {
            _activeTab = tab;
            _leftPage.Controls.Clear();
            _rightPage.Controls.Clear();

            StyleTabBtn(_btnFiles, tab == Tab.Files);
            StyleTabBtn(_btnClues, tab == Tab.Clues);

            if (tab == Tab.Files)
            {
                _leftPage.Controls.Add(PageHeader("Lexicon"));
                _leftPage.Controls.Add(_wordList);
                _rightPage.Controls.Add(PageHeader("Definition"));
                _rightPage.Controls.Add(_defPanel);
            }
            else
            {
                _leftPage.Controls.Add(PageHeader("Evidence"));
                _leftPage.Controls.Add(_clueList);
                _rightPage.Controls.Add(PageHeader("Details"));
                _rightPage.Controls.Add(_clueDetail);
            }

            _body.Invalidate(true);
        }

        private static Label PageHeader(string text) =>
            new Label
            {
                Dock = DockStyle.Top, Height = 26,
                Text = text, Font = new Font("Georgia", 10f, FontStyle.Bold | FontStyle.Italic),
                ForeColor = C_Header, TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(6, 0, 0, 0)
            };

        // ══════════════════════════════════════════════════════════════════
        //  DYNAMIC ITEM BUILDERS
        // ══════════════════════════════════════════════════════════════════
        private void AttachWordButton(TermEntry entry)
        {
            var btn = new Button
            {
                Text      = entry.Locked ? "???" : entry.Word,
                Width     = 180, Height = 32,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Georgia", 10f, entry.Locked ? FontStyle.Italic : FontStyle.Regular),
                ForeColor = entry.Locked ? C_Locked : C_WordNormal,
                BackColor = Color.Transparent,
                Cursor    = entry.Locked ? Cursors.Default : Cursors.Hand,
                Tag       = entry,
                Margin    = new Padding(4, 3, 4, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(8, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, C_WordHover);

            btn.MouseEnter += (s, e) => { if (!entry.Locked) btn.ForeColor = C_WordHover; };
            btn.MouseLeave += (s, e) => { if (!entry.Locked && btn.ForeColor != C_WordSel) btn.ForeColor = C_WordNormal; };

            btn.Click += (s, e) =>
            {
                if (entry.Locked) return;
                _defTitle.Text = entry.Word;
                _defBody.Text  = entry.Definition;
                foreach (Control c in _wordList.Controls)
                    if (c is Button b) b.ForeColor = (b.Tag as TermEntry)?.Locked == true ? C_Locked : C_WordNormal;
                btn.ForeColor = C_WordSel;
            };

            _wordList.Controls.Add(btn);
        }

        private void RebuildWordList()
        {
            _wordList.Controls.Clear();
            foreach (var t in _terms) AttachWordButton(t);
        }

        private void AttachClueCard(ClueEntry entry)
        {
            var card = new Panel
            {
                Width = 180, Height = 64,
                BackColor = Color.FromArgb(50, C_Spine),
                Cursor = Cursors.Hand,
                Margin = new Padding(4, 4, 4, 0),
                Tag = entry
            };

            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(Color.FromArgb(100, C_Border), 1f);
                g.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                if (entry.Thumbnail != null)
                    g.DrawImage(entry.Thumbnail, new Rectangle(4, 4, 56, 56));
                using var fnt = new Font("Georgia", 9f, FontStyle.Bold);
                using var br  = new SolidBrush(C_TabText);
                g.DrawString(entry.Title, fnt, br, new RectangleF(66, 8, 110, 48));
            };

            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(80, C_Spine);
            card.MouseLeave += (s, e) => card.BackColor = Color.FromArgb(50, C_Spine);

            card.Click += (s, e) =>
            {
                _cluePic.Image  = entry.Thumbnail;
                _clueInfo.Text  = entry.Information;
            };

            _clueList.Controls.Add(card);
        }

        // ══════════════════════════════════════════════════════════════════
        //  PAINTING
        // ══════════════════════════════════════════════════════════════════
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var notebookRect = new Rectangle(2, TAB_H, Width - 12, Height - TAB_H - 2);

            // Drop shadow
            using var shadow = new SolidBrush(Color.FromArgb(30, 0, 0, 0));
            g.FillRoundedRect(shadow, new Rectangle(notebookRect.X + 4, notebookRect.Y + 4,
                notebookRect.Width, notebookRect.Height), CORNER_R);

            // Cover background
            using var coverBr = new SolidBrush(C_Cover);
            g.FillRoundedRect(coverBr, notebookRect, CORNER_R);

            // Border
            using var borderPen = new Pen(C_Border, 2f);
            g.DrawRoundedRect(borderPen, notebookRect, CORNER_R);

            // Decorative side tabs
            int tabH   = 34;
            int startY = TAB_H + 18;
            int x      = Width - 12;
            for (int i = 0; i < SideTabColors.Length; i++)
            {
                int y = startY + i * (tabH + 5);
                using var tabBr = new SolidBrush(SideTabColors[i]);
                g.FillRoundedRect(tabBr, new Rectangle(x - SIDE_TAB_W, y, SIDE_TAB_W + 4, tabH), 4);
            }
        }

        private void Body_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int w  = _body.ClientSize.Width;
            int h  = _body.ClientSize.Height;
            int lw = (w - SPINE_W) / 2;

            // Left page
            using var leftBr = new SolidBrush(C_PageLeft);
            g.FillRectangle(leftBr, 0, 0, lw, h);

            // Right page
            using var rightBr = new SolidBrush(C_PageRight);
            g.FillRectangle(rightBr, lw + SPINE_W, 0, w - lw - SPINE_W, h);

            // Spine gradient
            using var spineGrad = new LinearGradientBrush(
                new Point(lw, 0), new Point(lw + SPINE_W, 0),
                Color.FromArgb(210, C_Spine), Color.FromArgb(60, C_Spine));
            g.FillRectangle(spineGrad, lw, 0, SPINE_W, h);

            // Binder rings
            PaintRings(g, lw, h);

            // Faint ruled lines
            using var linePen = new Pen(C_LineRule, 1f);
            for (int y = 38; y < h; y += 22)
            {
                g.DrawLine(linePen, 10, y, lw - 8, y);
                g.DrawLine(linePen, lw + SPINE_W + 8, y, w - 18, y);
            }
        }

        private void PaintRings(Graphics g, int spineX, int h)
        {
            int cx   = spineX + SPINE_W / 2;
            int step = h / (RING_COUNT + 1);

            for (int i = 1; i <= RING_COUNT; i++)
            {
                int cy = i * step;
                int rw = 22, rh = 12;

                // Shadow
                using var shadowBr = new SolidBrush(Color.FromArgb(55, 0, 0, 0));
                g.FillEllipse(shadowBr, cx - rw / 2 + 1, cy - rh / 2 + 1, rw, rh);

                // Ring gradient
                using var grad = new LinearGradientBrush(
                    new Point(cx - rw / 2, cy - rh / 2),
                    new Point(cx + rw / 2, cy + rh / 2),
                    Color.FromArgb(230, C_Ring), Color.FromArgb(170, Color.White));
                g.FillEllipse(grad, cx - rw / 2, cy - rh / 2, rw, rh);

                using var pen = new Pen(Color.FromArgb(150, C_Ring), 1.2f);
                g.DrawEllipse(pen, cx - rw / 2, cy - rh / 2, rw, rh);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        //  HELPERS
        // ══════════════════════════════════════════════════════════════════
        private static Button CreateTabBtn(string text)
        {
            var b = new Button
            {
                Text = text, AutoSize = true, Height = TAB_H - 4,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Georgia", 9.5f),
                ForeColor = C_TabText, BackColor = C_TabInactive,
                Cursor = Cursors.Hand, Padding = new Padding(12, 0, 12, 0)
            };
            b.FlatAppearance.BorderColor = C_Border;
            b.FlatAppearance.BorderSize  = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 202, 230);
            return b;
        }

        private static void StyleTabBtn(Button btn, bool active)
        {
            btn.BackColor = active ? C_TabActive : C_TabInactive;
            btn.Font      = new Font("Georgia", 9.5f, active ? FontStyle.Bold : FontStyle.Regular);
            btn.FlatAppearance.BorderSize = active ? 1 : 0;
        }
    }
}
