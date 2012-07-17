using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Delta.Examples.Entities
{
    public class TextureEntity : Entity
    {
        string _path;
        Texture2D _texture;

        public TextureEntity(string texturePath)
        {
            _path = texturePath;
        }
        public override void LoadContent()
        {
            _texture = G.Content.Load<Texture2D>(_path);
            base.LoadContent();
        }

        protected override void LightUpdate(DeltaTime time)
        {
            Layer = Position.Y;
            base.LightUpdate(time);
        }
        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null , Tint, Rotation, new Vector2(_texture.Width / 2, _texture.Height / 2), Scale, SpriteEffects.None, 0);
            base.Draw(time, spriteBatch);
        }
    }
}
