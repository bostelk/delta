using System;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public interface IGameComponentCollection
    {
        bool NeedsToSort { get; set; }
        bool AlwaysSort { get; set; }

        void Add(IGameComponent item);
        void Remove(IGameComponent item);

        void Update(DeltaTime time);
        void Draw(DeltaTime time, SpriteBatch spriteBatch);
    }
}
