using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{
    public abstract class CollisionShape
    {

        /// <summary>
        /// In local space.
        /// </summary>
        public Vector2[] Vertices { get; protected set; }

        /// <summary>
        /// In local space.
        /// </summary>
        public Vector2[] Normals { get; protected set; }

        public abstract void CalculateAABB(ref Transform transform, out AABB aabb);

        protected void OnShapeChanged() { }

    }
}
