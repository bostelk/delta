using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Graphics
{

    public class SpriteSheet
    {
        //ContentSerializer attributes are used XML only here, for binary reading/writing see SpriteSheetReader/SpriteSheetWriter.
        [ContentSerializer(FlattenContent=true, CollectionItemName="Animation")]
        internal List<Animation> _animations = new List<Animation>();
        internal Dictionary<string, int> _animationReferences = new Dictionary<string, int>();
        internal Dictionary<string, Dictionary<int, Rectangle>> _imageFrameReferences = new Dictionary<string, Dictionary<int, Rectangle>>();

        [ContentSerializerIgnore]
        public ReadOnlyCollection<Animation> Animations { get; private set; }
        [ContentSerializerIgnore]
        public Texture2D Texture { get; set; }

        public SpriteSheet()
            : base()
        {
            Animations = new ReadOnlyCollection<Animation>(_animations);
        }

        public Animation GetAnimation(string animationName)
        {
            if (string.IsNullOrEmpty(animationName))
                return null;
            if (!_animationReferences.ContainsKey(animationName))
                return null;
            return Animations[_animationReferences[animationName]];
        }

        public Rectangle GetFrameSourceRectangle(string externalImageName, int imageFrame)
        {
            if (string.IsNullOrEmpty(externalImageName))
                return Rectangle.Empty;
            if (!_imageFrameReferences.ContainsKey(externalImageName))
                return Rectangle.Empty;
            if (!_imageFrameReferences[externalImageName].ContainsKey(imageFrame))
                return Rectangle.Empty;
            return _imageFrameReferences[externalImageName][imageFrame];
        }

    }

}
