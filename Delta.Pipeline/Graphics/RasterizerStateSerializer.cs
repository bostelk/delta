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
    public class RasterizerStateSerializer : ContentTypeSerializer<RasterizerState>
    {
        protected override RasterizerState Deserialize(IntermediateReader input, ContentSerializerAttribute format, RasterizerState existingInstance)
        {
            switch (input.Xml.ReadContentAsString().ToLower())
            {
                case "cullclockwise":
                    return RasterizerState.CullClockwise;
                case "cullcounterclockwise":
                    return RasterizerState.CullCounterClockwise;
                case "cullnone":
                    return RasterizerState.CullNone;
                default:
                    return new RasterizerState();
            }
        }

        protected override void Serialize(IntermediateWriter output, RasterizerState value, ContentSerializerAttribute format)
        {
            if (RasterizerStateEquals(value, RasterizerState.CullClockwise))
                output.Xml.WriteString("CullClockwise");
            else if (RasterizerStateEquals(value, RasterizerState.CullCounterClockwise))
                output.Xml.WriteString("CullCounterClockwise");
            else if (RasterizerStateEquals(value, RasterizerState.CullNone))
                output.Xml.WriteString("CullNone");
            else
                throw new InvalidContentException("Unknown RasterizerState: " + value);
        }

        static bool RasterizerStateEquals(RasterizerState a, RasterizerState b)
        {
            return a.CullMode == b.CullMode &&
                   a.DepthBias == b.DepthBias &&
                   a.FillMode == b.FillMode &&
                   a.MultiSampleAntiAlias == b.MultiSampleAntiAlias &&
                   a.ScissorTestEnable == b.ScissorTestEnable &&
                   a.SlopeScaleDepthBias == b.SlopeScaleDepthBias;
        }
    }
#pragma warning restore 1591
}
