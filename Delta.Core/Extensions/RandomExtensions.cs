using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
