using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace terraria
{
    public class GameWindow : OpenTK.GameWindow
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly Brain gameBrain;
        private readonly HashSet<Key> pressedKeys = new HashSet<Key>();
        private Point mousePosition = new Point(0, 0);
        private readonly HashSet<MouseButton?> mouseClicks = new HashSet<MouseButton?>();
        private int mouseScrollCount;
        private int tickCount;
        private readonly int animationPrecision = 8;
        private Game game;


        public GameWindow(Game game)
        {
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            MouseMove += OnMouseMove;
            MouseWheel += OnMouseWheel;
            UpdateFrame += OnPaint; 
            
            GL.Enable(EnableCap.Texture2D);

            this.game = game;
            gameBrain = new Brain();
            ClientSize = new Size(
                Brain.CellSize * game.MapWidth,
                Brain.CellSize * game.MapHeight + Brain.CellSize);
            // FormBorderStyle = FormBorderStyle.FixedDialog;
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
            // Text = "Terraria";
            // DoubleBuffered = true;
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            pressedKeys.Add(e.Key);
            game.KeyPressed = e.Key;
        }


        private void OnKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            pressedKeys.Remove(e.Key);
            game.KeyPressed = pressedKeys.Any() ? pressedKeys.Min() : Key.Unknown;
        }

        //protected override void OnMouseClick(MouseEventArgs e)
        //{
        //    mouseClicks.Add(e.Button);
        //    game.MouseClicked = e.Button;
        //    Console.WriteLine($"{game.MouseClicked} sent");
        //}
        
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseClicks.Add(e.Button);
            game.MouseClicked = e.Button;
            Console.WriteLine($"{game.MouseClicked} sent");
        }
        
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseClicks.Remove(e.Button);
            game.MouseClicked = mouseClicks.Any() ? mouseClicks.Min() : null;
        }

        private void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            mousePosition = e.Position;
            game.MousePosition = mousePosition;
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var numberOfTextLinesToMove = e.Delta; //TODO  * SystemInformation.MouseWheelScrollLines / 200 AKA e.DeltaPrecise 
            // var numberOfPixelsToMove = numberOfTextLinesToMove * fontSize;
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

        private void OnPaint(object sender, FrameEventArgs e)
        {
            // e.Graphics.TranslateTransform(0, Brain.CellSize);
            GL.Begin(BeginMode.Quads);
            GL.Vertex2(0, 0);
            GL.Vertex2(Brain.CellSize * game.MapWidth, 0);
            GL.Vertex2(Brain.CellSize * game.MapWidth, Brain.CellSize * game.MapHeight);
            GL.Vertex2(0, Brain.CellSize * game.MapHeight);
            GL.Color3(0, 255, 0);
            GL.End();
            
            foreach (var animation in gameBrain.Animations)
            {
                DrawImage(animation.Character.GetImageFileName(), animation.Location);
            }
            // e.Graphics.ResetTransform();
            var inventoryLocationYOffset = game.MapHeight * Brain.CellSize;
            var playerPos = GetPlayerPosition();
            var player = (Player)game.world.map[playerPos.X, playerPos.Y];
            for (var i = 0; i < Inventory.maxSize; i++)
            {
                var slot = player.Inventory.inventory[i];
                if (slot == null)
                    continue;
                
                DrawImage(slot.Item.GetIconFileName(), new Point(i * Brain.CellSize, inventoryLocationYOffset));

                // TODO fix in console  "Fontconfig warning: ignoring UTF-8: not a valid region tag"
                // e.Graphics.DrawString(slot.Amount.ToString(), new Font("Arial", 10), Brushes.Black, i * Brain.CellSize, inventoryLocationYOffset);
                if (i == player.Inventory.Selected)
                {
                    DrawImage("border.png", new Point(i * Brain.CellSize, inventoryLocationYOffset));
                }
            }
        }

        private void DrawImage(string name, Point location)
        {
            // TODO Texture load 1 time another place 
            var id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            var bmp = bitmaps[name];
            var data = bmp.LockBits(new Rectangle(new Point(0, 0), bmp.Size), 
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
                data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
                
            GL.BindTexture(TextureTarget.Texture2D, id);
            GL.Begin(BeginMode.Quads);
            GL.Vertex2(location.X * Brain.CellSize, location.Y * Brain.CellSize);
            GL.Vertex2((location.X + 1) * Brain.CellSize, location.Y * Brain.CellSize);
            GL.Vertex2(location.X * Brain.CellSize, (location.Y + 1) * Brain.CellSize);
            GL.Vertex2((location.X + 1) * Brain.CellSize, (location.Y + 1) * Brain.CellSize);
            GL.End();
        }

        private void TimerTick(object sender, EventArgs args)
        {
            if (tickCount == 0)
            {
                gameBrain.CollectAnimations(game);
              
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
            // Invalidate();
        }
    }
}