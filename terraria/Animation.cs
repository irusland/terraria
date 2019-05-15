using System.Drawing;

namespace terraria
{
    public class Animation
    {
        public Wish Wish;
        public ICharacter Character;
        public Point Location;
        public Point Target;

        public override string ToString()
        {
            return Character.ToString();
        }
    }
}
