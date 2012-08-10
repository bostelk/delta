using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Delta.Graphics
{
    [ContentTypeWriter, EditorBrowsable(EditorBrowsableState.Never)]
    public class AnimatedSpriteEntityWriter : ContentTypeWriter<AnimatedSpriteEntity>
    {
        protected override void Write(ContentWriter output, AnimatedSpriteEntity existingInstance)
        {
            output.WriteRawObject<BaseSpriteEntity>(existingInstance as BaseSpriteEntity);
            output.Write(existingInstance.AnimationName);
            output.WriteObject<AnimationOptions>(existingInstance.AnimationOptions);
            output.Write(existingInstance.FrameOffset);
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
