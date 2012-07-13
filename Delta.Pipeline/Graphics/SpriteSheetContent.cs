using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace Delta.Graphics
{
    public class SpriteSheetContent
    {
        public static Dictionary<string, SpriteSheetContent> _spriteSheetImages = new Dictionary<string, SpriteSheetContent>();

        internal Texture2DContent _texture = null;
        internal List<SpriteBlockContent> _blocks = new List<SpriteBlockContent>();
        internal Dictionary<string, Dictionary<int, Rectangle>> _imageFrameSourceRectangles = new Dictionary<string, Dictionary<int, Rectangle>>();
        internal string _inputPath = string.Empty;

        [ContentSerializer(FlattenContent = true, CollectionItemName = "Image")]
        public List<ImageContent> Images { get; set; }

        public SpriteSheetContent()
            : base()
        {
            Images = new List<ImageContent>();
        }
    }

}


