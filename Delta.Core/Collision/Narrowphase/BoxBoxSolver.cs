using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{
    class BoxBoxSolver : ICollisionSolver
    {

        public CollisionResult SolveCollision(Collider colA, Collider colB)
        {
            Box boxA = (Box)colA.Shape;
            Box boxB = (Box)colB.Shape;
            Matrix2D transformA = colA.WorldTransform;
            Matrix2D transformB = colB.WorldTransform;

            float projectedDistance = 0;
            float minPenetration = float.MaxValue;
            Vector2 distance = colA.Position - colB.Position;
            Vector2 mtv = default(Vector2); // the minimum translation vector

            // merge normals from both polygons
            // NOTE: For OBB's we only need to check their half widths. ie. 4 axis total.
            // For AABB's we only need to check 2 axis since they don't rotate.
            Vector2[] axisToCheck = new Vector2[4];
            boxA.CalculateOrientation(ref transformA, out axisToCheck[0]);
            boxB.CalculateOrientation(ref transformB, out axisToCheck[1]);
            axisToCheck[2] = Vector2Extensions.PerpendicularLeft(axisToCheck[0]);
            axisToCheck[3] = Vector2Extensions.PerpendicularLeft(axisToCheck[1]);

            // TODO: remove parallel normals

            for (int i = 0; i < axisToCheck.Length; i++)
            {
                Vector2 projectionA, projectionB;
                projectedDistance = Math.Abs(Vector2.Dot(distance, axisToCheck[i]));
                boxA.ProjectOnto(ref transformA, ref axisToCheck[i], out projectionA);
                boxB.ProjectOnto(ref transformB, ref axisToCheck[i], out projectionB);
                float aSize = Math.Abs(projectionA.X) + Math.Abs(projectionA.Y);
                float bSize = Math.Abs(projectionB.X) + Math.Abs(projectionB.Y);
                float abSize = aSize + bSize;
                float penetration = abSize - projectedDistance;

                // a seperating axis found; there is no collision.
                if (penetration <= 0)
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
