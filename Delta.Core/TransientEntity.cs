using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Graphics;

namespace Delta
{
    public class TransientEntity : Entity
    {
        public Entity _entity;
        float _duration;
        float _elapsed;

        public TransientEntity(Entity e, float duration)
        {
            _entity = e;
            _duration = duration;
            _elapsed = 0f;
            G.World.Add(_entity);
        }

        protected override void LightUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_elapsed > _duration)
            {
                G.World.Remove(this);
                G.World.Remove(_entity);
            }
            _elapsed += (float) gameTime.ElapsedGameTime.TotalSeconds;
            base.LightUpdate(gameTime);
        }

    }
}
