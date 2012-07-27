using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Collision
{
    interface ICollisionSolver
    {
        CollisionResult SolveCollision(CollisionBody colA, CollisionBody colB);
        bool IsSolveable(CollisionBody colA, CollisionBody colB);
    }
}
