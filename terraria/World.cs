﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace terraria
{
    public class World
    {
        public int MapWidth => map.GetLength(0);
        public int MapHeight => map.GetLength(1);
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
                builder.Append(System.Environment.NewLine);
            }
            return builder.ToString();
        }

        private World(string stringMap)
        {
            var separator = System.Environment.NewLine;
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
                        case 'Z':
                            map[x, y] = new Zombie();
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

        public static World CreateWithInfo(string stringMap, string stringInfo)
        {
            var world = new World(stringMap);

            var slots = ParseInfo(stringInfo);
            var playerPos = GetPlayerPos(world);
            var character = world.map[playerPos.X, playerPos.Y];
            if (character is Player player)
            {
                player.Inventory = new Inventory();
                foreach (var slot in slots)
                {
                    player.Inventory.TryPush(slot.Item, slot.Amount);
                }
                Console.WriteLine(player.Inventory);
            }
            else
            {
                throw new Exception("Not a player");
            }
            return world;
        }

        public static Point GetPlayerPos(World world)
        {
            for (var x = 0; x < world.MapWidth; x++)
            {
                for (var y = 0; y < world.MapHeight; y++)
                {
                    if (world.map[x, y] is Player)
                    {
                        return new Point(x, y);
                    }
                }
            }
            throw new Exception("GAME ENDED PLAYER DIED!");
            return new Point(-1, -1);
        }

        private static List<Inventory.Slot> ParseInfo(string stringInfo)
        {
            var separator = System.Environment.NewLine;
            var lines = stringInfo.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<Inventory.Slot>();
            foreach (var line in lines)
            {
                var info = line.Split();
                var itemStr = info[0];
                var amountStr = info[1];
                var itemConstructor = itemFromStr[itemStr];
                result.Add(new Inventory.Slot(itemConstructor(), int.Parse(amountStr)));
            }
            return result;
        }

        private static readonly Dictionary<string, Func<IInventoryItem>> itemFromStr = new Dictionary<string, Func<IInventoryItem>>
        {
            {"Axe", () => new Axe()},
            {"Wood", () => new Wood()},
            {"Pick", () => new Pick()},
            {"Rock", () => new Rock()},
            {"Shovel", () => new Shovel()},
            {"Grass", () => new Grass()},
            {"Sword", () => new Sword()},
        };
    }
}