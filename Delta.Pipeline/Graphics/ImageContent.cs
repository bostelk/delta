using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace Delta.Graphics
{

    public class ImageContent
    {
        internal static ImageContent Current { get; set; }

        internal BitmapContent _bitmapContent = null;
        internal Dictionary<int, SpriteBlockContent> _frameReferences = new Dictionary<int, SpriteBlockContent>();

        [ContentSerializer]
        public string Path { get; internal set; }
        [ContentSerializer]
        public int FrameWidth { get; internal set; }
        [ContentSerializer]
        public int FrameHeight { get; internal set; }
        [ContentSerializer(FlattenContent = true, CollectionItemName = "Animation")]
        public List<Animation> Animations { get; set; }

        public ImageContent()
            : base()
        {
            Current = this;
            Animations = new List<Animation>();
        }

    }
}
