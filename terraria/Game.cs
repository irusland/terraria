using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace terraria
{
    public class Game
    {
        private readonly World world;

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
                case Keys.Up | Keys.W | Keys.Space:
                    Jump();
                    break;
                case Keys.Down | Keys.S:
                    Dig();
                    break;
                case Keys.Left | Keys.A:
                    GoLeft();
                    break;
                case Keys.Right | Keys.D:
                    GoRight();
                    break;
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
            { Direction.Up, new Point(0, 1) },
            { Direction.Down, new Point(0, -1) },
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
                // Hit
            }
        }

        private Point GetNextPlayersPosition(Point position, Direction direction) => 
            new Point(position.X + directionToOffset[direction].X,
                position.Y + directionToOffset[direction].Y);

        private void Dig()
        {
            var block = CheckWhatsAhead(world, world.player.position, Direction.Down);
            var itemInHand = world.player.inventory.GetInformationAboutWeapon();
            var destination = GetNextPlayersPosition(world.player.position, Direction.Down);
            if (block == World.Block.Wood && itemInHand == Inventory.TypeItem.Axe)
            {
                world.map[world.player.position.X, world.player.position.Y] = World.Block.Air;
                world.map[destination.X, destination.Y] = World.Block.Player;
                world.player.inventory.AddItem(new Inventory.Item(Inventory.TypeItem.Wood));
            }
            else if (block == World.Block.Grass && itemInHand == Inventory.TypeItem.Shovel)
            {
                // Hit
            }
            else if (block == World.Block.Rock && itemInHand == Inventory.TypeItem.Pick)
            {

            }
        }

        private void GoLeft()
        {
            // Left
        }

        private void GoRight()
        {
            // Right
        }

        private World.Block CheckWhatsAhead(World world, Point initial, Direction direction)
        {
            var offset = directionToOffset[direction];
            return world.map[initial.X + offset.X, initial.Y + offset.Y];
        }
    }
}
