using System;
using System.Collections.Generic;
using System.Text;

namespace terraria
{
    public class WorldGenerator
    {
        public World Generate(int width, int height)
        {
            var mapBuilder = new StringBuilder();

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    mapBuilder.Append("G");
                }
                mapBuilder.Append("\n");
            }
            return World.Create(mapBuilder.ToString());
        }
    }
}
