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
using System.Globalization;

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

        /// <summary>
        /// Creates an ARGB or RGB hexadecimal string representation of the <see cref="Color"/> value.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> value to parse.</param>
        /// <param name="includeHash">Determines whether to include the hash symbol (#) at the beginning of the string.</param>
        /// <returns>A hexadecimal string representation of the specified <see cref="Color"/> value.</returns>
        public static string ToHexadecimal(this Color color, bool includeHash)
        {
            string[] argb = { color.A.ToString("X2"), color.R.ToString("X2"), color.G.ToString("X2"), color.B.ToString("X2"), };
            return (includeHash ? "#" : string.Empty) + string.Join(string.Empty, argb);
        }

        /// <summary>
        /// Creates an ARGB hexadecimal string representation of the <see cref="Color"/> value.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> value to parse.</param>
        /// <returns>A hexadecimal string representation of the specified <see cref="Color"/> value.</returns>
        public static string ToHexadecimal(this Color color)
        {
            return ToHexadecimal(color, true);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> value from an ARGB or RGB hexadecimal string.
        /// </summary>
        /// <param name="hexString">The ARGB or RGB hexadecimal string to parse.</param>
        /// <returns>A <see cref="Color"/> value as defined by the ARGB or RGB hexadecimal string.</returns>
        /// <remarks>The string may begin with or without the hash symbol (#).</remarks>
        public static Color ToColor(this string value)
        {
            if (value.StartsWith("#"))
                value = value.Substring(1); //just remove the hash symbol
            while (value.Length < 8) //ensure we have a proper uint string, allows us to parse RGB strings by adding in the A value.
                value = "F" + value;
            uint packedValue = uint.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            //we can't do just: return new Color() { PackedValue = packedValue); XNA Color.PackedValue format is AABBGGRR, but We want AARRGGBB!
            return new Color() { PackedValue = packedValue, R = (byte)(packedValue >> 16), B = (byte)packedValue };
        }
    }
}
