using System;
using System.ComponentModel;
using Microsoft.Xna.Framework.Content;

namespace Delta.Graphics
{
    public class SpriteEntity : BaseSpriteEntity
    {
        string _externalImageName = string.Empty;
        /// <summary>
        /// Gets or sets the name of the <see cref="SpriteEntity"/>'s external image.
        /// </summary>
        [ContentSerializer, Description("The name of the external image to use."), Category("Sprite"), DefaultValue("")]
        public string ExternalImageName
        {
            get { return _externalImageName; }
            set
            {
                if (_externalImageName != value)
                {
                    _externalImageName = value;
                    UpdateSourceRectangle();
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public SpriteEntity()
            : base()
        {
        }

        public static SpriteEntity Create(AnimatedSpriteEntity sprite)
        {
            return Create(sprite.SpriteSheetName, sprite.Animation.ImageName, sprite.Frame);
        }

        public static SpriteEntity Create(string spriteSheet, string externalImage, int frame)
        {
            SpriteEntity spriteFrame = Pool.Fetch<SpriteEntity>();
            spriteFrame.SpriteSheetName = spriteSheet;
            spriteFrame.ExternalImageName = externalImage;
            spriteFrame.Frame = frame;
            return spriteFrame;
        }

        protected override void Recycle(bool isReleasing)
        {
            _externalImageName = string.Empty;
            base.Recycle(isReleasing);
        }

        /// <summary>
        /// Updates the source <see cref="Rectangle"/> used when drawing the <see cref="SpriteEntity"/>.
        /// </summary>
        protected override void UpdateSourceRectangle()
        {
            if (SpriteSheet == null)
                return;
            SourceRectangle = SpriteSheet.GetFrameSourceRectangle(_externalImageName, Frame);
        }
    }
}
