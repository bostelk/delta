using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Delta
{
    public static class MathExtensions
    {
        public static float Wrap(this float value, float min, float max)
        {
            if (value > max) return min;
            if (value < min) return max;
            return value;
        }

        public static int Wrap(this int value, int min, int max)
        {
            if (value > max) return min;
            if (value < min) return max;
            return value;
        }

        public static float Clamp(this float value, float min, float max)
        {
            if (value > max) return max;
            if (value < min) return min;
            return value;
        }

        public static int Clamp(this int value, int min, int max)
        {
            if (value > max) return max;
            if (value < min) return min;
            return value;
        }

        public static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max)
        {
            return Vector2.Clamp(value, min, max);
        }

        public static float Square(this float value)
        {
            return value * value;
        }

        public static int Square(this int value)
        {
            return value * value;
        }

        public static Vector2 SimpleRound(this Vector2 value)
        {
            value.X = SimpleRound(value.X);
            value.Y = SimpleRound(value.Y);
            return value;
        }

        public static float SimpleRound(this float value)
        {
            value = (int)(value + 0.5f);
            return value;
            //return (float)Math.Round(value, 1);
        }

        public static float RandomSign()
        {
            return (float)Math.Pow(-1f, (double)G.Random.Next(2));
        }

        /// <summary>
        /// The magnitude of value1 and the sign of value2.
        /// </summary>
        /// <param name="value1">Magnitude.</param>
        /// <param name="value2">Sign.</param>
        /// <returns></returns>
        public static float CopySign(this float value1, float value2)
        {
            return Math.Abs(value1) * (value2 / value2);
        }

    }
}
