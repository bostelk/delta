using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Physics.Geometry;

namespace Delta.Examples.Entities
{
    public class CollisionGeometry
    {
        public Polygon Geometry;
        public float Bounce;
        public float Friction;

        /// <summary>
        /// No collision response. ie. Stays stationary.
        /// </summary>
        public bool IsStatic;
    }
}
