using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{
    class PolyPolySolver : ICollisionSolver
    {

        public bool IsSolveable(CollisionBody colA, CollisionBody colB)
        {
            // we assume the calling function has checked for null shapes.
            if (colA.Shape is ConvexShape && colB.Shape is ConvexShape) return true; // catch all.
            else return false;
        }

        public CollisionResult SolveCollision(CollisionBody colA, CollisionBody colB)
        {
            // we want boxes and circles included too..
            ConvexShape polyA = (ConvexShape)colA.Shape;
            ConvexShape polyB = (ConvexShape)colB.Shape;

            float projectedDistance = 0;
            float minPenetration = float.MaxValue;
            Vector2 distance = colA.Position - colB.Position;
            Vector2 mtv = default(Vector2); // the minimum translation vector

            // merge normals from both polygons
            Vector2[] axisToCheck = new Vector2[polyA.Normals.Length + polyB.Normals.Length];
            for (int i = 0; i < polyA.Normals.Length; i++)
                axisToCheck[i] = polyA.Normals[i];
            for (int i = polyA.Normals.Length; i < axisToCheck.Length; i++)
                axisToCheck[i] = polyB.Normals[i - polyA.Normals.Length];

            // TODO: remove parallel normals

            for (int i = 0; i < axisToCheck.Length; i++)
            {
                float minA, maxA, minB, maxB;
                minA = maxA = minB = maxB = 0;

                projectedDistance = Math.Abs(Vector2.Dot(distance, axisToCheck[i]));
                polyA.ProjectOnto(ref axisToCheck[i], out minA, out maxA);
                polyB.ProjectOnto(ref axisToCheck[i], out minB, out maxB);
                float penetration = maxB - minA;

                // a seperating axis has been found; there is no collision.
                if (minA - maxB > 0f || minB - maxA > 0f)
                {
                    return CollisionResult.NoCollision;
                }
                // project the object along the axis with the smalled penetration depth.
                else if (Math.Abs(penetration) < Math.Abs(minPenetration))
                {
                    minPenetration = penetration;
                    mtv = axisToCheck[i];
                }
            }
            // the distance vector determines the direction of movement. the distance and mtv
            // should always oppose each other to repel collisions.
            if (Vector2.Dot(distance, mtv) < 0f)
                mtv = -mtv;
            // seperating axis could not be found; a collision occurs.
            return new CollisionResult()
            {
                Us = colA,
                Them = colB,
                CollisionResponse = minPenetration * mtv,
                IsColliding = true
            };
        }

    }
}
