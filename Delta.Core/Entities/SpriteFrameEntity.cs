using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Delta.Graphics;

namespace Delta.Entities
{
    public class SpriteFrameEntity : Entity
    {
        string _spriteSheetName;
        string _animationName;
        int _frame;

        Texture2D _texture;
        Rectangle _sourceRectangle;

        public SpriteFrameEntity(SpriteEntity sprite)
        {
            _spriteSheetName = sprite._spriteSheetName;
            _animationName = sprite._animationName;
            _frame = sprite._animationFrame;
        }

        public SpriteFrameEntity(string spriteSheet, string animation, int frame)
        {
            _spriteSheetName = spriteSheet;
            _animationName = animation;
            _frame = frame;
        }

        public override void LoadContent()
        {
            SpriteSheet _spriteSheet = G.Content.Load<SpriteSheet>(_spriteSheetName);
            _texture = _spriteSheet.Texture;
            _sourceRectangle = _spriteSheet.GetFrameSourceRectangle(_spriteSheet.GetAnimation(_animationName).ImageName, _frame);
            Size = new Vector2(_sourceRectangle.Width, _sourceRectangle.Height);

            Origin = new Vector2(0.5f, 0.5f);
            base.LoadContent();
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null , Tint, Rotation, Origin, Scale, SpriteEffects.None, 0);
        }

        protected internal override void OnPositionChanged()
        {
            base.OnPositionChanged();
            Layer = Position.Y;
        }
    }
}
