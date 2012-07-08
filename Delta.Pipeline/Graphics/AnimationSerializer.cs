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
    public class AnimationSerializer : ContentTypeSerializer<Animation>
    {
        protected override void Serialize(IntermediateWriter output, Animation value, ContentSerializerAttribute format)
        {
            throw new NotImplementedException("Animation XML serializer.");
        }

        protected override Animation Deserialize(IntermediateReader input, ContentSerializerAttribute format, Animation value)
        {
            if (value == null)
                value = new Animation(
                    input.ReadObject<string>(new ContentSerializerAttribute() { ElementName = "Name" }),
                    input.ReadObject<string>(new ContentSerializerAttribute() { ElementName = "Frames" }),
                    input.ReadObject<float>(new ContentSerializerAttribute() { ElementName = "FrameDuration" })
                    );
            value.ImageName = Path.GetFileNameWithoutExtension(ImageContent.Current.Path);
            return value;
        }
    }
#pragma warning restore 1591
}
