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
    public static class ColorExtensions
    {
        /// <summary>
        /// Set the transparancy percentage of a Color. On a scale of: 0 is transparent to 1 is opaque.
        /// </summary>
        /// <param name="color">The color to modify.</param>
        /// <param name="amoutn">The amount of transparency.</param>
        /// <returns>The alpha'd Color</returns>
        public static Color SetAlpha(this Color color, float value)
        {
            value = MathHelper.Clamp(value, 0f, 1f);
            return color * value;
        }
    }
}
