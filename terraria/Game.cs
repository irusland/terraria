using System;
using System.Collections.Generic;

namespace terraria
{
    public class Game
    {
        public enum MapCell
        {
            Grass,
            Player,
            Tree,
            Rock,
            Air
        }

        public class Map
        {
            private readonly int width;
            private readonly int height;
            private readonly Tuple<int, int> intialPlayerPossition = Tuple.Create(-1, -1);

            public MapCell[,] map;

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

            public Map(int w, int h)
            {
                width = w;
                height = h;
                map = new MapCell[width, height];
            }

            public Map(MapCell[,] cells)
            {
                map = cells;
            }

            public Map(string[] stringMap)
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
                map = new MapCell[width, height];

                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        switch (stringMap[y][x])
                        {
                            case 'P':
                                map[x, y] = MapCell.Player;
                                intialPlayerPossition = Tuple.Create(x, y);
                                break;
                            case 'G':
                                map[x, y] = MapCell.Grass;
                                break;
                            case 'T':
                                map[x, y] = MapCell.Tree;
                                break;
                            case 'R':
                                map[x, y] = MapCell.Rock;
                                break;
                            case ' ':
                                map[x, y] = MapCell.Air;
                                break;
                            default:
                                throw new FormatException($"{stringMap[y][x]} is unknown cell type");
                        }
                    }
                }
            }
        }

        private readonly Map map;

        public Game()
        {
            var strMap = new[]{
                "P ",
                "RR"
            };

            map = new Map(strMap);
            map.PrintMap();
        }
    }
}
