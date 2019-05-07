using System;
using System.Drawing;

namespace terraria
{
    public class World
    {
        public enum Block
        {
            Grass,
            Player,
            Tree,
            Rock,
            Air
        }

        public readonly int width;
        public readonly int height;
        public readonly Player player;

        public Block[,] map;

        public void PrintMap()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    Console.Write(map[x, y]);
                }
                Console.WriteLine();
            }
        }

        private World(int w, int h)
        {
            width = w;
            height = h;
            map = new Block[width, height];
        }

        private World(Block[,] blocks)
        {
            map = blocks;
        }

        private World(string[] stringMap)
        {
            height = stringMap.Length;
            width = 0;
            try
            {
                width = stringMap[0].Length;
            }
            catch
            {

            }
            map = new Block[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    switch (stringMap[y][x])
                    {
                        case 'P':
                            map[x, y] = Block.Player;
                            player = new Player(new Point(x, y));
                            break;
                        case 'G':
                            map[x, y] = Block.Grass;
                            break;
                        case 'T':
                            map[x, y] = Block.Tree;
                            break;
                        case 'R':
                            map[x, y] = Block.Rock;
                            break;
                        case ' ':
                            map[x, y] = Block.Air;
                            break;
                        default:
                            throw new FormatException($"{stringMap[y][x]} is unknown cell type");
                    }
                }
            }
        }

        public static World Create(string[] stringMap)
        {
            return new World(stringMap);
        }
    }
}