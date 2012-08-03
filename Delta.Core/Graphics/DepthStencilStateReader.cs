using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Graphics
{
#pragma warning disable 1591
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DepthStencilStateReader : ContentTypeReader<DepthStencilState>
    {
        protected override DepthStencilState Read(ContentReader input, DepthStencilState value)
        {
            if (value == null)
                value = new DepthStencilState();

            value.Name = input.ReadObject<string>();
            value.CounterClockwiseStencilDepthBufferFail = input.ReadObject<StencilOperation>();
            value.CounterClockwiseStencilFail = input.ReadObject<StencilOperation>();
            value.CounterClockwiseStencilFunction = input.ReadObject<CompareFunction>();
            value.CounterClockwiseStencilPass = input.ReadObject<StencilOperation>();
            value.DepthBufferEnable = input.ReadObject<bool>();
            value.DepthBufferFunction = input.ReadObject<CompareFunction>();
            value.DepthBufferWriteEnable = input.ReadObject<bool>();
            value.ReferenceStencil = input.ReadObject<int>();
            value.StencilDepthBufferFail = input.ReadObject<StencilOperation>();
            value.StencilEnable = input.ReadObject<bool>();
            value.StencilFail = input.ReadObject<StencilOperation>();
            value.StencilFunction = input.ReadObject<CompareFunction>();
            value.StencilMask = input.ReadObject<int>();
            value.StencilPass = input.ReadObject<StencilOperation>();
            value.StencilWriteMask = input.ReadObject<int>();
            value.TwoSidedStencilMode = input.ReadObject<bool>();

            return value;
        }
    }
#pragma warning restore 1591
}