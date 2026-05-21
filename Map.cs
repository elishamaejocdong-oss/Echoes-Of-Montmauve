using Echoes_of_Montmauve.GameLogic;
using Echoes_of_Montmauve.MarrowMarket;
using Echoes_of_Montmauve.VeloryxSpire;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;
namespace Echoes_of_Montmauve
{
    public partial class Map : Form
    {
        private System.Windows.Forms.Timer phase2CountDownTimer;

        public Map()
        {
            InitializeComponent();
            RefreshMapStatus();
            InitializePhaseControls();


            UIHelper.AddButtonScaleEffect(MarrowMarketBtn);
            UIHelper.AddButtonScaleEffect(CinderpipesBtn);
            UIHelper.AddButtonScaleEffect(EidraxisHallBtn);
            UIHelper.AddButtonScaleEffect(VeloryxBtn);
            UIHelper.AddButtonScaleEffect(LunavireBtn);
            UIHelper.AddButtonScaleEffect(MainMenuBtn);
        }

        private void Map_Load(object sender, EventArgs e)
        {
            RefreshMapStatus();

            if (SessionContent.CurrentPhase == SessionContent.GamePhase.Phase2_Madman)
            {
                StartPhase2TimerSystem();
            }
        }

        private void InitializePhaseControls()
        {
            bool phase2Active = SessionContent.CurrentPhase == SessionContent.GamePhase.Phase2_Madman;
            btnFinalConfrontation.Visible = false;
            lblUrgencyTimer.Visible = phase2Active;
            lblUrgencyTimer.Text = "TIME REMAINING: " + SessionContent.GetFormattedPhase2Time();
        }

        private void MainMenuBtn_Click(object sender, EventArgs e)
        {
            MainMenu menu = new MainMenu();
            menu.Show();
            this.Close();
        }

        private void LunavireBtn_Click(object sender, EventArgs e)
        {
            string user = SessionContent.CurrentActivePlayer.Username;
            if (DatabaseManager.IsDistrictUnlocked(user, "Lunavaire Groove"))
            {
                LunavaireGrooveForm level = new LunavaireGrooveForm();
                level.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("This district is shrouded in miasma. Purify Marrow Market first!");
            }
        }

        private void EidraxisHallBtn_Click(object sender, EventArgs e)
        {
            string user = SessionContent.CurrentActivePlayer.Username;
            if (DatabaseManager.IsDistrictUnlocked(user, "Eidraxis Hall"))
            {
                EidraxisMain level = new EidraxisMain();
                level.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("This district is shrouded in miasma. Purify Marrow Market first!");
            }
        }

        private void RefreshMapStatus()
        {
            if (SessionContent.CurrentActivePlayer == null) return;

            if (SessionContent.CurrentPhase == SessionContent.GamePhase.Phase2_Madman)
            {
                lblMapStatus.Text = "CRITICAL URGENCY: THE MIASMA HAS RETURNED. PURIFY THE DISTRICTS BEFORE TIME EXPIRES!";
                lblMapStatus.ForeColor = Color.Red;
            }

            MarrowMarketBtn.Enabled = SessionContent.IsDistrictUnlocked("Marrow Market");
            MarrowMarketBtn.Image = MarrowMarketBtn.Enabled ? Properties.Resources.MarketIcon : Properties.Resources.LockedIcon;

            bool isVeloryxUnlocked = SessionContent.IsDistrictUnlocked("Veloryx Spire");
            VeloryxBtn.Enabled = isVeloryxUnlocked;
            VeloryxBtn.Image = isVeloryxUnlocked ? Properties.Resources.SpireIcon : Properties.Resources.LockedIcon;

            bool isLunavireUnlocked = SessionContent.IsDistrictUnlocked("Lunavaire Groove");
            LunavireBtn.Enabled = isLunavireUnlocked;
            LunavireBtn.Image = isLunavireUnlocked ? Properties.Resources.LunavireGroove : Properties.Resources.LockedIcon;

            bool isEidraxisUnlocked = SessionContent.IsDistrictUnlocked("Eidraxis Hall");
            EidraxisHallBtn.Enabled = isEidraxisUnlocked;
            EidraxisHallBtn.Image = isEidraxisUnlocked ? Properties.Resources.HallIcon : Properties.Resources.LockedIcon;

            bool isCinderUnlocked = SessionContent.IsDistrictUnlocked("Cinder Pipes");
            CinderpipesBtn.Enabled = isCinderUnlocked;
            CinderpipesBtn.Image = isCinderUnlocked ? Properties.Resources.PipesIcon : Properties.Resources.LockedIcon;


        }

        private void StartPhase2TimerSystem()
        {
            if (phase2CountDownTimer != null) return;

            SessionContent.Phase2TimerActive = true;
            phase2CountDownTimer = new System.Windows.Forms.Timer();
            phase2CountDownTimer.Interval = 1000;
            phase2CountDownTimer.Tick += (s, ev) =>
            {
                if (SessionContent.CurrentPhase != SessionContent.GamePhase.Phase2_Madman)
                {
                    phase2CountDownTimer.Stop();
                    return;
                }

                if (SessionContent.Phase2SecondsRemaining > 0)
                {
                    SessionContent.Phase2SecondsRemaining--;
                    lblUrgencyTimer.Text = "TIME REMAINING: " + SessionContent.GetFormattedPhase2Time();
                }
                else
                {
                    phase2CountDownTimer.Stop();
                    SessionContent.Phase2TimerActive = false;
                    HandleFailureTimeout();
                }
            };
            phase2CountDownTimer.Start();
        }

        private void HandleFailureTimeout()
        {
            MessageBox.Show("TIME EXPIRED: The Miasma completely overcomes the municipal partitions...", "Miasma Critical Overrun");
            FinalAccusationForm accusation = new FinalAccusationForm(isTimeout: true);
            accusation.Show();
            this.Close();
        }

        private void MarrowMarketBtn_Click(object sender, EventArgs e)
        {
            MarrowMarketMain marrowMarket = new MarrowMarketMain();
            marrowMarket.Show();
            this.Close();
        }

        private void CinderpipesBtn_Click(object sender, EventArgs e)
        {
            CinderPipesMain cinderPipes = new CinderPipesMain();
            cinderPipes.Show();
            this.Close();
        }

        private void VeloryxBtn_Click(object sender, EventArgs e)
        {
            string user = SessionContent.CurrentActivePlayer.Username;
            if (DatabaseManager.IsDistrictUnlocked(user, "Veloryx Spire"))
            {
                VeloryxMain level = new VeloryxMain();
                level.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("This district is shrouded in miasma. Purify Marrow Market first!");
            }
        }

        private void btnFinalConfrontation_Click(object sender, EventArgs e)
        {
            phase2CountDownTimer?.Stop();
            SessionContent.AdvanceToPhase3();
            FinalAccusationForm finalAccForm = new FinalAccusationForm(isTimeout: false);
            finalAccForm.Show();
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            phase2CountDownTimer?.Stop();
            phase2CountDownTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
