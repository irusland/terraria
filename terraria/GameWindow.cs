using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace terraria
{
    public class GameWindow : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly Brain gameBrain;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private int tickCount;
        private readonly int animationPrecision = 8;
        private Game game;


        public GameWindow(Game game)
        {
            this.game = game;
            gameBrain = new Brain();
            ClientSize = new Size(
                Brain.CellSize * game.MapWidth,
                Brain.CellSize * game.MapHeight + Brain.CellSize);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            var imagesDirectory = new DirectoryInfo("Images");
            var files = imagesDirectory.GetFiles();
            foreach (var e in files)
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            var timer = new Timer();
            timer.Interval = 5;
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
            //e.Graphics.TranslateTransform(0, Brain.CellSize);
            e.Graphics.FillRectangle(
            Brushes.Green, 0, 0, Brain.CellSize * game.MapWidth,
            Brain.CellSize * game.MapHeight);
            foreach (var animation in gameBrain.Animations)
            {
                e.Graphics.DrawImage(bitmaps[animation.Character.GetImageFileName()], animation.Location);
            }
            e.Graphics.ResetTransform();
            //e.Graphics.DrawString(game.world.ToString(), new Font("Arial", 16), Brushes.Green, 0, 0);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var mousePosition = e.Location;
            var playerPosition = GetPlayerPosition();
            var mouseOffsetFromPlayer = GetMouseOffsetFromPlayer(playerPosition, mousePosition);
            var player = (Player)game.world.map[playerPosition.X, playerPosition.Y];
            var x = mouseOffsetFromPlayer.X;
            var y = mouseOffsetFromPlayer.Y;
            if (y - x <= 0 && y + x >= 0)
                player.SetDirection(Direction.Right);
            if (y - x > 0 && y + x < 0)
                player.SetDirection(Direction.Left);
            if (y - x <= 0 && y + x < 0)
                player.SetDirection(Direction.Up);
            if (y - x > 0 && y + x >= 0)
                player.SetDirection(Direction.Down);
        }

        private Point GetMouseOffsetFromPlayer(Point player, Point mouse) =>
            new Point(mouse.X - player.X * Brain.CellSize - Brain.CellSize / 2,
            mouse.Y - player.Y * Brain.CellSize - Brain.CellSize / 2);

        private Point GetPlayerPosition()
        {
            for (var y = 0; y < game.MapHeight; y++)
            {
                for (var x = 0; x < game.MapWidth; x++)
                {
                    if (game.world.map[x, y] is Player)
                        return new Point(x, y);
                }
            }
            return new Point(-1, -1);
        }

        private void TimerTick(object sender, EventArgs args)
        {
            if (tickCount == 0)
            {
                gameBrain.CollectWishes(game);
                //Console.WriteLine($"Updated");
                //Console.WriteLine($"{game.world}\t{game.MapWidth}x{game.MapHeight}");
            }
            foreach (var animation in gameBrain.Animations)
                animation.Location = new Point(animation.Location.X + 4 * animation.Wish.XOffset,
                    animation.Location.Y + 4 * animation.Wish.YOffset);
            tickCount++;
            if (tickCount == animationPrecision)
            {
                gameBrain.ApplyWishes(game);
                tickCount = 0;
            }
            Invalidate();
        }
    }
}