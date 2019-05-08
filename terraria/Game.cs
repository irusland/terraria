using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace terraria
{
    public class Game
    {
        public readonly World world;

        public Game(World world)
        {
            this.world = world;
        }

        public void Update()
        {
            //Recount all props
        }

        public void UpdateOnKeyPress(Keys key)
        {
            switch (key)
            {
                case Keys.Up: case Keys.W: case Keys.Space:
                    Jump();
                    break;
                case Keys.Down: case Keys.S:
                    Dig();
                    break;
                case Keys.Left: case Keys.A:
                    GoLeft();
                    break;
                case Keys.Right: case Keys.D:
                    GoRight();
                    break;
                default:
                    throw new FormatException("Key is not set");
            }
        }

        private enum Direction
        {
            Up,
            Down,
            Right,
            Left
        }

        private Dictionary<Direction, Point> directionToOffset = new Dictionary<Direction, Point>
        {
            { Direction.Up, new Point(0, -1) },
            { Direction.Down, new Point(0, 1) },
            { Direction.Right, new Point(1, 0) },
            { Direction.Left, new Point(-1, 0) }

        };

        private void Jump()
        {
            var block = CheckWhatsAhead(world, world.player.position, Direction.Up);
            var destination = GetNextPlayersPosition(world.player.position, Direction.Up);
            if (block == World.Block.Air)
            {
                world.map[world.player.position.X, world.player.position.Y] = World.Block.Air;
                world.map[destination.X, destination.Y] = World.Block.Player;
            }
            else
            {
                // TODO OnCollision();
            }
        }

        private Point GetNextPlayersPosition(Point position, Direction direction) => 
            new Point(position.X + directionToOffset[direction].X,
                position.Y + directionToOffset[direction].Y);

        private bool TryMovePlayer(Point initial, Point destination)
        {
            if (world.player.position != initial)
                throw new Exception("You r trying to move not a player");
            world.map[world.player.position.X, world.player.position.Y] = World.Block.Air;
            world.map[destination.X, destination.Y] = World.Block.Player;
            return true;
        }

        private void Dig()
        {
            var block = CheckWhatsAhead(world, world.player.position, Direction.Down);
            var itemInHand = world.player.inventory.GetInformationAboutWeapon();
            var destination = GetNextPlayersPosition(world.player.position, Direction.Down);
            if (block == World.Block.Wood && itemInHand == Inventory.TypeItem.Axe)
            {
                TryMovePlayer(world.player.position, destination);
                world.player.inventory.AddItem(new Inventory.Item(Inventory.TypeItem.Wood));
            }
            else if (block == World.Block.Grass && itemInHand == Inventory.TypeItem.Shovel)
            {
                TryMovePlayer(world.player.position, destination);
                world.player.inventory.AddItem(new Inventory.Item(Inventory.TypeItem.Dirt));
            }
            else if (block == World.Block.Rock && itemInHand == Inventory.TypeItem.Pick)
            {
                TryMovePlayer(world.player.position, destination);
                world.player.inventory.AddItem(new Inventory.Item(Inventory.TypeItem.Wood));
            }
            else
            {
                Console.Write("You dont have a nessesery item in hand");
            }
        }

        private void GoLeft()
        {
            var block = CheckWhatsAhead(world, world.player.position, Direction.Left);
            var itemInHand = world.player.inventory.GetInformationAboutWeapon();
            var destination = GetNextPlayersPosition(world.player.position, Direction.Left);
            if (block == World.Block.Air)
            {
                TryMovePlayer(world.player.position, destination);
            }
            else
            {
                // TODO OnCollision
            }
        }

        private void GoRight()
        {
            var block = CheckWhatsAhead(world, world.player.position, Direction.Right);
            var itemInHand = world.player.inventory.GetInformationAboutWeapon();
            var destination = GetNextPlayersPosition(world.player.position, Direction.Right);
            if (block == World.Block.Air)
            {
                TryMovePlayer(world.player.position, destination);
            }
            else
            {
                // TODO OnCollision
            }
        }

        private World.Block CheckWhatsAhead(World world, Point initial, Direction direction)
        {
            var offset = directionToOffset[direction];
            return world.map[initial.X + offset.X, initial.Y + offset.Y];
        }
    }
}
