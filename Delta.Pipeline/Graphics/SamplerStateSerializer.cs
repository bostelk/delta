using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace Delta.Graphics
{
#pragma warning disable 1591
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ContentTypeSerializer]
    public class SamplerStateSerializer : ContentTypeSerializer<SamplerState>
    {
        protected override SamplerState Deserialize(IntermediateReader input, ContentSerializerAttribute format, SamplerState existingInstance)
        {
            switch (input.Xml.ReadContentAsString().ToLower())
            {
                case "anisotropicclamp":
                    return SamplerState.AnisotropicClamp;
                case "anisotropicwrap":
                    return SamplerState.AnisotropicWrap;
                case "linearclamp":
                    return SamplerState.LinearClamp;
                case "linearwrap":
                    return SamplerState.LinearWrap;
                case "pointclamp":
                    return SamplerState.PointClamp;
                case "pointwrap":
                    return SamplerState.PointWrap;
                default:
                    return new SamplerState();
            }
        }

        protected override void Serialize(IntermediateWriter output, SamplerState value, ContentSerializerAttribute format)
        {
            if (SamplerStateEquals(value, SamplerState.AnisotropicClamp))
                output.Xml.WriteString("AnisotropicClamp");
            else if (SamplerStateEquals(value, SamplerState.AnisotropicWrap))
                output.Xml.WriteString("AnisotropicWrap");
            else if (SamplerStateEquals(value, SamplerState.LinearClamp))
                output.Xml.WriteString("LinearClamp");
            else if (SamplerStateEquals(value, SamplerState.LinearWrap))
                output.Xml.WriteString("LinearWrap");
            else if (SamplerStateEquals(value, SamplerState.PointClamp))
                output.Xml.WriteString("PointClamp");
            else if (SamplerStateEquals(value, SamplerState.PointWrap))
                output.Xml.WriteString("PointWrap");
            else
                throw new InvalidContentException("Unknown SamplerState: " + value);
        }

        static bool SamplerStateEquals(SamplerState a, SamplerState b)
        {
            return a.AddressU == b.AddressU &&
                   a.AddressV == b.AddressV &&
                   a.AddressW == b.AddressW &&
                   a.Filter == b.Filter &&
                   a.MaxAnisotropy == b.MaxAnisotropy &&
                   a.MaxMipLevel == b.MaxMipLevel &&
                   a.MipMapLevelOfDetailBias == b.MipMapLevelOfDetailBias;
        }
    }
#pragma warning restore 1591
}
