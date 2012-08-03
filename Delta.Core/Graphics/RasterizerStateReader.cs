using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Graphics
{
#pragma warning disable 1591
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class RasterizerStateReader : ContentTypeReader<RasterizerState>
    {
        protected override RasterizerState Read(ContentReader input, RasterizerState value)
        {
            if (value == null)
                value = new RasterizerState();

            value.Name = input.ReadObject<string>();
            value.CullMode = input.ReadObject<CullMode>();
            value.DepthBias = input.ReadObject<float>();
            value.FillMode = input.ReadObject<FillMode>();
            value.MultiSampleAntiAlias = input.ReadObject<bool>();
            value.ScissorTestEnable = input.ReadObject<bool>();
            value.SlopeScaleDepthBias = input.ReadObject<float>();

            return value;
        }
    }
#pragma warning restore 1591
}