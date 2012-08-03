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
    public class RasterizerStateWriter : ContentTypeWriter<RasterizerState>
    {
        protected override void Write(ContentWriter output, RasterizerState value)
        {
            output.WriteObject<string>(value.Name);
            output.WriteObject<CullMode>(value.CullMode);
            output.WriteObject<float>(value.DepthBias);
            output.WriteObject<FillMode>(value.FillMode);
            output.WriteObject<bool>(value.MultiSampleAntiAlias);
            output.WriteObject<bool>(value.ScissorTestEnable);
            output.WriteObject<float>(value.SlopeScaleDepthBias);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(RasterizerState).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(RasterizerStateReader).AssemblyQualifiedName;
        }
    }
#pragma warning restore 1591
}