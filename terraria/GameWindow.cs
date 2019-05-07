using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace terraria
{
    internal class GameWindow : Form
    {
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private Game game;


        public GameWindow(Game game)
        {
            this.game = game;
            var timer = new Timer();
            timer.Interval = 100;
            timer.Tick += TimerTick;
            timer.Start();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
        }

        private void TimerTick(object sender, EventArgs args)
        {
            game.Update();
            Invalidate();
        }
    }
}