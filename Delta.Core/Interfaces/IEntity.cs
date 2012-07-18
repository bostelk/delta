using System;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public interface IEntity
    {
        string Name { get; set; }
        float Layer { get; }
        IEntityCollection Collection { get; set; }

        void LoadContent();
        void Update(DeltaTime time);
        void Draw(DeltaTime time, SpriteBatch spriteBatch);
        void OnAdded();
        void OnRemoved();
    }
}
