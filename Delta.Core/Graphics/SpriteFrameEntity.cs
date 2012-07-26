using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Delta.Graphics;
using Delta.Structures;

namespace Delta.Entities
{
    public class SpriteFrameEntity : TransformableEntity
    {
        static Pool<SpriteFrameEntity> _pool;

        string _spriteSheetName;
        string _animationName;
        int _frame;

        Texture2D _texture;
        Rectangle _sourceRectangle;

        static SpriteFrameEntity()
        {
            _pool = new Pool<SpriteFrameEntity>(100);
        }

        public static SpriteFrameEntity Create(SpriteEntity sprite)
        {
            return Create(sprite._spriteSheetName, sprite._animationName, sprite._animationFrame);
        }

        public static SpriteFrameEntity Create(string spriteSheet, string animation, int frame)
        {
            SpriteFrameEntity spriteFrame = _pool.Fetch();
            spriteFrame._spriteSheetName = spriteSheet;
            spriteFrame._animationName = animation;
            spriteFrame._frame = frame;
            return spriteFrame;
        }

        public SpriteFrameEntity() { }

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

        protected override void LoadContent()
        {
            SpriteSheet _spriteSheet = G.Content.Load<SpriteSheet>(_spriteSheetName);
            _texture = _spriteSheet.Texture;
            _sourceRectangle = _spriteSheet.GetFrameSourceRectangle(_spriteSheet.GetAnimation(_animationName).ImageName, _frame);
            Size = new Vector2(_sourceRectangle.Width, _sourceRectangle.Height);

            Origin = new Vector2(0.5f, 0.5f);
            base.LoadContent();
        }

        protected override void LightUpdate(DeltaGameTime time)
        {
            Depth = Position.Y;
            base.LightUpdate(time);
        }

        protected override void Draw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, _sourceRectangle , Tint, Rotation, Origin, Scale, SpriteEffects.None, 0);
            base.Draw(time, spriteBatch);
        }

        public override void Recycle()
        {
            base.Recycle();
            _spriteSheetName = String.Empty;
            _animationName = String.Empty;
            _frame = 0;
            _texture = null;
            _sourceRectangle = Rectangle.Empty;

            _pool.Release(this);
        }
    }
}
