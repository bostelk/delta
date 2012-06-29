using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Graphics
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SpriteSheetReader : ContentTypeReader<SpriteSheet>
    {
        protected override SpriteSheet Read(ContentReader input, SpriteSheet value)
        {
            if (value == null)
                value = new SpriteSheet();
            value._animations.AddRange(input.ReadObject<List<Animation>>());
            for (int x = 0; x < value.Animations.Count; x++)
                value._animationReferences.Add(value.Animations[x].Name, x);
            value._imageFrameReferences = input.ReadObject<Dictionary<string, Dictionary<int, Rectangle>>>();
            value.Texture = input.ReadObject<Texture2D>();
            return value;
        }
    }
}
