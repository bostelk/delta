using System;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public interface IDrawable : ILayerable
    {
        void InternalDraw(DeltaTime time, SpriteBatch spriteBatch);
    }
}
