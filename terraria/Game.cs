using System.Drawing;
using OpenTK;
using OpenTK.Input;

namespace terraria
{
    public class Game
    {
        public readonly World world;
        public Key KeyPressed { get; set; }
        public Point MousePosition { get; set; }
        public MouseButton? MouseClicked { get; set; }
        public int MapWidth => world.map.GetLength(0);
        public int MapHeight => world.map.GetLength(1);
        public int mouseScrollCount;

        public Game(World world)
        {
            this.world = world;
        }
    }
}
