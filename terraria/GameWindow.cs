using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace terraria
{
    public class GameWindow : Form
    {
        //private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly Brain gameBrain;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private int tickCount;
        private Game game;


        public GameWindow(Game game) //, DirectoryInfo imagesDirectory = null
        {
            this.game = game;
            gameBrain = new Brain();
            //ClientSize = new Size(
            //    GameState.ElementSize * game.MapWidth,
            //    GameState.ElementSize * game.MapHeight + GameState.ElementSize);
            //FormBorderStyle = FormBorderStyle.FixedDialog;
            //if (imagesDirectory == null)
            //    imagesDirectory = new DirectoryInfo("Images");
            //foreach (var e in imagesDirectory.GetFiles("*.png"))
            //bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            var timer = new Timer();
            timer.Interval = 150;
            timer.Tick += TimerTick;
            timer.Start();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "Terraria";
            DoubleBuffered = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);
            game.KeyPressed = e.KeyCode;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
            game.KeyPressed = pressedKeys.Any() ? pressedKeys.Min() : Keys.None;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(0, Brain.CellSize);
            e.Graphics.FillRectangle(
                Brushes.Black, 0, 0, Brain.CellSize * game.MapWidth,
                Brain.CellSize * game.MapHeight);
            foreach (var a in gameBrain.Animations)
            {
                // TODO Draw bitmap
                //e.Graphics.DrawImage(bitmaps[a.Creature.GetImageFileName()], a.Location);
            }
            e.Graphics.ResetTransform();
            //e.Graphics.DrawString(game.Scores.ToString(), new Font("Arial", 16), Brushes.Green, 0, 0);
        }

        private void TimerTick(object sender, EventArgs args)
        {
            if (tickCount == 0)
            {
                gameBrain.CollectWishes(game);
                Console.WriteLine($"Updated {tickCount}");
            }
            foreach (var animation in gameBrain.Animations)
                animation.Location = new Point(animation.Location.X + 4 * animation.Wish.XOffset,
                    animation.Location.Y + 4 * animation.Wish.YOffset);
            if (tickCount == 7)
                gameBrain.ApplyWishes(game);
            tickCount++;
            if (tickCount == 8) tickCount = 0;
            Invalidate();
        }
    }
}