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
P W  WW
RRRGGGW
GGGGGGW
GGGRRGW
GGGGGGW
GGGRRGW
GGGGGGW
GGGGGGW
GGGGGGW
GGGGGGW
RRRGGGW";

            var stringInfo = @"
Axe 1
Shovel 3
Pick 2";
            var world = World.CreateWithInfo(stringMap, stringInfo);
            var game = new Game(world);

            //todo
            var p = (Player)world.map[0, 0];
            p.Inventory.selected = 0;
            var inv = p.Inventory.GetSelectedSlot;
            Console.WriteLine(inv);
            Application.Run(new GameWindow(game));
        }
    }
}
