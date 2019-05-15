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
        private Point mousePosition = new Point(0, 0);
        private readonly HashSet<MouseButtons> mouseClicks = new HashSet<MouseButtons>();
        private int mouseScrollCount;
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
            {
                if (e.Name == ".DS_Store")
                    continue;
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            }

            var timer = new Timer
            {
                Interval = 5
            };

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

        //protected override void OnMouseClick(MouseEventArgs e)
        //{
        //    mouseClicks.Add(e.Button);
        //    game.MouseClicked = e.Button;
        //    Console.WriteLine($"{game.MouseClicked} sent");
        //}

        protected override void OnMouseDown(MouseEventArgs e)
        {
            mouseClicks.Add(e.Button);
            game.MouseClicked = e.Button;
            Console.WriteLine($"{game.MouseClicked} sent");
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            mouseClicks.Remove(e.Button);
            game.MouseClicked = mouseClicks.Any() ? mouseClicks.Min() : MouseButtons.None;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            mousePosition = e.Location;
            game.MousePosition = mousePosition;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            var numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 200;
            //var numberOfPixelsToMove = numberOfTextLinesToMove * fontSize;
            mouseScrollCount = numberOfTextLinesToMove;
            game.mouseScrollCount = mouseScrollCount;
        }

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
            var inventoryLocationYOffset = game.MapHeight * Brain.CellSize;
            var playerPos = GetPlayerPosition();
            var player = (Player)game.world.map[playerPos.X, playerPos.Y];
            for (var i = 0; i < Inventory.maxSize; i++)
            {
                var slot = player.Inventory.inventory[i];
                if (slot == null)
                    continue;
                e.Graphics.DrawImage(bitmaps[slot.Item.GetIconFileName()], new Point(i * Brain.CellSize, inventoryLocationYOffset));
                // TODO fix in console  "Fontconfig warning: ignoring UTF-8: not a valid region tag"
                e.Graphics.DrawString(slot.Amount.ToString(), new Font("Arial", 10), Brushes.Black, i * Brain.CellSize, inventoryLocationYOffset);
                if (i == player.Inventory.Selected)
                {
                    e.Graphics.DrawImage(bitmaps["border.png"], new Point(i * Brain.CellSize, inventoryLocationYOffset));
                }
            }
        }

        private void TimerTick(object sender, EventArgs args)
        {
            if (tickCount == 0)
            {
                gameBrain.CollectAnimations(game);
                //Console.WriteLine($"Updated");
                //Console.WriteLine($"{game.world}\t{game.MapWidth}x{game.MapHeight}");
            }
            foreach (var animation in gameBrain.Animations)
            {
                animation.Location = new Point(animation.Location.X + 4 * animation.Wish.XOffset,
                        animation.Location.Y + 4 * animation.Wish.YOffset);
            }

            tickCount++;

            if (tickCount == animationPrecision)
            {
                gameBrain.ApplyAnimations(game);
                tickCount = 0;
            }
            Invalidate();
        }
    }
}