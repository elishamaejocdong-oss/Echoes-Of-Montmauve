using System;
using System.Collections.Generic;
using System.Text;

namespace Echoes_of_Montmauve
{
    public static class sprite
    {
        public static Image walkleft {  get; private set; }
        public static Image walkright { get; private set; }
        public static Image walkup { get; private set; }
        public static Image walkdown { get; private set; }
        public static Image frontidle { get; private set; }
        public static Image backidle { get; private set; }

        private static bool loaded = false;

        public static void Load()
        {
            if (loaded) return;

            string movePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "movement");
            walkleft = Image.FromFile(Path.Combine(movePath, "walkleft.gif"));
            walkright = Image.FromFile(Path.Combine(movePath, "walkright.gif"));
            walkup = Image.FromFile(Path.Combine(movePath, "walkup.gif"));
            walkdown = Image.FromFile(Path.Combine(movePath, "walkdown.gif"));
            frontidle = Image.FromFile(Path.Combine(movePath, "frontidle.png"));
            backidle = Image.FromFile(Path.Combine(movePath, "backidle.png"));
            loaded = true;
        }

        public static void register(EventHandler onFrame)
        {
            ImageAnimator.Animate(walkleft, onFrame);
            ImageAnimator.Animate(walkright, onFrame);
            ImageAnimator.Animate(walkup, onFrame);
            ImageAnimator.Animate(walkdown, onFrame);
            ImageAnimator.Animate(frontidle, onFrame);
            ImageAnimator.Animate(backidle, onFrame);
        }

        public static void unregister(EventHandler onFrame)
        {
            ImageAnimator.StopAnimate(walkleft, onFrame);
            ImageAnimator.StopAnimate(walkright, onFrame);
            ImageAnimator.StopAnimate(walkup, onFrame);
            ImageAnimator.StopAnimate(walkdown, onFrame);
            ImageAnimator.StopAnimate(frontidle, onFrame);
            ImageAnimator.StopAnimate(backidle, onFrame);
        }
    }
}
