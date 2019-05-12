using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace terraria
{
    public class Game
    {
        public readonly World world;
        public Keys KeyPressed { get; set; }
        public int MapWidth => world.map.GetLength(0);
        public int MapHeight => world.map.GetLength(1);

        public Game(World world)
        {
            this.world = world;
        }
    }
}
