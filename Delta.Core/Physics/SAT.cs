using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Physics.Geometry;
using Microsoft.Xna.Framework;

namespace Delta.Physics
{

    public struct CollisionResult 
    {
        public Polygon Us;
        public Polygon Them;
        public Vector2 CollisionResponse;
        public bool IsColliding;
    }
    
    /// <summary>
    /// SAT, or Seperating Axis Theorem provides narrow phase collision detection. Ie. It checks if polygons
    /// are intersecting each other (colliding) and what the proper response is to resolve the collision.
    /// </summary>
	public static class SAT
	{
        public static CollisionResult NoCollision = new CollisionResult() { IsColliding = false };

        public static CollisionResult BoxBox(Box a, Box b)
        {
            float projectedDistance = 0;
            float minPenetration = float.MaxValue;
            Vector2 distance = a.Position - b.Position;
            Vector2 mtv = default(Vector2); // the minimum translation vector

            // merge normals from both polygons
            Vector2[] axisToCheck = new Vector2[a.Normals.Length + b.Normals.Length];
            for (int i = 0; i < a.Normals.Length; i++)
                axisToCheck[i] = a.Normals[i];
            for (int i = a.Normals.Length; i < axisToCheck.Length; i++)
                axisToCheck[i] = b.Normals[i - a.Normals.Length];

            // TODO: remove parallel normals

            for (int i = 0; i < axisToCheck.Length; i++)
            {
                if (a is Box && b is Box)
                {
                    Box boxA = a as Box;
                    Box boxB = b as Box;
                    Vector2 projectionA, projectionB;

                    projectedDistance = Math.Abs(Vector2.Dot(distance, axisToCheck[i]));
                    boxA.ProjectOntoAxis(axisToCheck[i], out projectionA);
                    boxB.ProjectOntoAxis(axisToCheck[i], out projectionB);

                    float aSize = Math.Abs(projectionA.X) + Math.Abs(projectionA.Y);
                    float bSize = Math.Abs(projectionB.X) + Math.Abs(projectionB.Y);
                    float abSize = aSize + bSize;
                    float penetration = abSize - projectedDistance;

                    // there is no intersection, a seperating axis found; the boxes do not overlap.
                    if (penetration <= 0)
                    {
                        return NoCollision;
                    }
                    // project the object along the axis with the smalled penetration depth.
                    else if (Math.Abs(penetration) < Math.Abs(minPenetration))
                    {
                        minPenetration = penetration;
                        mtv = axisToCheck[i];

                        // this is fucked!!!
                        if (Vector2.Dot(distance, mtv) < 0f)
                        {
                            mtv = -mtv;
                        }
                    }
                }
            }

           // seperating axis could not be found; the boxes overlap.
            return new CollisionResult()
            {
                Us = a,
                Them = b,
                CollisionResponse = minPenetration * mtv,
                IsColliding = true
            };
        }

        public static void CircleCircle(Circle circleA, Circle circleB)
        {
        }

        public static void CircleBox(Circle cicleA, Box boxB)
        {
        }
	}
}
