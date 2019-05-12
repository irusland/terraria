using System;
namespace terraria
{
    public class Wish
    {
        public int XOffset;
        public int YOffset;
        public ICharacter TransformTo;

        public override string ToString()
        {
            return $"{XOffset}, {YOffset}";
        }
    }
}
