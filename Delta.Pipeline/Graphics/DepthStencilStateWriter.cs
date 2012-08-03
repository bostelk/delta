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
    public class DepthStencilStateWriter : ContentTypeWriter<DepthStencilState>
    {
        protected override void Write(ContentWriter output, DepthStencilState value)
        {
            output.WriteObject<string>(value.Name);
            output.WriteObject<StencilOperation>(value.CounterClockwiseStencilDepthBufferFail);
            output.WriteObject<StencilOperation>(value.CounterClockwiseStencilFail);
            output.WriteObject<CompareFunction>(value.CounterClockwiseStencilFunction);
            output.WriteObject<StencilOperation>(value.CounterClockwiseStencilPass);
            output.WriteObject<bool>(value.DepthBufferEnable);
            output.WriteObject<CompareFunction>(value.DepthBufferFunction);
            output.WriteObject<bool>(value.DepthBufferWriteEnable);
            output.WriteObject<int>(value.ReferenceStencil);
            output.WriteObject<StencilOperation>(value.StencilDepthBufferFail);
            output.WriteObject<bool>(value.StencilEnable);
            output.WriteObject<StencilOperation>(value.StencilFail);
            output.WriteObject<CompareFunction>(value.StencilFunction);
            output.WriteObject<int>(value.StencilMask);
            output.WriteObject<StencilOperation>(value.StencilPass);
            output.WriteObject<int>(value.StencilWriteMask);
            output.WriteObject<bool>(value.TwoSidedStencilMode);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(DepthStencilState).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(DepthStencilStateReader).AssemblyQualifiedName;
        }
    }
#pragma warning restore 1591
}