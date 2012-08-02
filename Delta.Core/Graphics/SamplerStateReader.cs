using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Graphics
{
#pragma warning disable 1591
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SamplerStateReader : ContentTypeReader<SamplerState>
    {
        protected override SamplerState Read(ContentReader input, SamplerState value)
        {
            if (value == null)
                value = new SamplerState();

            value.Name = input.ReadObject<string>();
            value.AddressU = input.ReadObject<TextureAddressMode>();
            value.AddressV = input.ReadObject<TextureAddressMode>();
            value.AddressW = input.ReadObject<TextureAddressMode>();
            value.Filter = input.ReadObject<TextureFilter>();
            value.MaxAnisotropy = input.ReadObject<int>();
            value.MaxMipLevel = input.ReadObject<int>();
            value.MipMapLevelOfDetailBias = input.ReadObject<float>();

            return value;
        }
    }
#pragma warning restore 1591
}
