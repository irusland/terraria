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
PGWGGGGRWRGWGWR
RRRGGGWGWGWRWWW
GGGGGGGGWGWRGGR
GGGRRGWRRRGGGGW
GGGGGGRRRRRGGGG
GGGRRGWGRGGGWGG
GGGGGGGRGGRRRRR
RRRGGWGGGGGGRGG
GGGGGGWGGGRGGGW
GGGGGGGWGRRGGGG
RRRGGGWGGGRRGGG";

            var stringInfo = @"
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
