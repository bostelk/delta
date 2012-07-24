using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{

    /// <summary>
    /// SAT, or Seperating Axis Theorem provides narrow phase collision detection. Ie. It checks if polygons
    /// are intersecting each other (colliding) and what the proper response is to resolve the collision.
    /// This is done by checking all potential seperating axisis for seperating lines. ie. For a space
    /// in their projected size. If a seperating axis exists, then we know the objects are not intersecting
    /// and are provided with an early out.
    /// </summary>
    public class SeperatingAxisNarrowphase : INarrowphase
    {
        BoxBoxSolver BoxBox;
        CircleBoxSolver CircleBox;
        CircleCircleSolver CircleCircle;
        PolyPolySolver PolyPoly;

        public SeperatingAxisNarrowphase()
        {
            CollisionGlobals.Results = new Stack<CollisionResult>(10);
            BoxBox = new BoxBoxSolver();
            CircleBox = new CircleBoxSolver();
            CircleCircle = new CircleCircleSolver();
            PolyPoly = new PolyPolySolver();
        }

        public void SolveCollisions(OverlappingPairCache overlappingPairs)
        {
            CollisionGlobals.NarrowphaseDetections = 0;
            foreach (OverlappingPair pair in overlappingPairs)
            {
                CollisionResult result;
                Collider colA = pair.ProxyA.ClientObject as Collider;
                Collider colB = pair.ProxyB.ClientObject as Collider;

                if (colA.Shape is Box && colB.Shape is Box)
                {
                    result = BoxBox.SolveCollision(colA, colB);
                }
                else if (colA.Shape is Circle && colB.Shape is Circle)
                {
                    result = CircleCircle.SolveCollision(colA, colB);
                }
                else if (colA.Shape is Box && colB.Shape is Circle)
                {
                    result = CircleBox.SolveCollision(colB, colA);
                }
                else if (colA.Shape is Circle && colB.Shape is Box)
                {
                    result = CircleBox.SolveCollision(colA, colB);
                }
                else
                {
                    result = PolyPoly.SolveCollision(colA, colB);
                }

                if (result.IsColliding)
                {
                   CollisionGlobals.Results.Push(result);

                    // translate the polygon to a safe non-interecting position.
                    //result.Us.Position += colA.Mass * result.CollisionResponse;
                    //result.Them.Position += -colB.Mass * result.CollisionResponse;
                    // on collision events
                    if (colA.OnCollision != null)
                        colA.OnCollision(colB, Vector2.Zero);
                    if (colB.OnCollision != null)
                        colB.OnCollision(colA, Vector2.Zero);
                    CollisionGlobals.NarrowphaseDetections++;
                }
            }
        }

    }
}
