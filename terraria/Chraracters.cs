using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace terraria
{
    public class Player : ICharacter, IPlayer
    {
        public Wish GetWish(int x, int y, Game game)
        {
            var wish = new Wish { XOffset = 0, YOffset = 0 };
            switch (game.KeyPressed)
            {
                case Keys.Left:
                case Keys.A:
                    if (x - 1 >= 0)
                        wish.XOffset = -1;
                    break;
                case Keys.Right:
                case Keys.D:
                    if (x + 1 < game.world.map.GetLength(0))
                        wish.XOffset = 1;
                    break;
                case Keys.Up:
                case Keys.W:
                    if (y - 1 >= 0)
                        wish.YOffset = -1;
                    break;
                case Keys.Down:
                case Keys.S:
                    if (y + 1 < game.world.map.GetLength(1))
                        wish.YOffset = 1;
                    break;
            }

            var mouseOffset = GetMouseOffset(new Point(x, y), game.MousePosition);
            var mx = mouseOffset.X;
            var my = mouseOffset.Y;
            if (my - mx <= 0 && my + mx >= 0)
                Direction = Direction.Right;
            if (my - mx > 0 && my + mx < 0)
                Direction = Direction.Left;
            if (my - mx <= 0 && my + mx < 0)
                Direction = Direction.Up;
            if (my - mx > 0 && my + mx >= 0)
                Direction = Direction.Down;

            if (game.MouseClicked == MouseButtons.Left)
            {
                wish.BreakBlockOnPossition = MousePositionToMapCell(game.MousePosition);
            }
            if (game.MouseClicked == MouseButtons.Right)
            {
                wish.PlaceBlockOnPossition = MousePositionToMapCell(game.MousePosition);
                wish.PlaceBlockFromInventorySlot = Inventory.Selected;
            }
            return wish;
        }

        private Point MousePositionToMapCell(Point mousePosition)
        {
            return new Point(
                mousePosition.X / Brain.CellSize,
                mousePosition.Y / Brain.CellSize);
        }

        public Point GetMouseOffset(Point player, Point mouse)
        {
            return new Point(
                mouse.X - player.X * Brain.CellSize - Brain.CellSize / 2,
                mouse.Y - player.Y * Brain.CellSize - Brain.CellSize / 2);
        }

        public bool DeadInConflict(ICharacter conflictedObject, Game game)
        {
            if (conflictedObject is Zombie && !((Zombie)conflictedObject).DeadInConflict(this, game))
                return true;
            if (!deadlyCharacters.Contains(conflictedObject))
                return false;
            return true;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            switch (Direction)
            {
                case Direction.Down:
                    return "player_front.png";
                case Direction.Up:
                    return "player_back.png";
                case Direction.Left:
                    return "player_left.png";
                case Direction.Right:
                    return "player_right.png";
                default:
                    throw new Exception($"{Direction} is incorrect");
            }
        }

        public override string ToString() => "P";

        public bool IsBrokenBy(IInventoryItem weapon, Game game)
        {
            return false;
        }

        public Direction Direction { get; set; }
        public Inventory Inventory;

        // TODO add some inventory logic for fight
        private static readonly HashSet<ICharacter> deadlyCharacters = new HashSet<ICharacter> { new Player(), new Zombie() };
    }

    public class Rock : ICharacter, IInventoryItem
    {
        public Wish GetWish(int x, int y, Game game)
        {
            return new Wish();
        }

        public bool DeadInConflict(ICharacter conflictedObject, Game game)
        {
            if (conflictedObject is Player /*&& conflictedObject.Inventory.SelectedItem is Pick*/ )
            {
                //conflictedObject.Inventory.AddItem(this);
                return true;
            }
            return false;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName() => "rock.png";

        public override string ToString() => "R";

        public bool IsBrokenBy(IInventoryItem weapon, Game game)
        {
            if (weapon is Pick)
                return true;
            return false;
        }

        public string GetIconFileName()
        {
            return "rock_icon.png";
        }
    }

    public class Grass : ICharacter, IInventoryItem
    {
        public Wish GetWish(int x, int y, Game game)
        {
            return new Wish();
        }

        public bool DeadInConflict(ICharacter conflictedObject, Game game)
        {
            if (conflictedObject is Player /*&& conflictedObject.Inventory.SelectedItem is Shovel*/ )
            {
                return true;
            }
            return false;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName() => "grass.png";

        public override string ToString() => "G";

        public bool IsBrokenBy(IInventoryItem weapon, Game game)
        {
            if (weapon is Shovel)
                return true;
            return false;
        }

        public string GetIconFileName()
        {
            return "grass_icon.png";
        }
    }

    public class Air : ICharacter
    {
        public Wish GetWish(int x, int y, Game game)
        {
            return new Wish();
        }

        public bool DeadInConflict(ICharacter conflictedObject, Game game)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName() => "air.png";

        public override string ToString() => " ";

        public bool IsBrokenBy(IInventoryItem weapon, Game game)
        {
            throw new Exception($"some one tries to break air with {weapon}");
        }
    }

    public class Wood : ICharacter, IInventoryItem
    {
        public Wish GetWish(int x, int y, Game game)
        {
            return new Wish();
        }

        public bool DeadInConflict(ICharacter conflictedObject, Game game)
        {
            if (conflictedObject is Player /*&& conflictedObject.Inventory.SelectedItem is Axe*/ )
            {
                return true;
            }
            return false;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName() => "wood.png";

        public override string ToString() => "W";

        public bool IsBrokenBy(IInventoryItem weapon, Game game)
        {
            if (weapon is Axe)
                return true;
            return false;
        }

        public string GetIconFileName()
        {
            return "wood_icon.png";
        }
    }

    public class Zombie : ICharacter, IInventoryItem
    {
        public Wish GetWish(int x, int y, Game game)
        {
            var commands = new List<Wish>();
            SearchPathToDigger(commands, x, y, new List<Wish>(), int.MaxValue, game);
            if (commands.Count == 0)
                return new Wish();
            return commands.Last();
        }

        public bool HasCycledPath(List<Wish> path, int deltaX, int deltaY)
        {
            var displacementX = deltaX;
            var displacementY = deltaY;
            foreach (var command in path)
            {
                displacementX += command.XOffset;
                displacementY += command.YOffset;
                if (displacementX == 0 && displacementY == 0)
                    return true;
            }
            return displacementX == 0 && displacementY == 0;
        }

        public static int[][] Moves =
        {
            new [] { 1, 0 },
            new [] { 0, 1 },
            new [] { -1, 0 },
            new [] { 0, -1 },
        };

        private int SearchPathToDigger(List<Wish> result,
            int posX,
            int posY,
            List<Wish> path,
            int minLength,
            Game game)
        {
            if (game.world.map[posX, posY] is Player && path.Count < minLength)
            {
                result.Clear();
                foreach (var element in path)
                {
                    result.Add(element);
                }
                minLength = path.Count;
            }

            foreach (var move in Moves)
            {
                if (IsValidPath(posX, posY, move[0], move[1], path, minLength, game))
                {
                    var pathCopy = new List<Wish>(path);
                    pathCopy.Insert(0, new Wish { XOffset = move[0], YOffset = move[1] });
                    minLength = SearchPathToDigger(result, posX + move[0], posY + move[1], pathCopy, minLength, game);
                }
            }
            return minLength;
        }

        public bool IsValidPath(int x,
            int y,
            int dX,
            int dY,
            List<Wish> path,
            int minLength,
            Game game)
        {
            var posX = x + dX;
            var posY = y + dY;
            return path.Count < minLength &&
                !HasCycledPath(path, dX, dY) &&
                IsInBorders(posX, posY, game) &&
                IsAbleToPass(posX, posY, game);
        }

        public bool IsInBorders(int posX, int posY, Game game)
        {
            return posX < game.MapWidth &&
                posY < game.MapHeight &&
                posX >= 0 &&
                posY >= 0;
        }

        public bool IsAbleToPass(int posX, int posY, Game game)
        {
            return game.world.map[posX, posY] is Air | game.world.map[posX, posY] is Player;
        }

        public string GetImageFileName()
        {
            return "zombie.png";
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public bool DeadInConflict(ICharacter conflictedObject, Game game)
        {
            if (conflictedObject is Player player)
            {
                if (player.Inventory.GetSelectedSlot.Item is Sword)
                    return true;
                return false;
            }
            return false;
        }

        public bool IsBrokenBy(IInventoryItem weapon, Game game)
        {
            if (weapon is Sword)
                return true;
            return false;
        }

        public string GetIconFileName()
        {
            return "zombie_icon.png";
        }
    }

    public class Pick : IInventoryItem
    {
        public string GetIconFileName()
        {
            return "pick.png";
        }
    }

    public class Axe : IInventoryItem
    {
        public string GetIconFileName()
        {
            return "axe.png";
        }
    }

    public class Shovel : IInventoryItem
    {
        public string GetIconFileName()
        {
            return "shovel.png";
        }
    }

    public class Sword : IInventoryItem
    {
        public string GetIconFileName()
        {
            return "sword.png";
        }
    }
}