using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Delta.Collision
{
    public abstract class AbstractCollisionWorld
    {
        public abstract void Simulate(float timeStep);

        public abstract void AddCollider(Collider colider);

        public abstract void RemoveCollider(Collider colider);

        public abstract void DrawDebug(ref Matrix view, ref Matrix projection);
    }
}
