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
        Vector2[] _vertices;
        public Vector2[] Vertices
        {
            get
            {
                return _vertices;     
            }
            protected set
            {
                _vertices = value;
            }
        }

        public Vector2[] VerticesCopy
        {
            get
            {
                // System.Array is a reference type, the last thing we want is
                // someone modifying our vertices.
                Vector2[] verticesCopy = new Vector2[_vertices.Length];
                Array.Copy(_vertices, verticesCopy, _vertices.Length);
                return verticesCopy;
            }
        }

        /// <summary>
        /// In local space.
        /// </summary>
        public Vector2[] Normals { get; protected set; }

        public abstract void CalculateAABB(ref Matrix2D transform, out AABB aabb);

        protected void OnShapeChanged() { }

    }
}
