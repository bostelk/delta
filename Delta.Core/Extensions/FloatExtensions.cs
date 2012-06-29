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
    public static class FloatExtensions
    {
        public static bool AlmostEqual(float a, float b, float epsilon)
        {
            float num = Math.Abs(a);
            float num2 = Math.Abs(b);
            float num3 = Math.Abs(a - b);
            if (a * b == 0f)
            {
                return num3 < epsilon * epsilon;
            }
            return num3 / (num + num2) < epsilon;
        }

        public static bool IsValid(this float value)
        {
            return !float.IsNaN(value);
        }
    }
}
