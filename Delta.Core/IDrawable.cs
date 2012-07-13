using System;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public interface IDrawable : ILayerable
    {
        void Draw(DeltaTime time, SpriteBatch spriteBatch);
    }
}
