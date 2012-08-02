using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace Monoceros.Graphics
{
#pragma warning disable 1591
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ContentTypeSerializer]
    public class DepthStenctilStateSerializer : ContentTypeSerializer<DepthStencilState>
    {
        protected override DepthStencilState Deserialize(IntermediateReader input, ContentSerializerAttribute format, DepthStencilState existingInstance)
        {
            switch (input.Xml.ReadContentAsString().ToLower())
            {
                case "default":
                    return DepthStencilState.Default;
                case "depthread":
                    return DepthStencilState.DepthRead;
                case "none":
                    return DepthStencilState.None;
                default:
                    return new DepthStencilState();
            }
        }

        protected override void Serialize(IntermediateWriter output, DepthStencilState value, ContentSerializerAttribute format)
        {
            if (DepthStencilStateEquals(value, DepthStencilState.Default))
                output.Xml.WriteString("Default");
            else if (DepthStencilStateEquals(value, DepthStencilState.DepthRead))
                output.Xml.WriteString("DepthRead");
            else if (DepthStencilStateEquals(value, DepthStencilState.None))
                output.Xml.WriteString("None");
            else
                throw new InvalidContentException("Unknown DepthStencilState: " + value);
        }

        static bool DepthStencilStateEquals(DepthStencilState a, DepthStencilState b)
        {
            return a.CounterClockwiseStencilDepthBufferFail == b.CounterClockwiseStencilDepthBufferFail &&
                   a.CounterClockwiseStencilFail == b.CounterClockwiseStencilFail &&
                   a.CounterClockwiseStencilFunction == b.CounterClockwiseStencilFunction &&
                   a.CounterClockwiseStencilPass == b.CounterClockwiseStencilPass &&
                   a.DepthBufferEnable == b.DepthBufferEnable &&
                   a.DepthBufferFunction == b.DepthBufferFunction &&
                   a.DepthBufferWriteEnable == b.DepthBufferWriteEnable &&
                   a.ReferenceStencil == b.ReferenceStencil &&
                   a.StencilDepthBufferFail == b.StencilDepthBufferFail &&
                   a.StencilEnable == b.StencilEnable &&
                   a.StencilFail == b.StencilFail &&
                   a.StencilFunction == b.StencilFunction &&
                   a.StencilMask == b.StencilMask &&
                   a.StencilPass == b.StencilPass &&
                   a.StencilWriteMask == b.StencilWriteMask &&
                   a.TwoSidedStencilMode == b.TwoSidedStencilMode;
        }
    }
#pragma warning restore 1591
}
