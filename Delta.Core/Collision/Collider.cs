using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Collision.Geometry;
using Delta.Collision;

namespace Delta.Collision
{
    public class Collider
    {
        public Entity Tag;

        public Polygon Geom;

        public float Bounce;

        public float Friction;

        public float Mass;

        /// <summary>
        /// Will not move due to collision. (No collision response).
        /// </summary>
        public bool IsStatic;

        /// <summary>
        /// A bounding area for fast detections.
        /// </summary>
        public AABB AABB { get; set; }

        /// <summary>
        /// The Geom is about to collide. (broad-phase intersection).
        /// </summary>
        public BeforeCollisionEventHandler BeforeCollision;

        /// <summary>
        /// The Geom is colliding. (narrow-phase intersection).
        /// </summary>
        public OnCollisionEventHandler OnCollision;

        /// <summary>
        /// The Geom has been resolved since collision.
        /// </summary>
        public AfterCollisionEventHandler AfterCollision;

        public OnSeparationEventHandler OnSeparation;

        public Collider() { }

        public Collider(Entity tag, Polygon geom)
        {
            Tag = tag;
            Geom = geom;
        }

        public Collider(Polygon geom)
        {
            Geom = geom;
            AABB = geom.AABB;
        }

    }
}
