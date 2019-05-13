using System;
using System.Drawing;
using System.Text;

namespace terraria
{
    public class World
    {
        public int MapWidth => map.GetLength(0);
        public int MapHeight => map.GetLength(1);
        public readonly Player player;
        public ICharacter[,] map;

        public bool IsInBounds(int x, int y) => x < MapWidth && x >= 0 && y < MapHeight && y >= 0;
        public bool IsInBounds(Point point) => IsInBounds(point.X, point.Y);

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var y = 0; y < MapHeight; y++)
            {
                for (var x = 0; x < MapWidth; x++)
                {
                    builder.Append(map[x, y]);
                }
                builder.Append("\n");
            }
            return builder.ToString();
        }

        private World(string stringMap)
        {
            var separator = "\n";
            var lines = stringMap.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            var height = lines.Length;
            var width = 0;
            try
            {
                width = lines[0].Length;
            }
            catch
            {

            }
            map = new ICharacter[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    switch (lines[y][x])
                    {
                        case 'P':
                            map[x, y] = new Player();
                            break;
                        case 'G':
                            map[x, y] = new Grass();
                            break;
                        case 'W':
                            map[x, y] = new Wood();
                            break;
                        case 'R':
                            map[x, y] = new Rock();
                            break;
                        case ' ':
                            map[x, y] = new Air();
                            break;
                        default:
                            throw new FormatException($"{lines[y][x]} is unknown cell type");
                    }
                }
            }
        }

        public static World Create(string stringMap)
        {
            return new World(stringMap);
        }
    }
}