using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Timers;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace terraria
{
    public class GameWindow : OpenTK.GameWindow
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
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
            Load += OnLoad;
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            MouseMove += OnMouseMove;
            MouseWheel += OnMouseWheel;

            Resize += ResizeWindow;
            // RenderFrame += OnRenderFrame;
            RenderFrame += OnPaint;
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
                textures[e.Name] = new Texture(LoadTexture(e.Name), Brain.CellSize, Brain.CellSize);

            }

            var timer = new Timer
            {
                Interval = 5
            };

            timer.Elapsed += TimerTick;
            timer.Start();
        }

        private void ResizeWindow(object sender, EventArgs e)
        {
            GL.Viewport(this.ClientRectangle);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            // TODO fix mouse position relative to player's  
            GL.Ortho(0.0, this.Size.Width * 2, Size.Height * 2, 0.0, -1.0, 1.0);
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            GL.ClearColor(.5f, 0.0f, 0.0f, 0.0f);
            this.Title = "Terraria";
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
            Console.WriteLine($"{e.Key} pressed");
        }

        // protected override void OnMouseClick(MouseEventArgs e)
        // {
        //     mouseClicks.Add(e.Button);
        //     game.MouseClicked = e.Button;
        //     Console.WriteLine($"{game.MouseClicked} sent");
        // }
        
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
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.CornflowerBlue);
            // e.Graphics.TranslateTransform(0, Brain.CellSize);
            GL.Begin(BeginMode.Quads);
            GL.Vertex2(0, 0);
            GL.Vertex2(Brain.CellSize * game.MapWidth, 0);
            GL.Vertex2(Brain.CellSize * game.MapWidth, Brain.CellSize * game.MapHeight);
            GL.Vertex2(0, Brain.CellSize * game.MapHeight);
            GL.End();
            
            for (var i = 0; i < gameBrain.Animations.Count; i++)
            {
                var animation = gameBrain.Animations[i];
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
            SwapBuffers();
        }

        private int LoadTexture(string name)
        {
            var id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            var bmp = bitmaps[name];
            var data = bmp.LockBits(new Rectangle(new Point(0, 0), new Size(Brain.CellSize, Brain.CellSize)), 
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
                data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                (int) TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                (int) TextureWrapMode.Clamp);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Linear);

            return id;
        }
        
        private void DrawImage(string name, Point location)
        {
            var texture = textures[name];
            GL.BindTexture(TextureTarget.Texture2D, texture.ID);
            GL.Begin(BeginMode.Quads);
            
            GL.TexCoord2(0, 0);
            GL.Vertex2(location.X, location.Y);
            
            GL.TexCoord2(1, 0);
            GL.Vertex2(location.X + texture.Width, location.Y);
            
            GL.TexCoord2(1, 1);
            GL.Vertex2(location.X + texture.Width, location.Y + texture.Height);

            GL.TexCoord2(0, 1);
            GL.Vertex2(location.X, location.Y + texture.Height);
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
                tickCount = 0;
                gameBrain.ApplyAnimations(game);
            }
            // Invalidate();
        }
    }
}