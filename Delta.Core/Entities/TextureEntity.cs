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
    public class TextureEntity : Entity
    {
        static Pool<TextureEntity> _pool;

        string _path;
        Texture2D _texture;

        static TextureEntity()
        {
            _pool = new Pool<TextureEntity>(100);
        }

        public static TextureEntity Create(string texturePath)
        {
            TextureEntity texture = Create();
            texture._path = texturePath;
            return texture;
        }

        public static TextureEntity Create()
        {
            TextureEntity texture = _pool.Fetch();
            return texture;
        }

        public TextureEntity()
        {
        }

        public TextureEntity(string texturePath)
        {
            _path = texturePath;
        }

        public override void LoadContent()
        {
            if (String.IsNullOrEmpty(_path))
            {
                _texture = G.PixelTexture;
                Size = Vector2.One;
            }
            else
            {
                _texture = G.Content.Load<Texture2D>(_path);
                Size = new Vector2(_texture.Width, _texture.Height);
            }

            Origin = new Vector2(0.5f, 0.5f);
            base.LoadContent();
        }

        protected override void LightUpdate(DeltaTime time)
        {
            Layer = Position.Y;
            base.LightUpdate(time);
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null , Tint, Rotation, Origin, Scale, SpriteEffects.None, 0);
            base.Draw(time, spriteBatch);
        }

        public override void Recycle()
        {
            base.Recycle();
            _path = String.Empty;
            _texture = null;

            _pool.Release(this);
        }
    }
}
