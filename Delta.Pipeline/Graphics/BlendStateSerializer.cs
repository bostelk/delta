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
    public class BlendStateSerializer : ContentTypeSerializer<BlendState>
    {
        protected override BlendState Deserialize(IntermediateReader input, ContentSerializerAttribute format, BlendState existingInstance)
        {
            switch (input.Xml.ReadContentAsString().ToLower())
            {
                case "additive":
                    return BlendState.Additive;
                case "alphablend":
                    return BlendState.AlphaBlend;
                case "nonpremultiplied":
                    return BlendState.NonPremultiplied;
                case "opaque":
                    return BlendState.Opaque;
                default:
                    return new BlendState();
            }
        }

        protected override void Serialize(IntermediateWriter output, BlendState value, ContentSerializerAttribute format)
        {
            if (BlendStateEquals(value, BlendState.Additive))
                output.Xml.WriteString("Additive");
            else if (BlendStateEquals(value, BlendState.AlphaBlend))
                output.Xml.WriteString("AlphaBlend");
            else if (BlendStateEquals(value, BlendState.NonPremultiplied))
                output.Xml.WriteString("NonPremultiplied");
            else if (BlendStateEquals(value, BlendState.Opaque))
                output.Xml.WriteString("Opaque");
            else
                throw new InvalidContentException("Unknown BlendState: " + value);
        }

        static bool BlendStateEquals(BlendState a, BlendState b)
        {
            return a.AlphaBlendFunction == b.AlphaBlendFunction &&
                   a.AlphaDestinationBlend == b.AlphaDestinationBlend &&
                   a.AlphaSourceBlend == b.AlphaSourceBlend &&
                   a.BlendFactor == b.BlendFactor &&
                   a.ColorBlendFunction == b.ColorBlendFunction &&
                   a.ColorDestinationBlend == b.ColorDestinationBlend &&
                   a.ColorSourceBlend == b.ColorSourceBlend &&
                   a.ColorWriteChannels == b.ColorWriteChannels &&
                   a.ColorWriteChannels1 == b.ColorWriteChannels1 &&
                   a.ColorWriteChannels2 == b.ColorWriteChannels2 &&
                   a.ColorWriteChannels3 == b.ColorWriteChannels3 &&
                   a.MultiSampleMask == b.MultiSampleMask;
        }
    }
#pragma warning restore 1591
}
