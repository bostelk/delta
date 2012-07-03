using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Physics.Geometry;
using Delta.Physics;

namespace Delta.Physics
{
    public class CollisionGeometry
    {
        public Entity Tag;

        public Polygon Geom;

        public float Bounce;

        public float Friction;

        public float Mass;

        /// <summary>
        /// No collision response. ie. Stays stationary.
        /// </summary>
        public bool IsStatic;

        /// <summary>
        /// An inflated bounding area to encompass the geom for fast detections.
        /// </summary>
        public AABB AABB { get; set; }

        /// <summary>
        /// The Geom is about to collide.
        /// </summary>
        public BeforeCollisionEventHandler BeforeCollision;

        /// <summary>
        /// The Geom is colliding.
        /// </summary>
        public OnCollisionEventHandler OnCollision;

        /// <summary>
        /// The Geom has been resolved since collision.
        /// </summary>
        public AfterCollisionEventHandler AfterCollision;

        public OnSeparationEventHandler OnSeperation;

        public CollisionGeometry() { }

        public CollisionGeometry(Polygon geom)
        {
            Geom = geom;
            AABB = geom.AABB;
        }
    }
}
