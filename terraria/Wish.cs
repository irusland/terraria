using System;
using System.Drawing;

namespace terraria
{
    public class Wish
    {
        public int XOffset;
        public int YOffset;
        public ICharacter TransformTo;
        public Point BreakBlockOnPossition = new Point(-1, -1);

        public override string ToString()
        {
            return $"{XOffset}, {YOffset}";
        }
    }
}
