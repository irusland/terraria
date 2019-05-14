using System;
using System.Collections.Generic;
using System.Drawing;
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
            return wish;
        }

        private Point MousePositionToMapCell(Point mousePosition) =>
            new Point(mousePosition.X / Brain.CellSize,
            mousePosition.Y / Brain.CellSize);

        public Point GetMouseOffset(Point player, Point mouse) =>
            new Point(mouse.X - player.X * Brain.CellSize - Brain.CellSize / 2,
            mouse.Y - player.Y * Brain.CellSize - Brain.CellSize / 2);

        public bool DeadInConflict(ICharacter conflictedObject, Game game)
        {
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
        private static readonly HashSet<ICharacter> deadlyCharacters = new HashSet<ICharacter> { new Player() };
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
}