using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Graphics
{
    public abstract class Emitter : Entity
    {
        internal class Particle<T> : IRecyclable where T: Entity
        {
            public T Entity;
            public float Lifespan;
            public Vector2 Velocity;
            public float AngularVelocity;
            public bool IsDead { get { return Lifespan <= 0; } }

            public virtual void Recycle()
            {
                Entity.Recycle();
                Entity = null;
                Lifespan = 0;
                Velocity = Vector2.Zero;
                AngularVelocity = 0;
            }

            public virtual void OnEmitted()
            {
            }

        }
    }
}
