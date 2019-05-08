using System;
using System.Windows.Forms;

namespace terraria
{
    class MainForm
    {
        public static void Main()
        {
            var stringMap = new[]{
                "P  ",
                "GGG",
                "RRR",
            };
            var world = World.Create(stringMap);
            var game = new Game(world);
            Application.Run(new GameWindow(game));
        }
    }
}
