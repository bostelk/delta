using System;
using System.ComponentModel;
using Microsoft.Xna.Framework.Content;
using Delta.Structures;

namespace Delta.Graphics
{
    public class SpriteEntity : BaseSpriteEntity
    {
        static Pool<SpriteEntity> _pool = new Pool<SpriteEntity>(100);

        public static SpriteEntity Create(AnimatedSpriteEntity sprite)
        {
            return Create(sprite.SpriteSheetName, sprite.Animation.ImageName, sprite.Frame);
        }

        public static SpriteEntity Create(string spriteSheet, string externalImage, int frame)
        {
            SpriteEntity spriteFrame = _pool.Fetch();
            spriteFrame.SpriteSheetName = spriteSheet;
            spriteFrame.ExternalImageName = externalImage;
            spriteFrame.Frame = frame;
            return spriteFrame;
        }

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

        /// <summary>
        /// Updates the source <see cref="Rectangle"/> used when drawing the <see cref="SpriteEntity"/>.
        /// </summary>
        protected override void UpdateSourceRectangle()
        {
            if (SpriteSheet == null)
                return;
            SourceRectangle = SpriteSheet.GetFrameSourceRectangle(_externalImageName, Frame);
        }

        /// <summary>
        /// Recycles the <see cref="BaseSpriteEntity"/> so it may be re-used.
        /// </summary>
        public override void Recycle()
        {
            base.Recycle();
            _externalImageName = string.Empty;
            _pool.Release(this);
        }
    }
}
