using System;
using System.Collections.Generic;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using WMPLib;

namespace Echoes_of_Montmauve.GameLogic
{
    internal static class UIHelper
    {
        private static WindowsMediaPlayer _bgMusic = new WindowsMediaPlayer();
        private static System.Windows.Forms.Timer _timer;
        private static string _targetText;
        private static int _index;
        private static Label _outputLabel;

        private static SoundPlayer _clickPlayer;

        static UIHelper()
        {
            try
            {
                _clickPlayer = new SoundPlayer(Properties.Resources.Click);
                _clickPlayer.LoadAsync(); 
            }
            catch { }
        }

        public static void PlayBackgroundMusic(string fileName)
        {
            try { 
         

                string path = Path.Combine(Application.StartupPath, "Assets", fileName);

                if (File.Exists(path))
                {
                    _bgMusic.URL = path;
                    _bgMusic.settings.setMode("loop", true);
                    _bgMusic.settings.volume = 30; 
                    _bgMusic.controls.play();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Music Error: " + ex.Message);
            }
        }

        

        public static void StopMusic()
        {
            _bgMusic.controls.stop();
        }
        private const int Growth = 6;

        public static void PlayClickSound()
        {
            _clickPlayer?.Play();
        }

        public static void AddButtonScaleEffect(Button btn)
        {
            btn.MouseDown += (s, e) =>
            {
                PlayClickSound();
                btn.Size = new Size(btn.Width + Growth, btn.Height + Growth);
                btn.Location = new Point(btn.Location.X - (Growth / 2), btn.Location.Y - (Growth / 2));
            };

            btn.MouseUp += (s, e) =>
            {
                btn.Size = new Size(btn.Width - Growth, btn.Height - Growth);
                btn.Location = new Point(btn.Location.X + (Growth / 2), btn.Location.Y + (Growth / 2));
            };
        }

        public static void StartTypewriter(Label lbl, string text, int speed = 20)
        {
            _outputLabel = lbl;
            _targetText = text;
            _index = 0;
            _outputLabel.Text = "";

            if (_timer != null) _timer.Stop();

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = speed;
            _timer.Tick += (s, e) =>
            {
                if (_index < _targetText.Length)
                {
                    _outputLabel.Text += _targetText[_index];
                    _index++;
                }
                else
                {
                    _timer.Stop();
                }
            };
            _timer.Start();
        }

       
        public static async Task PlayDejavuEffect(Form targetform)
        {
            Point originalLocation = targetform.Location; 
            Random rnd = new Random();

            for(int i = 0; i < 15; i++)
            { 
                targetform.Location = new Point(
                originalLocation.X + rnd.Next(-10, 11),
                originalLocation.Y + rnd.Next(-10, 11));

                await Task.Delay(50);
            }
            targetform.Location = originalLocation;
        }

    }
}
