using System;
using System.Collections.Generic;
using System.Drawing;

namespace terraria
{
    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }
    public class Translator
    {
        public Point DirectionToOffset(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Point(0, -1);
                case Direction.Down:
                    return new Point(0, 1);
                case Direction.Right:
                    return new Point(1, 0);
                case Direction.Left:
                    return new Point(-1, 0);
                default:
                    throw new NotSupportedException($"{direction} is incorrect");
            }
        }
    }
}
