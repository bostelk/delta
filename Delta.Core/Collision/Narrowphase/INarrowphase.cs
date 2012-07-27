using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Collision
{
    public interface INarrowphase
    {
        void SolveCollisions(OverlappingPairCache collisionPairs);
        void SolveCollision(CollisionBody colA, CollisionBody colB, out CollisionResult result);
        //ICollisionSolver SolveCollision(CollisionBody colA, CollisionBody colB, out CollisionResult result);
    }
}
