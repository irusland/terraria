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
P W
GGG
RRR";
            var world = World.Create(stringMap);
            var game = new Game(world);
            Application.Run(new GameWindow(game));
        }
    }
}
