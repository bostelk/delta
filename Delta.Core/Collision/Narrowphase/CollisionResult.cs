using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{

    public struct CollisionResult
    {
        public Collider Us;
        public Collider Them;

        /// <summary>
        /// The shapes interesct each other.
        /// </summary>
        public bool IsColliding;

        /// <summary>
        /// The minimum translation vector to seperate the two shapes.
        /// </summary>
        public Vector2 CollisionResponse;

        public static CollisionResult NoCollision = new CollisionResult() { IsColliding = false };
    }

}
