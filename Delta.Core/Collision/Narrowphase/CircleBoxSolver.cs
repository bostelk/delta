using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{
    class CircleBoxSolver : ICollisionSolver
    {
 
        public CollisionResult SolveCollision(Collider colA, Collider colB)
        {
            Circle circleA = (Circle)colA.Shape;
            Box boxB = (Box)colB.Shape;
            Matrix3 transformB = colB.WorldTransform;

            float projectedDistance = 0;
            float minPenetration = float.MaxValue;
            Vector2 distance = colA.Position - colB.Position;
            Vector2 mtv = default(Vector2); // the minimum translation vector

            Vector2[] axisToCheck = new Vector2[3];
            boxB.CalculateOrientation(ref transformB, out axisToCheck[0]);
            axisToCheck[1] = Vector2Extensions.PerpendicularLeft(axisToCheck[0]);
            float minDistance = float.MaxValue;
            Vector2 closestVertex = default(Vector2);
            for (int i = 0; i < boxB.Vertices.Length; i++)
            {
                float vertexDistance = Vector2.DistanceSquared(colA.Position, boxB.Vertices[i]);
                if (vertexDistance < minDistance)
                {
                    minDistance = vertexDistance;
                    closestVertex = colB.Position - boxB.Vertices[i];
                }
            }
            axisToCheck[2] = closestVertex;
            Vector2Extensions.SafeNormalize(ref axisToCheck[2]);

            for (int i = 0; i < axisToCheck.Length; i++)
            {
                Vector2 projectionA, projectionB;

                projectedDistance = Math.Abs(Vector2.Dot(distance, axisToCheck[i]));
                circleA.ProjectOnto(ref axisToCheck[i], out projectionA);
                boxB.ProjectOnto(ref transformB, ref axisToCheck[i], out projectionB);

                float aSize = projectionA.Length(); //Math.Abs(projectionA.X) + Math.Abs(projectionA.Y);
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
