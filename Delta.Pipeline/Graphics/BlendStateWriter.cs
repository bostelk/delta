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
    public class BlendStateWriter : ContentTypeWriter<BlendState>
    {
        protected override void Write(ContentWriter output, BlendState value)
        {
            output.WriteObject<string>(value.Name);
            output.WriteObject<BlendFunction>(value.AlphaBlendFunction);
            output.WriteObject<Blend>(value.AlphaDestinationBlend);
            output.WriteObject<Blend>(value.AlphaSourceBlend);
            output.WriteObject<Color>(value.BlendFactor);
            output.WriteObject<BlendFunction>(value.ColorBlendFunction);
            output.WriteObject<Blend>(value.ColorDestinationBlend);
            output.WriteObject<Blend>(value.ColorSourceBlend);
            output.WriteObject<ColorWriteChannels>(value.ColorWriteChannels);
            output.WriteObject<ColorWriteChannels>(value.ColorWriteChannels1);
            output.WriteObject<ColorWriteChannels>(value.ColorWriteChannels2);
            output.WriteObject<ColorWriteChannels>(value.ColorWriteChannels3);
            output.WriteObject<int>(value.MultiSampleMask);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(BlendState).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(BlendStateReader).AssemblyQualifiedName;
        }
    }
#pragma warning restore 1591
}