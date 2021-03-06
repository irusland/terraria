﻿using OpenTK;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace terraria
{
    class MainForm
    {
        private static void Main()
        {
            var stringMap = @"
P WGGGGRWRGWGWR
R RGGGWGWGWRWWW
G GGGGGGWGWRGGR
G GRRGWRRRGGGGW
GGGGGGRRRRRGGGG
GZGRRGWGRGGGWGG
GGGGGGGRGGRRRRR
RRRGGWGGGGGGRGG
GGGGGGWGGGRGGGW
GGGGGGGWGRRGGGG
RRRGGGWGGGRRGGG";

            var stringInfo = @"
Sword 1
Axe 1
Shovel 1
Pick 1
Wood 100";
            var world = World.CreateWithInfo(stringMap, stringInfo);
            var game = new Game(world);
            GameWindow window = new GameWindow(game);
            window.VSync = OpenTK.VSyncMode.Adaptive;
            window.Run();
        }
    }
}