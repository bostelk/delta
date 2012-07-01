using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public interface IEntity
    {
        string ID { get; set; }
        bool IsVisible { get; set; }
        IEntityParent Parent { get; set; }
        float Order { get; set; }

        void LoadContent();

        void InternalUpdate(GameTime gameTime);
        void InternalDraw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
