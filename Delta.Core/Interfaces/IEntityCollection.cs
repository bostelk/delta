using System;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public interface IEntityCollection
    {
        bool NeedsToSort { get; set; }
        bool AlwaysSort { get; set; }

        void Add(IEntity item);
        void Remove(IEntity item);

        void Update(DeltaTime time);
        void Draw(DeltaTime time, SpriteBatch spriteBatch);
    }
}
