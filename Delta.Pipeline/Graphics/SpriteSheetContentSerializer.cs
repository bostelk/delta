using System;
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
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
    public class SpriteSheetContentSerializer : ContentTypeSerializer<SpriteSheetContent>
    {
        protected override void Serialize(IntermediateWriter output, SpriteSheetContent value, ContentSerializerAttribute format)
        {
            throw new NotImplementedException("SpriteSheetContent XML serializer.");
        }

        protected override SpriteSheetContent Deserialize(IntermediateReader input, ContentSerializerAttribute format, SpriteSheetContent value)
        {
            if (value == null)
                value = new SpriteSheetContent();
            value.Images = input.ReadObject<List<ImageContent>>(new ContentSerializerAttribute() { FlattenContent = true, CollectionItemName = "Image" });
            foreach (var image in value.Images)
            {
                if (!SpriteSheetContent._spriteSheetImages.ContainsKey(image.Path))
                    SpriteSheetContent._spriteSheetImages.Add(image.Path, value);
            }
            return value;
        }
    }
#pragma warning restore 1591
}
