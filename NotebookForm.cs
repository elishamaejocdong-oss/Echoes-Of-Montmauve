using Echoes_of_Montmauve.GameLogic;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Echoes_of_Montmauve
{
    public partial class NotebookForm : Form
    {
        // Define your color palette here for easy changes
        private readonly Color PageColor = Color.PapayaWhip;
        private readonly Color TextColor = Color.SaddleBrown;
        private readonly Color SelectionColor = Color.SaddleBrown;

        public NotebookForm()
        {
            InitializeComponent();
            ApplyHardcodedStyles(); // This forces the UI to change
        }

        private void ApplyHardcodedStyles()
        {
            // 1. Form Background
            this.BackColor = Color.Wheat;

            // 2. The Tabs - Make them all match the notebook page
            tabSDG.BackColor = PageColor;
            tabGame.BackColor = PageColor;
            tabClues.BackColor = PageColor;

            // 3. The TextBox (lblDescriptionView)
            lblDescriptionView.BackColor = PageColor;
            lblDescriptionView.ForeColor = TextColor;
            lblDescriptionView.BorderStyle = BorderStyle.None; // Removes the "sunken" look
            lblDescriptionView.Font = new Font("Calisto MT", 12, FontStyle.Regular);

            // 4. ListBoxes - Borderless and matching colors
            ListBox[] allLists = { lbSDGNotes, lbGameNotes, lbClues };
            foreach (var lb in allLists)
            {
                lb.BackColor = PageColor;
                lb.ForeColor = TextColor;
                lb.BorderStyle = BorderStyle.None;
                lb.Font = new Font("Calisto MT", 12, FontStyle.Bold);

                // This allows us to change the selection color from blue to brown
                lb.DrawMode = DrawMode.OwnerDrawFixed;
                lb.DrawItem += ListBox_DrawItem;
            }
        }

        // This method draws the items in the ListBox so we can use custom colors
        private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            ListBox lb = (ListBox)sender;

            e.DrawBackground();

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                // Draw brown background for selected item
                e.Graphics.FillRectangle(new SolidBrush(SelectionColor), e.Bounds);
                e.Graphics.DrawString(lb.GetItemText(lb.Items[e.Index]), e.Font, Brushes.White, e.Bounds);
            }
            else
            {
                // Draw normal background
                e.Graphics.FillRectangle(new SolidBrush(PageColor), e.Bounds);
                e.Graphics.DrawString(lb.GetItemText(lb.Items[e.Index]), e.Font, new SolidBrush(TextColor), e.Bounds);
            }

            e.DrawFocusRectangle();
        }

        private void NotebookForm_Load(object sender, EventArgs e)
        {
            PopulateNotebookTab("SDG", lbSDGNotes);
        }


        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;

            // Note: Since we are using a DataTable as DataSource, 
            // the item is a DataRowView.
            if (lb.SelectedItem is DataRowView rowView)
            {
                lblDescriptionView.Text = Convert.ToString(rowView["Description"]);

                string imgFile = Convert.ToString(rowView["ImagePath"]);
                string totalPath = ResolveNotebookImagePath(imgFile);

                if (!string.IsNullOrEmpty(totalPath))
                {
                    if (pbClueIllustration.Image != null) pbClueIllustration.Image.Dispose();
                    pbClueIllustration.Image = LoadImageWithoutLocking(totalPath);
                    pbClueIllustration.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pbClueIllustration.Image = null;
                }
            }
        }

        private void tabNotebookContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabNotebookContainer.SelectedTab == tabSDG)
                PopulateNotebookTab("SDG", lbSDGNotes);
            else if (tabNotebookContainer.SelectedTab == tabGame)
                PopulateNotebookTab("Game", lbGameNotes);
            else if (tabNotebookContainer.SelectedTab == tabClues)
                PopulateNotebookTab("Clue", lbClues);
        }

        private void PopulateNotebookTab(string tabType, ListBox activeBox)
        {
            activeBox.DataSource = null;
            // Note: Do not use .Items.Clear() when using DataSource
            DataTable dt = DatabaseManager.GetUnlockedNotesByTable(SessionContent.CurrentActivePlayer.Username, tabType);
            activeBox.DataSource = dt;
            activeBox.DisplayMember = "ItemName";
        }

        private string ResolveNotebookImagePath(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return "";

            string fileName = imagePath.Trim();
            string[] roots =
            {
                Path.Combine(Application.StartupPath, "Clues"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Clues"),
                Path.Combine(Application.StartupPath, "..", "..", "..", "Clues")
            };

            string[] candidates = Path.HasExtension(fileName)
                ? new[] { fileName }
                : new[] { fileName, fileName + ".png", fileName + ".jpg", fileName + ".jpeg" };

            foreach (string root in roots)
            {
                foreach (string candidate in candidates)
                {
                    string fullPath = Path.GetFullPath(Path.Combine(root, candidate));
                    if (File.Exists(fullPath))
                        return fullPath;
                }
            }

            return "";
        }

        private Image LoadImageWithoutLocking(string imagePath)
        {
            using (FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            using (Image source = Image.FromStream(stream))
            {
                return new Bitmap(source);
            }
        }

        private void btnCloseNotebook_Click(object sender, EventArgs e)
        {
            MainMenu menu = new MainMenu();
            menu.Show();
            this.Close(); // Use .Close() instead of .Hide() to free memory
        }
    }
}

