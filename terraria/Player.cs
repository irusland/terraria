using System.Drawing;

namespace terraria
{
    public class Player
    {
        public readonly Inventory inventory;
        public readonly Point position;

        public Player(Point position)
        {
            this.position = position;
        }
    }
}