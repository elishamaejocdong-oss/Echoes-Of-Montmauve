using System.Windows.Forms;

namespace Echoes_of_Montmauve.GameLogic
{
    public class TransparentLabel : Label
    {
        public TransparentLabel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint, true);
            BackColor = System.Drawing.Color.Transparent;
            UpdateStyles();
        }

        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
            if (Parent != null)
            {
                var point = Parent.PointToScreen(Location);
                var formPoint = FindForm().PointToClient(point);
                e.Graphics.TranslateTransform(-formPoint.X + 0, -formPoint.Y + 0);
                using (var pe = new System.Windows.Forms.PaintEventArgs(
                    e.Graphics, new System.Drawing.Rectangle(formPoint, Size)))
                {
                    InvokePaintBackground(Parent, pe);
                    InvokePaint(Parent, pe);
                }
                e.Graphics.ResetTransform();
            }
        }
    }
}