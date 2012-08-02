using System;
using System.ComponentModel;
using Microsoft.Xna.Framework.Content;
using Delta.Structures;

namespace Delta.Graphics
{
    public class SpriteEntity : BaseSpriteEntity
    {
        static Pool<SpriteEntity> _pool = new Pool<SpriteEntity>(100);

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
        /// Gets or sets the name of the external image used by the <see cref="SpriteEntity"/>.
        /// </summary>
        [ContentSerializer, Description("The name of the external image used by the game object."), Category("Sprite"), DefaultValue("")]
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

        public SpriteEntity()
            : base()
        {
        }

        protected override void UpdateSourceRectangle()
        {
            if (SpriteSheet == null)
                return;
            SourceRectangle = SpriteSheet.GetFrameSourceRectangle(_externalImageName, Frame);
        }

    }
}
