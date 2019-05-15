using System;
using System.Drawing;

namespace terraria
{
    public enum Direction
    {
        None,
        Down,
        Right,
        Left,
        Up
    }
    public static class Translator
    {
        private static Point DirectionToOffset(Direction direction)
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
