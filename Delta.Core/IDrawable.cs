using System;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public interface IDrawable
    {
        void InternalDraw(DeltaTime time, SpriteBatch spriteBatch);
    }
}
