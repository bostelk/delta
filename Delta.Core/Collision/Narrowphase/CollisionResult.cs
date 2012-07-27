using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{

    public struct CollisionResult
    {
        public CollisionBody Us;
        public CollisionBody Them;

        /// <summary>
        /// The shapes interesct each other.
        /// </summary>
        public bool IsColliding;

        /// <summary>
        /// The minimum translation vector to seperate the us from them.
        /// </summary>
        public Vector2 CollisionResponse;

        public static CollisionResult NoCollision = new CollisionResult() { IsColliding = false };
    }

}
