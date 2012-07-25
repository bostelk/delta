using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Delta;
using Delta.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Delta.Examples.Entities
{
    public class Image : TransformableEntity
    {
        Texture2D _texture;
        string _filepath;

        public Image(string filepath)
        {
            _filepath = filepath;
        }

        protected override void LoadContent()
        {
            _texture = G.Content.Load<Texture2D>(_filepath);

            base.LoadContent();
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            Vector2 offset = new Vector2(_texture.Width / 2, _texture.Height / 2);
            spriteBatch.Draw(_texture, Position - offset, Color.White);
            base.Draw(time, spriteBatch);
        }

    }
}
