using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Delta.Graphics
{
    [ContentTypeWriter, EditorBrowsable(EditorBrowsableState.Never)]
    public class SpriteEntityWriter : ContentTypeWriter<SpriteEntity>
    {
        protected override void Write(ContentWriter output, SpriteEntity existingInstance)
        {
            output.WriteRawObject<BaseSpriteEntity>(existingInstance as BaseSpriteEntity);
            output.Write(existingInstance.ExternalImageName);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(SpriteEntity).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SpriteEntity).AssemblyQualifiedName;
        }
    }
}
