using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Extensions
{
    public static class BlendStateExtensions
    {
        public static BlendState Parse(string value)
        {
            return Parse(null, value);
        }

        public static BlendState Parse(this BlendState blendState, string value)
        {
            switch (value) {
                case "alpha":
                case "alphablend":
                    return BlendState.AlphaBlend;
                case "add":
                case "additive":
                    return BlendState.Additive;
                case "opaque":
                case "solid":
                    return BlendState.Opaque;
                // reach does not support differt blends for Color and Alpha; ie. all the states below.
                case "multiply":
                    return new BlendState()
                    {
                        ColorSourceBlend = Blend.DestinationColor,
                        ColorDestinationBlend = Blend.Zero,
                        ColorBlendFunction = BlendFunction.Add,
                        AlphaSourceBlend = Blend.SourceAlpha,
                        AlphaDestinationBlend = Blend.InverseSourceAlpha
                    };
                case "screen":
                    return new BlendState()
                    {
                        ColorSourceBlend = Blend.InverseDestinationColor,
                        ColorDestinationBlend = Blend.One,
                        ColorBlendFunction = BlendFunction.Add,
                        AlphaSourceBlend = Blend.SourceAlpha,
                        AlphaDestinationBlend = Blend.InverseSourceAlpha
                    };
                case "lighten":
                    return new BlendState()
                    {
                        ColorSourceBlend = Blend.One,
                        ColorDestinationBlend = Blend.One,
                        ColorBlendFunction = BlendFunction.Max,
                        AlphaSourceBlend = Blend.SourceAlpha,
                        AlphaDestinationBlend = Blend.InverseSourceAlpha
                    };
                case "darken":
                    return new BlendState()
                    {
                        ColorSourceBlend = Blend.One,
                        ColorDestinationBlend = Blend.One,
                        ColorBlendFunction = BlendFunction.Min,
                        AlphaSourceBlend = Blend.SourceAlpha,
                        AlphaDestinationBlend = Blend.InverseSourceAlpha
                    };
                default:
                    return BlendState.AlphaBlend;
            }
        }
    }
}
