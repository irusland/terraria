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
            var map = World.Create(stringMap);
            var game = new Game(map);
            Application.Run(new GameWindow(game));
        }
    }
}
