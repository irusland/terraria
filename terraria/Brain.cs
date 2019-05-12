using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace terraria
{
    public class Brain
    {
        public List<Animation> Animations = new List<Animation>();
        public static readonly int CellSize = 1;

        public void CollectWishes(Game game)
        {
            Animations.Clear();
            for (var y = 0; y < game.MapHeight; y++)
            {
                for (var x = 0; x < game.MapWidth; x++)
                {
                    var character = game.world.map[x, y];
                    if (character == null) continue;
                    var wish = character.GetWish(x, y, game);
                    Console.WriteLine($"{character} {wish}");
                    Animations.Add(
                    new Animation
                    {
                        Wish = wish,
                        Character = character,
                        Location = new Point(x * CellSize, y * CellSize),
                        Target = new Point(x + wish.XOffset, y + wish.YOffset)
                    });
                }
            }
            Console.WriteLine(game.world);
        }

        public void ApplyWishes(Game game)
        {
            var cohabitants = GetCohabitants(game);
            for (var x = 0; x < game.MapWidth; x++)
            {
                for (var y = 0; y < game.MapHeight; y++)
                {
                    game.world.map[x, y] = GetNextResident(cohabitants, x, y, game);
                    //Console.WriteLine($"{x}, {y} updated to '{game.world.map[x, y]}' ({game.MapWidth} by {game.MapHeight})");
                }
            }
        }

        private static ICharacter GetNextResident(List<ICharacter>[,] field, int x, int y, Game game)
        {
            var chunk = field[x, y];
            var residents = chunk.ToList();
            foreach (var pretender in chunk)
            {
                foreach (var resident in chunk)
                {
                    if (resident != pretender && pretender.DeadInConflict(resident, game))
                        residents.Remove(pretender);
                }
            }

            if (residents.Count > 1)
            {
                throw new Exception(
                    $"{residents[0].GetType().Name} and {residents[1].GetType().Name} on same cell");
            }

            if (residents.Count == 0)
                return new Air();
            return residents.First();
        }

        private List<ICharacter>[,] GetCohabitants(Game game)
        {
            var characters = new List<ICharacter>[game.MapWidth, game.MapHeight];
            for (var x = 0; x < game.MapWidth; x++)
            {
                for (var y = 0; y < game.MapHeight; y++)
                    characters[x, y] = new List<ICharacter>();
            }
            foreach (var animation in Animations)
            {
                var x = animation.Target.X;
                var y = animation.Target.Y;
                var next = animation.Wish.TransformTo ?? animation.Character;
                characters[x, y].Add(next);
            }
            return characters;
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
            //var block = CheckWhatsAhead(world, world.player.position, Direction.Up);
            //var destination = GetNextPlayersPosition(world.player.position, Direction.Up);
            //if (block == World.Block.Air)
            //{
            //    world.map[world.player.position.X, world.player.position.Y] = World.Block.Air;
            //    world.map[destination.X, destination.Y] = World.Block.Player;
            //}
            //else
            //{
            //    // TODO OnCollision();
            //}
        }

        private Point GetNextPlayersPosition(Point position, Direction direction) =>
            new Point(position.X + directionToOffset[direction].X,
                position.Y + directionToOffset[direction].Y);

        private bool TryMovePlayer(Point initial, Point destination)
        {
            //if (world.player.position != initial)
            //    throw new Exception("You r trying to move not a player");
            //world.map[world.player.position.X, world.player.position.Y] = World.Block.Air;
            //world.map[destination.X, destination.Y] = World.Block.Player;
            return true;
        }

        private void Dig()
        {
            //var block = CheckWhatsAhead(world, world.player.position, Direction.Down);
            //var itemInHand = world.player.inventory.GetInformationAboutWeapon();
            //var destination = GetNextPlayersPosition(world.player.position, Direction.Down);
            //if (block == World.Block.Wood && itemInHand == Inventory.TypeItem.Axe)
            //{
            //    TryMovePlayer(world.player.position, destination);
            //    world.player.inventory.AddItem(new Inventory.Item(Inventory.TypeItem.Wood));
            //}
            //else if (block == World.Block.Grass && itemInHand == Inventory.TypeItem.Shovel)
            //{
            //    TryMovePlayer(world.player.position, destination);
            //    world.player.inventory.AddItem(new Inventory.Item(Inventory.TypeItem.Dirt));
            //}
            //else if (block == World.Block.Rock && itemInHand == Inventory.TypeItem.Pick)
            //{
            //    TryMovePlayer(world.player.position, destination);
            //    world.player.inventory.AddItem(new Inventory.Item(Inventory.TypeItem.Wood));
            //}
            //else
            //{
            //    Console.Write("You dont have a nessesery item in hand");
            //}
        }

        private void GoLeft()
        {
            //var block = CheckWhatsAhead(world, world.player.position, Direction.Left);
            //var itemInHand = world.player.inventory.GetInformationAboutWeapon();
            //var destination = GetNextPlayersPosition(world.player.position, Direction.Left);
            //if (block == World.Block.Air)
            //{
            //    TryMovePlayer(world.player.position, destination);
            //}
            //else
            //{
            //    // TODO OnCollision
            //}
        }

        private void GoRight()
        {
            //var block = CheckWhatsAhead(world, world.player.position, Direction.Right);
            //var itemInHand = world.player.inventory.GetInformationAboutWeapon();
            //var destination = GetNextPlayersPosition(world.player.position, Direction.Right);
            //if (block == World.Block.Air)
            //{
            //    TryMovePlayer(world.player.position, destination);
            //}
            //else
            //{
            //    // TODO OnCollision
            //}
        }

        private ICharacter CheckWhatsAhead(World world, Point initial, Direction direction)
        {
            var offset = directionToOffset[direction];
            return world.map[initial.X + offset.X, initial.Y + offset.Y];
        }
    }
}
