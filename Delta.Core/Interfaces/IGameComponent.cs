using System;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public interface IGameComponent
    {
        IGameComponentCollection Collection { get; set; }
        float Layer { get; }

        void LoadContent();
        void Update(DeltaTime time);
        void Draw(DeltaTime time, SpriteBatch spriteBatch);
    }
}
