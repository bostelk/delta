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
    public static class DeltaMath
    {
        public static float Wrap(float value, float min, float max)
        {
            value = (value > max) ? min : value;
            value = (value < min) ? max : value;
            return value;
        }

        public static int Wrap(int value, int min, int max)
        {
            value = (value > max) ? min : value;
            value = (value < min) ? max : value;
            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;
            return value;
        }

        public static float Sqr(float value)
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

        //public static float SimpleRound(float value)
        //{
        //    return (float)Math.Round(value, 1);
        //}
    }
}
