using System;
using System.IO;
using System.Windows.Forms;

namespace terraria
{
    class MainForm
    {
        public static void Main()
        {
            var stringMap = @"
P WGGGGRWRGWGWR
R RGGGWGWGWRWWW
G GGGGGGWGWRGGR
G GRRGWRRRGGGGW
G GGGGRRRRRGGGG
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
            Application.Run(new GameWindow(game));
        }
    }
}
