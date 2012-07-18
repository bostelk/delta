using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta
{
    public static class RandomExtensions
    {
        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        public static float Between(this Random random, float min, float max)
        {
            return (random.NextFloat() * (max - min) + min);
        }

        public static Vector2 Between(this Random random, Vector2 min, Vector2 max)
        {
            return new Vector2(random.Between(min.X, max.X), random.Between(min.Y, max.Y));
        }

        public static bool FiftyFifty(this Random random)
        {
            if (random.NextDouble() >= 0.5f)
                return true;
            else
                return false;
        }
    }
}
