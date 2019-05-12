using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace terraria
{
    public class Player : ICharacter, IPlayer
    {
        //Direction IPlayer.Direction { get; set; }

        public Wish GetWish(int x, int y, Game game)
        {
            var command = new Wish { XOffset = 0, YOffset = 0 };
            switch (game.KeyPressed)
            {
                case Keys.Left:
                case Keys.A:
                    if (x - 1 >= 0)
                        command.XOffset = -1;
                    break;
                case Keys.Right:
                case Keys.D:
                    if (x + 1 < game.world.map.GetLength(0))
                        command.XOffset = 1;
                    break;
                case Keys.Up:
                case Keys.W:
                    if (y - 1 >= 0)
                        command.YOffset = -1;
                    break;
                case Keys.Down:
                case Keys.S:
                    if (y + 1 < game.world.map.GetLength(1))
                        command.YOffset = 1;
                    break;
            }
            return command;
        }

        public bool DeadInConflict(ICharacter conflictedObject, Game game)
        {
            if (!deadlyCharacters.Contains(conflictedObject))
                return false;
            return true;
        }

        public int GetDrawingPriority()
        {
            throw new System.NotImplementedException();
        }

        public string GetImageFileName()
        {
            switch (direction)
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
                    throw new Exception($"{direction} is incorrect");
            }
        }

        public override string ToString() => "P";

        private Direction direction = Direction.Down;

        public Direction GetDirection()
        {
            return direction;
        }

        public void SetDirection(Direction dir)
        {
            direction = dir;
        }

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
            throw new System.NotImplementedException();
        }

        public string GetImageFileName() => "rock.png";

        public override string ToString() => "R";
    }

    public class Grass : ICharacter
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
            throw new System.NotImplementedException();
        }

        public string GetImageFileName() => "grass.png";

        public override string ToString() => "G";
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
            throw new System.NotImplementedException();
        }

        public string GetImageFileName() => "air.png";

        public override string ToString() => " ";
    }

    public class Wood : ICharacter
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
            throw new System.NotImplementedException();
        }

        public string GetImageFileName() => "wood.png";

        public override string ToString() => "W";
    }
}