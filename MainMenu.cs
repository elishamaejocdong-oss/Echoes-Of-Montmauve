using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Windows.Forms;
using Echoes_of_Montmauve.GameLogic;
using Echoes_of_Montmauve.SharedUI;

namespace Echoes_of_Montmauve
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
            UIHelper.PlayBackgroundMusic("MontmauveTheme.mp3");
            UIHelper.AddButtonScaleEffect(MontmauveMapBtn);
            UIHelper.AddButtonScaleEffect(btnAchievement);
            UIHelper.AddButtonScaleEffect(btnOracle);
            AddFeedbackButton();
        }

        private void AddFeedbackButton()
        {
            Button btnFeedback = new Button
            {
                Text = "Rate Game",
                BackColor = Color.PapayaWhip,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Calisto MT", 12f, FontStyle.Bold),
                ForeColor = Color.Maroon,
                Location = new Point(895, 482),
                Name = "btnFeedback",
                Size = new Size(321, 52),
                UseVisualStyleBackColor = false
            };
            btnFeedback.FlatAppearance.BorderColor = Color.SaddleBrown;
            btnFeedback.FlatAppearance.BorderSize = 2;
            btnFeedback.Click += btnFeedback_Click;
            Controls.Add(btnFeedback);
            UIHelper.AddButtonScaleEffect(btnFeedback);
        }

        private void MontmauveMapBtn_Click(object sender, EventArgs e)
        {
            Map mainmap = new Map();
            mainmap.Show();
            this.Hide();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
        }


        public void UpdateMaismaUI()
        {
            int totalDistrict = 5;
            int purifiedDistricts = DatabaseManager.GetPurifiedDistrictCount(SessionContent.CurrentActivePlayer.Username);

            int currentMaisma = 100 - (int)(((double)purifiedDistricts / totalDistrict) * 100);

            miasmaProgressBar.Value = currentMaisma;

            lblMaismaLevel.Text = $"Maisma Level: {currentMaisma}%";

        }

        private void MainMenu_Activated(object sender, EventArgs e)
        {
            UpdateMaismaUI();
        }

        private void btnOracle_Click(object sender, EventArgs e)
        {
            TaskDialog.ShowDialog(new TaskDialogPage()
            {
                Heading = "Oracle of the Miasma",
                Text = "The Oracle of the Miasma is a mystical entity that resides within the heart of the Miasma. It is said to possess ancient knowledge and insights about the world, the Miasma, and the secrets hidden within it. The Oracle can provide guidance, answer questions, and offer cryptic clues to those who seek its wisdom. However, interacting with the Oracle is not without risks, as it may demand a price for its knowledge or test the worthiness of those who approach it.",
                Buttons = { TaskDialogButton.OK }
            });
        }

        private void btnAchievement_Click(object sender, EventArgs e)
        {
            UrbanMetrics metrics = new UrbanMetrics();
            metrics.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string currentUser = SessionContent.CurrentActivePlayer.Username;

            DialogResult result = MessageBox.Show(
                "ARE YOU SURE? This will permanently erase your progress, districts, and artifacts.",
                "Confirm Self-Destruct",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                if (DatabaseManager.DeleteAccount(currentUser))
                {
                    MessageBox.Show("Account successfully purged. Returning to Login.");

                    SessionContent.CurrentActivePlayer = null;
                    this.Hide();
                    new SignIn().Show();
                }
            }
        }

        private void btnNotes_Click(object sender, EventArgs e)
        {
            NotebookForm notebook = new NotebookForm();
            notebook.Show();
            this.Hide();
        }

        private void btnFeedback_Click(object sender, EventArgs e)
        {
            PlayerFeedbackForm feedbackForm = new PlayerFeedbackForm();
            feedbackForm.Show();
            this.Hide();
        }
    }
}
