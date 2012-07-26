using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{
    class BoxBoxSolver : ICollisionSolver
    {

        Vector2[] _axisToCheck = new Vector2[4];

        public BoxBoxSolver()
            : base()
        {
        }

        public CollisionResult SolveCollision(Collider colA, Collider colB)
        {
            Box boxA = colA.Shape as Box;
            Box boxB = colB.Shape as Box;
            Matrix2D transformA = colA.WorldTransform;
            Matrix2D transformB = colB.WorldTransform;

            float projectedDistance = 0;
            float minPenetration = float.MaxValue;
            Vector2 distance = colA.Position - colB.Position;
            Vector2 mtv = default(Vector2); // the minimum translation vector

            // merge normals from both polygons
            // NOTE: For OBB's we only need to check their half widths. ie. 4 axis total.
            // For AABB's we only need to check 2 axis since they don't rotate.
            boxA.CalculateOrientation(ref transformA, out _axisToCheck[0]);
            boxB.CalculateOrientation(ref transformB, out _axisToCheck[1]);
            _axisToCheck[2] = Vector2Extensions.PerpendicularLeft(_axisToCheck[0]);
            _axisToCheck[3] = Vector2Extensions.PerpendicularLeft(_axisToCheck[1]);

            // TODO: remove parallel normals

            for (int i = 0; i < _axisToCheck.Length; i++)
            {
                Vector2 projectionA, projectionB;
                projectedDistance = Math.Abs(Vector2.Dot(distance, _axisToCheck[i]));
                boxA.ProjectOnto(ref transformA, ref _axisToCheck[i], out projectionA);
                boxB.ProjectOnto(ref transformB, ref _axisToCheck[i], out projectionB);
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
                    mtv = _axisToCheck[i];
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
