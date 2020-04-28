using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK.Input;

namespace terraria
{
    public class Brain
    {
        public List<Animation> Animations = new List<Animation>();
        public static readonly int CellSize = 32;
        private static readonly double reachableDistance = 2;

        public void CollectAnimations(Game game)
        {
            Animations.Clear();
            for (var y = 0; y < game.MapHeight; y++)
            {
                for (var x = 0; x < game.MapWidth; x++)
                {
                    var character = game.world.map[x, y];
                    if (character == null)
                        continue;
                    var wish = character.GetWish(x, y, game);
                    if (character is Player player)
                    {
                        var target = game.world.map[x + wish.XOffset, y + wish.YOffset];
                        if (!(target is Air))
                        {
                            if (player.Inventory.GetSelectedSlot != null && target.IsBrokenBy(player.Inventory.GetSelectedSlot.Item, game))
                            {
                                wish.BreakBlockOnPossition = new Point(x + wish.XOffset, y + wish.YOffset);
                            }
                            else
                            {
                                wish.XOffset = 0;
                                wish.YOffset = 0;
                            }
                        }
                    }
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
            Animations = Animations
                .OrderByDescending(animation => animation.Character.GetDrawingPriority())
                .ToList();
        }

        public void ApplyAnimations(Game game)
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
            ApplyBlockBreaking(game);
            ApplyBlockPlaceing(game);
            RefreshInventory(game);
        }

        private void ApplyBlockPlaceing(Game game)
        {
            foreach (var animation in Animations)
            {
                if (animation.Wish.PlaceBlockOnPossition.X != -1
                    && animation.Wish.PlaceBlockOnPossition.Y != -1)
                {
                    var playerPosition = animation.Target;
                    if (!(game.world.map[playerPosition.X, playerPosition.Y] is Player))
                        throw new Exception($"{game.world.map[playerPosition.X, playerPosition.Y]} at {playerPosition} is not player and he want to place block");
                    var player = (Player)game.world.map[playerPosition.X, playerPosition.Y];
                    var target = animation.Wish.PlaceBlockOnPossition;
                    if (target.X == playerPosition.X && target.Y == playerPosition.Y)
                        continue;
                    var offset = Math.Sqrt(Math.Pow(playerPosition.X - target.X, 2) + Math.Pow(playerPosition.Y - target.Y, 2));
                    if (game.world.IsInBounds(target) && (game.world.map[target.X, target.Y] is Air) && offset < reachableDistance)
                    {
                        var sourceSlot = player.Inventory.GetSelectedSlot;
                        if (!(sourceSlot.Item is ICharacter))
                            continue;
                        if (sourceSlot.Amount > 0)
                        {
                            if (player.Inventory.TryPopSelectedItem(1, out var item))
                            {
                                game.world.map[target.X, target.Y] = (ICharacter)sourceSlot.Item;
                            }
                        }
                        //Console.WriteLine($"{target} was changed to {game.world.map[target.X, target.Y]}");
                    }
                    else
                    {
                        //Console.WriteLine($"cannot break '{game.world.map[target.X, target.Y]}' at {target.X} {target.Y}");
                    }
                }
            }
        }

        private void RefreshInventory(Game game)
        {
            var playerPos = World.GetPlayerPos(game.world);
            var player = (Player)game.world.map[playerPos.X, playerPos.Y];
            player.Inventory.Selected += game.mouseScrollCount;

            if (game.KeyPressed >= Key.Number0 && game.KeyPressed <= Key.Number9)
            {
                var number = game.KeyPressed - Key.Number0 - 1;
                player.Inventory.Selected = number;
            }
        }

        private void ApplyBlockBreaking(Game game)
        {
            foreach (var animation in Animations)
            {
                if (animation.Wish.BreakBlockOnPossition.X != -1
                   && animation.Wish.BreakBlockOnPossition.Y != -1)
                {
                    var playerPosition = animation.Target;
                    if (!(game.world.map[playerPosition.X, playerPosition.Y] is Player))
                        throw new Exception($"{game.world.map[playerPosition.X, playerPosition.Y]} at {playerPosition} is not player and he want to break block");
                    var player = (Player)game.world.map[playerPosition.X, playerPosition.Y];
                    var target = animation.Wish.BreakBlockOnPossition;
                    if (target.X == playerPosition.X && target.Y == playerPosition.Y)
                        continue;
                    var offset = Math.Sqrt(Math.Pow(playerPosition.X - target.X, 2) + Math.Pow(playerPosition.Y - target.Y, 2));
                    if (game.world.IsInBounds(target) && !(game.world.map[target.X, target.Y] is Air) && offset < reachableDistance)
                    {
                        var block = game.world.map[target.X, target.Y];
                        if (block.IsBrokenBy(player.Inventory.GetSelectedSlot.Item, game))
                        {
                            game.world.map[target.X, target.Y] = new Air();
                            player.Inventory.TryPush((IInventoryItem)block, 1);
                        }
                        //Console.WriteLine($"{target} was changed to {game.world.map[target.X, target.Y]}");
                    }
                    else
                    {
                        //Console.WriteLine($"cannot break '{game.world.map[target.X, target.Y]}' at {target.X} {target.Y}");
                    }
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
                    {
                        residents.Remove(pretender);
                    }
                    //if (resident is Player && pretender is Zombie && resident.DeadInConflict(pretender, game))
                        //residents.Remove(resident);
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
                if (next is Player player && characters[x, y].Any())
                {
                    var block = characters[x, y].Last();
                    if (!(block is Air))
                    {
                        player.Inventory.TryPush((IInventoryItem)block, 1);
                    }
                }
                characters[x, y].Add(next);
            }
            return characters;
        }
    }
}
