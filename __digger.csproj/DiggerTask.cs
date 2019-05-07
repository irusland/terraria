using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Digger;

namespace Digger
{
    public class Terrain : ICreature
    {
        public string GetImageFileName()
        {
            return "Terrain.png";
        }

        public int GetDrawingPriority()
        {
            return 10;
        }

        public CreatureCommand Act(int x, int y, Game game)
        {
            return new CreatureCommand { };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            return conflictedObject is Player;
        }
    }

    public class Player : ICreature
    {
        public string GetImageFileName()
        {
            return "Digger.png";
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        public CreatureCommand Act(int x, int y, Game game)
        {
            var command = new CreatureCommand { DeltaX = 0, DeltaY = 0 };
            switch (game.KeyPressed)
            {
                case Keys.Left:
                    if (x - 1 >= 0)
                        command.DeltaX = -1;
                    break;
                case Keys.Right:
                    if (x + 1 < game.MapWidth)
                        command.DeltaX = 1;
                    break;
                case Keys.Up:
                    if (y - 1 >= 0)
                        command.DeltaY = -1;
                    break;
                case Keys.Down:
                    if (y + 1 < game.MapHeight)
                        command.DeltaY = 1;
                    break;
            }
            if (game.Map[x + command.DeltaX, y + command.DeltaY] is Sack)
                command = new CreatureCommand { DeltaX = 0, DeltaY = 0 };
            return command;
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            if (conflictedObject is Gold)
            {
                game.Scores += 10;
            }
            return conflictedObject is Sack || conflictedObject is Monster;
        }
    }

    public class Sack : ICreature
    {
        public string GetImageFileName()
        {
            return "Sack.png";
        }

        public int GetDrawingPriority()
        {
            return 10;
        }

        public int FallDistance;

        public CreatureCommand Act(int x, int y, Game game)
        {
            var command = new CreatureCommand { DeltaX = 0, DeltaY = 0 };

            if (IsHighVelocity(x, y, game))
            {
                Break(command);
            }
            if (IsAbleToFall(x, y, game))
            {
                Fall(command);
            }
            if (IsLaying(x, y, game))
            {
                Stop(command);
            }

            return command;
        }

        public void Break(CreatureCommand command)
        {
            command.TransformTo = new Gold();
        }

        public void Fall(CreatureCommand command)
        {
            FallDistance++;
            command.DeltaY = 1;
        }

        public void Stop(CreatureCommand command)
        {
            FallDistance = 0;
        }

        public bool IsHighVelocity(int x, int y, Game game)
        {
            return ((y < game.MapHeight - 1 &&
                !(game.Map[x, y + 1] is null) &&
                !(game.Map[x, y + 1] is Player) &&
                !(game.Map[x, y + 1] is Monster)) ||
                y == game.MapHeight - 1) &&
                FallDistance > 1;
        }

        public bool IsAbleToFall(int x, int y, Game game)
        {
            return y < game.MapHeight - 1 &&
                (((game.Map[x, y + 1] is Player ||
                game.Map[x, y + 1] is Monster) &&
                FallDistance > 0) ||
                game.Map[x, y + 1] is null);
        }

        public bool IsLaying(int x, int y, Game game)
        {
            return y < game.MapHeight - 1 && !(game.Map[x, y + 1] is null) && FallDistance <= 1;
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            return false;
        }
    }

    public class Gold : ICreature
    {
        public string GetImageFileName()
        {
            return "Gold.png";
        }

        public int GetDrawingPriority()
        {
            return 4;
        }

        public CreatureCommand Act(int x, int y, Game game)
        {
            return new CreatureCommand { };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            return conflictedObject is Monster || conflictedObject is Player;
        }
    }

    public class Monster : ICreature
    {
        public string GetImageFileName()
        {
            return "Monster.png";
        }

        public int GetDrawingPriority()
        {
            return 5;
        }

        public CreatureCommand Act(int x, int y, Game game)
        {
            var commands = new List<CreatureCommand>();
            SearchPathToDigger(commands, x, y, new List<CreatureCommand>(), int.MaxValue, game);
            if (commands.Count == 0)
                return new CreatureCommand();
            return commands.Last();
        }

        public bool HasCycledPath(List<CreatureCommand> path, int deltaX, int deltaY)
        {
            var displacementX = deltaX;
            var displacementY = deltaY;
            foreach (var command in path)
            {
                displacementX += command.DeltaX;
                displacementY += command.DeltaY;
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

        public int SearchPathToDigger(List<CreatureCommand> result,
            int posX,
            int posY,
            List<CreatureCommand> path,
            int minLength,
            Game game)
        {
            if (game.Map[posX, posY] is Player && path.Count < minLength)
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
                    var pathCopy = new List<CreatureCommand>(path);
                    pathCopy.Insert(0, new CreatureCommand { DeltaX = move[0], DeltaY = move[1] });
                    minLength = SearchPathToDigger(result, posX + move[0], posY + move[1], pathCopy, minLength, game);
                }
            }
            return minLength;
        }

        public bool IsValidPath(int x,
            int y,
            int dX,
            int dY,
            List<CreatureCommand> path,
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
            return !(game.Map[posX, posY] is Terrain) &&
                !(game.Map[posX, posY] is Monster) &&
                !(game.Map[posX, posY] is Sack);
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }
    }
}