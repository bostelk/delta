using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Collision.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Delta.Collision
{
    public abstract class CollisionEngine
    {
        public abstract void Simulate(float seconds);

        public abstract void DefineWorld(int width, int height, int size);

        public abstract void AddCollider(Collider colider);

        public abstract void RemoveColider(Collider colider);

        public abstract List<Polygon> Raycast(Vector2 start, Vector2 end, bool returnFirst);

        public abstract List<Polygon> InArea(Rectangle area);

        public abstract void DrawDebug(ref Matrix view, ref Matrix projection);
    }
}
