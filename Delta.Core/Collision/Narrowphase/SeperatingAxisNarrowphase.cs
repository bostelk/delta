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
        /// <summary>
        /// The collision solving algorithms that are available for use.
        /// </summary>
        List<ICollisionSolver> _solvers;

        public SeperatingAxisNarrowphase()
        {
            CollisionGlobals.Results = new Stack<CollisionResult>(10);
            _solvers = new List<ICollisionSolver>(4);

            // ORDER MATTERS: use the more specific algorithms first.
            _solvers.Add(new BoxBoxSolver());
            _solvers.Add(new CircleBoxSolver());
            _solvers.Add(new CircleCircleSolver());
            _solvers.Add(new PolyPolySolver());
        }

        public void SolveCollisions(OverlappingPairCache overlappingPairs)
        {
            CollisionGlobals.NarrowphaseDetections = 0;
            foreach (OverlappingPair pair in overlappingPairs._pairs)
            {
                CollisionResult result;
                CollisionBody colA = pair.ProxyA.ClientObject as CollisionBody;
                CollisionBody colB = pair.ProxyB.ClientObject as CollisionBody;

                // calculates the collision result if there's an algorithm that matches the pair of shapes.
                SolveCollision(colA, colB, out result);

                // handle collision events and resolve shape penetration.
                if (result.IsColliding)
                {
                   CollisionGlobals.Results.Push(result);
    
                    // translate the body to a safe non-penetrating position.
                    //result.Us.Position += colA.Mass * result.CollisionResponse;
                    //result.Them.Position += -colB.Mass * result.CollisionResponse;

                    // handle collision events
                    if (colA.OnCollision != null)
                        colA.OnCollision(colB, Vector2.Zero);
                    if (colB.OnCollision != null)
                        colB.OnCollision(colA, Vector2.Zero);
                    CollisionGlobals.NarrowphaseDetections++;
                }
            }
        }

        public void SolveCollision(CollisionBody colA, CollisionBody colB, out CollisionResult result)
        {
            result = CollisionResult.NoCollision;
            ICollisionSolver solver = FindCollisionSovler(colA, colB);
            if (solver != null) 
                result = solver.SolveCollision(colA, colB);
        }

        /// <summary>
        /// From the available algorithms, find the algorithm that matches the shapes the closest.
        /// </summary>
        private ICollisionSolver FindCollisionSovler(CollisionBody colA, CollisionBody colB)
        {
            // factor out the null checks for speed. TODO: what about null collisionbodies?
            if (colA.Shape == null || colB.Shape == null)
                return null;
            foreach (ICollisionSolver solver in _solvers)
            {
                if (solver.IsSolveable(colA, colB))
                    return solver;
            }
            return null;
        }

    }
}
