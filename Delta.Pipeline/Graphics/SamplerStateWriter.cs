using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Delta.Graphics;

namespace Delta.Graphics
{
#pragma warning disable 1591
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ContentTypeWriter]
    public class SamplerStateWriter : ContentTypeWriter<SamplerState>
    {
        protected override void Write(ContentWriter output, SamplerState value)
        {
            output.WriteObject<string>(value.Name);
            output.WriteObject<TextureAddressMode>(value.AddressU);
            output.WriteObject<TextureAddressMode>(value.AddressV);
            output.WriteObject<TextureAddressMode>(value.AddressW);
            output.WriteObject<TextureFilter>(value.Filter);
            output.WriteObject<int>(value.MaxAnisotropy);
            output.WriteObject<int>(value.MaxMipLevel);
            output.WriteObject<float>(value.MipMapLevelOfDetailBias);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(SamplerState).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SamplerStateReader).AssemblyQualifiedName;
        }
    }
#pragma warning restore 1591
}
