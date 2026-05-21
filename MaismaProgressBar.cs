using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace Echoes_of_Montmauve
{
    public class MiasmaProgressBar : ProgressBar
    {
        public MiasmaProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint, true);
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new int Value
        {
            get => base.Value;
            set
            {
                // Ensure value stays within 0 and Maximum
                int newValue = value;
                if (newValue < 0) newValue = 0;
                if (newValue > Maximum) newValue = Maximum;

                base.Value = newValue;
                this.Invalidate(); // Forces the OnPaint method to run immediately
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = ClientRectangle;

            // 1. Draw Background
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(30, 30, 30)))
            {
                g.FillRectangle(bgBrush, rect);
            }

            // 2. Calculate the width of the progress bar
            float scale = (float)base.Value / Maximum;
            int progressWidth = (int)(rect.Width * scale);

            if (progressWidth > 0)
            {
                Rectangle progressRect = new Rectangle(0, 0, progressWidth, rect.Height);

                // We can adjust color based on Miasma intensity
                Color startColor = Color.Indigo;
                Color endColor = Color.MediumOrchid;

                // If Miasma is high (Value is high), make it look more 'corrupted' (Darker Purple)
                if (base.Value > 80) startColor = Color.Black;

                using (LinearGradientBrush progressBrush = new LinearGradientBrush(
                    progressRect,
                    startColor,
                    endColor,
                    LinearGradientMode.Horizontal))
                {
                    g.FillRectangle(progressBrush, progressRect);
                }
            }

            // 3. Draw Border
            using (Pen borderPen = new Pen(Color.White, 1))
            {
                g.DrawRectangle(borderPen, 0, 0, rect.Width - 1, rect.Height - 1);
            }
        }
    }
}