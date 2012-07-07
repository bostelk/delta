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

        public static Vector2 SimpleRound(Vector2 value)
        {
            value.X = SimpleRound(value.X);
            value.Y = SimpleRound(value.Y);
            return value;
        }

        public static float SimpleRound(float value)
        {
            value += 0.5f;
            return (int)value;
        }

        public static float RandomSign()
        {
            return (float)Math.Pow(-1f, (double)G.Random.Next(2));
        }

        //public static float SimpleRound(float value)
        //{
        //    return (float)Math.Round(value, 1);
        //}
    }
}
