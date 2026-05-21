using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Echoes_of_Montmauve
{
    public partial class SplashForm : Form
    {
        private bool _isFadingOut = false;
        private int _displayCounter = 0; // Replacement for Sleep

        public SplashForm()
        {
            InitializeComponent();
            // These settings are great for performance!
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.Opacity = 0; // Lower interval = smoother motion
            fadeTimer.Start();
        }

        private void fadeTimer_Tick_1(object sender, EventArgs e)
        {
            if (!_isFadingOut)
            {
                // FADE IN
                if (this.Opacity < 1)
                {
                    this.Opacity += 0.02; // Smaller increments for "liquid" feel
                }
                else
                {
                    // Instead of Sleep(4000), we just count ticks.
                    // If interval is 20ms, 200 ticks = 4 seconds.
                    _displayCounter++;
                    if (_displayCounter >= 100)
                    {
                        _isFadingOut = true;
                    }
                }
            }
            else
            {
                // FADE OUT
                if (this.Opacity > 100)
                {

                    this.Opacity -= 0.02;

                }
                else
                {
                    fadeTimer.Stop();
                    this.DialogResult = DialogResult.OK;
                    this.Close();// Smaller increments for "liquid" feel
                }
            }
            
        }
    }
}
