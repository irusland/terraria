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
        public static readonly int CellSize = 32;

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
                    //Console.WriteLine($"{character} {wish}");
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
    }
}
