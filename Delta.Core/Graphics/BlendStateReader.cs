using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Graphics
{
#pragma warning disable 1591
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class BlendStateReader : ContentTypeReader<BlendState>
    {
        protected override BlendState Read(ContentReader input, BlendState value)
        {
            if (value == null)
                value = new BlendState();

            value.Name = input.ReadObject<string>();
            value.AlphaBlendFunction = input.ReadObject<BlendFunction>();
            value.AlphaDestinationBlend = input.ReadObject<Blend>();
            value.AlphaSourceBlend = input.ReadObject<Blend>();
            value.BlendFactor = input.ReadObject<Color>();
            value.ColorBlendFunction = input.ReadObject<BlendFunction>();
            value.ColorDestinationBlend = input.ReadObject<Blend>();
            value.ColorSourceBlend = input.ReadObject<Blend>();
            value.ColorWriteChannels = input.ReadObject<ColorWriteChannels>();
            value.ColorWriteChannels1 = input.ReadObject<ColorWriteChannels>();
            value.ColorWriteChannels2 = input.ReadObject<ColorWriteChannels>();
            value.ColorWriteChannels3 = input.ReadObject<ColorWriteChannels>();
            value.MultiSampleMask = input.ReadObject<int>();

            return value;
        }
    }
#pragma warning restore 1591
}