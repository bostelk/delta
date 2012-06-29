using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace Delta.Graphics
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ContentTypeWriter]
    public class SpriteSheetContentWriter : ContentTypeWriter<SpriteSheetContent>
    {
        protected override void Write(ContentWriter output, SpriteSheetContent value)
        {
            List<Animation> animations = new List<Animation>();
            Dictionary<string, Dictionary<int, Rectangle>> imageFrameReferences = new Dictionary<string, Dictionary<int, Rectangle>>();
            for (int x = 0; x < value.Images.Count; x++)
            {
                ImageContent image = value.Images[x];
                animations.AddRange(image.Animations);
                Dictionary<int, Rectangle> frameReferences = new Dictionary<int, Rectangle>();
                foreach (var frameReference in image._frameReferences)
                    frameReferences.Add(frameReference.Key, frameReference.Value.DestinationRegion);
                imageFrameReferences.Add(Path.GetFileName(image.Path), frameReferences);
            }
            output.WriteObject<List<Animation>>(animations);
            output.WriteObject<Dictionary<string, Dictionary<int, Rectangle>>>(imageFrameReferences);
            output.WriteObject<Texture2DContent>(value._texture);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        { 
            return typeof(SpriteSheet).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SpriteSheetReader).AssemblyQualifiedName;
        }
    }
}
